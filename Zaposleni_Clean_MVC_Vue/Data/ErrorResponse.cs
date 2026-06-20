using System.Text.Json.Serialization;

namespace Zaposleni_Clean_MVC_Vue.Data
{
    public class ErrorResponse
    {
        [JsonPropertyName("title")] // Atribut koji osigurava da se prilikom konverzije između JSON-a i objekta ovo svojstvo mapira na ključno polje "title" u JSON-u.
        public string? Title { get; set; } // Naslov greške, npr. "Validation Error" ili "Unauthorized Access"

        [JsonPropertyName("status")] // Atribut [JsonPropertyName] mapira svojstvo Status na JSON ključ "status".
        public int Status { get; set; } // Ovo svojstvo predstavlja HTTP statusni kod odgovora o kom tipu greške se radi Npr. 400, 401, 404, 500, itd.

        [JsonPropertyName("errors")]
        public Dictionary<string, List<string>> Errors { get; set; } // Ovo svojstvo predstavlja mapu koja sadrži listu grešaka za svako svojstvo koje nije prošlo validaciju.

        // sadrži detaljne greške u formi Dictionary<string, List<string>>, što omogućava jasno organizovanje informacija o više grešaka istovremeno.
        // Ključ u Dictionary predstavlja naziv svojstva koje NIJE prošlo validaciju, a vrednost je lista grešaka koje su se desile prilikom validacije tog svojstva.
        // Fleksibilnost: Kroz Dictionary strukturu grešaka, aplikacija podržava prikazivanje više PORUKA za više POLJA U ISTO vreme.
    }
}