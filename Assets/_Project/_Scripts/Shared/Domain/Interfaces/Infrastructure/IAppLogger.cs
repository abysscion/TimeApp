using System;

namespace Shared.Domain.Interfaces.Infrastructure
{
	public interface IAppLogger
	{
		public void Log(string entry);
		public void Warning(string entry);
		public void Error(string entry);
		public void Exception(Exception exception);
		public void Exception(string entry, Exception exception);
	}
}