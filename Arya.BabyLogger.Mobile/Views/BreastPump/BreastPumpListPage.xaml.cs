using Arya.BabyLogger.Mobile.ViewModels.BreastPump;

namespace Arya.BabyLogger.Mobile.Views.BreastPump;

public partial class BreastPumpListPage : ContentPage
{
	public BreastPumpListPage(BreastPumpListViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		await ((BreastPumpListViewModel)BindingContext).LoadDataAsync();
	}
}