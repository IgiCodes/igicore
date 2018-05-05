using System;

namespace IgiCore.Client.Managers
{
    public abstract class Manager : IManager, IDisposable
    {
	    public abstract void Dispose();
    }
}
