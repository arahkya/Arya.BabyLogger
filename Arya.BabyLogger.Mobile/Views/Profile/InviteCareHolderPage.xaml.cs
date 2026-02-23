using Arya.BabyLogger.Mobile.ViewModels.Profile;

namespace Arya.BabyLogger.Mobile.Views.Profile;

public partial class InviteCareHolderPage
{
    public InviteCareHolderPage()
    {
        InitializeComponent();
        BindingContext = App.Services.GetRequiredService<InviteCareHolderViewModel>();
    }

    private void EmailEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is not Entry entry)
            return;

        var lower = entry.Text.ToLowerInvariant();
        if (lower == entry.Text) return;
        
        entry.Text = lower;
    }
}
 