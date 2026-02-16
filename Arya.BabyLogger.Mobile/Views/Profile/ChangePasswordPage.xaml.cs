namespace Arya.BabyLogger.Mobile.Views.Profile;

using Arya.BabyLogger.Mobile.ViewModels.Profile;

public partial class ChangePasswordPage
{
	public ChangePasswordPage()
	{
		InitializeComponent();
		BindingContext = App.Services.GetRequiredService<ChangePasswordViewModel>();
	}
}
