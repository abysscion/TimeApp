using Shared.Domain.Interfaces.Views;
using Shared.Domain.Models;
using System;
using System.Text;
using UnityEngine;

namespace Shared.UI.Presenters
{
	public class LoadingScreenPresenter : IDisposable
	{
		private const string LoadingTitle = "Time App";
		private const string LoadingHint = "Hint: there are no other hints";
		private readonly ILoadingScreenView _view;
		private readonly LoadingModel _loadingModel;
		private readonly StringBuilder _sb = new(4);

		public LoadingScreenPresenter(ILoadingScreenView view, LoadingModel loadingModel)
		{
			_view = view;
			_loadingModel = loadingModel;
			_loadingModel.IsActiveChanged += OnIsActiveChanged;
			_loadingModel.ProgressChanged += OnProgressChanged;
		}

		private void OnProgressChanged(float progress)
		{
			_view.SetProgressFillAmount(Mathf.Clamp01(progress));
			_sb.Clear();
			_sb.Append(Mathf.RoundToInt(progress * 100f));
			_sb.Append("%");
			_view.SetProgressStringValue(_sb);
		}

		private void OnIsActiveChanged(bool isActive)
		{
			if (isActive)
				_view.Show(LoadingTitle, LoadingHint);
			else
				_view.Hide();
		}

		public void Dispose()
		{
			_loadingModel.IsActiveChanged -= OnIsActiveChanged;
			_loadingModel.ProgressChanged -= OnProgressChanged;
		}
	}
}
