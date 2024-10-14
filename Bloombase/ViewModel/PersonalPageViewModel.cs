using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Bloombase.Utilities;

namespace Bloombase;

public class PersonalPageViewModel : INotifyPropertyChanged
{
    private readonly BloombaseContext _context;
    private readonly ErrorHandler _errorHandler;
    public event PropertyChangedEventHandler? PropertyChanged;
    public ObservableCollection<Employee> Employees { get; set; }
    public Employee SelectedEmployee { get; set; }
    public ObservableCollection<FlowerbedCare> FlowerbedCares { get; set; }
    public ObservableCollection<Flowerbed> Flowerbeds { get; set; }
    public Flowerbed SelectedFlowerbed { get; set; }
    public ICommand AddFlowerbedCareCommand { get; private set; }
    public ICommand SaveFlowerbedCareCommand { get; private set; }
    public ICommand DeleteFlowerbedCareCommand { get; private set; }
    public ICommand SelectFlowerbedCareCommand { get; private set; }
    public ICommand LoadPageCommand { get; private set; }


    public PersonalPageViewModel(BloombaseContext context)
    {
        _context = context;

        EmployeeDAO employeeDAO = new(_context);
        FlowerbedCareDAO flowerbedCareDAO = new(_context);
        FlowerbedDAO flowerbedDAO = new(_context);

        Employees = new ObservableCollection<Employee>(employeeDAO.GetAllEmployees());
        Flowerbeds = new ObservableCollection<Flowerbed>(flowerbedDAO.GetAllFlowerbeds());
        FlowerbedCares = new ObservableCollection<FlowerbedCare>(flowerbedCareDAO.GetAllFlowerbedCares());

        FlowerbedCare = new FlowerbedCare();
        SelectedEmployee = new Employee();
        SelectedFlowerbed = new Flowerbed();

        FlowerbedCare.Date = DateTime.Today;

        AddFlowerbedCareCommand = new Command(AddFlowerbedCare);
        SaveFlowerbedCareCommand = new Command(SaveFlowerbedCare);
        DeleteFlowerbedCareCommand = new Command(DeleteFlowerbedCare);
        LoadPageCommand = new Command(LoadPage);

        SelectFlowerbedCareCommand = new Command<FlowerbedCare>(SelectFlowerbedCare);
        _errorHandler = new ErrorHandler();

        IsButtonAddEnabled = true;
        IsButtonSaveEnabled = false;
        IsButtonDeleteEnabled = false;
    }

    public void LoadPage()
    {
        LoadEmployees();
        LoadFlowerbeds();
        LoadFlowerbedCares();
        FlowerbedCare = new FlowerbedCare();
        SelectedEmployee = new Employee();
        SelectedFlowerbed = new Flowerbed();

        IsButtonAddEnabled = true;
        IsButtonSaveEnabled = false;
        IsButtonDeleteEnabled = false;

        FlowerbedCare.Date = DateTime.Today;

    }

    private void AddFlowerbedCare()
    {
        if (SelectedFlowerbed.FlowerbedId == null || SelectedEmployee.EmployeeId == null || string.IsNullOrEmpty(FlowerbedCare.Description) || string.IsNullOrEmpty(FlowerbedCare.FlowerbedCareType))
        {
            _errorHandler.ShowErrorMessage("Please fill in all fields.");
            return;
        }
        if (FlowerbedCare.Date < DateTime.Today)
        {
            _errorHandler.ShowErrorMessage("Please choose a valid date.");
            return;
        }

        string flowerbedCareId = IdGenerator.GenerateRandomId(5, FlowerbedCares.ToList(), flowerbedCare => flowerbedCare.FlowerbedCareId);
        FlowerbedCare.FlowerbedCareId = flowerbedCareId;
        FlowerbedCare.Employee = SelectedEmployee;
        FlowerbedCare.EmployeeId = SelectedEmployee.EmployeeId;
        try
        {
            SelectedEmployee.AddFlowerbedCare(FlowerbedCare);
        }
        catch (Exception e)
        {
            _errorHandler.ShowErrorMessage(e.Message);
            return;
        }

        FlowerbedCare.Flowerbed = SelectedFlowerbed;
        FlowerbedCare.FlowerbedId = SelectedFlowerbed.FlowerbedId;
        FlowerbedCare.PlaceId = SelectedFlowerbed.PlaceId;
        SelectedFlowerbed.FlowerbedCares.Add(FlowerbedCare);

        FlowerbedCareDAO flowerbedCareDAO = new(_context);
        flowerbedCareDAO.AddFlowerbedCare(FlowerbedCare);

        _errorHandler.ShowSuccessMessage("Task added successfully!");

        FlowerbedCare = new FlowerbedCare();

        FlowerbedCare.Date = DateTime.Today;

        LoadPage();
    }

