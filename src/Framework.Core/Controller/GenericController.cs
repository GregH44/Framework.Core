using Framework.Core.Enums;
using Framework.Core.Models;
using Framework.Core.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Core.Controller
{
    public class GenericController : ControllerBase
    {
        public GenericController(ILogger<GenericController> logger)
            : base(logger)
        {
        }

        [HttpPost]
        public IActionResult Add([FromRoute] string model, [FromBody] object content)
        {
            IActionResult response = null;
            var jToken = JToken.Parse(content.ToString());
            object values = null;
            Type contentObjectType = ServiceResolver.GetModelType(model);

            if (jToken is JArray)
            {
                var typeList = typeof(List<>);
                contentObjectType = typeList.MakeGenericType(contentObjectType);
            }

            values = jToken.ToObject(contentObjectType);

            if (!TryValidateModel(values))
            {
                var exceptions = new List<ExceptionModel>();

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

                response = BadRequest(exceptions);
            }
            else
            {
                ServiceCaller.Call(GlobalEnums.Api.Add, model, values);
                response = Ok();
            }

            return response;
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] string model, long id, [FromBody] object content)
        {
            var jToken = JToken.Parse(content.ToString());
            object values = null;
            Type contentObjectType = ServiceResolver.GetModelType(model);

            if (jToken is JArray)
            {
                var typeList = typeof(List<>);
                contentObjectType = typeList.MakeGenericType(contentObjectType);
            }

            values = jToken.ToObject(contentObjectType);

            //this.TryValidateModel();

            ServiceCaller.Call(GlobalEnums.Api.Update, model, values);

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] string model, long id)
        {
            ServiceCaller.Call(GlobalEnums.Api.Delete, model, id);

            return Ok();
        }

        [HttpGet]
        public IActionResult GetAll([FromRoute] string model)
        {
            return Json(ServiceCaller.Call(GlobalEnums.Api.GetList, model));
        }

        [HttpGet]
        public IActionResult Get([FromRoute] string model, long id)
        {
            return Json(ServiceCaller.Call(GlobalEnums.Api.Get, model, id));
        }
    }
}
