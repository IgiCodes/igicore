using System.Data.Entity;
using MySql.Data.EntityFramework;

namespace IgiCore.Server.Storage.Contexts
{
	[DbConfigurationType(typeof(MySqlEFConfiguration))]
	public abstract class Context : DbContext
	{
		public IDbSet<TEntity> Repository<TEntity>() where TEntity : class
		{
			return base.Set<TEntity>();
		}

		public Context() : base(Config.MySqlConnString) { }
	}
}
