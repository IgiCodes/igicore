using System;
using JetBrains.Annotations;

namespace IgiCore.SDK.Core.Diagnostics
{
	[PublicAPI]
	public interface ILogger
	{
		string Prefix { get; }

		void Debug(string message);

		void Info(string message);

		void Warn(string message);

		void Error(Exception exception);

		void Log(string message, LogLevel level);
	}
}
