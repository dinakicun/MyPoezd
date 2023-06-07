using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using MyPoezd.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using MyPoezd.Models.ViewModels;
using MyPoezd;
using Microsoft.AspNetCore.Routing;

namespace Poezd.Controllers
{
    public class RoutesController : Controller
    {
        private readonly MyTrainContext _db;

        public RoutesController(MyTrainContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> RoutesIndex()
        {
            var result = await _db.Routes.Include(x => x.DepartureCity).Include(x => x.ArrivalCity).ToListAsync();

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> RoutesCreate()
        {
            var result = await _db.Cities.ToListAsync();
            var result2 = await _db.Trains.ToListAsync();
            RoutesView routesVM = new();
            routesVM.Cities = result;
            routesVM.Trains = result2;
            return View(routesVM);
        }

        [HttpPost]
        public async Task<IActionResult> RoutesCreate(RoutesView routesVM)
        {
            await UsingTrains(routesVM);

            _db.Routes.Add(routesVM.Route);
            await _db.SaveChangesAsync();

            return RedirectToAction("RoutesIndex");
        }

        public async Task<RoutesView> UsingTrains(RoutesView routesVM)
        {
            string departureDate = routesVM.Route.DepartureDate.ToString();
            departureDate = departureDate.Remove(departureDate.Length - 8);
            string arrivalDate = routesVM.Route.ArrivalDate.ToString();
            arrivalDate = arrivalDate.Remove(arrivalDate.Length - 8);

            string allDepartureDate = departureDate + " " + routesVM.DepartureTime;
            string allArrivalTime = arrivalDate + " " + routesVM.ArrivalTime;

            DateTime departureTime = DateTime.ParseExact(allDepartureDate, "dd.MM.yyyy HH\\:mm", CultureInfo.InvariantCulture);
            DateTime arrivalTime = DateTime.ParseExact(allArrivalTime, "dd.MM.yyyy HH\\:mm", CultureInfo.InvariantCulture);

            routesVM.Route.DepartureDate = departureTime;
            routesVM.Route.ArrivalDate = arrivalTime;
            routesVM.Route.DepartureCityId = await FoundCityByName(routesVM.DepartureCity);
            routesVM.Route.ArrivalCityId = await FoundCityByName(routesVM.ArrivalCity);

            routesVM.Route.TrainsId = await FoundTrainByName(routesVM.TrainName);

            
            return routesVM;
        }

        public async Task<int> FoundCityByName(string name)
        {
            var result = await _db.Cities.Where(x => x.Name == name).FirstOrDefaultAsync();
            return result.Id;
        }
        public async Task<int> FoundTrainByName(string name)
        {
            var result = await _db.Trains.Where(x => x.Name == name).FirstOrDefaultAsync();
            return result.Id;
        }

        public async Task<IActionResult> Delete(int id)
        {
            var result = await _db.Routes.Where(x => x.Id == id).FirstOrDefaultAsync();
            _db.Routes.Remove(result);
            await _db.SaveChangesAsync();

            return RedirectToAction("RoutesIndex");
        }

       
    }
}
