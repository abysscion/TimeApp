using Shared.Domain.Interfaces.Infrastructure;
using System;
using UnityEngine;

namespace Shared.Infrastructure.Services
{
	public sealed class AppLoggerService : IAppLogger
	{
		private const string Prefix = "[MGA]";

		public void Log(string entry)
		{
			Debug.Log($"{Prefix} {entry}");
		}

		public void Warning(string entry)
		{
			Debug.LogWarning($"{Prefix} {entry}");
		}

		public void Error(string entry)
		{
			Debug.LogError($"{Prefix} {entry}");
		}

		public void Exception(Exception exception)
		{
			if (exception == null)
			{
				Debug.LogError($"{Prefix} Exception is null.");
				return;
			}

			Debug.LogException(exception);
		}

		public void Exception(string entry, Exception exception)
		{
			if (exception == null)
			{
				Debug.LogError($"{Prefix} {entry} | Exception is null.");
				return;
			}

			Debug.LogError($"{Prefix} {entry} | {exception}");
		}
	}
}