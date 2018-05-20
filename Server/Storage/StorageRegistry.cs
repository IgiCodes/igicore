using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IgiCore.Models.Player;
using IgiCore.SDK.Server.Storage;
using IgiCore.SDK.Server.Storage.Contexts;

namespace IgiCore.Server.Storage
{
	//public class StorageRegistry : IStorageRegistry
	//{
	//	private string connection;
	//	private List<Type> types;

	//	public StorageRegistry(string connection)
	//	{
	//		this.connection = connection;
	//		this.types = new List<Type>();
	//	}

	//	public void Register<T>() where T : class, new()
	//	{
	//		this.types.Add(typeof(T));
	//	}

	//	public EFContext NewContext()
	//	{
	//		return new EFContext(new List<Type> { typeof(Map<User>) }, this.connection);
	//	}
	//}

	//public class Map<T> where T : class
	//{
	//	public Map(DbModelBuilder modelBuilder)
	//	{
	//		modelBuilder.Entity<T>();
	//	}
	//}
}
