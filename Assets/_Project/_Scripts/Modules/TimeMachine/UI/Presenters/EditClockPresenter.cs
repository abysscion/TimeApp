using Modules.TimeMachine.Domain.Interfaces;
using Modules.TimeMachine.UI.Interfaces;
using Modules.TimeMachine.UseCases;
using System;
using System.Globalization;
using Zenject;

namespace Modules.TimeMachine.UI.Presenters
{
	public class EditClockPresenter : IInitializable, IDisposable
	{
		private const string TimeFormat = "HH:mm:ss";
		private readonly EditTimePresentationUseCase _editPresentationUseCase;
		private readonly ITimePresentationModel _presentationModel;
		private readonly IGlobalTimeModel _timeModel;
		private readonly IEditClockView _view;

		public EditClockPresenter(
			EditTimePresentationUseCase editPresentationUseCase,
			ITimePresentationModel presentationModel,
			IGlobalTimeModel timeModel,
			IEditClockView view)
		{
			_editPresentationUseCase = editPresentationUseCase;
			_presentationModel = presentationModel;
			_timeModel = timeModel;
			_view = view;

			_presentationModel.IsEditModeEnabledChanged += OnIsEditModeEnabledChanged;
			_presentationModel.EditedTimeChanged += OnEditedTimeChanged;

			_view.CancelButtonClicked.AddListener(OnCancelButtonClicked);
			_view.EditButtonClicked.AddListener(OnEditButtonClicked);
			_view.SaveButtonClicked.AddListener(OnSaveButtonClicked);
			_view.TimeInputChanged.AddListener(OnTimeInputChanged);
		}

		public void Initialize()
		{
			ToggleUIForEditMode(false);
		}

		public void Dispose()
		{
			_presentationModel.IsEditModeEnabledChanged -= OnIsEditModeEnabledChanged;
			_presentationModel.EditedTimeChanged -= OnEditedTimeChanged;

			_view.CancelButtonClicked.RemoveListener(OnCancelButtonClicked);
			_view.EditButtonClicked.RemoveListener(OnEditButtonClicked);
			_view.SaveButtonClicked.RemoveListener(OnSaveButtonClicked);
			_view.TimeInputChanged.RemoveListener(OnTimeInputChanged);
		}

		private void OnTimeInputChanged(string userInput)
		{
			if (!TryParseUserTimeInput(userInput, out var userTime))
			{
				_view.ToggleSaveButtonInteractable(false);
				return;
			}
			var editedTime = _timeModel.InstalledTime - _timeModel.InstalledTime.TimeOfDay + userTime;
			_editPresentationUseCase.SetEditedTime(editedTime);
		}

		private void ToggleUIForEditMode(bool isEditing)
		{
			_view.ToggleEditButton(!isEditing);
			_view.ToggleEditingButtonsGroup(isEditing);
			_view.ToggleInputFieldInteractableState(isEditing);
			_view.ToggleSaveButtonInteractable(false);
			if (!isEditing)
				_view.SetInputFieldText(string.Empty, false);
		}

		private void OnEditedTimeChanged(DateTime? time)
		{
			if (!time.HasValue)
				return;
			_view.SetInputFieldText(time.Value.ToString(TimeFormat, CultureInfo.InvariantCulture), true);
			_view.ToggleSaveButtonInteractable(true);
		}

		private bool TryParseUserTimeInput(string userInput, out TimeSpan userTime)
		{
			userTime = default;
			if (!TimeSpan.TryParseExact(userInput, @"hh\:mm\:ss", null, out var parsedTime))
				return false;
			if (parsedTime.TotalHours > 24)
				return false;

			userTime = parsedTime;
			return true;
		}

		private void OnIsEditModeEnabledChanged(bool isEditing) => ToggleUIForEditMode(isEditing);
		private void OnCancelButtonClicked() => _editPresentationUseCase.CancelEditMode();
		private void OnEditButtonClicked() => _editPresentationUseCase.EnterEditMode();
		private void OnSaveButtonClicked() => _editPresentationUseCase.SaveTime();
	}
}
