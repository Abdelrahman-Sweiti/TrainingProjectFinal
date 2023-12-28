using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PersonalProject.Models;

namespace PersonalProject.Models.Services
{
    public class CardService
    {
        private readonly HttpClient _httpClient;

        public CardService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse> GetCardsAsync(string name)
        {
            var apiUrl = $"https://db.ygoprodeck.com/api/v7/cardinfo.php?name={name}";

            var response = await _httpClient.GetStringAsync(apiUrl);
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response);

            // Process the API response and extract the card information
            if (apiResponse?.Data != null && apiResponse.Data.Count > 0)
            {
                foreach (var card in apiResponse.Data)
                {
                    // Check if CardImages is not null before iterating over it
                    if (card.CardImages != null)
                    {
                        // Update image URLs to include the full URLs from the API
                        foreach (var image in card.CardImages)
                        {
                            if (image != null) // additional check for individual images
                            {
                                image.ImageUrl = $"https://images.ygoprodeck.com/images/cards/{image.Id}.jpg";
                                image.ImageUrlSmall = $"https://images.ygoprodeck.com/images/cards_small/{image.Id}.jpg";
                                image.ImageUrlCropped = $"https://images.ygoprodeck.com/images/cards_cropped/{image.Id}.jpg";
                            }
                        }
                    }
                }
            }

            return apiResponse;
        }
    }
}
