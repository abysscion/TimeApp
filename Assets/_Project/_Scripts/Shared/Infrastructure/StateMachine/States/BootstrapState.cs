using Cysharp.Threading.Tasks;
using Shared.Domain.Interfaces.Infrastructure;
using Shared.Domain.Models;
using System.Threading;

namespace Infrastructure.StateMachine.States
{
	public sealed class BootstrapState : StateBase
	{
		private readonly IAppStateMachine _stateMachine;

		public BootstrapState(IAppStateMachine stateMachine, AppStateType type) : base(type)
		{
			_stateMachine = stateMachine;
		}

		public override async UniTask EnterAsync(CancellationToken ct = default)
		{
			await base.EnterAsync(ct);
			await _stateMachine.ChangeStateAsync(AppStateType.Startup);
		}
	}
}