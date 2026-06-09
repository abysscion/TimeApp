using Cysharp.Threading.Tasks;
using Shared.Domain.Interfaces.Infrastructure;
using Shared.Domain.Models;
using System.Threading;

namespace Infrastructure.StateMachine.States
{
	public sealed class GameplayState : StateBase
	{
		private readonly LoadingModel _loadingModel;
		private readonly IAppStateMachine _stateMachine;
		private readonly IAppLogger _logger;

		public GameplayState(
			LoadingModel loadingModel,
			IAppStateMachine stateMachine,
			IAppLogger logger,
			AppStateType type) : base(type)
		{
			_loadingModel = loadingModel;
			_stateMachine = stateMachine;
			_logger = logger;
		}

		public override async UniTask EnterAsync(CancellationToken ct)
		{
			await base.EnterAsync(ct);
		}
	}
}