    private void SaveFlowerbedCare()
    {
        if (SelectedEmployee == null || SelectedFlowerbed == null || string.IsNullOrEmpty(FlowerbedCare.Description) || string.IsNullOrEmpty(FlowerbedCare.FlowerbedCareType))
        {
            _errorHandler.ShowErrorMessage("Please fill in all fields correctly");
        }

        FlowerbedCare.Employee = SelectedEmployee;
        FlowerbedCare.EmployeeId = SelectedEmployee.EmployeeId;
        FlowerbedCare.Flowerbed = SelectedFlowerbed;
        FlowerbedCare.FlowerbedId = SelectedFlowerbed.FlowerbedId;
        FlowerbedCare.PlaceId = SelectedFlowerbed.PlaceId;

        FlowerbedCareDAO flowerbedCareDAO = new(_context);
        flowerbedCareDAO.UpdateFlowerbedCare(FlowerbedCare);

        _errorHandler.ShowSuccessMessage("Task updated successfully!");

        FlowerbedCare.Date = DateTime.Today;
        FlowerbedCare = new FlowerbedCare();

        LoadPage();

        IsButtonAddEnabled = true;
        IsButtonDeleteEnabled = false;
        IsButtonSaveEnabled = false;
    }

    private void DeleteFlowerbedCare()
    {
        FlowerbedCareDAO flowerbedCareDAO = new(_context);
        flowerbedCareDAO.DeleteFlowerbedCare(FlowerbedCare.FlowerbedCareId);

        _errorHandler.ShowSuccessMessage("Task deleted successfully!");

        FlowerbedCare = new FlowerbedCare();
        FlowerbedCare.Date = DateTime.Today;
        LoadPage();

        IsButtonAddEnabled = true;
        IsButtonDeleteEnabled = false;
        IsButtonSaveEnabled = false;

    }

    private void LoadEmployees()
    {
        EmployeeDAO employeeDAO = new(_context);
        var employees = employeeDAO.GetAllEmployees();

        Employees.Clear();
        foreach (var employee in employees)
        {
            Employees.Add(employee);
        }
    }

    private void LoadFlowerbeds()
    {
        FlowerbedDAO flowerbedDAO = new(_context);
        var flowerbeds = flowerbedDAO.GetAllFlowerbeds();

        Flowerbeds.Clear();
        foreach (var flowerbed in flowerbeds)
        {
            Flowerbeds.Add(flowerbed);
        }
    }

    private void LoadFlowerbedCares()
    {
        FlowerbedCareDAO flowerbedCareDAO = new(_context);
        var flowerbedCares = flowerbedCareDAO.GetAllFlowerbedCares();


        FlowerbedCares.Clear();
        foreach (var flowerbedCare in flowerbedCares)
        {
            FlowerbedCares.Add(flowerbedCare);
        }
    }

    private void SelectFlowerbedCare(FlowerbedCare flowerbedCare)
    {
        if (FlowerbedCare != null)
        {
            FlowerbedCare = flowerbedCare;

            FlowerbedCareDAO flowerbedCareDAO = new FlowerbedCareDAO(_context);
            var employee = flowerbedCareDAO.FindEmployeeById(FlowerbedCare.EmployeeId);
            var flowerbed = flowerbedCareDAO.FindFlowerbedById(FlowerbedCare.FlowerbedId);

            SelectedEmployee = employee;
            SelectedFlowerbed = flowerbed;

            IsButtonAddEnabled = false;
            IsButtonSaveEnabled = true;
            IsButtonDeleteEnabled = true;
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



    private FlowerbedCare flowerbedCare;
    public FlowerbedCare FlowerbedCare
    {
        get { return flowerbedCare; }
        set
        {
            if (flowerbedCare != value)
            {
                flowerbedCare = value;
                OnPropertyChanged(nameof(FlowerbedCare));
            }
        }
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
