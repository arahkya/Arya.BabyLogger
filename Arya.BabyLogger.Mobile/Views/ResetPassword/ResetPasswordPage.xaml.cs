using Arya.BabyLogger.Mobile.ViewModels.ResetPassword;

namespace Arya.BabyLogger.Mobile.Views.ResetPassword;

public partial class ResetPasswordPage : ContentPage
{
    public ResetPasswordPage(ResetPasswordViewModel viewModel)
    {
        InitializeComponent();
        
        BindingContext = viewModel;
    }
}