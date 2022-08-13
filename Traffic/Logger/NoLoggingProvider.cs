using Hangfire.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Traffic.Api.Logger
{
	public class NoLoggingProvider : ILogProvider
	{
		public ILog GetLogger(string name)
		{
			return new NoLoggingLogger();
		}
	}

	public class NoLoggingLogger : ILog
	{
		public bool Log(LogLevel logLevel, Func<string> messageFunc, Exception exception = null)
		{
			return false;
		}
	}
}
