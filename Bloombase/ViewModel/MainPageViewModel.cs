using System.ComponentModel;
using System.Windows.Input;

namespace Bloombase;

public class MainPageViewModel : INotifyPropertyChanged
{
    private readonly BloombaseContext _context;
    public event PropertyChangedEventHandler? PropertyChanged;
    public ICommand LoadPageCommand { get; private set; }
    
    

    public MainPageViewModel(BloombaseContext context)
    {
        _context = context;
        LoadPageCommand = new Command(LoadPage);
    }

    private void LoadPage()
    {
        OverviewViewModel overviewViewModel = new(_context);
        TotalEmployees = overviewViewModel.Employees.Count;

        PersonalPageViewModel personalViewModel = new(_context);
        TotalFlowerbedCares = personalViewModel.FlowerbedCares.Count;

        PlantInFlowerbedDAO plantInFlowerbedDAO = new(_context);
        TotalPlantQuantity = plantInFlowerbedDAO.GetTotalQuantity();

    }

    private int _totalEmployees;
    public int TotalEmployees
    {
        get => _totalEmployees;
        set
        {
            _totalEmployees = value;
            OnPropertyChanged(nameof(TotalEmployees));
        }
    }

    private int _totalFlowerbedCares;
    public int TotalFlowerbedCares
    {
        get => _totalFlowerbedCares;
        set
        {
            _totalFlowerbedCares = value;
            OnPropertyChanged(nameof(TotalFlowerbedCares));
        }
    }

    private int _totalPlantQuantity;
    public int TotalPlantQuantity
    {
        get => _totalPlantQuantity;
        set
        {
            _totalPlantQuantity = value;
            OnPropertyChanged(nameof(TotalPlantQuantity));
        }
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
