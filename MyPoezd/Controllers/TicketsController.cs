using MyPoezd.Models;
using MyPoezd.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPoezd;

namespace Poezd.Controllers
{
    public class TicketsController : Controller
    {
        private readonly MyTrainContext _db;

        public TicketsController(MyTrainContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<IActionResult> TicketsIndex()
        {
            var result = await _db.Tickets.Include(t => t.Route.ArrivalCity)
         .Include(t => t.Route.DepartureCity)
         .Include(t => t.Wagon)
         .Include(t => t.Place)
         .ToListAsync();

            return View(result);
        }

        public async Task<IActionResult> Delete(int id)
        {

            var result = await _db.Tickets.FirstOrDefaultAsync(x => x.Id == id);
            var selectedUserId = result.UserId;
            _db.Tickets.Remove(result);
            _db.Places.Remove(_db.Places.Find(selectedUserId));
            await _db.SaveChangesAsync();
            return RedirectToAction("TicketsIndex");
        }

       
    }
}
