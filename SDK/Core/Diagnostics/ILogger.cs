using System;

namespace IgiCore.SDK.Core.Diagnostics
{
	public interface ILogger
	{
		void Log(string message);

		void Error(Exception exception);
	}
}
