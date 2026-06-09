using Modules.TimeMachine.Configs;
using Modules.TimeMachine.Domain.Models;
using Modules.TimeMachine.Services;
using Modules.TimeMachine.UI.Interfaces;
using Modules.TimeMachine.UI.Presenters;
using Modules.TimeMachine.UI.Views;
using Modules.TimeMachine.UseCases;
using UnityEngine;
using Zenject;

namespace Modules.TimeMachine.Installers
{
	public class TimeMachineInstaller : MonoInstaller
	{
		[SerializeField] private TimeServerSettings _settings;

		public override void InstallBindings()
		{
			BindDomain();
			BindServices();
			BindUseCases();
			BindUI();
		}

		private void BindDomain()
		{
			Container.Bind<TimeServerSettings>().FromInstance(_settings).AsSingle();

			Container.BindInterfacesAndSelfTo<GlobalTimeModel>().AsSingle();
			Container.BindInterfacesAndSelfTo<TimePresentationModel>().AsSingle();
		}

		private void BindServices()
		{
			Container.BindInterfacesAndSelfTo<TimeService>().AsSingle();
		}

		private void BindUseCases()
		{
			Container.BindInterfacesAndSelfTo<TimeSyncUseCase>().AsSingle();
			Container.BindInterfacesAndSelfTo<EditTimePresentationUseCase>().AsSingle();
		}

		private void BindUI()
		{
			var digitalView = GetComponentInChildren<DigitalClockView>(true);
			var analogView = GetComponentInChildren<AnalogClockView>(true);
			var editView = GetComponentInChildren<EditClockView>(true);

			Container.Bind<IDigitalClockView>().FromInstance(digitalView).AsSingle();
			Container.Bind<IAnalogClockView>().FromInstance(analogView).AsSingle();
			Container.Bind<IEditClockView>().FromInstance(editView).AsSingle();

			Container.BindInterfacesAndSelfTo<DigitalClockPresenter>().AsSingle().NonLazy();
			Container.BindInterfacesAndSelfTo<AnalogClockPresenter>().AsSingle().NonLazy();
			Container.BindInterfacesAndSelfTo<EditClockPresenter>().AsSingle().NonLazy();
		}
	}
}