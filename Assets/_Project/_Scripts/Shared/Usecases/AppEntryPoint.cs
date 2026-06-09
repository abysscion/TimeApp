using Shared.Domain.Interfaces.Infrastructure;
using Shared.Domain.Models;
using Zenject;

namespace Shared.Usecases
{
	public class AppEntryPoint : IInitializable
	{
		private readonly IAppStateMachine _stateMachine;
		private readonly IAppLogger _logger;

		public AppEntryPoint(IAppStateMachine stateMachine, IAppLogger logger)
		{
			_stateMachine = stateMachine;
			_logger = logger;
		}

		public async void Initialize()
		{
			_logger.Log("Starting application...");
			try
			{
				await _stateMachine.ChangeStateAsync(AppStateType.Bootstrap);
			}
			catch (System.Exception e)
			{
				_logger.Exception(e);
			}
		}
	}
}
