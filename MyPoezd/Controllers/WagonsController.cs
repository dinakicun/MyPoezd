using MyPoezd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPoezd;
using MyPoezd.Models.ViewModels;
using System.Globalization;

namespace Poezd.Controllers
{
    public class WagonsController : Controller
    {
        private readonly MyTrainContext _db;

        public WagonsController(MyTrainContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> WagonsIndex()
        {
            var result = await _db.Wagons.ToListAsync();

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> WagonsCreate()
        {
            var result = await _db.Types.ToListAsync();
            var result2 = await _db.Trains.ToListAsync();
            WagonsView wagonsView = new();
            wagonsView.TrainTypes = result;
            wagonsView.Trains = result2;
            return View(wagonsView);
        }

        [HttpPost]
        public async Task<IActionResult> WagonsCreate(WagonsView wagonsView)
        {
            //await UsingTrains(wagonsView);

            //_db.Wagons.Add(wagonsView.Wagon);
            //await _db.SaveChangesAsync();

            return RedirectToAction("WagonsIndex");
        }

        public async Task<WagonsView> UsingTrains(WagonsView wagonsView)
        {

            wagonsView.Wagon.TypeId = await FoundTypeByName(wagonsView.WagonTypeName);
            wagonsView.Wagon.TrainsId = await FoundTrainByName(wagonsView.TrainName);
            return wagonsView;
        }
        public async Task<int> FoundTrainByName(string name)
        {
            var result = await _db.Trains.Where(x => x.Name == name).FirstOrDefaultAsync();
            return result.Id;
        }
        public async Task<int> FoundTypeByName(string name)
        {
            var result = await _db.Types.Where(x => x.Name == name).FirstOrDefaultAsync();
            return result.Id;
        }
        public async Task<IActionResult> Delete(int? id)
        {
            var result = await _db.Wagons.Where(x => x.Id == id).FirstOrDefaultAsync();
            _db.Wagons.Remove(result);
            await _db.SaveChangesAsync();
            return RedirectToAction("WagonsIndex");
        }

        [HttpGet]
        public async Task<IActionResult> WagonsEdit(int? id)
        {
            var result = await _db.Wagons.Where(x => x.Id == id).FirstOrDefaultAsync();
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> WagonsEdit(Wagon wagon)
        {
            var result = await _db.Wagons.Where(x => x.Id == wagon.Id).FirstOrDefaultAsync();
            result.Name = wagon.Name;
            _db.Update(result);
            await _db.SaveChangesAsync();
            return RedirectToAction("WagonsIndex");
        }
    }
}
