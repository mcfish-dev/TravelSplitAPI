using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;
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

        // 查詢指定旅程的旅伴
        [HttpGet]
        public ActionResult<List<Participant>> GetParticipants(int tripId)
        {
            if (!TripStore.Trips.Any(t => t.Id == tripId))
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

        // 新增旅伴
        [HttpPost]
        public ActionResult<Participant> CreateParticipant(
            int tripId,
            [FromBody] Participant participant)
        {
            if (!TripStore.Trips.Any(t => t.Id == tripId))
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

            // 由伺服器分配旅伴編號
            participant.Id = Participants.Count == 0
                ? 1
                : Participants.Max(p => p.Id) + 1;

            // 所屬旅程以網址傳入的編號為準
            participant.TripId = tripId;

            Participants.Add(participant);

            return CreatedAtAction(
                nameof(GetParticipant),
                new { tripId, id = participant.Id },
                participant);
        }

        // 查詢指定旅程中的一位旅伴
        [HttpGet("{id:int}")]
        public ActionResult<Participant> GetParticipant(int tripId, int id)
        {
            var participant = Participants.FirstOrDefault(
                p => p.TripId == tripId && p.Id == id);
            if (participant == null)
            {
                return NotFound(new { message = $"在旅程 {tripId} 中找不到編號 {id} 的旅伴" });
            }
            return Ok(participant);
        }

        // 備改指定旅程中的一位旅伴
        [HttpPut("{id:int}")]
        public IActionResult updateParticipant(
            int tripId,
            int id,
            [FromBody] Participant updatedParticipant)
        {
            if (!TripStore.Trips.Any(t => t.Id == tripId))
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
                    message = $"在旅程 {tripId} 中找不到編號為 {id} 的旅伴"
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

        // 刪除指定旅程中的一位旅伴
        [HttpDelete("{id:int}")]
        public IActionResult DeleteParticipant(int tripId, int id)
        {
            if (!TripStore.Trips.Any(t => t.Id == tripId))
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
                    message = $"在旅程 {tripId} 中找不到編號為 {id} 的旅伴"
                });
            }

            Participants.Remove(participant);

            return NoContent();
        }
    }
        
}