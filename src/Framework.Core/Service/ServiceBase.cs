using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework.Core.DAL.Infrastructure;
using Framework.Core.DAL.Repository;
using Framework.Core.Exceptions;

namespace Framework.Core.Service
{
    public abstract class ServiceBase<TDataModel> : IServiceBase<TDataModel>
        where TDataModel : class
    {
        protected DataBaseContext context = null;
        protected IGenericRepository<TDataModel> repository = null;

        public DataBaseContext Context
        {
            get
            {
                return context;
            }
        }

        protected virtual IGenericRepository<TDataModel> Repository
        {
            get
            {
                if (repository == null)
                {
                    repository = new GenericRepository<TDataModel>(context);
                }

                return repository;
            }
        }

        public ServiceBase(DataBaseContext context)
        {
            this.context = context;
        }

        public virtual void Add(TDataModel entity)
        {
            try
            {
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

        public virtual void Add(IEnumerable<TDataModel> entities)
        {
            try
            {
                Repository.Add(entities);
                Save();
            }
            catch (GenericRepositoryException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new ServiceLayerException("An error occured while inserting entities !", ex);
            }
        }

        public abstract void AddOrUpdate(TDataModel entity);

        public abstract void AddOrUpdate(IEnumerable<TDataModel> entities);

        public virtual void Delete(TDataModel entity)
        {
            try
            {
                Repository.Delete(entity);
                Save();
            }
            catch (GenericRepositoryException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new ServiceLayerException("An error occured while deleting entity !", ex);
            }
        }

        public abstract void Delete(long id);

        public virtual void Delete(IEnumerable<TDataModel> entities)
        {
            Parallel.ForEach(entities, entity =>
            {
                Delete(entity);
            });
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public virtual void Update(TDataModel entity)
        {
            try
            {
                Repository.Update(entity);
                Save();
            }
            catch (GenericRepositoryException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new ServiceLayerException("An error occured while updating entity !", ex);
            }
        }

        public virtual void Update(IEnumerable<TDataModel> entities)
        {
            try
            {
                Repository.Update(entities);
                Save();
            }
            catch (GenericRepositoryException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new ServiceLayerException("An error occured while updating entities !", ex);
            }
        }

        public abstract TDataModel Get(long id);

        public abstract Task<IEnumerable<TDataModel>> GetList(object searchModel = null);

        public virtual int Save()
        {
            try
            {
                return Repository.Save();
            }
            catch (GenericRepositoryException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new ServiceLayerException("An error occurred while saving user !", ex);
            }
        }
    }
}
