using Cysharp.Threading.Tasks;
using Shared.Domain.Interfaces.Infrastructure;
using Shared.Domain.Models;
using System;
using System.Threading;

namespace Infrastructure.StateMachine.States
{
	public sealed class LoadingState : StateBase
	{
		private readonly LoadingModel _loadingModel;
		private readonly IAppStateMachine _stateMachine;
		private readonly IAppLogger _logger;

		public LoadingState(
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
			var request = _loadingModel.request ?? throw new InvalidOperationException("Null loading request");
			if (request.Operation == null)
			{
				_logger.Warning("Empty loading operation?");
				await ProceedToNextState(request.NextState);
				return;
			}
			_loadingModel.IsActive = true;
			try
			{
				await request.Operation(ct);
			}
			catch (Exception ex)
			{
				_logger.Exception("Exception during operation executing", ex);
				throw;
			}
			finally
			{
				_loadingModel.IsActive = false;
				_loadingModel.request = null;
			}
			await ProceedToNextState(request.NextState);
		}

		private async UniTask ProceedToNextState(AppStateType type)
		{
			if (type == AppStateType.None)
			{
				_logger.Warning("Intentional empty next state?");
				return;
			}
			await _stateMachine.ChangeStateAsync(type);
		}
	}
}