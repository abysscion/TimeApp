using Cysharp.Threading.Tasks;
using System;
using System.Threading;

namespace Modules.TimeMachine.Domain.Interfaces
{
	public interface ITimeService
	{
		public UniTask<DateTime> GetCurrentTimeAsync(CancellationToken ct = default);
	}
}
