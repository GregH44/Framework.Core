using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Core.Helpers
{
    public static class GlobalHelpers
    {
        /// <summary>
        /// Generate a custom correlationId
        /// </summary>
        /// <param name="compte">User account connected</param>
        /// <returns>The custom correlationId</returns>
        public static string GenererCorrelationId(string compte)
        {
            return $"{Environment.MachineName}-{compte}-{DateTime.Now.ToLocalTime()}";
        }
    }
}
