using Framework.Core.Configuration;
using Framework.Core.DAL.Repository;
using Framework.Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Framework.Core.Service
{
    public class GenericService<TDataModel> : ServiceBase<TDataModel>
        where TDataModel : class
    {
        protected override IGenericRepository<TDataModel> Repository
        {
            get
            {
                if (repository == null)
                {
                    repository = new GenericApiRepository<TDataModel>(context);
                }

                return repository;
            }
        }

        public GenericService(DbContext context) : base(context)
        {
        }
        
        public override void Add(TDataModel entity)
        {
            try
            {
                var identityPropertyInfo = ConfigureFramework.GetIdentityPropertyInfoModelTypes(typeof(TDataModel).GetHashCode());

                SetIdentityValueToDefaultValue(entity, identityPropertyInfo);
                Repository.Add(entity);
                Save();
            }
            catch (GenericRepositoryException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new ServiceLayerException("An error occured while inserting entity !", ex);
            }
        }

        public override void Add(IEnumerable<TDataModel> entities)
        {
            try
            {
                var identityPropertyInfo = ConfigureFramework.GetIdentityPropertyInfoModelTypes(typeof(TDataModel).GetHashCode());

                foreach (var entity in entities)
                {
                    SetIdentityValueToDefaultValue(entity, identityPropertyInfo);
                    Repository.Add(entity);
                }

                Save();
            }
            catch (GenericRepositoryException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new ServiceLayerException("An error occured while inserting entity !", ex);
            }
        }

        public override void AddOrUpdate(IEnumerable<TDataModel> entities)
        {
            throw new NotImplementedException();
        }

        public override void AddOrUpdate(TDataModel entity)
        {
            throw new NotImplementedException();
        }

        public override void Delete(long id)
        {
            var method = ((GenericApiRepository<TDataModel>)Repository).GetType().GetMethod("Delete", new Type[] { typeof(long) });
            method.Invoke((GenericApiRepository<TDataModel>)Repository, new object[] { id });
        }

        public override TDataModel Get(long id)
        {
            var method = ((GenericApiRepository<TDataModel>)Repository).GetType().GetMethod("Get", new Type[] { typeof(long) });
            return (TDataModel)method.Invoke((GenericApiRepository<TDataModel>)Repository, new object[] { id });
        }

        public override Task<IEnumerable<TDataModel>> GetList(object searchModel = null)
        {
            var method = Repository.GetType().GetMethod("GetList");
            return (Task<IEnumerable<TDataModel>>)method.Invoke(Repository, new object[] { null, null, null, null, null });
        }

        private void SetIdentityValueToDefaultValue(object instance, PropertyInfo identityPropertyInfo)
        {
            if (identityPropertyInfo != null)
                identityPropertyInfo.SetValue(instance, 0);
        }
    }
}
