using Microsoft.AspNetCore.Mvc;
using RESTful_API.Models;

namespace RESTful_API.Controllers
{
    [ApiController]
    [Route("api/trips")]
    public class TripsController : ControllerBase
    {
        private static readonly List<Trip> Trips = new List<Trip>
        {
            new Trip
            {
                Id = 1,
                Name = "金廈小三通",
                Destination = "金門、廈門",
                StartDate = new DateTime(2026, 11, 14),
                EndDate = new DateTime(2026, 11, 16)
            },
            new Trip
            {
                Id = 2,
                Name = "台南美食之旅",
                Destination = "台南",
                StartDate = new DateTime(2026, 12, 5),
                EndDate = new DateTime(2026, 12, 6)
            }
        };

        // 查詢全部旅程
        [HttpGet]
        public ActionResult<List<Trip>> GetTrips()
        {
            return Ok(Trips);
        }

        // 依編號查詢單一旅程
        [HttpGet("{id:int}")]
        public ActionResult<Trip> GetTrip(int id)
        {
            var trip = Trips.FirstOrDefault(t => t.Id == id);

            if (trip == null)
            {
                return NotFound(new
                {
                    message = $"找不到編號為 {id} 的旅程"
                });
            }

            return Ok(trip);
        }

        // 新增旅程
        [HttpPost]
        public ActionResult<Trip> CreateTrip([FromBody] Trip trip)
        {
            if (string.IsNullOrWhiteSpace(trip.Name))
            {
                return BadRequest(new
                {
                    message = "旅程名稱不能空白"
                });
            }
            if (trip.EndDate < trip.StartDate)
            {
                return BadRequest(new
                {
                    message = "結束日期不能早於開始日期"
                });
            }

            // 由伺服器產生編號（清單沒有資料就從 1 開始，否則取最大編號加1
            trip.Id = Trips.Count == 0
                ? 1
                : Trips.Max(t => t.Id) + 1;

            Trips.Add(trip);

            return CreatedAtAction(
                nameof(GetTrip),
                new { id = trip.Id },
                trip);
        }

        // 修改旅程
        [HttpPut("{id:int}")]
        public IActionResult UpdateTrip(int id, [FromBody] Trip updatedTrip)
        {
            var trip = Trips.FirstOrDefault(t => t.Id == id);

            if (trip == null)
            {
                return NotFound(new
                {
                    message = $"找不到編號為 {id} 的旅程"
                });
            }

            if (string.IsNullOrWhiteSpace(updatedTrip.Name))
            {
                return BadRequest(new
                {
                    message = "旅程名稱不能空白"
                });
            }

            if (updatedTrip.EndDate < updatedTrip.StartDate)
            {
                return BadRequest(new
                {
                    message = "結束日期不能早於開始日期"
                });
            }

            trip.Name = updatedTrip.Name;
            trip.Destination = updatedTrip.Destination;
            trip.StartDate = updatedTrip.StartDate;
            trip.EndDate = updatedTrip.EndDate;

            return NoContent();
        }
    }
}