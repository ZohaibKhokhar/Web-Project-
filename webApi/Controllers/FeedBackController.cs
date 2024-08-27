using Domain.Entities;
using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace webApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedBackController : ControllerBase
    {
        private readonly IFeedBackService _feedBackService;

        public FeedBackController(IFeedBackService feedBackService)
        {
            _feedBackService = feedBackService;
        }

 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeedBack>>> GetAllFeedBacks()
        {
            var feedbacks = await _feedBackService.GetAll();
            return Ok(feedbacks);
        }

     
        [HttpPost]
        public async Task<ActionResult> AddFeedBack([FromBody] FeedBack feedback)
        {
            if (ModelState.IsValid)
            {
                await _feedBackService.Add(feedback);
                return CreatedAtAction(nameof(GetAllFeedBacks), new { id = feedback.Id }, feedback);
            }
            return BadRequest(ModelState);
        }
    }
}
