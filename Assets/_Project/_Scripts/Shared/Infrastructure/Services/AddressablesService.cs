using Cysharp.Threading.Tasks;
using Shared.Domain.Interfaces.Infrastructure;
using System;
using System.Threading;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Shared.Infrastructure.Services
{
	public class AddressablesService : IAddressablesService
	{
		private readonly IAppLogger _logger;

		public AddressablesService(IAppLogger logger)
		{
			_logger = logger;
		}

		public AsyncOperationHandle<T> LoadAssetAsync<T>(
			object key,
			Action<float> onProgress = null,
			CancellationToken ct = default)
		{
			var handle = Addressables.LoadAssetAsync<T>(key);
			ProcessLoading(handle, onProgress, ct);
			return handle;
		}

		public AsyncOperationHandle<SceneInstance> LoadSceneAsync(
			object key,
			LoadSceneMode mode = LoadSceneMode.Additive,
			bool activateOnLoad = false,
			Action<float> onProgress = null,
			CancellationToken ct = default)
		{
			var handle = Addressables.LoadSceneAsync(key, mode, activateOnLoad);
			ProcessLoading(handle, onProgress, ct);
			return handle;
		}

		public void Release(AsyncOperationHandle handle)
		{
			if (handle.IsValid())
			{
				Addressables.Release(handle);
				_logger.Log($"Released handle for {handle.DebugName}");
			}
		}

		public void UnloadScene(AsyncOperationHandle<SceneInstance> handle)
		{
			if (handle.IsValid() && handle.Result.Scene.isLoaded)
			{
				Addressables.UnloadSceneAsync(handle, true).Completed += _ =>
				{
					_logger.Log($"Scene resource unloaded: {handle.Result.Scene.name}");
				};
			}
			else
			{
				Release(handle);
			}
		}

		private async void ProcessLoading(
			AsyncOperationHandle handle,
			Action<float> onProgress,
			CancellationToken ct)
		{
			if (onProgress == null && ct == default)
				return;

			try
			{
				while (!handle.IsDone)
				{
					if (ct.IsCancellationRequested)
					{
						Addressables.Release(handle);
						_logger.Log("Operation cancelled");
						return;
					}
					onProgress?.Invoke(handle.PercentComplete);
					await UniTask.Yield(ct);
				}

				if (handle.Status == AsyncOperationStatus.Succeeded)
				{
					onProgress?.Invoke(1f);
					_logger.Log($"Resource loaded: {handle.DebugName}");
				}
				else
				{
					_logger.Error($"Resource failed to load: {handle.DebugName} | Error: {handle.OperationException}");
				}
			}
			catch (Exception e)
			{
				_logger.Exception($"Exception during resource loading", e);
			}
		}
	}
}