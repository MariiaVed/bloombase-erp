using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Bloombase.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Bloombase;

public class OverviewViewModel : INotifyPropertyChanged
{
    private readonly BloombaseContext _context;
    private readonly ErrorHandler _errorHandler;
    public event PropertyChangedEventHandler? PropertyChanged;
    public ObservableCollection<Employee> Employees { get; set; }
    public ObservableCollection<Flowerbed> Flowerbeds { get; set; }
    public ICommand AddEmployeeCommand { get; private set; }
    public ICommand SaveEmployeeCommand { get; private set; }
    public ICommand DeleteEmployeeCommand { get; private set; }
    public ICommand SelectEmployeeCommand { get; private set; }
    public ICommand LoadPageCommand { get; private set; }
    public ICommand AssignResponsibilityCommand { get; private set; }


    public OverviewViewModel(BloombaseContext context)
    {
        _context = context;

        EmployeeDAO employeeDAO = new(_context);
        FlowerbedDAO flowerbedDAO = new(_context);

        Employee = new Employee();
        SelectedFlowerbed = new Flowerbed();

        Employees = new ObservableCollection<Employee>(employeeDAO.GetAllEmployees());
        Flowerbeds = new ObservableCollection<Flowerbed>(flowerbedDAO.GetAllFlowerbeds());

        SaveEmployeeCommand = new Command(SaveEmployee);
        DeleteEmployeeCommand = new Command(DeleteEmployee);
        AddEmployeeCommand = new Command(AddEmployee);
        SelectEmployeeCommand = new Command<Employee>(SelectEmployee);
        LoadPageCommand = new Command(LoadPage);
        AssignResponsibilityCommand = new Command(AssignResponsibility);

        IsButtonAddEnabled = true;
        IsButtonSaveEnabled = false;
        IsButtonDeleteEnabled = false;
        IsButtonConfirmEnabled = false;


        _errorHandler = new ErrorHandler();

    }

    private void AssignResponsibility()
    {
        if (Employee.EmployeeId == null)
        {
            _errorHandler.ShowErrorMessage("Please select an employee in the list.");
            return;
        } else if (Employee.EmployeeId.Equals(SelectedFlowerbed.ResponsibleEmployeeId))
        {
            _errorHandler.ShowErrorMessage("This employee is already responsible for this flowerbed.");
            return;
        }

        FlowerbedDAO flowerbedDAO = new(_context);
        SelectedFlowerbed.ResponsibleEmployeeId = Employee.EmployeeId;
        flowerbedDAO.UpdateFlowerbed(SelectedFlowerbed);

        _errorHandler.ShowSuccessMessage("Responsibility assigned successfully!");

        LoadPage();
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
    private void LoadPage()
    {
        Employee = new Employee();
        SelectedFlowerbed = new Flowerbed();
        LoadEmployees();
        LoadFlowerbeds();
        IsButtonAddEnabled = true;
        IsButtonDeleteEnabled = false;
        IsButtonSaveEnabled = false;
        IsButtonConfirmEnabled = false;
    }
    private void SaveEmployee()
    {
        if (string.IsNullOrEmpty(Employee.EmployeeName) || string.IsNullOrEmpty(Employee.PhoneNumber) || string.IsNullOrEmpty(Employee.Role) || Employee.HourlySalary == 0 || Employee.AuthorityLevel == 0)
        {
            _errorHandler.ShowErrorMessage("Please fill in all fields correctly!");
            return;
        }
        EmployeeDAO employeeDAO = new(_context);
        employeeDAO.UpdateEmployee(Employee);

        _errorHandler.ShowSuccessMessage("Employee updated successfully!");

        LoadPage();
    }

    private void DeleteEmployee()
    {
        try
        {
            EmployeeDAO employeeDAO = new(_context);
            employeeDAO.DeleteEmployee(Employee.EmployeeId);
            _errorHandler.ShowSuccessMessage("Employee deleted successfully!");
        }
        catch (DbUpdateException ex)
        {
            _errorHandler.ShowErrorMessage("This employee is responsible for a flowerbed or assigned to a task and cannot be deleted.");
        }

        LoadPage();
    }

    public void AddEmployee()
    {
        if (string.IsNullOrEmpty(Employee.EmployeeName) || string.IsNullOrEmpty(Employee.PhoneNumber) || string.IsNullOrEmpty(Employee.Role) || Employee.HourlySalary == 0 || Employee.AuthorityLevel == 0)
        {
            _errorHandler.ShowErrorMessage("Please fill in all fields correctly!");
            return;

        }

        string employeeId = IdGenerator.GenerateRandomId(5, Employees.ToList(), employee => employee.EmployeeId);
        Employee.EmployeeId = employeeId;

        EmployeeDAO employeeDAO = new(_context);
        employeeDAO.AddEmployee(Employee);
        _errorHandler.ShowSuccessMessage("Employee added successfully!");

        LoadPage();

    }

    public void SelectEmployee(Employee employee)
    {
        if (employee != null)
        {
            // Update UI based on SelectedFlowerbed
            if (SelectedFlowerbed != null)
            {
                IsButtonSaveEnabled = false;
                IsButtonDeleteEnabled = false;
                IsButtonAddEnabled = false;
                Employee = employee;
            }
            else
            {
                IsButtonAddEnabled = true; 

            // Enable Save and Delete buttons when an employee is selected
            IsButtonSaveEnabled = true;
            IsButtonDeleteEnabled = true;

            Employee = employee;
            }
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

    private bool isButtonConfirmEnabled;
    public bool IsButtonConfirmEnabled
    {
        get { return isButtonConfirmEnabled; }
        set
        {
            if (isButtonConfirmEnabled != value)
            {
                isButtonConfirmEnabled = value;
                OnPropertyChanged(nameof(IsButtonConfirmEnabled));
            }
        }

    }

    private Employee _employee;
    public Employee Employee
    {
        get => _employee;
        set
        {
            _employee = value;
            OnPropertyChanged(nameof(Employee));
        }
    }

    private Flowerbed _selectedFlowerbed;
    public Flowerbed SelectedFlowerbed
    {
        get => _selectedFlowerbed;
        set
        {
            _selectedFlowerbed = value;
            IsButtonConfirmEnabled = true;
            IsButtonAddEnabled = false;
            IsButtonDeleteEnabled = false;
            IsButtonSaveEnabled = false;
            OnPropertyChanged(nameof(SelectedFlowerbed));
        }
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
