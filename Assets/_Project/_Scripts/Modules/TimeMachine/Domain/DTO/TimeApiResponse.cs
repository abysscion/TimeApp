using System;

namespace Modules.TimeMachine.Domain.DTO
{
	[Serializable]
	public struct TimeApiResponse
	{
		public long time; //yandex
		public string dateTime; //timeapi
	}
}
