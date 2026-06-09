using Cysharp.Threading.Tasks;
using Shared.Domain.Interfaces.Infrastructure;
using Shared.Domain.Models;
using System;
using System.Threading;
using UnityEngine.AddressableAssets;

namespace Shared.Usecases
{
	public class AppStartupUsecase
	{
		private const string NextSceneName = "S_Gameplay";
		private readonly IAddressablesService _addressables;
		private readonly ISceneController _sceneController;
		private readonly IAppStateMachine _stateMachine;
		private readonly LoadingModel _loadingModel;
		private readonly IAppLogger _logger;

		public AppStartupUsecase(
			IAddressablesService addressables,
			ISceneController sceneController,
			IAppStateMachine stateMachine,
			LoadingModel loadingModel,
			IAppLogger logger)
		{
			_addressables = addressables;
			_sceneController = sceneController;
			_stateMachine = stateMachine;
			_loadingModel = loadingModel;
			_logger = logger;
		}

		public async UniTask ExecuteAsync(CancellationToken ct = default)
		{
			_loadingModel.request = new LoadingRequest(LoadDependencies, AppStateType.None);
			await _stateMachine.ChangeStateAsync(AppStateType.Loading, ct);
		}

		private async UniTask LoadDependencies(CancellationToken ct = default)
		{
			try
			{
				await Addressables.InitializeAsync(true);
				await _sceneController.LoadSceneAsync(
					NextSceneName,
					activateOnLoad: false,
					onProgress: OnProgress,
					ct: ct);
				await _sceneController.ActivateSceneAsync(NextSceneName);
				await _stateMachine.ChangeStateAsync(AppStateType.Gameplay, ct);
			}
			catch (Exception e)
			{
				_logger.Exception("Unexpected error during startup", e);
				throw;
			}
		}

		//TODO: eeehh... smells bad, gotta escape this closure somehow
		//probly should rewrite to job model to merge jobs gracefully in one flow and pass it to model
		private void OnProgress(float progress)
		{
			_loadingModel.Progress = progress;
		}
	}
}
