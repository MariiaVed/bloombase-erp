using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Bloombase;

//This is the viewmodel for the Planting Flowerbeds page
public class MapPageViewModel : INotifyPropertyChanged
{
    private readonly BloombaseContext _context;
    private ErrorHandler _errorHandler = new ErrorHandler();
    public ObservableCollection<Place> Places { get; set; }
    public ObservableCollection<Flowerbed> Flowerbeds { get; set; }
    public ObservableCollection<Flowerbed> FlowerbedsInPlace { get; set; }
    public ObservableCollection<Climate> Climates { get; set; }
    public ObservableCollection<Plant> Plants { get; set; }
    public ObservableCollection<PlantInFlowerbed> PlantsInFlowerbed { get; set; }
    //Note this is only for displaying the correct attributes in the gui
    public ObservableCollection<PlantInFlowerbedDetails> PlantInFlowerbedDetails { get; set; }
    public event PropertyChangedEventHandler? PropertyChanged;
    private static PlantInFlowerbedDetails SelectedPlantInFlowerbedDetailsObject { get; set; }
    private ICommand _onPlaceSelectedCommand;
    public ICommand OnPlaceSelectedCommand => _onPlaceSelectedCommand ??= new Command<object>(OnPlaceSelected);
    private ICommand _onFlowerbedSelectedCommand;
    public ICommand OnFlowerbedSelectedCommand => _onFlowerbedSelectedCommand ??= new Command<object>(OnFlowerbedSelected);
    public ICommand OnPlantSelectedCommand { get; private set; }
    public ICommand OnPlantInFlowerbedDetailsSelectedCommand { get; private set; }
    public ICommand AddPlantInFlowerbedCommand { get; private set; }
    public ICommand DeletePlantInFlowerbedCommand { get; private set; }
    public ICommand UpdatePlantInFlowerbedCommand { get; private set; }

    PlaceDAO placeDAO;
    FlowerbedDAO flowerbedDAO;
    ClimateDAO climateDAO;
    PlantDAO plantDAO;
    PlantInFlowerbedDAO plantInFlowerbedDAO;
    EmployeeDAO employeeDAO;

    public MapPageViewModel(BloombaseContext context)
    {
        _context = context;

        PlaceDAO placeDAO = new(_context);
        FlowerbedDAO flowerbedDAO = new(_context);
        ClimateDAO climateDAO = new(_context);
        PlantDAO plantDAO = new(_context);
        PlantInFlowerbedDAO plantInFlowerbedDAO = new(_context);
        EmployeeDAO employeeDAO = new(_context);

        Place = new Place();
        Flowerbed = new Flowerbed();
        Plant = new Plant();
        PlantInFlowerbed = new PlantInFlowerbed();

        Places = new ObservableCollection<Place>(placeDAO.GetAllPlaces());
        Flowerbeds = new ObservableCollection<Flowerbed>(flowerbedDAO.GetAllFlowerbeds());
        Climates = new ObservableCollection<Climate>(climateDAO.GetAllClimates());
        Plants = new ObservableCollection<Plant>(plantDAO.GetAllPlants());
        PlantsInFlowerbed = new ObservableCollection<PlantInFlowerbed>(plantInFlowerbedDAO.GetAllPlantsInFlowerbed(Flowerbed.FlowerbedId));


        FlowerbedsInPlace = new ObservableCollection<Flowerbed>();
        PlantInFlowerbedDetails = new ObservableCollection<PlantInFlowerbedDetails>();


        OnPlantInFlowerbedDetailsSelectedCommand = new Command(OnPlantInFlowerbedDetailsSeletected);
        OnPlantSelectedCommand = new Command(OnPlantSelected);
        AddPlantInFlowerbedCommand = new Command(AddPlantInFlowerbed);
        DeletePlantInFlowerbedCommand = new Command(DeletePlantInFlowerbed);
        UpdatePlantInFlowerbedCommand = new Command(UpdatePlantInFlowerbed);
    }

