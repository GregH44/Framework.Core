using System;

namespace Framework.Core.Exceptions
{
    public class GenericRepositoryException : Exception
    {
        public GenericRepositoryException() { }

        public GenericRepositoryException(string message) : base(message) { }

        public GenericRepositoryException(string message, Exception inner) : base(message, inner) { }
    }
}
