namespace Arya.BabyLogger.Mobile.Views;

public partial class Feeding : ContentPage
{
	public Feeding()
	{
		InitializeComponent();
	}

	private void CancelButton_Clicked(object sender, EventArgs e)
	{
		Navigation.PopModalAsync();
	}
}