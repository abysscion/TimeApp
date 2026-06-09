using TMPro;
using UnityEngine.UI;

namespace Modules.TimeMachine.UI.Interfaces
{
	public interface IEditClockView
	{
		TMP_InputField.OnChangeEvent TimeInputChanged { get; }
		TMP_InputField.SubmitEvent TimeInputSubmited { get; }
		Button.ButtonClickedEvent CancelButtonClicked { get; }
		Button.ButtonClickedEvent SaveButtonClicked { get; }
		Button.ButtonClickedEvent EditButtonClicked { get; }

		void ToggleInputFieldInteractableState(bool isEnabled);
		void ToggleSaveButtonInteractable(bool isActive);
		void ToggleEditingButtonsGroup(bool isActive);
		void SetInputFieldText(string text, bool ignoreNotify);
		void ToggleEditButton(bool isActive);
	}
}
