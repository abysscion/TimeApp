using Cysharp.Threading.Tasks;
using Modules.TimeMachine.Configs;
using Modules.TimeMachine.Domain.DTO;
using Modules.TimeMachine.Domain.Interfaces;
using Shared.Domain.Interfaces.Infrastructure;
using System;
using System.Globalization;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

namespace Modules.TimeMachine.Services
{
	public class TimeService : ITimeService
	{
		private readonly TimeServerSettings _settings;
		private readonly IAppLogger _logger;

		public TimeService(TimeServerSettings settings, IAppLogger logger)
		{
			_settings = settings;
			_logger = logger;
		}

		public async UniTask<DateTime> GetCurrentTimeAsync(CancellationToken ct = default)
		{
			foreach (var url in _settings.TimeServerUrls)
			{
				ct.ThrowIfCancellationRequested();
				try
				{
					using var request = UnityWebRequest.Get(url);
					request.timeout = (int)_settings.TimeoutSeconds;
					var operation = await request.SendWebRequest().ToUniTask(cancellationToken: ct);

					if (request.result != UnityWebRequest.Result.Success)
					{
						_logger.Warning($"Time server {url} failed: [{request.responseCode}] {request.error}");
						continue;
					}
					var json = request.downloadHandler.text;
					var time = ParseTimeFromJson(json);
					if (time.HasValue)
					{
						_logger.Log($"Server time obtained from {url}: {time.Value}");
						return time.Value;
					}
				}
				catch (OperationCanceledException)
				{
					_logger.Log("Time request cancelled");
					throw;
				}
				catch (Exception e)
				{
					_logger.Warning($"Exception during time request from {url}: {e.Message}");
				}
			}

			_logger.Log("All time servers unreachable. Falling back to system UTC time");
			return DateTime.UtcNow;
		}

		private DateTime? ParseTimeFromJson(string json)
		{
			if (string.IsNullOrWhiteSpace(json))
				return null;

			try
			{
				var response = JsonUtility.FromJson<TimeApiResponse>(json);

				if (!string.IsNullOrEmpty(response.dateTime))
				{
					if (DateTime.TryParse(response.dateTime, null, DateTimeStyles.RoundtripKind, out var dt))
						return dt.ToUniversalTime();
				}
				if (response.time > 0)
					return DateTimeOffset.FromUnixTimeMilliseconds(response.time).UtcDateTime;
			}
			catch (Exception e)
			{
				_logger.Exception($"Failed to parse time JSON", e);
			}
			return null;
		}
	}
}
