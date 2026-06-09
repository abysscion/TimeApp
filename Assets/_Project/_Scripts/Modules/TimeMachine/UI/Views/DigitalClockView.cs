using Modules.TimeMachine.UI.Interfaces;
using TMPro;
using UnityEngine;

namespace Modules.TimeMachine.UI.Views
{
	public class DigitalClockView : MonoBehaviour, IDigitalClockView
	{
		[SerializeField] private TMP_Text labelClock;

		public void SetTime(string timeStr)
		{
			labelClock.text = timeStr;
		}
	}
}
