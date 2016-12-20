using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Sample.DotNetFramework.Common.DTO;

namespace Sample.DotNetFramework.MVC6.Configuration
{
    public class ConfigureGenericModel : ModelBuilder
    {
        public ConfigureGenericModel()
            : base(new ConventionSet())
        {
            Entity<UserModel>();
        }
    }
}
