using System;
using System.Threading;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Shared.Domain.Interfaces.Infrastructure
{
	public interface IAddressablesService
	{
		AsyncOperationHandle<T> LoadAssetAsync<T>(
			object key,
			Action<float> onProgress = null,
			CancellationToken ct = default);
		AsyncOperationHandle<SceneInstance> LoadSceneAsync(
			object key,
			LoadSceneMode mode = LoadSceneMode.Additive,
			bool activateOnLoad = false,
			Action<float> onProgress = null,
			CancellationToken ct = default);
		void Release(AsyncOperationHandle handle);
		void UnloadScene(AsyncOperationHandle<SceneInstance> handle);
	}
}