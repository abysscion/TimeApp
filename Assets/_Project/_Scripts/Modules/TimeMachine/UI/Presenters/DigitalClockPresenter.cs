using Modules.TimeMachine.Domain.Interfaces;
using Modules.TimeMachine.UI.Interfaces;
using Shared.Domain.Interfaces.Infrastructure;
using System;
using System.Globalization;

namespace Modules.TimeMachine.UI.Presenters
{
	public class DigitalClockPresenter : ClockPresenterBase
	{
		private const string TimeFormat = "HH:mm:ss";
		private readonly IDigitalClockView _view;

		public DigitalClockPresenter(
			IDigitalClockView view,
			ITimePresentationModel presentationModel,
			IGlobalTimeModel timeModel,
			IAppLogger logger) : base(presentationModel, timeModel, logger)
		{
			_view = view;

			_presentationModel.EditedTimeChanged += OnEditedTimeChanged;
		}

		protected override void InnerDispose()
		{
			base.InnerDispose();
			_presentationModel.EditedTimeChanged -= OnEditedTimeChanged;
		}

		protected override void DrawTime(DateTime time)
		{
			_view.SetTime(FormatTime(time));
		}

		protected override void OnInstalledDateChanged(DateTime time) => DrawTime(time);

		private void OnEditedTimeChanged(DateTime? time)
		{
			if (!time.HasValue)
				return;
			DrawTime(time.Value);
		}

		private string FormatTime(DateTime time) => time.ToString(TimeFormat, CultureInfo.InvariantCulture);
	}
}
