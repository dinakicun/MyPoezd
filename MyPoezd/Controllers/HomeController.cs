
using MyPoezd.Models;
using MyPoezd.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Globalization;
using System.Net;
using Microsoft.AspNetCore.Identity;
using System.IO.Pipelines;

namespace MyPoezd.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MyTrainContext _db;

        public HomeController(ILogger<HomeController> logger, MyTrainContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var cities = await _db.Cities.ToListAsync();

            RoutesView ticketVM = new();
            ticketVM.Cities = cities;

            return View(ticketVM);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Logout()
        {
            if (HttpContext.Session.IsAvailable)
            {
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            };
            return RedirectToAction("Privacy");
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            var userModel = _db.Users.Include(x => x.Role).FirstOrDefault(p => p.NumberPhone == user.NumberPhone && p.Password == user.Password);
            if (userModel == null)
            {
                ViewBag.InvalidCredentialsError = "Неправильный номер телефона или пароль.";
                return View();
            }
            else
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultRoleClaimType, userModel.Role.Name),
                new Claim(ClaimsIdentity.DefaultNameClaimType, userModel.Id.ToString()),
            };

                var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                HttpContext.SignInAsync(claimsPrincipal);
                return RedirectToAction("Index");
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }
        [HttpGet]
        public IActionResult PersonalArea()
        {
            User user = _db.Users.FirstOrDefault(); 

            return View(user);
        }


        [HttpPost]
        public IActionResult Registration(User user)
        {
            var userModel = _db.Users.Include(x => x.Role).FirstOrDefault(p => p.NumberPhone == user.NumberPhone);
            if (userModel != null)
            {
                ViewBag.InvalidCredentialsError = "Этот номер уже занят";
                return View();
            }
            else
            {
                user.RoleId = 2;

                _db.Users.Add(user);
                _db.SaveChanges();

                return RedirectToAction("Login");
            }
            

        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<int> FoundCityByName(string name)
        {
            var result = await _db.Cities.Where(x => x.Name == name).FirstOrDefaultAsync();
            return result.Id;
        }

        [HttpPost]
        public async Task<IActionResult> BuyingTicket(RoutesView ticketVm)
        {
            int departureCity = await FoundCityByName(ticketVm.DepartureCity);
            int arrivalCity = await FoundCityByName(ticketVm.ArrivalCity);

            DateTime departureDate = DateTime.ParseExact(ticketVm.DepartureTime, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            DateTime lastDepartureDate = departureDate.AddDays(1);

            var routes = await _db.Routes.Where(x => x.DepartureCityId == departureCity)
                                                    .Where(x => x.ArrivalCityId == arrivalCity)
                                                    .Where(x => x.DepartureDate >= departureDate & x.DepartureDate < lastDepartureDate)
                                                    .ToListAsync();
            List<RoutesView> ticketsVM = new();
            foreach (var item in routes)
            {
                RoutesView ticket = new();
                ticket.Route = item;
                ticket.StringDeparutureDate = departureDate.Day + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(departureDate.Month) + "," + CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(departureDate.DayOfWeek);

                var arrivalTime = item.ArrivalDate.ToString();
                arrivalTime = arrivalTime.Remove(arrivalTime.Length - 9);
                DateTime arrivalDate = DateTime.ParseExact(arrivalTime, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                ticket.StringArrivalDate = arrivalDate.Day + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(arrivalDate.Month) + "," + CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(arrivalDate.DayOfWeek);

                ticket.DepartureCity = item.DepartureCity.Name;
                ticket.ArrivalCity = item.ArrivalCity.Name;

                ticket.ArrivalTime = item.ArrivalDate.ToString().Remove(0, 11).Remove(5, 3);
                ticket.DepartureTime = item.DepartureDate.ToString().Remove(0, 11).Remove(5, 3);

                var result = item.ArrivalDate.Subtract(item.DepartureDate);
                if (result.Days > 0)
                {
                    ticket.TravelTime = ($"{result.Days.ToString()} д {result.Hours.ToString()} ч {result.Minutes.ToString()} мин");
                }
                else
                {
                    ticket.TravelTime = ($"{result.Hours.ToString()} ч {result.Minutes.ToString()} мин");
                }
                // Найдите нужное поле Name для поезда
                var train = await _db.Trains.FirstOrDefaultAsync(x => x.Id == item.TrainsId);
                if (train != null)
                {
                    ticket.Train = train;
                }

                ticketsVM.Add(ticket);
            }

            return View(ticketsVM);
        }
        public async Task<IActionResult> WagonsList(int id, string category, string price, string depcity, string arcity)
        {

            var routes = await FoundRoute(id);
            var trains = await _db.Trains.Where(x => x.Id == routes.TrainsId).ToListAsync();
            var wagons = await _db.Wagons.Where(x => x.TrainsId == id).ToListAsync();
            List<WagonsView> wagonVM = new();

            if (category == "Плацкарт")
            {
                wagons = wagons.Where(x => x.TypeId == 1).ToList();
                foreach (var item in wagons)
                {
                    WagonsView wagon = new();
                    wagon.Route = routes;
                    wagon.Wagons = wagons;
                    wagon.Trains = trains;
                    wagon.WagonTypeName = category;
                    wagon.Price = price;
                    var wagonId = wagons.FirstOrDefault(x => x.Id == item.Id).Id;
                    wagon.WagonId = wagonId;
                    var trainId = trains.FirstOrDefault(x => x.Id == item.TrainsId).Id;
                    wagon.TrainId = trainId;
                    var wagonName = wagons.FirstOrDefault(x => x.Id == item.Id)?.Name;
                    var trainName = trains.FirstOrDefault(x => x.Id == item.TrainsId)?.Name;
                    wagon.TrainName = trainName;
                    wagon.WagonName = wagonName;
                    wagon.ArrivalCity = arcity;
                    wagon.DepartureCity = depcity;
                    wagon.CountOfPlaces = item.Count;
                    wagon.RouteId = id;
                    wagonVM.Add(wagon);
                }
            }
            else if (category == "Купе")
            {
                wagons = wagons.Where(x => x.TypeId == 2).ToList();
                foreach (var item in wagons)
                {
                    WagonsView wagon = new();
                    wagon.Route = routes;
                    wagon.Wagons = wagons;
                    wagon.Trains = trains;
                    wagon.WagonTypeName = category;
                    wagon.Price = price;
                    var wagonId = wagons.FirstOrDefault(x => x.Id == item.Id).Id;
                    wagon.WagonId = wagonId;
                    var trainId = trains.FirstOrDefault(x => x.Id == item.TrainsId).Id;
                    wagon.TrainId = trainId;
                    var wagonName = wagons.FirstOrDefault(x => x.Id == item.Id)?.Name;
                    var trainName = trains.FirstOrDefault(x => x.Id == item.TrainsId)?.Name;
                    wagon.TrainName = trainName;
                    wagon.WagonName = wagonName;
                    wagon.ArrivalCity = arcity;
                    wagon.DepartureCity = depcity;
                    wagon.CountOfPlaces = item.Count;
                    wagon.RouteId = id;
                    wagonVM.Add(wagon);
                }
            }
            return View(wagonVM);
        }
        public async Task<Models.Route> FoundRoute(int Id)
        {
            var result = await _db.Routes
                .Include(x => x.DepartureCity)
                .Include(x => x.ArrivalCity)
                .Where(x => x.Id == Id)
                .FirstOrDefaultAsync();
            return result;
        }

        [HttpGet]
        public IActionResult Places(int routeId, string count, string category, int trainId, int wagonId, string price, string seatNumber, string status)
        {
            var wagon = _db.Wagons.FirstOrDefault(w => w.Id == wagonId);
            var train = _db.Trains.FirstOrDefault(w => w.Id == trainId);
            var place = _db.Places.FirstOrDefault(p => p.WagonId == wagonId && p.Name == seatNumber);

            if (place != null && place.UserId != null)
            {
                var placesViewModel = new PlacesView
                {
                    Wagon = wagon,
                    WagonTypeName = category,
                    WagonId = wagonId,
                    Price = price,
                    Train = train,
                    WagonName = wagon?.Name,
                    TrainName = train?.Name,
                    PlaceName = seatNumber,
                    TrainId = trainId,
                    Status = "Куплен",
                    CountOfPlaces = count,
                    RouteId = routeId
                };
                return View(placesViewModel);
            }
            else
            {
                var placesViewModel = new PlacesView
                {
                    Wagon = wagon,
                    WagonTypeName = category,
                    Price = price,
                    WagonId = wagonId,
                    Train = train,
                    WagonName = wagon?.Name,
                    TrainName = train?.Name,
                    PlaceName = seatNumber,
                    TrainId = trainId,
                    Wagons = _db.Wagons.ToList(),
                    Trains = _db.Trains.ToList(),
                    CountOfPlaces = count,
                    RouteId = routeId
                };
                return View(placesViewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Places(int routeId, string count, int wagonId, int trainId, string seatNumber, string price, string category, PlacesView placesView)
        {
            var place = await _db.Places.FirstOrDefaultAsync(p => p.WagonId == wagonId && p.Name == seatNumber);

            if (place != null && place.UserId != null)
            {
                placesView.PlaceName = seatNumber;
                placesView.Status = "Куплен";
                TempData["Status"] = placesView.Status; // Сохраняем значение в TempData
                return RedirectToAction("Places", new { seatNumber = seatNumber, status = placesView.Status });
            }
            else
            {
                placesView.PlaceName = seatNumber;
                placesView.Status = "Свободен";
                TempData["Status"] = placesView.Status; // Сохраняем значение в TempData
                return RedirectToAction("Places", new { seatNumber = seatNumber, status = placesView.Status });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Credit(int routeId, string category, int trainId, int wagonId, string price, string placeName)
        {
            var routesTask = FoundRoute(routeId);
            var routes = await routesTask;
            var places = _db.Places
            .FirstOrDefault(p => p.UserId == null && p.WagonId == wagonId && p.Name == placeName);
            var wagons = _db.Wagons
              .Where(w => w.Id == wagonId).ToList();
            var wagon = _db.Wagons
                .FirstOrDefault(w => w.Id == wagonId);
            var train = _db.Trains
                .FirstOrDefault(w => w.Id == trainId);


            var orderVM = new OrderView
            {
                Route = routes,
                RouteId = routeId,
                Wagon = wagon,
                WagonTypeName = category,
                Price = price,
                WagonId = wagonId,
                Train = train,
                TrainName = train?.Name,
                WagonName = wagon?.Name,
                PlaceName = placeName,
                PlaceId = places.Id,
                TrainId = trainId,
            };

            return View(orderVM);
        }
    
    [HttpPost]
    public async Task<IActionResult> Credit(int id,int wagonId, int placeId, OrderView orderVM)
    {
                if (User.Identity.IsAuthenticated)
                {


                    Ticket ticket = new();
                    ticket.UserId = Int32.Parse(HttpContext.User.Identity.Name);
                    ticket.RouteId = id;
                    ticket.WagonId = wagonId;
                    ticket.PlaceId = placeId;
                    _db.Tickets.Add(ticket);
                    await _db.SaveChangesAsync();

                }
                else
                    return RedirectToAction("Login");

                return RedirectToAction("Index");
    }
        [HttpGet]
        public async Task<IActionResult> HistoryOrder()
        {
            var result = await _db.Tickets.Include(t => t.Route)
        .Include(t => t.Wagon)
        .Include(t => t.Place)
        .Where(x => x.UserId == Int32.Parse(HttpContext.User.Identity.Name))
        .ToListAsync();

            List<OrderView> orderVMList = new();
            foreach (var item in result)
            {
                OrderView orderVM = new();
                var route = await _db.Routes.Where(x => x.Id == item.RouteId).FirstOrDefaultAsync();
                orderVM.Route = route;
                var train = await _db.Trains.Where(x => x.Id == route.TrainsId).FirstOrDefaultAsync();
                orderVM.Train = train;
                var wagon = await _db.Wagons.Where(x => x.Id == item.WagonId).FirstOrDefaultAsync();
                orderVM.Wagon = wagon;
                var type = wagon.TypeId;
                var types = await _db.Types.Where(x => x.Id == type).FirstOrDefaultAsync();

                orderVM.WagonTypeName = types.Name;
                if(orderVM.WagonTypeName == "Плацкарт")
                {
                    orderVM.Price = route.PriceEconom.ToString();
                }
                else if(orderVM.WagonTypeName == "Купе")
                {
                    orderVM.Price = route.PriceCoupe.ToString();
                }    
                orderVM.WagonName = wagon.Name;
                orderVM.TrainName = train.Name;
                orderVM.TicketId = item.Id;
                var place = await _db.Places.Where(x => x.Id == item.PlaceId).FirstOrDefaultAsync();
                orderVM.PlaceName = place.Name;
                orderVM.PlaceId = place.Id;
                var depCity = route.DepartureCityId;
                var deparCity = await _db.Cities.Where(x => x.Id == depCity).FirstOrDefaultAsync();
                orderVM.DepartureCity = deparCity.Name;
                var arCity = route.ArrivalCityId;
                var arrCity = await _db.Cities.Where(x => x.Id == arCity).FirstOrDefaultAsync();
                orderVM.ArrivalCity = arrCity.Name;
                orderVMList.Add(orderVM);
            }

            return View(orderVMList);
        }
        public async Task<IActionResult> Delete(int id)
        {

            var result = await _db.Tickets.FirstOrDefaultAsync(x => x.Id == id);
            var placeId = result.PlaceId;
            var place = await _db.Places.FirstOrDefaultAsync(x => x.Id == placeId);
            if (place != null)
            {
                place.UserId = null;
                await _db.SaveChangesAsync();
            }
            _db.Tickets.Remove(result);

            await _db.SaveChangesAsync();
            return RedirectToAction("HistoryOrder");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateUser()
        {
            int userId = Int32.Parse(HttpContext.User.Identity.Name);
            var result = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(User user)
        {
            var result = await _db.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
            result.Name = user.Name;
            result.Surname = user.Surname;
            result.MiddleName = user.MiddleName;
            result.PassportData = user.PassportData;

            _db.Users.Update(result);
            await _db.SaveChangesAsync();
            return RedirectToAction("PersonalArea");
        }

    }

}