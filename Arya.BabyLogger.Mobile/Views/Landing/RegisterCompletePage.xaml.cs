namespace Arya.BabyLogger.Mobile.Views.Landing;

public partial class RegisterCompletePage : ContentPage
{
    public RegisterCompletePage()
    {
        InitializeComponent();
    }

    private void OnContinueClicked(object sender, EventArgs e)
    {
        App.SwapGreetingPage().GetAwaiter().GetResult();
    }
}
