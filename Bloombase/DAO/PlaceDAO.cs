namespace Bloombase;

public class PlaceDAO
{
    private readonly BloombaseContext _context;

    public PlaceDAO(BloombaseContext context)
    {
        _context = context;
    }

    // Add a new place
    public void AddPlace(Place place)
    {
        _context.Places.Add(place);
        _context.SaveChanges();
    }
    // Update an existing place
    public void UpdatePlace(Place place)
    {
        _context.Places.Update(place);
        _context.SaveChanges();
    }

    // Delete a place
    public void DeletePlace(string id)
    {
        var place = _context.Places.Find(id);
        if (place != null)
        {
            _context.Places.Remove(place);
            _context.SaveChanges();
        }
    }

    // List all places
    public List<Place> GetAllPlaces()
    {
        return _context.Places.ToList();
    }

}
