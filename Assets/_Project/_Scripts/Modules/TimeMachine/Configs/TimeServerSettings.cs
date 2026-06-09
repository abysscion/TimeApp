using UnityEngine;

namespace Modules.TimeMachine.Configs
{
	//[CreateAssetMenu(fileName = "TimeServerSettings", menuName = "Configs/Time Server Settings")]
	public class TimeServerSettings : ScriptableObject
	{
		[SerializeField] private float _timeoutSeconds = 3f;
		[SerializeField] private string[] _timeServerUrls = new[]
		{
			"https://worldtimeapi.org/api/timezone/Etc/UTC",
			"https://timeapi.io/api/Time/current/zone?timeZone=UTC",
			"http://worldclockapi.com/api/json/utc/now" // запасной вариант с HTTP
		};

		public string[] TimeServerUrls => _timeServerUrls;
		public float TimeoutSeconds => _timeoutSeconds;
	}
}
