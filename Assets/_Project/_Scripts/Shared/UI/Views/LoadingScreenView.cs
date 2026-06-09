using Shared.Domain.Interfaces.Views;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shared.UI.Views
{
	public class LoadingScreenView : MonoBehaviour, ILoadingScreenView
	{
		[SerializeField] private TMP_Text labelFillProgress;
		[SerializeField] private TMP_Text labelTitle;
		[SerializeField] private TMP_Text labelHint;
		[SerializeField] private Image fillImg;

		public void Show(string title, string hint)
		{
			labelFillProgress.text = string.Empty;
			labelTitle.text = title;
			labelHint.text = hint;
			gameObject.SetActive(true);
		}

		public void SetProgressFillAmount(float value)
		{
			fillImg.fillAmount = value;
		}

		public void SetProgressStringValue(string value)
		{
			labelFillProgress.SetText(value);
		}

		public void SetProgressStringValue(StringBuilder value)
		{
			labelFillProgress.SetText(value);
		}

		public void Hide()
		{
			gameObject.SetActive(false);
		}
	}
}
