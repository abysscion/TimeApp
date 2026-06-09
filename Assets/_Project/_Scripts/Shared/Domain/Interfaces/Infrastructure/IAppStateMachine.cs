using Cysharp.Threading.Tasks;
using Shared.Domain.Models;
using System.Threading;

namespace Shared.Domain.Interfaces.Infrastructure
{
	public interface IAppStateMachine
	{
		AppStateType CurStateType { get; }

		UniTask ChangeStateAsync(AppStateType targetState, CancellationToken ct = default);
	}
}