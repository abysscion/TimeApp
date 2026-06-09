using Modules.TimeMachine.Domain.Interfaces;
using Shared.Domain.Interfaces.Infrastructure;
using System;
using Zenject;

namespace Modules.TimeMachine.UI.Presenters
{
	public abstract class ClockPresenterBase : IDisposable, ITickable
	{
		protected readonly ITimePresentationModel _presentationModel;
		protected readonly IGlobalTimeModel _timeModel;
		protected readonly IAppLogger _logger;

		protected DateTime _lastClockDrawTime;

		protected ClockPresenterBase(
			ITimePresentationModel presentationModel,
			IGlobalTimeModel timeModel,
			IAppLogger logger)
		{
			_presentationModel = presentationModel;
			_timeModel = timeModel;
			_logger = logger;
			_presentationModel.IsEditModeEnabledChanged += IsEditModeEnabledChanged;
			_presentationModel.IsClockFreezedChanged += OnIsClockFreezedChanged;
			_timeModel.InstalledDateChanged += OnInstalledDateChanged;
		}

		protected DateTime CurrentDrawingTime { get; private set; }

		public void Tick()
		{
			if (_presentationModel.IsClockFreezed)
				return;
			if ((DateTime.Now - _lastClockDrawTime).Seconds < 1f)
				return;
			_lastClockDrawTime = DateTime.Now;
			var timeElapsedSinceLastSync = DateTime.Now - _timeModel.LastSyncSystemTime;
			var timeToSet = _timeModel.InstalledTime + timeElapsedSinceLastSync;
			CurrentDrawingTime = timeToSet;
			DrawTime(timeToSet);
		}

		public void Dispose()
		{
			_presentationModel.IsEditModeEnabledChanged -= IsEditModeEnabledChanged;
			_presentationModel.IsClockFreezedChanged -= OnIsClockFreezedChanged;
			_timeModel.InstalledDateChanged -= OnInstalledDateChanged;
			InnerDispose();
		}

		protected abstract void DrawTime(DateTime time);
		protected virtual void IsEditModeEnabledChanged(bool isEditModeEnabled) { }
		protected virtual void OnIsClockFreezedChanged(bool isFreezed) { }
		protected virtual void OnInstalledDateChanged(DateTime time) { }
		protected virtual void InnerDispose() { }
	}
}
