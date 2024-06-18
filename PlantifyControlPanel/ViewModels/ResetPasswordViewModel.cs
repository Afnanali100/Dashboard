using System.ComponentModel.DataAnnotations;

namespace PlantifyControlPanel.ViewModels
{
	public class ResetPasswordViewModel
	{
		[DataType(DataType.Password)]
		[Required(ErrorMessage = "Password Is Required")]
		public string NewPassword { get; set; }
	}
}
