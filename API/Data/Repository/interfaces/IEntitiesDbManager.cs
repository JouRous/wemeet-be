using API.Data.Models.Shared;
using System;
using System.Threading.Tasks;

namespace API.Data.Repository.interfaces
{
	public interface IEntitiesDbManager
	{
		Task ConnectingToDB<Entity>(Action<IEntitiesRepo<Entity>> action) where Entity : BaseEntity;
		Task ConnectingToDB<Entity, T>(Func<IEntitiesRepo<Entity>, T> action) where Entity : BaseEntity;
	}
}