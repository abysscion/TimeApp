using Shared.Domain.Models;
using Zenject;

namespace Shared.Infrastructure.StateMachine
{
	public class ModelsInstaller : Installer<ModelsInstaller>
	{
		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<LoadingModel>().AsSingle();
		}
	}
}