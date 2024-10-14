namespace Bloombase;

public class EmployeeDAO
{
	private readonly BloombaseContext _context;
	public EmployeeDAO(BloombaseContext context)
	{
		_context = context;
	}
	// Add a new employee
	public void AddEmployee(Employee employee)
	{
		_context.Employees.Add(employee);
		_context.SaveChanges();
	}
	// Update an existing employee
	public void UpdateEmployee(Employee employee)
	{
		_context.Employees.Update(employee);
		_context.SaveChanges();
	}
	// Delete an employee
	public void DeleteEmployee(string id)
	{
		var employee = _context.Employees.Find(id);
		if (employee != null)
		{
			_context.Employees.Remove(employee);
			_context.SaveChanges();
		}
	}
	// List all employees, optionally including Botanical Garden information
	public List<Employee> GetAllEmployees()
	{
		return _context.Employees.ToList();
	}
}
