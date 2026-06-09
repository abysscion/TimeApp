using Cysharp.Threading.Tasks;
using System;
using System.Threading;

namespace Shared.Domain.Models
{
	public sealed class LoadingRequest
	{
		public Func<CancellationToken, UniTask> Operation { get; }
		public AppStateType NextState { get; }

		public LoadingRequest(Func<CancellationToken, UniTask> operation, AppStateType nextState)
		{
			Operation = operation;
			NextState = nextState;
		}
	}
}