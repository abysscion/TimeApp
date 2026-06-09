using Modules.TimeMachine.UI.Views;
using System;

namespace Modules.TimeMachine.UI.Interfaces
{
	public interface IAnalogClockView
	{
		event Action<float> MinuteArrowDragged;
		event Action<float> HourArrowDragged;

		void ToggleArrowsDraggability(bool shouldBeDraggable);
		void RotateArrowByAngle(ClockArrowType arrowType, float angleDelta);
		void GetArrowsAngles(out float hourAngle, out float minuteAngle, out float secondAngle);
		void SetArrowsAngles(float hourAngle, float minuteAngle, float secondAngle);
	}
}
