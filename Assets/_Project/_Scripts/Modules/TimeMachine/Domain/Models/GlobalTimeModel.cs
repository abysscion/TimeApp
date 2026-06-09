using Modules.TimeMachine.Domain.Interfaces;
using System;

namespace Modules.TimeMachine.Domain.Models
{
	public sealed class GlobalTimeModel : IGlobalTimeModel
	{
		private DateTime _lastSyncSystemDate;
		private DateTime _installedDate;

		public event Action<DateTime> LastSyncSystemDateChanged;
		public event Action<DateTime> InstalledDateChanged;

		public DateTime LastSyncSystemTime
		{
			get => _lastSyncSystemDate;
			set
			{
				_lastSyncSystemDate = value;
				LastSyncSystemDateChanged?.Invoke(_lastSyncSystemDate);
			}
		}

		public DateTime InstalledTime
		{
			get => _installedDate;
			set
			{
				_installedDate = value;
				InstalledDateChanged?.Invoke(_installedDate);
			}
		}
	}
}
