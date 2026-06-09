using System;

namespace Modules.TimeMachine.Domain.Interfaces
{
	public interface ITimePresentationModel
	{
		DateTime? EditedTime { get; }
		bool IsClockFreezed { get; }
		bool IsEditModeEnabled { get; }

		event Action<DateTime?> EditedTimeChanged;
		event Action<bool> IsClockFreezedChanged;
		event Action<bool> IsEditModeEnabledChanged;
	}
}