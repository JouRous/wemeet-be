using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data.Models.Shared;

namespace API.Data.Repository.interfaces
{
	public interface IEntitiesRepo<T> : IDisposable where T : BaseEntity
	{
		Task InsertOne(T data);
		Task InsertMany(T[] data);
		Task UpdateOne(T data);
		Task UpdateMay(T[] data);
		IQueryable<T> FindAll();
		IEnumerable<T> FindByQuery(Func<T, bool> filter);

	}
}