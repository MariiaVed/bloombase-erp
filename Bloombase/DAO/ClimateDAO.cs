namespace Bloombase;

public class ClimateDAO
{

    private readonly BloombaseContext _context;

    public ClimateDAO(BloombaseContext context)
    {
        _context = context;
    }

    // Add a new climate
    public void AddClimate(Climate climate)
    {
        _context.Climates.Add(climate);
        _context.SaveChanges();
    }

    // Get a climate by ID
    public Climate? GetClimateById(string id)
    {
        return _context.Climates.Find(id);
    }

    // Update an existing climate
    public void UpdateClimate(Climate climate)
    {
        _context.Climates.Update(climate);
        _context.SaveChanges();
    }

    // Delete a climate
    public void DeleteClimate(string id)
    {
        var climate = _context.Climates.Find(id);
        if (climate != null)
        {
            _context.Climates.Remove(climate);
            _context.SaveChanges();
        }
    }

    // List all climates
    public List<Climate> GetAllClimates()
    {
        return _context.Climates.ToList();
    }

}
