using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Bloombase.Utilities;

namespace Bloombase;

public class PlantViewModel : INotifyPropertyChanged
{
    private readonly BloombaseContext _context;
    private readonly ErrorHandler _errorHandler;
    public event PropertyChangedEventHandler? PropertyChanged;
    public ObservableCollection<Plant> Plants { get; set; }
    public ICommand LoadPlantsCommand { get; private set; }
    public ICommand AddPlantCommand { get; private set; }
    public ICommand SavePlantCommand { get; private set; }
    public ICommand DeletePlantCommand { get; private set; }
    public ICommand SelectPlantCommand { get; private set; }
    public ICommand LoadPageCommand { get; private set; }



    public PlantViewModel(BloombaseContext context)
    {
        _context = context;

        PlantDAO plantDAO = new(_context);

        Plant = new Plant();

        Plants = new ObservableCollection<Plant>(plantDAO.GetAllPlants());

        SavePlantCommand = new Command(SavePlant);
        DeletePlantCommand = new Command(DeletePlant);
        SelectPlantCommand = new Command<Plant>(SelectPlant);
        AddPlantCommand = new Command(AddPlant);
        LoadPageCommand = new Command(LoadPage);

        _errorHandler = new ErrorHandler();
        IsButtonAddEnabled = true;
        IsButtonSaveEnabled = false;
        IsButtonDeleteEnabled = false;
    }

    public void LoadPage()
    {
        Plant = new Plant();
        LoadPlants();
        IsButtonAddEnabled = true;
        IsButtonSaveEnabled = false;
        IsButtonDeleteEnabled = false;
    }


    private void LoadPlants()
    {
        PlantInFlowerbedDAO plantInFlowerbedDAO = new(_context);
        PlantDAO plantDAO = new(_context);
        var plants = plantDAO.GetAllPlants();

        Plants.Clear();
        foreach (var plant in plants)
        {
            Plants.Add(plant);
        }
    }

    private void SavePlant()
    {
        if (string.IsNullOrEmpty(Plant.Name) || string.IsNullOrEmpty(Plant.BotanicalName) || string.IsNullOrEmpty(Plant.Origin) || Plant.Price == 0)
        {
            _errorHandler.ShowErrorMessage("Please fill in all fields correctly");
            return;
        }
        PlantDAO plantDAO = new(_context);
        plantDAO.UpdatePlant(Plant);

        _errorHandler.ShowSuccessMessage("Plant updated successfully");

        Plant = new Plant();
        LoadPlants();
        IsButtonAddEnabled = true;
        IsButtonDeleteEnabled = false;
        IsButtonSaveEnabled = false;
    }

    private void DeletePlant()
    {   
        PlantInFlowerbedDAO plantInFlowerbedDAO = new(_context);
        if(plantInFlowerbedDAO.IsPlantInFlowerbedExists(Plant.PlantId) == true) 
        {
            _errorHandler.ShowErrorMessage("This plant is in flowerbed, please remove it from the flowerbed first.");
            return;
        }
        PlantDAO plantDAO = new(_context);
        plantDAO.DeletePlant(Plant.PlantId);

        _errorHandler.ShowSuccessMessage("Plant deleted successfully");

        Plant = new Plant();
        LoadPlants();
        IsButtonAddEnabled = true;
        IsButtonDeleteEnabled = false;
        IsButtonSaveEnabled = false;
    }


    private void AddPlant()
    {
        if (string.IsNullOrEmpty(Plant.Name) || string.IsNullOrEmpty(Plant.BotanicalName) || string.IsNullOrEmpty(Plant.Origin) || Plant.Price == 0)
        {
            _errorHandler.ShowErrorMessage("Please fill in all fields correctly");
            return;
        }

        string plantId = IdGenerator.GenerateRandomId(5, Plants.ToList(), plant => plant.PlantId);

        Plant.PlantId = plantId;
        Plant.MaintenanceId = "M001";
        Plant.ClimateId = "C003";

        PlantDAO plantDAO = new(_context);
        plantDAO.AddPlant(Plant);

        _errorHandler.ShowSuccessMessage("Plant added successfully");

        Plant = new Plant();

        LoadPlants();

    }

    public void SelectPlant(Plant plant)
    {
        if (plant != null)
        {
            IsButtonAddEnabled = false;
            IsButtonDeleteEnabled = true;
            IsButtonSaveEnabled = true;
            Plant = plant;
        }
    }

    private bool isButtonAddEnabled;
    public bool IsButtonAddEnabled
    {
        get { return isButtonAddEnabled; }
        set
        {
            if (isButtonAddEnabled != value)
            {
                isButtonAddEnabled = value;
                OnPropertyChanged(nameof(IsButtonAddEnabled));
            }
        }
    }

    private bool isButtonSaveEnabled;
    public bool IsButtonSaveEnabled
    {
        get { return isButtonSaveEnabled; }
        set
        {
            if (isButtonSaveEnabled != value)
            {
                isButtonSaveEnabled = value;
                OnPropertyChanged(nameof(IsButtonSaveEnabled));
            }
        }
    }

    private bool isButtonDeleteEnabled;
    public bool IsButtonDeleteEnabled
    {
        get { return isButtonDeleteEnabled; }
        set
        {
            if (isButtonDeleteEnabled != value)
            {
                isButtonDeleteEnabled = value;
                OnPropertyChanged(nameof(IsButtonDeleteEnabled));
            }
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

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
