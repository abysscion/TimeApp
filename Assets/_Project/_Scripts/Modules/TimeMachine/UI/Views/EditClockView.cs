using Modules.TimeMachine.UI.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.TimeMachine.UI.Views
{
	public class EditClockView : MonoBehaviour, IEditClockView
	{
		[SerializeField] private TMP_InputField timeInputField;
		[SerializeField] private GameObject editingButtonsGroupContainerGObj;
		[SerializeField] private GameObject editButtonContainerGObj;
		[SerializeField] private Button cancelButton;
		[SerializeField] private Button saveButton;
		[SerializeField] private Button editButton;

		public TMP_InputField.OnChangeEvent TimeInputChanged => timeInputField.onValueChanged;
		public TMP_InputField.SubmitEvent TimeInputSubmited => timeInputField.onSubmit;
		public Button.ButtonClickedEvent CancelButtonClicked => cancelButton.onClick;
		public Button.ButtonClickedEvent SaveButtonClicked => saveButton.onClick;
		public Button.ButtonClickedEvent EditButtonClicked => editButton.onClick;

		public void ToggleInputFieldInteractableState(bool isEnabled) => timeInputField.interactable = isEnabled;
		public void ToggleSaveButtonInteractable(bool isActive) => saveButton.interactable = isActive;
		public void ToggleEditingButtonsGroup(bool isActive) => editingButtonsGroupContainerGObj.SetActive(isActive);
		public void SetInputFieldText(string text, bool ignoreNotify)
		{
			if (ignoreNotify)
				timeInputField.SetTextWithoutNotify(text);
			else
				timeInputField.text = text;
		}
		public void ToggleEditButton(bool isActive) => editButtonContainerGObj.SetActive(isActive);
	}
}
