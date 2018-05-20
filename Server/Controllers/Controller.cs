using IgiCore.SDK.Core.Diagnostics;

namespace IgiCore.Server.Controllers
{
	public abstract class Controller
	{
		protected readonly ILogger Logger;

		public Controller(ILogger logger)
		{
			this.Logger = logger;
		}

		public abstract void Initialize();
	}
}
