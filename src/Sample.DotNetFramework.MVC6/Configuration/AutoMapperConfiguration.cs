using AutoMapper;
using Sample.DotNetFramework.Common.DTO;
using Sample.DotNetFramework.MVC6.Areas.UserManager.ViewModels;

namespace Sample.DotNetFramework.MVC6.Configuration
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<UserModel, UserViewModel>();
            CreateMap<UserViewModel, UserModel>();
        }
    }
}
