using Microsoft.EntityFrameworkCore;

namespace Bloombase;

public class FlowerbedDAO
{
    private readonly BloombaseContext _context;

    public FlowerbedDAO(BloombaseContext context)
    {
        _context = context;
    }

    // Add a new flowerbed
    public void AddFlowerbed(Flowerbed flowerbed)
    {
        _context.Flowerbeds.Add(flowerbed);
        _context.SaveChanges();
    }

    // Update an existing flowerbed
    public void UpdateFlowerbed(Flowerbed flowerbed)
    {
        _context.Flowerbeds.Update(flowerbed);
        _context.SaveChanges();
    }

    // Delete a flowerbed
    public void DeleteFlowerbed(string id)
    {
        var flowerbed = _context.Flowerbeds.Find(id);
        if (flowerbed != null)
        {
            _context.Flowerbeds.Remove(flowerbed);
            _context.SaveChanges();
        }
    }

    // List all flowerbeds, optionally including place, climate, and employee information
    public List<Flowerbed> GetAllFlowerbeds(bool includePlace = false, bool includeClimate = false, bool includeEmployee = false)
    {
        if (includePlace && includeClimate && includeEmployee)
        {
            return _context.Flowerbeds
            .Include(f => f.Place)
            .Include(f => f.Climate)
            .Include(f => f.ResponsibleEmployee)
            .ToList();
        }
        else if (includePlace && includeClimate)
        {
            return _context.Flowerbeds
            .Include(f => f.Place)
            .Include(f => f.Climate)
            .ToList();
        }
        else if (includePlace && includeEmployee)
        {
            return _context.Flowerbeds
            .Include(f => f.Place)
            .Include(f => f.ResponsibleEmployee)
            .ToList();
        }
        else if (includeClimate && includeEmployee)
        {
            return _context.Flowerbeds
            .Include(f => f.Climate)
            .Include(f => f.ResponsibleEmployee)
            .ToList();
        }
        else if (includePlace)
        {
            return _context.Flowerbeds
            .Include(f => f.Place)
            .ToList();
        }
        else if (includeClimate)
        {
            return _context.Flowerbeds
            .Include(f => f.Climate)
            .ToList();
        }
        else if (includeEmployee)
        {
            return _context.Flowerbeds
            .Include(f => f.ResponsibleEmployee)
            .ToList();
        }
        else
        {
            try
            {
                return _context.Flowerbeds.ToList();
            }
            catch (Exception e)
            {
                return new List<Flowerbed>();
            }
        }
    }

    // List all flowerbeds in a specific place
    public List<Flowerbed> GetFlowerbedsByPlaceId(string placeId)
    {
        //Return all flowerbeds in a specific place
        return _context.Flowerbeds.Where(f => f.PlaceId == placeId).ToList();
    }

}
