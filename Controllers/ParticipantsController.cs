using Microsoft.AspNetCore.Mvc;
using TravelSplitAPI.Models;
using TravelSplitAPI.Data;

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

            participant.Id = Participants.Count == 0
                ? 1
                : Participants.Max(p => p.Id) + 1;

            // 所屬旅程以網址傳入的編號為準
            participant.TripId = tripId;
            Participants.Add(participant);
            return StatusCode(201, participant);
        
        }
    }
}