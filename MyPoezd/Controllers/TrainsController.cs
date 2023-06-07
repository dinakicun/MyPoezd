using MyPoezd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPoezd;

namespace Poezd.Controllers
{
    public class TrainsController : Controller
    {
        private readonly MyTrainContext _db;

        public TrainsController(MyTrainContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> TrainsIndex()
        {
            var result = await _db.Trains.ToListAsync();

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> TrainsCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TrainsCreate(Train? train)
        {
            if (ModelState.IsValid)
            {
                await _db.Trains.AddAsync(train);
                await _db.SaveChangesAsync();
                return RedirectToAction("TrainsIndex");
            }

            return View(train);
        }
        
        public async Task<IActionResult> Delete(int? id)
        {
            var result = await _db.Trains.Where(x => x.Id == id).FirstOrDefaultAsync();
            _db.Trains.Remove(result);
            await _db.SaveChangesAsync();
            return RedirectToAction("TrainsIndex");
        }

        [HttpGet]
        public async Task<IActionResult> TrainsEdit(int? id)
        {
            var result = await _db.Trains.Where(x => x.Id == id).FirstOrDefaultAsync();
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> TrainsEdit(Train train)
        {
            var result = await _db.Trains.Where(x => x.Id == train.Id).FirstOrDefaultAsync();
            result.Name = train.Name;
            _db.Update(result);
            await _db.SaveChangesAsync();
            return RedirectToAction("TrainsIndex");
        }
    }
}
