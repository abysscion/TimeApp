using Modules.TimeMachine.Domain.Interfaces;
using System;

namespace Modules.TimeMachine.Domain.Models
{
	public sealed class TimePresentationModel : ITimePresentationModel
	{
		private DateTime? _editedTime;
		private bool _isEditModeEnabled;
		private bool _isClockFreezed;

		public event Action<DateTime?> EditedTimeChanged;
		public event Action<bool> IsEditModeEnabledChanged;
		public event Action<bool> IsClockFreezedChanged;

		public DateTime? EditedTime
		{
			get => _editedTime;
			set
			{
				_editedTime = value;
				EditedTimeChanged?.Invoke(_editedTime);
			}
		}

		public bool IsEditModeEnabled
		{
			get => _isEditModeEnabled;
			set
			{
				_isEditModeEnabled = value;
				IsEditModeEnabledChanged?.Invoke(_isEditModeEnabled);
			}
		}

		public bool IsClockFreezed
		{
			get => _isClockFreezed;
			set
			{
				_isClockFreezed = value;
				IsClockFreezedChanged?.Invoke(_isClockFreezed);
			}
		}
	}
}
