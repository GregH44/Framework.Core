using AutoMapper;
using System;
using System.Collections.Generic;

namespace Framework.Core.Configuration
{
    public static class Mapper
    {
        public readonly static Dictionary<Type, Type> modelsToMap = new Dictionary<Type, Type>();
        private static IMapper mapper = null;

        public static IMapper Instance
        {
            get
            {
                if (mapper == null)
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.AddProfile(new AutoMapperConfiguration(modelsToMap).SetMapping());
                    });

                    mapper = config.CreateMapper();
                }

                return mapper;
            }
        }
    }
}
