using System;
using IgiCore.SDK.Core.Diagnostics;

namespace IgiCore.Server.Diagnostics
{
	public class Logger : ILogger
	{
		public string Prefix { get; }

		public Logger(string prefix = "")
		{
			this.Prefix = prefix;
		}

		public void Debug(string message)
		{
			WriteLine(message);
		}

		public void Log(string message)
		{
			WriteLine(message);
		}

		public void Info(string message)
		{
			WriteLine(message);
		}

		public void Warn(string message)
		{
			WriteLine(message);
		}

		public void Error(Exception exception)
		{
			WriteLine($"ERROR: {exception.Message}");
		}

		private void WriteLine(string message)
		{
			var output = $"{DateTime.Now:s}";

			if (!string.IsNullOrEmpty(this.Prefix)) output += $" [{this.Prefix}]";

			CitizenFX.Core.Debug.Write($"{output} {message}{Environment.NewLine}");
		}
	}
}
