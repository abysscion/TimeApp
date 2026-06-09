using Cysharp.Threading.Tasks;
using Shared.Domain.Models;
using System;
using System.Threading;

namespace Infrastructure.StateMachine.States
{
	public abstract class StateBase : IDisposable
	{
		public readonly AppStateType type;

		protected StateBase(AppStateType type)
		{
			this.type = type;
		}

		public virtual UniTask EnterAsync(CancellationToken ct) => UniTask.CompletedTask;

		public virtual UniTask ExitAsync(CancellationToken ct = default)
		{
			Dispose();
			return UniTask.CompletedTask;
		}

		public virtual void Dispose()
		{
		}
	}
}