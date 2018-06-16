using System.Data.Entity;
using IgiCore.SDK.Server.Storage;
using TemplatePlugin.Core.Models;

namespace TemplatePlugin.Server.Storage
{
	public class TemplateContext : EFContext<TemplateContext>
	{
		public DbSet<TemplateModel> TemplateModels { get; set; }
	}
}
