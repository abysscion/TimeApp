using Shared.Usecases;
using Zenject;

namespace Shared.Infrastructure.StateMachine
{
	public class UseCasesInstaller : Installer<UseCasesInstaller>
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<AppStartupUsecase>().AsSingle();
		}
	}
}