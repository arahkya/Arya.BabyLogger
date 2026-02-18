using System.Diagnostics;
using System.Threading.Channels;
using Arya.BabyLogger.Mobile.ViewModels.BreastPump;

namespace Arya.BabyLogger.Mobile.Views.BreastPump;

public partial class BreastPumpListPage
{
	private readonly Channel<bool> _recalculateNextPumpTimeChannel;
	
	public BreastPumpListPage(
		Channel<bool> recalculateNextPumpTimeChannel)
	{
		InitializeComponent();
		_recalculateNextPumpTimeChannel = recalculateNextPumpTimeChannel;

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