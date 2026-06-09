using System.Text;

namespace Shared.Domain.Interfaces.Views
{
	public interface ILoadingScreenView
	{
		void Show(string title, string hint);
		void SetProgressFillAmount(float progress);
		void SetProgressStringValue(string value);
		void SetProgressStringValue(StringBuilder value);
		void Hide();
	}
}
