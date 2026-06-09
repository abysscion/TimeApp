using Shared.Domain.Interfaces.Views;
using Shared.UI.Presenters;
using UnityEngine;
using Zenject;

namespace Shared.Infrastructure
{
	public class SharedUIInstaller : MonoInstaller
	{
		[SerializeField] private GameObject _loadingScreenPrefab;
		[SerializeField] private Transform _uiRootTf;

		public override void InstallBindings()
		{
			Container.Bind<ILoadingScreenView>()
				.FromComponentInNewPrefab(_loadingScreenPrefab)
				.UnderTransform(_uiRootTf)
				.AsSingle()
				.NonLazy();

			Container.BindInterfacesAndSelfTo<LoadingScreenPresenter>().AsSingle().NonLazy();
		}
	}
}