    private void AddPlantInFlowerbed()
    {
        PlantInFlowerbedDAO plantInFlowerbedDAO = new(_context);

		PlantInFlowerbed = new PlantInFlowerbed()
		{
			PlaceId = Place.PlaceId,
			PlantId = Plant.PlantId,
			FlowerbedId = Flowerbed.FlowerbedId,
			Quantity = PlantInFlowerbed.Quantity
		};

        if (PlantInFlowerbed.Quantity > 0 && PlantInFlowerbed.PlaceId != null && PlantInFlowerbed.FlowerbedId != null
        && PlantInFlowerbed.PlantId != null)
        {
            if (PlantsInFlowerbed.Any(x => x.PlantId == PlantInFlowerbed.PlantId))
            {
                _errorHandler.ShowErrorMessage("Plant already exists. Update quantity instead.");
                return;
            }
            //you can technically add things to the wrong climate, this prevents that now
            else if (Plant.ClimateId != Flowerbed.ClimateId)
            {
                _errorHandler.ShowErrorMessage("Wrong climate. Can't add plant.");
                return;
            }

            int availableQuantity = Flowerbed.Size;

            foreach (var plantInFlowerbed in PlantsInFlowerbed)
            {
                availableQuantity -= plantInFlowerbed.Quantity;
            }

            if (PlantInFlowerbed.Quantity <= availableQuantity)
            {
                plantInFlowerbedDAO.AddPlantInFlowerbed(PlantInFlowerbed);
                ReloadPlantInFlowerbedDetails();
            }
            else
            {
                _errorHandler.ShowErrorMessage("Flowerbed capacity exceeded. Can't add plant.");
            }
        }
        else
        {
            if (PlantInFlowerbed.Quantity <= 0)
            {
                _errorHandler.ShowErrorMessage("Amount of plants to add must be greater than zero.");
            }
            else
            {
                _errorHandler.ShowErrorMessage("Please select a flowerbed and plant to add.");
            }
        }

        ReloadData();
    }

    private void DeletePlantInFlowerbed()
    {
        PlantInFlowerbedDAO plantInFlowerbedDAO = new(_context);

        if (SelectedPlantInFlowerbedDetailsObject != null)
        {
            plantInFlowerbedDAO.DeletePlantInFlowerbed(PlantInFlowerbed);
            SelectedPlantInFlowerbedDetailsObject = null;
        }
        else
        {
            _errorHandler.ShowErrorMessage("Please select a plant in a flowerbed to delete.");
        }

        ReloadData();
        ReloadPlantInFlowerbedDetails();
    }

    private void UpdatePlantInFlowerbed()
    {
        PlantInFlowerbedDAO plantInFlowerbedDAO = new(_context);

        if (SelectedPlantInFlowerbedDetailsObject != null)
        {
            if (PlantInFlowerbed.Quantity <= 0)
            {
                _errorHandler.ShowErrorMessage("Update number must be greater than zero.");
                return;
            }

            int availableQuantity = Flowerbed.Size;

            foreach (var plantInFlowerbed in PlantsInFlowerbed)
            {
                //Skip plant that's being updated
                if (plantInFlowerbed.PlantId == PlantInFlowerbed.PlantId)
                {
                    continue;
                }
                else
                {
                    availableQuantity -= plantInFlowerbed.Quantity;
                }
            }

            if (PlantInFlowerbed.Quantity <= availableQuantity)
            {
                plantInFlowerbedDAO.UpdatePlantInFlowerbed(PlantInFlowerbed);
                ReloadPlantInFlowerbedDetails();
            }
            else
            {
                _errorHandler.ShowErrorMessage("Flowerbed capacity exceeded. Can't add plant.");
            }
        }
        else
        {
            _errorHandler.ShowErrorMessage("Please select a plant in a flowerbed to update.");
        }
    }

    private Place _place;

    public Place Place
    {
        get => _place;
        set
        {
            _place = value;
            OnPropertyChanged(nameof(Place));
        }
    }

    private Flowerbed _flowerbed;

    public Flowerbed Flowerbed
    {
        get => _flowerbed;
        set
        {
            _flowerbed = value;
            OnPropertyChanged(nameof(Flowerbed));
        }
    }

    private Plant _plant;

