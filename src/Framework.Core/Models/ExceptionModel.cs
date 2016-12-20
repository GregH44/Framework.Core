using System.Collections.Generic;

namespace Framework.Core.Models
{
    public class ExceptionModel
    {
        public List<string> ErrorsMessage { get; set; }

        public void AddErrorMessage(string errorMessage)
        {
            if (ErrorsMessage == null)
                ErrorsMessage = new List<string>();

            ErrorsMessage.Add(errorMessage);
        }
    }
}
