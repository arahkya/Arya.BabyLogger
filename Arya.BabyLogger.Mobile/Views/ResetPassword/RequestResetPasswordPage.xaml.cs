using Arya.BabyLogger.Mobile.ViewModels.ResetPassword;

namespace Arya.BabyLogger.Mobile.Views.ResetPassword;

public partial class RequestResetPasswordPage : ContentPage
{
    public RequestResetPasswordPage(RequestResetPasswordViewModel viewModel)
    {
        InitializeComponent();
        
        BindingContext = viewModel;
    }
}