using Arya.BabyLogger.Mobile.ViewModels.Landing;

namespace Arya.BabyLogger.Mobile.Views.Landing;

public partial class RegisterPage : ContentPage
{
    public RegisterPage(RegisterViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}
