using Infrastructure.StateMachine;
using Infrastructure.StateMachine.States;
using Shared.Domain.Models;
using Zenject;

namespace Shared.Infrastructure.StateMachine
{
	public class StateMachineInstaller : Installer<StateMachineInstaller>
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<AppStateMachine>().AsSingle();

			Container.Bind<BootstrapState>().AsSingle().WithArguments(AppStateType.Bootstrap);
			Container.Bind<StartupState>().AsSingle().WithArguments(AppStateType.Startup);
			Container.Bind<LoadingState>().AsSingle().WithArguments(AppStateType.Loading);
			Container.Bind<GameplayState>().AsSingle().WithArguments(AppStateType.Gameplay);

			Container.Bind<AppStateRegistry>().AsSingle();
		}
	}
}