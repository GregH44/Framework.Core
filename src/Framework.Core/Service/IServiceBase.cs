using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.Core.Service
{
    public interface IServiceBase<TDataModel> : IDisposable
        where TDataModel : class
    {
        DbContext Context { get; }

        void Add(TDataModel entity);

        void Add(IEnumerable<TDataModel> entities);

        void AddOrUpdate(TDataModel entity);

        void AddOrUpdate(IEnumerable<TDataModel> entities);

        void Delete(object id);

        void Delete(TDataModel entity);

        void Delete(IEnumerable<TDataModel> entities);

        Task<IEnumerable<TDataModel>> GetList(object searchModel = null);

        TDataModel Get(object id);

        int Save();

        void Update(TDataModel entity);

        void Update(IEnumerable<TDataModel> entities);
    }
}
