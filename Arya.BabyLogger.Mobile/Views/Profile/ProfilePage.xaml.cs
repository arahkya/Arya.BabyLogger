using Arya.BabyLogger.Mobile.ViewModels.Profile;

namespace Arya.BabyLogger.Mobile.Views.Profile;

public partial class ProfilePage
{
	public ProfilePage(ProfileViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		
		if (BindingContext is ProfileViewModel vm)
		{
			vm.LoadCurrentUsername();
		}
	}
}
