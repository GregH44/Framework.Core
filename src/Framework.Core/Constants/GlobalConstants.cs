namespace Framework.Core.Constants
{
    public static class GlobalConstants
    {
        internal const string CorrelationIdName = "CorrelationId";

        public static class CrudOperations
        {
            public const byte Create = 0;
            public const byte Receive = 1;
            public const byte Update = 2;
            public const byte Delete = 3;
        }
    }
}
