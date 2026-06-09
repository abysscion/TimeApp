using Cysharp.Threading.Tasks;
using Shared.Domain.Models;
using Shared.Usecases;
using System.Threading;

namespace Infrastructure.StateMachine.States
{
	public sealed class StartupState : StateBase
	{
		private readonly AppStartupUsecase _startupUsecase;

		public StartupState(AppStartupUsecase startupUsecase, AppStateType type) : base(type)
		{
			_startupUsecase = startupUsecase;
		}

		public override async UniTask EnterAsync(CancellationToken ct = default)
		{
			await base.EnterAsync(ct);
			await _startupUsecase.ExecuteAsync(ct);
		}
	}
}