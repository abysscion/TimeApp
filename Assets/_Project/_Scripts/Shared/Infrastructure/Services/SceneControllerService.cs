using Cysharp.Threading.Tasks;
using Shared.Domain.Interfaces.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Shared.Infrastructure.Services
{
	public class SceneControllerService : ISceneController, IDisposable
	{
		private readonly Dictionary<string, AsyncOperationHandle<SceneInstance>> _sceneNameToLoadHandleDict = new();
		private readonly IAddressablesService _addressables;
		private readonly IAppLogger _logger;

		private AsyncOperationHandle<SceneInstance> _loadHandle;
		private CancellationTokenSource _loadCts;
		private string _pendingSceneName;
		private float _loadProgress;

		public event Action<string> LoadFailed;
		public event Action<string> SceneUnloaded;
		public event Action<Scene> SceneActivated;
		public event Action<Scene> SceneLoaded;
		public event Action<float> ProgressChanged;

		public SceneControllerService(IAddressablesService addressables, IAppLogger logger)
		{
			_addressables = addressables;
			_logger = logger;
		}

		public Scene CurrentScene { get; private set; }
		public bool IsLoading { get; private set; }
		public float LoadProgress
		{
			get => _loadProgress;
			private set
			{
				_loadProgress = value;
				ProgressChanged?.Invoke(value);
			}
		}

		public async UniTask<Scene> LoadSceneAsync(
			string sceneName,
			LoadSceneMode mode = LoadSceneMode.Single,
			bool activateOnLoad = true,
			Action<float> onProgress = null,
			CancellationToken ct = default)
		{
			if (string.IsNullOrWhiteSpace(sceneName))
				throw new ArgumentException($"Unknown scene: [{sceneName}]");
			if (IsLoading)
			{
				_logger.Warning($"Loading call for [{sceneName}] cancelled [{_pendingSceneName}] loading");
				CancelCurrentLoad();
				await UniTask.Yield();
			}
			LoadProgress = 0f;
			IsLoading = true;
			_pendingSceneName = sceneName;
			_loadCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
			try
			{
				_logger.Log($"Loading scene [{sceneName}]...");
				_loadHandle = _addressables.LoadSceneAsync(
					sceneName,
					mode,
					activateOnLoad,
					progress => { onProgress?.Invoke(progress); LoadProgress = progress; },
					_loadCts.Token);
				await _loadHandle.Task;
				_loadCts.Token.ThrowIfCancellationRequested();
				if (_loadHandle.Status != AsyncOperationStatus.Succeeded)
				{
					var errStr = _loadHandle.OperationException?.Message ?? "Unknown error";
					LoadFailed?.Invoke(errStr);
					throw new InvalidOperationException($"Failed to load scene [{sceneName}]: {errStr}");
				}
				_sceneNameToLoadHandleDict[sceneName] = _loadHandle;
				var sceneInstance = _loadHandle.Result;
				CurrentScene = sceneInstance.Scene;
				_logger.Log($"Scene loaded [{CurrentScene.name}].");
				SceneLoaded?.Invoke(CurrentScene);
				if (activateOnLoad)
					await ActivateSceneAsync(sceneName, ct);
				return CurrentScene;
			}
			catch (OperationCanceledException)
			{
				_logger.Warning($"Scene load cancelled: [{sceneName}]");
				LoadFailed?.Invoke("Cancelled");
				ReleaseHandle();
				throw;
			}
			catch (Exception e)
			{
				_logger.Exception($"Unexpected error", e);
				LoadFailed?.Invoke(e.Message);
				ReleaseHandle();
				throw;
			}
			finally
			{
				IsLoading = false;
				_loadCts?.Dispose();
				_loadCts = null;
			}
		}

		public async UniTask<Scene> ActivateSceneAsync(string sceneName, CancellationToken ct = default)
		{
			if (!_sceneNameToLoadHandleDict.TryGetValue(sceneName, out var handle) || !handle.IsValid())
				throw new InvalidOperationException($"Scene [{sceneName}] handle not found.");

			if (handle.Result.Scene.isLoaded)
			{
				_logger.Log($"Scene [{sceneName}] is already loaded and activated.");
				return default;
			}

			await handle.Result.ActivateAsync();
			_logger.Log($"Scene [{sceneName}] activated.");
			SceneActivated?.Invoke(handle.Result.Scene);
			return handle.Result.Scene;
		}

		public async UniTask UnloadSceneAsync(string sceneName)
		{
			if (string.IsNullOrWhiteSpace(sceneName))
				throw new ArgumentException($"Unknown scene: [{sceneName}]");
			if (!_sceneNameToLoadHandleDict.TryGetValue(sceneName, out var handle) || !handle.IsValid())
			{
				_logger.Warning($"Can't find [{sceneName}] among loaded scenes.");
				return;
			}
			try
			{
				if (handle.Result.Scene.isLoaded)
				{
					var op = Addressables.UnloadSceneAsync(handle, true);
					await op.Task;
					_logger.Log($"Scene unloaded: [{sceneName}]");
					SceneUnloaded?.Invoke(sceneName);
				}
				_addressables.Release(handle);
			}
			catch (Exception e)
			{
				_logger.Exception($"Error unloading scene [{sceneName}]", e);
			}
			finally
			{
				_sceneNameToLoadHandleDict.Remove(sceneName);
				if (CurrentScene.IsValid() && CurrentScene.name == sceneName)
					CurrentScene = default;
			}
		}

		public void Dispose()
		{
			CancelCurrentLoad();
		}

		private void CancelCurrentLoad()
		{
			_loadCts?.Cancel();
			_loadCts?.Dispose();
			_loadCts = null;
			ReleaseHandle();
			IsLoading = false;
		}

		private void ReleaseHandle()
		{
			if (_loadHandle.IsValid())
			{
				_addressables.Release(_loadHandle);
				_loadHandle = default;
			}
		}
	}
}