using AdApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class AdsController : ControllerBase
{
    private readonly AdDbContext _context;

    public AdsController(AdDbContext context)
    {
        _context = context;
    }

    // GET: api/Ads
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Ad>>> GetAds()
    {
        return await _context.Ads.Include(a => a.Seller).Include(a => a.Category).ToListAsync();
    }

    // GET: api/Ads/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Ad>> GetAd(int id)
    {
        var ad = await _context.Ads.Include(a => a.Seller).Include(a => a.Category).FirstOrDefaultAsync(a => a.Id == id);

        if (ad == null)
        {
            return NotFound();
        }

        return ad;
    }

    // GET: api/Ads/seller/1
    [HttpGet("seller/{sellerId}")]
    public async Task<ActionResult<IEnumerable<Ad>>> GetAdsBySeller(int sellerId)
    {
        var ads = await _context.Ads
                                 .Where(a => a.SellerId == sellerId)
                                 .Include(a => a.Seller)
                                 .Include(a => a.Category)
                                 .ToListAsync();

        if (!ads.Any())
        {
            return NotFound();
        }

        return ads;
    }

    // GET: api/Ads/category/1
    [HttpGet("category/{categoryId}")]
    public async Task<ActionResult<IEnumerable<Ad>>> GetAdsByCategory(int categoryId)
    {
        var ads = await _context.Ads
                                 .Where(a => a.CategoryId == categoryId)
                                 .Include(a => a.Seller)
                                 .Include(a => a.Category)
                                 .OrderBy(a => a.Description) // Grouping alphabetically
                                 .ToListAsync();

        if (!ads.Any())
        {
            return NotFound();
        }

        return ads;
    }

    // POST: api/Ads
    [HttpPost]
    public async Task<ActionResult<Ad>> PostAd(Ad ad)
    {
        _context.Ads.Add(ad);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAd), new { id = ad.Id }, ad);
    }

    // PUT: api/Ads/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAd(int id, Ad ad)
    {
        if (id != ad.Id)
        {
            return BadRequest();
        }

        _context.Entry(ad).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AdExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/Ads/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAd(int id)
    {
        var ad = await _context.Ads.FindAsync(id);
        if (ad == null)
        {
            return NotFound();
        }

        _context.Ads.Remove(ad);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool AdExists(int id)
    {
        return _context.Ads.Any(e => e.Id == id);
    }
}
