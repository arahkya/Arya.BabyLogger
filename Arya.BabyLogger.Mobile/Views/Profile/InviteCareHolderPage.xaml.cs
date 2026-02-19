using System.Diagnostics;
using Arya.BabyLogger.Mobile.ViewModels.Profile;

namespace Arya.BabyLogger.Mobile.Views.Profile;

public partial class InviteCareHolderPage
{
    public InviteCareHolderPage()
    {
        InitializeComponent();
        BindingContext = App.Services.GetRequiredService<InviteCareHolderViewModel>();
    }

    protected override void OnAppearing()
    {
        try
        {
            base.OnAppearing();

            if (BindingContext is InviteCareHolderViewModel viewModel)
            {
                viewModel.RequestInviteCodeCommand.ExecuteAsync(null);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }
}