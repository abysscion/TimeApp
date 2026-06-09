using Modules.TimeMachine.Domain.Interfaces;
using Modules.TimeMachine.UI.Interfaces;
using Modules.TimeMachine.UI.Views;
using Modules.TimeMachine.UseCases;
using Shared.Domain.Interfaces.Infrastructure;
using System;
using UnityEngine;

namespace Modules.TimeMachine.UI.Presenters
{
	public class AnalogClockPresenter : ClockPresenterBase
	{
		private readonly EditTimePresentationUseCase _timeEditUseCase;
		private readonly IAnalogClockView _view;

		public AnalogClockPresenter(
			EditTimePresentationUseCase timeEditUseCase,
			IAnalogClockView view,
			ITimePresentationModel presentationModel,
			IGlobalTimeModel timeModel,
			IAppLogger logger) : base(presentationModel, timeModel, logger)
		{
			_timeEditUseCase = timeEditUseCase;
			_view = view;
			_presentationModel.IsEditModeEnabledChanged += OnIsEditModeEnabledChanged;
			_presentationModel.EditedTimeChanged += OnEditedTimeChanged;
			_view.MinuteArrowDragged += OnMinuteArrowDragged;
			_view.HourArrowDragged += OnHourArrowDragged;
		}

		protected override void InnerDispose()
		{
			base.InnerDispose();
			_presentationModel.IsEditModeEnabledChanged -= OnIsEditModeEnabledChanged;
			_presentationModel.EditedTimeChanged -= OnEditedTimeChanged;
			_view.MinuteArrowDragged -= OnMinuteArrowDragged;
			_view.HourArrowDragged -= OnHourArrowDragged;
		}

		protected override void DrawTime(DateTime time)
		{
			TimeToArrowAngles(time, out var hourAngle, out var minuteAngle, out var secondAngle);
			_view.SetArrowsAngles(hourAngle, minuteAngle, secondAngle);
		}

		protected override void OnInstalledDateChanged(DateTime time) => DrawTime(time);

		private void OnHourArrowDragged(float angleDelta) => OnAnyArrowDragged(ClockArrowType.Hour, angleDelta);
		private void OnMinuteArrowDragged(float angleDelta) => OnAnyArrowDragged(ClockArrowType.Minute, angleDelta);

		private void OnIsEditModeEnabledChanged(bool isEditing)
		{
			_view.ToggleArrowsDraggability(isEditing);
		}

		private void OnEditedTimeChanged(DateTime? time)
		{
			if (!time.HasValue)
				return;
			DrawTime(time.Value);
		}

		private void OnAnyArrowDragged(ClockArrowType arrowType, float absoluteAngle)
		{
			_view.GetArrowsAngles(out var hourAngle, out var minuteAngle, out var secondAngle);
			if (arrowType == ClockArrowType.Hour)
				hourAngle = absoluteAngle;
			else if (arrowType == ClockArrowType.Minute)
				minuteAngle = absoluteAngle;
			var newTime = ArrowAnglesToTime(hourAngle, minuteAngle, secondAngle);
			var tmpTime = _presentationModel.EditedTime ?? CurrentDrawingTime;
			if (tmpTime.Hour >= 12 && newTime.Hour < 12)
				newTime = newTime.AddHours(12);
			_timeEditUseCase.SetEditedTime(newTime);
		}

		private void TimeToArrowAngles(DateTime time, out float hourAngle, out float minuteAngle, out float secondAngle)
		{
			var minutePercent = time.Minute / 60f;
			var hour12Part = time.Hour % 12f + minutePercent;
			hourAngle = hour12Part / 12f * 360f % 360f;
			minuteAngle = time.Minute / 60f * 360f;
			secondAngle = time.Second / 60f * 360f;
		}

		private DateTime ArrowAnglesToTime(float hourAngle, float minuteAngle, float secondAngle)
		{
			hourAngle = (hourAngle % 360f + 360f) % 360f;
			minuteAngle = (minuteAngle % 360f + 360f) % 360f;
			secondAngle = (secondAngle % 360f + 360f) % 360f;
			var seconds = Mathf.RoundToInt(secondAngle / 360f * 60f) % 60;
			var minutes = Mathf.RoundToInt(minuteAngle / 360f * 60f) % 60;
			var hour12Float = hourAngle / 360f * 12f;
			var hours = Mathf.FloorToInt(hour12Float) % 12;
			return DateTime.Today.AddHours(hours).AddMinutes(minutes).AddSeconds(seconds);
		}
	}
}
