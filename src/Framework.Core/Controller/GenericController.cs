using Framework.Core.Attributes;
using Framework.Core.DAL.Infrastructure;
using Framework.Core.Enums;
using Framework.Core.Models;
using Framework.Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Core.Controller
{
    [AllowCRUD]
    [Route("api/{model}/{id?}")]
    public class GenericController : ControllerBase
    {
        private readonly DataBaseContext context = null;

        public GenericController(DataBaseContext context)
            : base()
        {
            this.context = context;
        }

        [HttpPost]
        public IActionResult Add([FromRoute] string model, [FromBody] object content)
        {
            IActionResult response = null;
            object values = GetObjectsFromJson(model, content.ToString());
            
            var exceptions = ValidateModels(values);
            if (exceptions.Count() > 0)
            {
                response = BadRequest(exceptions);
            }
            else
            {
                ServiceCaller.Call(context, GlobalEnums.Api.Add, model, values);
                response = Ok();
            }
            
            return response;
        }

        [HttpPut]
        public IActionResult Update([FromRoute] string model, [FromRoute] long id, [FromBody] object content)
        {
            IActionResult response = null;
            object values = GetObjectsFromJson(model, content.ToString());

            var exceptions = ValidateModels(values);
            if (exceptions.Count() > 0)
            {
                response = BadRequest(exceptions);
            }
            else
            {
                ServiceCaller.Call(context, GlobalEnums.Api.Update, model, values);
                response = Ok();
            }

            return response;
        }

        [HttpDelete]
        public IActionResult Delete([FromRoute] string model, long id)
        {
            ServiceCaller.Call(context, GlobalEnums.Api.Delete, model, id);

            return Ok();
        }

        [HttpGet]
        public IActionResult Get([FromRoute] string model, long? id)
        {
            return Json(ServiceCaller.Call(context, GlobalEnums.Api.Get, model, id));
        }

        [NonAction]
        public static IActionResult Error()
        {
            return new ControllerBase().BadRequest("An unexpected error has occured. Please see log for more details.");
        }

        private object GetObjectsFromJson(string model, string content)
        {
            var jToken = JToken.Parse(content);
            Type contentObjectType = ServiceResolver.GetModelType(model);

            if (jToken is JArray)
            {
                var typeList = typeof(List<>);
                contentObjectType = typeList.MakeGenericType(contentObjectType);
            }

            return jToken.ToObject(contentObjectType);
        }

        private List<ExceptionModel> ValidateModels(object values)
        {
            var exceptions = new List<ExceptionModel>();

            if (!TryValidateModel(values))
            {
                foreach (var value in ModelState.Values.Where(mod => mod.ValidationState == ModelValidationState.Invalid))
                {
                    var exception = new ExceptionModel();
                    string message = "An error occured while adding value => ";

                    foreach (var errorMessage in value.Errors)
                    {
                        message += $"{errorMessage.ErrorMessage} ; ";
                    }
                    exception.AddErrorMessage(message);

                    exceptions.Add(exception);
                }
            }

            return exceptions;
        }
    }
}
