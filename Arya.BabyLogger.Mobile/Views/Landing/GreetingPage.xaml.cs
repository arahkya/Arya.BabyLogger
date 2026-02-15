using System.Diagnostics;
using Arya.BabyLogger.Mobile.ViewModels.Landing;

namespace Arya.BabyLogger.Mobile.Views.Landing;

public partial class GreetingPage : ContentPage
{
    public GreetingPage(GreetingViewModel viewModel)
    {
        InitializeComponent();
        
        BindingContext = viewModel;
    }

    private void TapGestureRecognizer_OnTapped(object? sender, TappedEventArgs e)
    {
        if (BindingContext is GreetingViewModel vm)
        {
            vm.GotoForgotPasswordPageCommand.ExecuteAsync(null);
        }
    }
}
