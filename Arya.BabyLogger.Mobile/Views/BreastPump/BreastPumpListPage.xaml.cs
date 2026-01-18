using Arya.BabyLogger.Mobile.ViewModels.BreastPump;

namespace Arya.BabyLogger.Mobile.Views.BreastPump;

public partial class BreastPumpListPage : ContentPage
{
	public BreastPumpListPage()
	{
		InitializeComponent();

		BindingContext = App.Services.GetRequiredService<BreastPumpListViewModel>();
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();

		await ((BreastPumpListViewModel)BindingContext).LoadDataAsync();
	}
}