using System.Diagnostics;
using Arya.BabyLogger.Mobile.ViewModels.BreastPump;

namespace Arya.BabyLogger.Mobile.Views.BreastPump;

public partial class BreastPumpListPage
{
	public BreastPumpListPage()
	{
		InitializeComponent();

		BindingContext = App.Services.GetRequiredService<BreastPumpListViewModel>();
	}

	protected override async void OnAppearing()
	{
		try
		{
			base.OnAppearing();

			if (BindingContext is BreastPumpListViewModel viewModel)
			{
				await viewModel.LoadDataAsync();
			}
		}
		catch (Exception ex)
		{
			Debug.WriteLine(ex);
		}
	}
}