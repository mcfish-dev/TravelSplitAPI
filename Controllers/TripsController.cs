using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using TravelSplitAPI.Models;
using TravelSplitAPI.Data;

namespace TravelSplitAPI.Controllers
{
    [ApiController]
    [Route("api/trips")]
    public class TripsController : ControllerBase
    {
        // 暫時保留記憶體清單，單筆查詢、修改和刪除還會用到。
        private static readonly List<Trip> Trips = TripStore.Trips;

        // 新增：保存 ASP.NET Core 傳進來的 SQLite 資料庫操作物件。
        private readonly TripSqlite _db;

        // 新增：透過建構函式接收 Program.cs 已註冊的 TripSqlite。
        public TripsController(TripSqlite db)
        {
            _db = db;
        }

        // 查詢全部旅程

        // 新版：等 SQLite 查詢完成後，回傳旅程清單。
        [HttpGet]
        public async Task<ActionResult<List<Trip>>> GetTrips()
        {
            var trips = await _db.Trips.ToListAsync();
            return Ok(trips);
        }

        // 依編號查詢單一旅程
        [HttpGet("{id:int}")]
        // 舊版：public ActionResult<Trip> GetTrip(int id)
        public async Task<ActionResult<Trip>> GetTrip(int id)
        {
            // 舊版：從記憶體清單查詢。
            // var trip = Trips.FirstOrDefault(t => t.Id == id);

            // 新版：用旅程編號到 SQLite 查詢。
            var trip = await _db.Trips.FindAsync(id);

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
        // 舊版：public ActionResult<Trip> CreateTrip([FromBody] Trip trip)
        // 新版：使用 async，讓方法可以等待資料庫儲存完成。
        public async Task<ActionResult<Trip>> CreateTrip([FromBody] Trip trip)
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

            // 新版：將 Id 設為預設值，讓 SQLite 分配編號。
            trip.Id = 0;

            // 將這筆旅程標記為待新增，再實際寫入 SQLite。
            _db.Trips.Add(trip);
            await _db.SaveChangesAsync();

            // 儲存完成後，trip.Id 已經有資料庫分配的編號。
            return CreatedAtAction(
                nameof(GetTrip),
                new { id = trip.Id },
                trip);
        }

        // 修改旅程
        [HttpPut("{id:int}")]
        // 新版：加上 async，讓方法可以用 await 等待查詢和儲存。
        public async Task<IActionResult> updateTrip(int id, [FromBody] Trip updatedTrip)
        {
            // 新版：用網址上的 id，從 SQLite 找出要修改的旅程。
            var trip = await _db.Trips.FindAsync(id);

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

            // 新增：EF Core 會追蹤 trip 的欄位變更，這行才把變更寫入 SQLite。
            await _db.SaveChangesAsync();

            return NoContent();
        }

        // 刪除旅程
        [HttpDelete("{id:int}")]
        // 新版：加上 async，讓方法可以等待查詢和儲存完成。
        public async Task<IActionResult> DeleteTrip(int id)
        {
            // 新版：依照網址上的 id，從 SQLite 找出旅程。
            var trip = await _db.Trips.FindAsync(id);

            if (trip == null)
            {
                return NotFound(new
                {
                    message = $"找不到編號為 {id} 的旅程"
                });
            }

            // 新版：先把這筆旅程標記為待刪除。
            _db.Trips.Remove(trip);

            // 新增：把刪除寫入 SQLite。
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}