using Shared.Infrastructure.Services;
using Shared.Infrastructure.StateMachine;
using Shared.Usecases;
using Zenject;

namespace Shared.Infrastructure
{
	public class InfrastructureInstaller : MonoInstaller
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<AppLoggerService>().AsSingle().NonLazy();
			Container.BindInterfacesAndSelfTo<AddressablesService>().AsSingle().NonLazy();
			Container.BindInterfacesAndSelfTo<SceneControllerService>().AsSingle().NonLazy();

			ModelsInstaller.Install(Container);
			StateMachineInstaller.Install(Container);
			UseCasesInstaller.Install(Container);

			Container.BindInterfacesAndSelfTo<AppEntryPoint>().AsSingle().NonLazy();
		}
	}
}