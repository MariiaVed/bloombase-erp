using Microsoft.EntityFrameworkCore;

namespace Bloombase;

public class FlowerbedCareDAO
{
    private readonly BloombaseContext _context;

    public FlowerbedCareDAO(BloombaseContext context)
    {
        _context = context;
    }

    // Add a new flowerbed care
    public void AddFlowerbedCare(FlowerbedCare flowerbedCare)
    {
        _context.FlowerbedCares.Add(flowerbedCare);
        _context.SaveChanges();
    }

    // Update an existing flowerbed care    
    public void UpdateFlowerbedCare(FlowerbedCare flowerbedCare)
    {
        _context.FlowerbedCares.Update(flowerbedCare);
        _context.SaveChanges();
    }

    // Delete a flowerbed care
    public void DeleteFlowerbedCare(string id)
    {
        var flowerbedCare = _context.FlowerbedCares.Find(id);
        if (flowerbedCare != null)
        {
            _context.FlowerbedCares.Remove(flowerbedCare);
            _context.SaveChanges();
        }
    }

    // List all flowerbed cares, optionally including flowerbed, employee, and place information
    private List<FlowerbedCare> GetFlowerbedCares(bool includeFlowerbed = false, bool includeEmployee = false, bool includePlace = false)
    {
        if (includeFlowerbed && includeEmployee && includePlace)
        {
            return _context.FlowerbedCares
            .Include(fc => fc.Flowerbed)
            .Include(fc => fc.Employee)
            .Include(fc => fc.Place)
            .ToList();
        }
        else if (includeFlowerbed && includeEmployee)
        {
            return _context.FlowerbedCares
            .Include(fc => fc.Flowerbed)
            .Include(fc => fc.Employee)
            .ToList();
        }
        else if (includeFlowerbed && includePlace)
        {
            return _context.FlowerbedCares
            .Include(fc => fc.Flowerbed)
            .Include(fc => fc.Place)
            .ToList();
        }
        else if (includeEmployee && includePlace)
        {
            return _context.FlowerbedCares
            .Include(fc => fc.Employee)
            .Include(fc => fc.Place)
            .ToList();
        }
        else if (includeFlowerbed)
        {
            return _context.FlowerbedCares
            .Include(fc => fc.Flowerbed)
            .ToList();
        }
        else if (includeEmployee)
        {
            return _context.FlowerbedCares
            .Include(fc => fc.Employee)
            .ToList();
        }
        else if (includePlace)
        {
            return _context.FlowerbedCares
            .Include(fc => fc.Place)
            .ToList();
        }
        else
        {
            return _context.FlowerbedCares.ToList();
        }
    }

    public List<FlowerbedCare> GetAllFlowerbedCares()
    {
        GetFlowerbedCares();
        foreach (var flowerbed in _context.FlowerbedCares)
        {
            flowerbed.Flowerbed = _context.Flowerbeds.Find(flowerbed.FlowerbedId);
            flowerbed.Employee = _context.Employees.Find(flowerbed.EmployeeId);
        }
        return _context.FlowerbedCares.ToList();
    }

    //find employee by id
    public Employee FindEmployeeById(string id)
    {
        return _context.Employees.Find(id);
    }

    //find flowerbed by id
    public Flowerbed FindFlowerbedById(string id)
    {
        return _context.Flowerbeds.Find(id);
    }

}
