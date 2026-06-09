using Cysharp.Threading.Tasks;
using Infrastructure.StateMachine.States;
using Shared.Domain.Interfaces.Infrastructure;
using Shared.Domain.Models;
using System;
using System.Threading;

namespace Infrastructure.StateMachine
{
	public sealed class AppStateMachine : IAppStateMachine
	{
		private readonly AppStateRegistry _registry;
		private readonly IAppLogger _logger;

		private StateBase _state;

		internal AppStateMachine(AppStateRegistry registry, IAppLogger logger)
		{
			_registry = registry;
			_logger = logger;
		}

		public AppStateType CurStateType => _state?.type ?? AppStateType.None;

		public async UniTask ChangeStateAsync(AppStateType targetState, CancellationToken ct = default)
		{
			if (!IsTransitionAvailable(CurStateType, targetState))
				throw new InvalidOperationException($"Invalid app transition: {CurStateType} -> {targetState}");

			var next = _registry.Get(targetState);

			try
			{
				if (_state != null)
				{
					//_logger.Log($"Exiting state [{CurStateType}]");
					await _state.ExitAsync(ct);
					//_logger.Log($"Exited state [{CurStateType}]!");
				}
				_logger.Log($"Changing state [{CurStateType}] -> [{targetState}]");
				_state = next;
				await next.EnterAsync(ct);
				//_logger.Log($"Entered state [{CurStateType}]!");
			}
			catch (Exception exception)
			{
				_logger.Exception(exception);
				throw;
			}
		}

		private static bool IsTransitionAvailable(AppStateType from, AppStateType to)
		{
			return true; //TODO: transition validation
		}
	}
}