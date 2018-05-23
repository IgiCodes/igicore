using System;
using CitizenFX.Core;
using IgiCore.SDK.Core.Diagnostics;

namespace IgiCore.Client.Diagnostics
{
	public class Logger : ILogger
	{
		public void Log(string message)
		{
			Debug.WriteLine(message);
		}

		public void Error(Exception exception)
		{
			Debug.WriteLine($"ERROR: {exception.Message}");
		}
	}
}
