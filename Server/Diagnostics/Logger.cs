using System;
using CitizenFX.Core;
using IgiCore.SDK.Core.Diagnostics;

namespace IgiCore.Server.Diagnostics
{
	public class Logger : ILogger
	{
		public void Log(string message)
		{
			Debug.WriteLine($"{DateTime.Now:s} [SERVER:LOGGER]: {message}");
		}

		public void Error(Exception exception)
		{
			Debug.WriteLine($"{DateTime.Now:s} [SERVER:LOGGER]: ERROR {exception.Message}");
		}
	}
}
