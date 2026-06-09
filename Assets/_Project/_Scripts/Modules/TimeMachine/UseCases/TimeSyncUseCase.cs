using Cysharp.Threading.Tasks;
using Modules.TimeMachine.Domain.Interfaces;
using Modules.TimeMachine.Domain.Models;
using Shared.Domain.Interfaces.Infrastructure;
using System;
using Zenject;

namespace Modules.TimeMachine.UseCases
{
	public class TimeSyncUseCase : IInitializable, IDisposable
	{
		private readonly GlobalTimeModel _globalTimeModel;
		private readonly ITimeService _timeService;
		private readonly IAppLogger _logger;

		public TimeSyncUseCase(GlobalTimeModel globalTimeModel, ITimeService timeService, IAppLogger logger)
		{
			_globalTimeModel = globalTimeModel;
			_timeService = timeService;
			_logger = logger;
		}

		public void Initialize()
		{
			SyncTimeAsync().Forget();
		}

		public async UniTaskVoid SyncTimeAsync()
		{
			try
			{
				var timeToInstall = await _timeService.GetCurrentTimeAsync();
				_globalTimeModel.LastSyncSystemTime = DateTime.Now;
				_globalTimeModel.InstalledTime = timeToInstall;
			}
			catch (Exception e)
			{
				_logger.Exception($"Time sync failed", e);
			}
		}

		public void Dispose() { }
	}
}