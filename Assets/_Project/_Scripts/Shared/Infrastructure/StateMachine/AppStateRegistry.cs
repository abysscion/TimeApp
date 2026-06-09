using Infrastructure.StateMachine.States;
using Shared.Domain.Models;
using System;
using Zenject;

namespace Infrastructure.StateMachine
{
	public sealed class AppStateRegistry
	{
		private readonly DiContainer _container;

		public AppStateRegistry(DiContainer container)
		{
			_container = container;
		}

		public StateBase Get(AppStateType type)
		{
			return type switch
			{
				AppStateType.Bootstrap => _container.Resolve<BootstrapState>(),
				AppStateType.Loading => _container.Resolve<LoadingState>(),
				AppStateType.Startup => _container.Resolve<StartupState>(),
				AppStateType.Gameplay => _container.Resolve<GameplayState>(),
				_ => throw new InvalidOperationException($"App state is not registered: {type}")
			};
		}
	}
}