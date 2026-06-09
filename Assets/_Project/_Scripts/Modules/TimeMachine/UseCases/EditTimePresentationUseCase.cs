using Modules.TimeMachine.Domain.Interfaces;
using Modules.TimeMachine.Domain.Models;
using Shared.Domain.Interfaces.Infrastructure;
using System;

namespace Modules.TimeMachine.UseCases
{
	public class EditTimePresentationUseCase : IDisposable
	{
		private readonly TimePresentationModel _presentationModel;
		private readonly GlobalTimeModel _globalTimeModel;
		private readonly ITimeService _timeService;
		private readonly IAppLogger _logger;

		public EditTimePresentationUseCase(
			TimePresentationModel presentationModel,
			GlobalTimeModel globalTimeModel,
			ITimeService timeService,
			IAppLogger logger)
		{
			_presentationModel = presentationModel;
			_globalTimeModel = globalTimeModel;
			_timeService = timeService;
			_logger = logger;
		}

		public void EnterEditMode()
		{
			_presentationModel.IsEditModeEnabled = true;
			_presentationModel.IsClockFreezed = true;
		}

		public void SaveTime()
		{
			CancelEditMode();
			if (_presentationModel.EditedTime.HasValue)
			{
				_globalTimeModel.InstalledTime = _presentationModel.EditedTime.Value;
				_globalTimeModel.LastSyncSystemTime = DateTime.Now;
			}
			_presentationModel.EditedTime = null;
		}

		public void CancelEditMode()
		{
			_presentationModel.IsEditModeEnabled = false;
			_presentationModel.IsClockFreezed = false;
		}

		public void SetEditedTime(DateTime time) => _presentationModel.EditedTime = time;

		public void Dispose() { }
	}
}