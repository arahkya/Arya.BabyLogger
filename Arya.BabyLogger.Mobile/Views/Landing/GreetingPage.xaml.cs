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
}
