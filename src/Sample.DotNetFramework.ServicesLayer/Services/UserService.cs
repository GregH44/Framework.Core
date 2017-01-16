using Framework.Core.DAL.Infrastructure;
using Framework.Core.Exceptions;
using Framework.Core.Service;
using Sample.DotNetFramework.Common.DTO;
using Sample.DotNetFramework.ServicesLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.DotNetFramework.ServicesLayer.Services
{
    public sealed class UserService : ServiceBase<UserModel>, IUserService
    {
        public UserService(DatabaseContext context) : base(context)
        {
        }

        public override void AddOrUpdate(UserModel user)
        {
            if (user.UserId == 0)
            {
                Repository.Add(user);
            }
            else
            {
                Repository.Update(user);
            }

            Save();
        }

        public override void AddOrUpdate(IEnumerable<UserModel> users)
        {
            try
            {
                if (users.Any(user => user.UserId == 0))
                {
                    Repository.Add(users.Where(user => user.UserId == 0));
                }
                else
                {
                    Repository.Update(users.Where(user => user.UserId != 0));
                }

                Save();
            }
            catch (GenericRepositoryException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new ServiceLayerException("An error occurred while adding or updating user !", ex);
            }
        }

        public override void Delete(long id)
        {
            try
            {
                Repository.Delete(user => user.UserId == id);
                Save();
            }
            catch (GenericRepositoryException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new ServiceLayerException("An error occurred while deleting user !", ex);
            }
        }

        public override UserModel Get(long id)
        {
            UserModel dataModel = null;

            try
            {
                dataModel = Repository.Get(x => x.UserId == id);
            }
            catch (GenericRepositoryException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new ServiceLayerException("An error occurred while getting user !", ex);
            }

            return dataModel;
        }

        public override Task<IEnumerable<UserModel>> GetList(object searchModel = null)
        {
            Task<IEnumerable<UserModel>> users = null;

            try
            {
                users = Repository.GetList();
            }
            catch (GenericRepositoryException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new ServiceLayerException("An error occurred while getting list of Users !", ex);
            }

            return users;
        }
    }
}
