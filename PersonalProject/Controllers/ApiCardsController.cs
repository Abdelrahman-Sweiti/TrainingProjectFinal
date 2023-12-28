// ApiCardsController.cs
using Microsoft.AspNetCore.Mvc;
using PersonalProject.Models.Services;
using System.Threading.Tasks;

namespace PersonalProject.Models.Services
{

    [ApiController]
    [Route("api/cards")]
    public class ApiCardsController : ControllerBase
    {
        private readonly CardService _cardService;

        public ApiCardsController(CardService cardService)
        {
            _cardService = cardService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCards(string cardName)
        {
            var cards = await _cardService.GetCardsAsync(cardName);
            return Ok(cards);
        }
    }
}