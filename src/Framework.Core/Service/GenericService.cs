using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Framework.Core.DAL.Infrastructure;
using System.Reflection;
using Framework.Core.DAL.Repository;
using Framework.Core.Exceptions;
using System.ComponentModel.DataAnnotations;

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

        public GenericService(DataBaseContext context) : base(context)
        {
        }

        public override void Add(TDataModel entity)
        {
            try
            {
                var property = typeof(TDataModel).GetProperties().SingleOrDefault(prop => prop.GetCustomAttributes<KeyAttribute>().Any());
                
                if (property != null)
                    property.SetValue(entity, Activator.CreateInstance(property.PropertyType));

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
                var property = typeof(TDataModel).GetProperties().SingleOrDefault(prop => prop.GetCustomAttributes<KeyAttribute>().Any());

                foreach (var entity in entities)
                {
                    if (property != null)
                        property.SetValue(entity, Activator.CreateInstance(property.PropertyType));

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
    }
}
