using PlantifyApp.Core.Models;

namespace PlantifyControlPanel.Helper
{
	public interface IEmailSetting
	{
		public  Task SendEmail(Email email);

	}
}
