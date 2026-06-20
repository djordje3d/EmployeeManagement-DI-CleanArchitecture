using System.Text.Json;

namespace Zaposleni_Clean_MVC_API.Data
{
    public class WebApiException : Exception
    {
        public ErrorResponse? ErrorResponse { get; } // Deserializovani objekat tipa ErrorResponse, koji sadrži informacije o grešci vraćenoj od API-ja (npr. Title, Status, i Errors polja).

        public WebApiException(string errorJson) // Parametar "errorJson" je JSON string koji API vraća kao odgovor kada dođe do greške(obično telo HTTP odgovora).
        {
            try
            {
                ErrorResponse = JsonSerializer.Deserialize<ErrorResponse>(errorJson); // Deserijalizuje JSON string "errorJson" u objekat tipa ErrorResponse.
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Deserialization error: {ex.Message}");
                ErrorResponse = new ErrorResponse { Title = "Unexpected error", Errors = new Dictionary<string, List<string>>() };
            }
        }
    }
}
