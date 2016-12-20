using System.Collections.Generic;
using System.Linq;

namespace Framework.Core.Models
{
    public class ExceptionModel
    {
        public string[] ErrorsMessage { get; set; }

        public void AddErrorMessage(string errorMessage)
        {
            List<string> errorsMessage = null;

            if (ErrorsMessage == null)
                errorsMessage = new List<string>();
            else
                errorsMessage = ErrorsMessage.ToList();

            errorsMessage.Add(errorMessage);

            ErrorsMessage = errorsMessage.ToArray();
        }
    }
}