    public Plant Plant
    {
        get => _plant;
        set
        {
            _plant = value;
            OnPropertyChanged(nameof(Plant));
        }
    }

    private PlantInFlowerbed _plantInFlowerbed;

    public PlantInFlowerbed PlantInFlowerbed
    {
        get => _plantInFlowerbed;
        set
        {
            _plantInFlowerbed = value;
            OnPropertyChanged(nameof(PlantInFlowerbed));
        }
    }

    //Reload all data
    public void ReloadData()
    {
        PlaceDAO placeDAO = new(_context);
        FlowerbedDAO flowerbedDAO = new(_context);
        PlantInFlowerbedDAO plantInFlowerbedDAO = new(_context);

        var places = placeDAO.GetAllPlaces();
        var flowerbeds = flowerbedDAO.GetAllFlowerbeds();
        var plantsInFlowerbed = plantInFlowerbedDAO.GetAllPlantsInFlowerbed(Flowerbed.FlowerbedId);

        Places.Clear();
        Flowerbeds.Clear();
        PlantsInFlowerbed.Clear();

        foreach (var place in places)
        {
            Places.Add(place);
        }
        foreach (var flowerbed in flowerbeds)
        {
            Flowerbeds.Add(flowerbed);
        }
        foreach (var plantInFlowerbed in plantsInFlowerbed)
        {
            PlantsInFlowerbed.Add(plantInFlowerbed);
        }
    }

    public void OnPlantSelected(object selectedPlant)
    {
        Plant = (Plant)selectedPlant;
        SelectedPlantInFlowerbedDetailsObject = null;

        //We have to do this because else the quantity is still bound to the PlantInFlowerbed
    }

    public void OnFlowerbedSelected(object selectedFlowerbed)
    {
        plantDAO = new(_context);
        SelectedPlantInFlowerbedDetailsObject = null;
        Flowerbed = (Flowerbed)selectedFlowerbed;

        Plants.Clear();
        foreach (var plant in plantDAO.GetPlantsByClimate(Flowerbed.ClimateId))
        {
            Plants.Add(plant);
        }
        ReloadPlantInFlowerbedDetails();
    }

    public void OnPlaceSelected(object selectedPlace)
    {
        Place = (Place)selectedPlace;
        SelectedPlantInFlowerbedDetailsObject = null;
        FlowerbedDAO flowerbedDAO = new(_context);

		//Reset the plant in flowerbed

        FlowerbedsInPlace.Clear();

        var flowerbedsInPlace = flowerbedDAO.GetFlowerbedsByPlaceId(Place.PlaceId);

        foreach (var flowerbed in flowerbedsInPlace)
        {
            FlowerbedsInPlace.Add(flowerbed);
        }
    }

    private void OnPlantInFlowerbedDetailsSeletected(object selectedPlantInFlowerbedDetails)
    {
        SelectedPlantInFlowerbedDetailsObject = (PlantInFlowerbedDetails)selectedPlantInFlowerbedDetails;

        PlantInFlowerbedDAO plantInFlowerbedDAO = new(_context);
        PlantInFlowerbed = plantInFlowerbedDAO.GetPlantInFlowerbedById(SelectedPlantInFlowerbedDetailsObject.PlantId, SelectedPlantInFlowerbedDetailsObject.FlowerbedId)!;
        Plant = Plants.FirstOrDefault(x => x.PlantId == PlantInFlowerbed.PlantId)!;
    }

    private void ReloadPlantInFlowerbedDetails()
    {
        PlantInFlowerbedDetails.Clear();

        plantInFlowerbedDAO = new(_context);
        PlantsInFlowerbed = new ObservableCollection<PlantInFlowerbed>(plantInFlowerbedDAO.GetAllPlantsInFlowerbed(Flowerbed.FlowerbedId));

        foreach (var plantInFlowerbed in PlantsInFlowerbed)
        {
            var plant = Plants.FirstOrDefault(p => p.PlantId == plantInFlowerbed.PlantId);
            PlantInFlowerbedDetails.Add(new PlantInFlowerbedDetails(plant, plantInFlowerbed));
        }
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
