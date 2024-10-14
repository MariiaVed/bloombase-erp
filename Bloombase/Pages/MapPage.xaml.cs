namespace Bloombase.Pages;

public partial class MapPage : ContentPage
{
	private MapPageViewModel MapPageViewModel;

	public MapPage()
	{
		InitializeComponent();

		//This is sort of a workaround to pickers not being able to run commands
		PlacePicker.SelectedIndexChanged += OnPlacePickerSelectedIndexChanged;

		MapPageViewModel = new MapPageViewModel(context: new BloombaseContext());
		BindingContext = MapPageViewModel;
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		MapPageViewModel = new MapPageViewModel(context: new BloombaseContext());
		BindingContext = MapPageViewModel;
	}

	private void OnPlacePickerSelectedIndexChanged(object sender, EventArgs e)
	{
		if (PlacePicker.SelectedItem != null)
		{
			var selectedPlace = (Place)PlacePicker.SelectedItem;

			MapPageViewModel.OnPlaceSelectedCommand.Execute(selectedPlace);
		}
	}

	private void OnAllPlantsItemSelected(object sender, EventArgs e)
	{
		if (AllPlantsList.SelectedItem != null)
		{
			var selectedPlant = (Plant)AllPlantsList.SelectedItem;

			MapPageViewModel.OnPlantSelectedCommand.Execute(selectedPlant);
		}
	}

	private void OnFlowerbedSelected(object sender, EventArgs e)
	{
		if (FlowerbedList.SelectedItem != null)
		{
			var selectedFlowerbed = (Flowerbed)FlowerbedList.SelectedItem;

			MapPageViewModel.OnFlowerbedSelectedCommand.Execute(selectedFlowerbed);
		}
	}

	private void OnPlantInFlowerbedDetailsSelected(object sender, EventArgs e)
	{
		if (PlantInFlowerbedDetailsList.SelectedItem != null)
		{
			var selectedPlantInFlowerbedDetails = (PlantInFlowerbedDetails)PlantInFlowerbedDetailsList.SelectedItem;

			MapPageViewModel.OnPlantInFlowerbedDetailsSelectedCommand.Execute(selectedPlantInFlowerbedDetails);
		}
	}
}