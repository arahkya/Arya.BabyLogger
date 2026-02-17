using Arya.BabyLogger.Mobile.ViewModels.BreastPump;

namespace Arya.BabyLogger.Mobile.Views.BreastPump;

public partial class BreastPumpSettingsPage : ContentPage
{
	public BreastPumpSettingsPage()
	{
		InitializeComponent();
		BindingContext = App.Services.GetRequiredService<BreastPumpSettingsViewModel>();
	}

	protected override void OnDisappearing()
	{
		base.OnDisappearing();

		if (BindingContext is BreastPumpSettingsViewModel vm && vm.SaveSettingsCommand.CanExecute(null))
		{
			vm.SaveSettingsCommand.Execute(null);
		}
	}
}
