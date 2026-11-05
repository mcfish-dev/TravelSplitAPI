using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TravelSplitAPI.Data;
using TravelSplitAPI.Models;

namespace TravelSplitAPI.Controllers
{
    [ApiController]
    [Route("api/trips/{tripId:int}/expenses")]
    public class ExpensesController : ControllerBase
    {
        private static readonly List<Expense> Expenses = new List<Expense>
        {
            new Expense
            {
                Id = 1,
                TripId = 2,
                PaidByParticipantId = 3,
                Description = "午餐",
                Amount = 120m
            }
        };

        [HttpGet]
        public ActionResult<List<Expense>> GetExpenses(int tripId)
        {
            if (!TripStore.Trips.Any(t => t.Id == tripId))
            {
                return NotFound(new
                {
                    message = $"找不到編號為 {tripId} 的旅程"
                });
            }

            var expenses = Expenses
                .Where(e => e.TripId == tripId)
                .ToList();

            return Ok(expenses);
        }
    }
}