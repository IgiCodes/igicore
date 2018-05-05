using System;
using System.Threading.Tasks;

namespace IgiCore.Client.Interface.Screens
{
	public abstract class Screen : IDisposable
	{
		public bool Visible { get; protected set; }

		public abstract Task Render();

		public abstract Task Show();
		public abstract Task Hide();

		public virtual void Dispose()
		{
			// TODO: Deregister
		}
	}
}
