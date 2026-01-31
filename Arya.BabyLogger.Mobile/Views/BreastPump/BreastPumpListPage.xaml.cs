using System.Threading.Channels;
using Arya.BabyLogger.Mobile.ViewModels.BreastPump;

namespace Arya.BabyLogger.Mobile.Views.BreastPump;

public partial class BreastPumpListPage : ContentPage
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
		base.OnAppearing();

		await ((BreastPumpListViewModel)BindingContext).LoadDataAsync();

		while (!_recalculateNextPumpTimeChannel.Reader.Completion.IsCompleted)
		{
			await _recalculateNextPumpTimeChannel.Reader.WaitToReadAsync();
			await _recalculateNextPumpTimeChannel.Reader.ReadAsync();

			await ((BreastPumpListViewModel)BindingContext).LoadDataAsync();
		}
	}
}