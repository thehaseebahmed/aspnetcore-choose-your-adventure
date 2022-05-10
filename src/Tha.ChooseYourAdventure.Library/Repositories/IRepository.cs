using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tha.ChooseYourAdventure.Data.Interfaces;

namespace Tha.ChooseYourAdventure.Library.Repositories
{
    public interface IRepository<T>
        where T : class, IEntity
    {
        Task<T> CreateAsync(T model, CancellationToken cancellatonToken = default);
        Task<T> DeleteAsync(object id, CancellationToken cancellatonToken = default);
        IQueryable<T> Read();
        T Read(object id);
        Task<T> UpdateAsync(object id, T model, CancellationToken cancellatonToken = default);
    }
}
