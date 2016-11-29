using AutoMapper;

namespace Sample.DotNetFramework.MVC6.Configuration
{
    public static class Mapper
    {
        private static IMapper mapper = null;

        public static IMapper Instance
        {
            get
            {
                if (mapper == null)
                {
                    if (mapper == null)
                    {
                        var config = new MapperConfiguration(cfg =>
                        {
                            cfg.AddProfile<AutoMapperConfiguration>();
                        });

                        mapper = config.CreateMapper();
                    }
                }

                return mapper;
            }
        }
    }
}
