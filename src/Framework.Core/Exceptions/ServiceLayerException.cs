using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Core.Exceptions
{
    public class ServiceLayerException : Exception
    {
        public ServiceLayerException() { }

        public ServiceLayerException(string message) : base(message) { }

        public ServiceLayerException(string message, Exception inner) : base(message, inner) { }
    }
}
