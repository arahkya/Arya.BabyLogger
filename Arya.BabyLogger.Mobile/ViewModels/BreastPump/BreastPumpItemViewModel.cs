using Arya.BabyLogger.Mobile.Views.BreastPump;
using CommunityToolkit.Mvvm.Input;

namespace Arya.BabyLogger.Mobile.ViewModels.BreastPump;

public partial class BreastPumpItemViewModel
{
    public Guid Id { get; set; }
    public DateTimeOffset PumpTime { get; set; }
    public string PumpTimeString => PumpTime.ToString("HH:mm");
    public double AmountInMl { get; set; }

    [RelayCommand]
    public async Task ItemSelectedAsync()
    {
        var viewModel = App.Services.GetRequiredService<BreastPumpEntryViewModel>();
        viewModel.ListItemId = Id;
        await viewModel.LoadDataAsync();

        var page = new BreastPumpEntryPage(viewModel);

        await Shell.Current.Navigation.PushModalAsync(page);
    }
}