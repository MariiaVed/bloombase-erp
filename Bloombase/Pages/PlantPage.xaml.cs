namespace Bloombase.Pages;

public partial class PlantPage : ContentPage
{
	public PlantPage()
	{
		InitializeComponent();

		BindingContext = new PlantViewModel(context: new BloombaseContext());
	}

	private async void OnSelectImageClicked(object sender, EventArgs e)
	{
		try
		{
			string[] dirSearch = AppDomain.CurrentDomain.BaseDirectory.Split("Bloombase");
			string resourceDir = dirSearch[0] + "Bloombase\\Resources\\Images";
			var result = await FilePicker.PickAsync(new PickOptions
			{
				PickerTitle = "Pick an Image"
			});

			if (result != null)
			{
				// Read the image stream
				using (var sourceStream = await result.OpenReadAsync())
				{
					// Construct the destination file path
					var destinationPath = Path.Combine(resourceDir, result.FileName);

					// Write the image stream to the destination file
					using (var destinationStream = File.Create(destinationPath))
					{
						await sourceStream.CopyToAsync(destinationStream);
					}

					// Display a success message
					await DisplayAlert("Success", "Image added successfully!", "OK");
				}
			}
		}
		catch (Exception ex)
		{
			// Handle any errors that occur during the process
			await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
		}
	}
	protected override void OnAppearing()
	{
		base.OnAppearing();
		// Refresh the list every time the page appears
		var viewModel = BindingContext as PlantViewModel;
		viewModel?.LoadPageCommand.Execute(null);
	}
}