using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine.SceneManagement;

namespace Shared.Domain.Interfaces.Infrastructure
{
	public interface ISceneController
	{
		event Action<string> LoadFailed;
		event Action<string> SceneUnloaded;
		event Action<Scene> SceneActivated;
		event Action<Scene> SceneLoaded;
		event Action<float> ProgressChanged;

		Scene CurrentScene { get; }
		float LoadProgress { get; }
		bool IsLoading { get; }

		UniTask<Scene> LoadSceneAsync(
			string sceneName,
			LoadSceneMode mode = LoadSceneMode.Single,
			bool activateOnLoad = true,
			Action<float> onProgress = null,
			CancellationToken ct = default);
		UniTask<Scene> ActivateSceneAsync(string sceneName, CancellationToken ct = default);
		UniTask UnloadSceneAsync(string sceneName);
	}
}