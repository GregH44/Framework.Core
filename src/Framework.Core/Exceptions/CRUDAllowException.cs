using System;

namespace Framework.Core.Exceptions
{
    public class CRUDAllowException : Exception
    {
        public CRUDAllowException() { }
        public CRUDAllowException(string message) : base(message) { }
        public CRUDAllowException(string message, Exception inner) : base(message, inner) { }
    }
}
