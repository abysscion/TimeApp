using Modules.TimeMachine.UI.Interfaces;
using System;
using UnityEngine;

namespace Modules.TimeMachine.UI.Views
{
	public class AnalogClockView : MonoBehaviour, IAnalogClockView
	{
		[SerializeField] private AnalogClockArrowView secondArrow;
		[SerializeField] private AnalogClockArrowView minuteArrow;
		[SerializeField] private AnalogClockArrowView hourArrow;
		[SerializeField] private RectTransform secondArrowRT;
		[SerializeField] private RectTransform minuteArrowRT;
		[SerializeField] private RectTransform hourArrowRT;

		public event Action<float> MinuteArrowDragged;
		public event Action<float> HourArrowDragged;

		private void Awake()
		{
			minuteArrow.AbsoluteAngleChanged += MinuteArrowDragged;
			hourArrow.AbsoluteAngleChanged += HourArrowDragged;
		}

		private void OnDestroy()
		{
			minuteArrow.AbsoluteAngleChanged -= MinuteArrowDragged;
			hourArrow.AbsoluteAngleChanged -= HourArrowDragged;
		}

		public void SetArrowsAngles(float hourAngle, float minuteAngle, float secondAngle)
		{
			secondArrow.SetAngle(secondAngle);
			minuteArrow.SetAngle(minuteAngle);
			hourArrow.SetAngle(hourAngle);
		}

		public void GetArrowsAngles(out float hourAngle, out float minuteAngle, out float secondAngle)
		{
			secondAngle = secondArrow.GetAngle();
			minuteAngle = minuteArrow.GetAngle();
			hourAngle = hourArrow.GetAngle();
		}

		public void ToggleArrowsDraggability(bool shouldBeDraggable)
		{
			minuteArrow.ShouldBeDraggable = shouldBeDraggable;
			hourArrow.ShouldBeDraggable = shouldBeDraggable;
		}

		public void RotateArrowByAngle(ClockArrowType arrowType, float angleDelta)
		{
			var arrowRt = arrowType switch
			{
				ClockArrowType.Second => secondArrowRT,
				ClockArrowType.Minute => minuteArrowRT,
				ClockArrowType.Hour => hourArrowRT,
				_ => throw new NotImplementedException(),
			};
			var resultRot = arrowRt.localRotation.eulerAngles.z + angleDelta;
			arrowRt.localRotation = Quaternion.Euler(0, 0, resultRot);
		}
	}
}
