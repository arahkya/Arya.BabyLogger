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
		
			await ((BreastPumpListViewModel)BindingContext).LoadDataAsync();

			while (!_recalculateNextPumpTimeChannel.Reader.Completion.IsCompleted)
			{
				await _recalculateNextPumpTimeChannel.Reader.WaitToReadAsync();
				await _recalculateNextPumpTimeChannel.Reader.ReadAsync();

				await ((BreastPumpListViewModel)BindingContext).LoadDataAsync();
			}
		}
		catch (Exception ex)
		{
			Debug.WriteLine(ex);
		}
	}
}