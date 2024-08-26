using Microsoft.AspNetCore.Mvc;
using FlashQuizzz.API.Models;
using FlashQuizzz.API.Services;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FlashQuizzz.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlashCardController : ControllerBase
    {
        private readonly IFlashCardService _flashCardService;

        public FlashCardController(IFlashCardService flashCardService)
        {
            _flashCardService = flashCardService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFlashCard([FromBody] FlashCardDTO newFlashCard)
        {
            if (newFlashCard == null)
                return BadRequest("FlashCard data is required.");

            var flashCard = await _flashCardService.CreateFlashCard(newFlashCard);
            if (flashCard == null)
                return BadRequest("Failed to create FlashCard.");

            return CreatedAtAction(nameof(GetFlashCardById), new { id = flashCard.FlashCardID }, flashCard);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlashCard(int id)
        {
            var flashCard = await _flashCardService.Delete(id);
            if (flashCard == null)
                return NotFound();

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFlashCards()
        {
            var flashCards = await _flashCardService.GetAllFlashCards();
            return Ok(flashCards);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFlashCardById(int id)
        {
            var flashCard = await _flashCardService.GetFlashCardById(id);
            if (flashCard == null)
                return NotFound();

            return Ok(flashCard);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(string userId)
        {
            var flashCards = await _flashCardService.GetByUser(userId);
            return Ok(flashCards);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFlashCard(int id, [FromBody] FlashCardDTO updatedFlashCard)
        {
            if (updatedFlashCard == null)
                return BadRequest("FlashCard data is required.");

            var result = await _flashCardService.Update(id, updatedFlashCard);
            if (!result)
                return BadRequest("Failed to update FlashCard.");

            return NoContent();
        }

        // Other endpoints based on the service methods can be added here.
    }
}
