using System;

namespace Modules.TimeMachine.Domain.Interfaces
{
	public interface IGlobalTimeModel
	{
		DateTime InstalledTime { get; }
		DateTime LastSyncSystemTime { get; }

		event Action<DateTime> InstalledDateChanged;
		event Action<DateTime> LastSyncSystemDateChanged;
	}
}