using Arya.BabyLogger.Mobile.ViewModels.BreastPump;

namespace Arya.BabyLogger.Mobile.Views.BreastPump;

public partial class BreastPumpEntryPage : ContentPage
{
	public BreastPumpEntryPage(BreastPumpEntryViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		if (BindingContext is BreastPumpEntryViewModel viewModel)
		{
			await viewModel.LoadDataAsync();
		}
	}
}