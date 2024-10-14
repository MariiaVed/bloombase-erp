namespace Bloombase;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
		BindingContext = new MainPageViewModel(context: new BloombaseContext());
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		// Refresh the list every time the page appears
		var viewModel = BindingContext as MainPageViewModel;
		viewModel?.LoadPageCommand.Execute(null);
	}
}

