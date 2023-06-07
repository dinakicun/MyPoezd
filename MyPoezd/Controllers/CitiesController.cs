using MyPoezd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPoezd;

namespace Poezd.Controllers
{
    public class CitiesController : Controller
    {
        private readonly MyTrainContext _db;

        public CitiesController(MyTrainContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> CitiesIndex()
        {
            var result = await _db.Cities.ToListAsync();

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> CitiesCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CitiesCreate(City? city)
        {
            if (ModelState.IsValid)
            {
                await _db.Cities.AddAsync(city);
                await _db.SaveChangesAsync();
                return RedirectToAction("CitiesIndex");
            }

            return View(city);
        }
        
        public async Task<IActionResult> Delete(int? id)
        {
            var result = await _db.Cities.Where(x => x.Id == id).FirstOrDefaultAsync();
            _db.Cities.Remove(result);
            await _db.SaveChangesAsync();
            return RedirectToAction("CitiesIndex");
        }

        [HttpGet]
        public async Task<IActionResult> CitiesEdit(int? id)
        {
            var result = await _db.Cities.Where(x => x.Id == id).FirstOrDefaultAsync();
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> CitiesEdit(City city)
        {
            var result = await _db.Cities.Where(x => x.Id == city.Id).FirstOrDefaultAsync();
            result.Name = city.Name;
            _db.Update(result);
            await _db.SaveChangesAsync();
            return RedirectToAction("CitiesIndex");
        }
    }
}
