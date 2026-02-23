using Arya.BabyLogger.Mobile.ViewModels.Profile;

namespace Arya.BabyLogger.Mobile.Views.Profile;

public partial class AcceptInviteCareHolderPage
{
    public AcceptInviteCareHolderPage()
    {
        InitializeComponent();

        BindingContext = App.Services.GetRequiredService<AcceptInviteCareHolderViewModel>();
    }
}
