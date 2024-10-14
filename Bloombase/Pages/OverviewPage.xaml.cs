
namespace Bloombase.Pages;

public partial class OverviewPage : ContentPage
{
	private OverviewViewModel overviewModel;

	public OverviewPage()
	{
		InitializeComponent();
		BindingContext = new OverviewViewModel(context: new BloombaseContext());

	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		// Refresh the list every time the page appears
		var viewModel = BindingContext as OverviewViewModel;
		viewModel?.LoadPageCommand.Execute(null);
	}

}