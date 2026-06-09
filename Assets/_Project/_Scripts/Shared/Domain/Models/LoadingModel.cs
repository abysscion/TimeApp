using System;

namespace Shared.Domain.Models
{
	public sealed class LoadingModel
	{
		private float _progress;
		private bool _isActive;

		public LoadingRequest request;

		public event Action<float> ProgressChanged;
		public event Action<bool> IsActiveChanged;

		public float Progress
		{
			get => _progress;
			set
			{
				_progress = value;
				ProgressChanged?.Invoke(_progress);
			}
		}

		public bool IsActive
		{
			get => _isActive;
			set
			{
				_isActive = value;
				IsActiveChanged?.Invoke(_isActive);
			}
		}
	}
}