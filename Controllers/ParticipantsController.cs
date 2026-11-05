using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelSplitAPI.Data;
using TravelSplitAPI.Models;

namespace TravelSplitAPI.Controllers
{
    [ApiController]
    [Route("api/trips/{tripId:int}/participants")]
    public class ParticipantsController : ControllerBase
    {
        private static readonly List<Participant> Participants =
            new List<Participant>
            {
                new Participant
                {
                    Id = 1,
                    TripId = 1,
                    Name = "小明"
                },
                new Participant
                {
                    Id = 2,
                    TripId = 1,
                    Name = "小華"
                },
                new Participant
                {
                    Id = 3,
                    TripId = 2,
                    Name = "小美"
                }
            };

        private readonly TripSqlite _db;

        // 接收 Program.cs 已註冊的 TripSqlite，供各方法查詢旅程。
        public ParticipantsController(TripSqlite db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<List<Participant>>> GetParticipants(int tripId)
        {
            // 等待 SQLite 確認旅程是否存在，再查詢該旅程的旅伴清單。
            if (!await _db.Trips.AnyAsync(t => t.Id == tripId))
            {
                return NotFound(new
                {
                    message = $"找不到編號為 {tripId} 的旅程"
                });
            }

            var participants = Participants
                .Where(p => p.TripId == tripId)
                .ToList();

            return Ok(participants);
        }

        [HttpPost]
        public async Task<ActionResult<Participant>> CreateParticipant(
            int tripId,
            [FromBody] Participant participant)
        {
            if (!await _db.Trips.AnyAsync(t => t.Id == tripId))
            {
                return NotFound(new
                {
                    message = $"找不到編號為 {tripId} 的旅程"
                });
            }

            if (string.IsNullOrWhiteSpace(participant.Name))
            {
                return BadRequest(new
                {
                    message = "旅伴名稱不能空白"
                });
            }

            // 只看這趟旅程的旅伴，從該旅程的最大編號加 1。
            participant.Id = Participants
                .Where(p => p.TripId == tripId)
                .Select(p => p.Id)
                .DefaultIfEmpty(0)
                .Max() + 1;

            participant.TripId = tripId;
            Participants.Add(participant);

            return CreatedAtAction(
                nameof(GetParticipant),
                new { tripId, id = participant.Id },
                participant);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Participant>> GetParticipant(int tripId, int id)
        {
            // 單筆查詢也先檢查旅程，旅程刪除後就會回傳 404。
            if (!await _db.Trips.AnyAsync(t => t.Id == tripId))
            {
                return NotFound(new
                {
                    message = $"找不到編號為 {tripId} 的旅程"
                });
            }

            var participant = Participants.FirstOrDefault(
                p => p.TripId == tripId && p.Id == id);

            if (participant == null)
            {
                return NotFound(new
                {
                    message = $"在旅程 {tripId} 中找不到編號 {id} 的旅伴"
                });
            }

            return Ok(participant);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> updateParticipant(
            int tripId,
            int id,
            [FromBody] Participant updatedParticipant)
        {
            if (!await _db.Trips.AnyAsync(t => t.Id == tripId))
            {
                return NotFound(new
                {
                    message = $"找不到編號為 {tripId} 的旅程"
                });
            }

            var participant = Participants.FirstOrDefault(
                p => p.TripId == tripId && p.Id == id);

            if (participant == null)
            {
                return NotFound(new
                {
                    message = $"在旅程 {tripId} 中找不到編號 {id} 的旅伴"
                });
            }

            if (string.IsNullOrWhiteSpace(updatedParticipant.Name))
            {
                return BadRequest(new
                {
                    message = "旅伴名稱不能空白"
                });
            }

            participant.Name = updatedParticipant.Name;

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteParticipant(int tripId, int id)
        {
            if (!await _db.Trips.AnyAsync(t => t.Id == tripId))
            {
                return NotFound(new
                {
                    message = $"找不到編號為 {tripId} 的旅程"
                });
            }

            var participant = Participants.FirstOrDefault(
                p => p.TripId == tripId && p.Id == id);

            if (participant == null)
            {
                return NotFound(new
                {
                    message = $"在旅程 {tripId} 中找不到編號 {id} 的旅伴"
                });
            }

            Participants.Remove(participant);

            return NoContent();
        }
    }
}