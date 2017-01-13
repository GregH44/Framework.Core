using AutoMapper;
using System;
using System.Collections.Generic;

namespace Framework.Core.Configuration
{
    public class AutoMapperConfiguration : Profile
    {
        private readonly Dictionary<Type, Type> modelsToMap = null;

        public AutoMapperConfiguration(Dictionary<Type, Type> modelsToMap)
        {
            this.modelsToMap = modelsToMap;
        }

        public AutoMapperConfiguration SetMapping()
        {
            foreach (var modelToMap in modelsToMap)
            {
                CreateMap(modelToMap.Key, modelToMap.Value);
                CreateMap(modelToMap.Value, modelToMap.Key);
            }

            return this;
        }
    }
}
