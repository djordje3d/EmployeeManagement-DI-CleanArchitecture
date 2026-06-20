
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Zaposleni_Clean_MVC_API.Data
{
    public class WebApiExecuter : IWebApiExecuter // Klasa MVC aplikacije za upravljanje komunikacijom sa API-jem koristeći HttpClient
    {
        private const string apiName = "ZaposleniApi";  // Ime API-ja koje koristimo za dobijanje konfigurisanog HttpClient objekta. Ovo ime je registrovano u Program.cs

        private readonly IHttpClientFactory httpClientFactory;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<WebApiExecuter> _logger;

        public WebApiExecuter(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, ILogger<WebApiExecuter> logger)
        {
            this.httpClientFactory = httpClientFactory;
            this.httpContextAccessor = httpContextAccessor;
            this._logger = logger;
        }

        // GET metoda za pozivanje API-ja
        public async Task<T> InvokeGet<T>(string relativeUrl)
        {
            var httpClient = httpClientFactory.CreateClient(apiName);
            SetAuthorizationHeader(httpClient); // Postavljanje JWT tokena u header

            _logger.LogInformation("Poziv GET: {Url}", relativeUrl);

            var request = new HttpRequestMessage(HttpMethod.Get, relativeUrl);
            var response = await httpClient.SendAsync(request);

            // Logujemo status odgovora
            _logger.LogInformation("Status odgovora: {StatusCode}", response.StatusCode);

            Console.WriteLine($"Response Status: {response.StatusCode}");

            // Proveravamo da li je odgovor uspešan pre parsiranja
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("GET zahtevu nije uspeo. Kod: {StatusCode}, Poruka: {Error}", response.StatusCode, errorContent);
                Console.WriteLine($"Error Content: {errorContent}");
                throw new Exception($"API request failed: {response.StatusCode}, Response: {errorContent}");
            }

            //    // Proveravamo da li je telo odgovora prazno
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Raw Content: {content}");

            if (string.IsNullOrWhiteSpace(content))
            {
                _logger.LogError("Prazan odgovor za URL: {Url}", relativeUrl);
                throw new Exception($"API response is empty for URL: {relativeUrl}");
            }

            _logger.LogInformation("API GET odgovor za {Url}: {Response}", relativeUrl, content);

            //    // Logujemo odgovor radi provere
            Console.WriteLine($"API Response for {relativeUrl}: {content}");

            await HandlePotentialError(response);
            return await response.Content.ReadFromJsonAsync<T>();
        }
        public async Task<T?> InvokePost<T>(string relativeUrl, T obj)
        {
            var httpClient = httpClientFactory.CreateClient(apiName); // koristimo httpClientFactory.CreateClient(ZaposleniApi) da dobijemo novu instancu HttpClient sa imenom ZaposleniApi koja je automatski konfigurisana.
            SetAuthorizationHeader(httpClient); // Postavljanje JWT tokena u header

            var response = await httpClient.PostAsJsonAsync<T>(relativeUrl, obj); // PostAsJsonAsync - Ova metoda šalje POST zahtev ka zadatom URL-u (relativeUrl) i
                                                                                  // serijalizuje prosleđeni objekat (obj) u JSON format pre nego što ga pošalje u telo zahteva
                                                                                  // response - Čuva odgovor (tipa HttpResponseMessage) koji API vraća nakon obrade zahteva.

            await HandlePotentialError(response);

            return await response.Content.ReadFromJsonAsync<T>();  //  Parsira telo odgovora iz JSON formata u objekat tipa T.
            //  Omogućava da direktno dobijete objekat sa podacima (npr. instancu Zaposlen ili List<Zaposlen>), bez potrebe za manuelnim parsiranjem JSON-a.
        }

        public async Task InvokePut<T>(string relativeUrl, T obj)
        {
            var httpClient = httpClientFactory.CreateClient(apiName); // koristimo httpClientFactory.CreateClient(ZaposleniApi) da dobijemo novu instancu HttpClient sa imenom ZaposleniApi koja je automatski konfigurisana.
            SetAuthorizationHeader(httpClient); // Postavljanje JWT tokena u header

            var response = await httpClient.PutAsJsonAsync<T>(relativeUrl, obj); // PutAsJsonAsync - Ova metoda šalje PUT zahtev ka zadatom URL-u (relativeUrl) i
                                                                                 // serijalizuje prosleđeni objekat (obj) u JSON format pre nego što ga pošalje u telo zahteva
                                                                                 // response - Čuva odgovor (tipa HttpResponseMessage) koji API vraća nakon obrade zahteva.

            await HandlePotentialError(response);
        }
        public async Task InvokeDelete(string relativeUrl)
        {
            var httpClient = httpClientFactory.CreateClient(apiName); // koristimo httpClientFactory.CreateClient(ZaposleniApi) da dobijemo novu instancu HttpClient sa imenom ZaposleniApi koja je automatski konfigurisana.
            //await AddJwtToHeader(httpClient); // Dodaje ručni JWT token u HTTP header pre slanja zahteva
            SetAuthorizationHeader(httpClient); // Postavljanje JWT tokena u header

            var response = await httpClient.DeleteAsync(relativeUrl); // DeleteAsync - Ova metoda šalje DELETE zahtev ka zadatom URL-u (relativeUrl).
                                                                      // response - Čuva odgovor (tipa HttpResponseMessage) koji API vraća nakon obrade zahteva.
            await HandlePotentialError(response);
        }

        public async Task<TResponse?> InvokePost<TRequest, TResponse>(string relativeUrl, TRequest obj)
        {
            try
            {
                var httpClient = httpClientFactory.CreateClient(apiName);

                var response = await httpClient.PostAsJsonAsync(relativeUrl, obj);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new Exception($"API greška: {response.StatusCode} - {errorMessage}");
                }

                return await response.Content.ReadFromJsonAsync<TResponse>();
            }
            catch (Exception ex)
            {
                // Logovanje greške ili dalja obrada
                throw new Exception($"Greška prilikom poziva API-ja: {ex.Message}");
            }
        }

        public async Task<TResponse?> InvokePut<TRequest, TResponse>(string relativeUrl, TRequest obj)
        {
            try
            {
                var httpClient = httpClientFactory.CreateClient(apiName);

                var response = await httpClient.PutAsJsonAsync(relativeUrl, obj);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new Exception($"API greška: {response.StatusCode} - {errorMessage}");
                }

                return await response.Content.ReadFromJsonAsync<TResponse>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Greška prilikom poziva API-ja: {ex.Message}");
            }
        }

        public async Task<TResponse?> InvokeDelete<TResponse>(string relativeUrl)
        {
            try
            {
                var httpClient = httpClientFactory.CreateClient(apiName);

                var response = await httpClient.DeleteAsync(relativeUrl);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new Exception($"API greška: {response.StatusCode} - {errorMessage}");
                }

                return await response.Content.ReadFromJsonAsync<TResponse>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Greška prilikom poziva API-ja: {ex.Message}");
            }
        }

        private async Task HandlePotentialError(HttpResponseMessage httpResponse)
        {
            if (!httpResponse.IsSuccessStatusCode) // Ako rezultat nije uspešan, baci izuzetak.
            {
                var errorJson = await httpResponse.Content.ReadAsStringAsync(); // Čita telo odgovora kao string i sada ga možemo deserijalizovati u objekat tipa ErrorResponse.
                _logger.LogError("Greška u odgovoru API-ja. Kod: {StatusCode}, Poruka: {ErrorJson}", httpResponse.StatusCode, errorJson);
                //var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(errorJson); // Deserijalizuje JSON string u objekat tipa ErrorResponse.
                throw new WebApiException(errorJson); // Baca izuzetak tipa WebApiException sa objektom tipa ErrorResponse kao argumentom.
            }
        }

        // Ako ti treba i POST bez odgovora, možeš imati i ovu verziju

        public async Task<TResponse?> InvokePostNeRadi<TRequest, TResponse>(string relativeUrl, TRequest obj) // ne uspeva u AccountControlleru
        {
            _logger.LogInformation("Poziv POST: {Url}", relativeUrl);

            var httpClient = httpClientFactory.CreateClient(apiName);
            SetAuthorizationHeader(httpClient);

            var response = await httpClient.PostAsJsonAsync(relativeUrl, obj);

            await HandlePotentialError(response);

            return await response.Content.ReadFromJsonAsync<TResponse>();
        } 
        public async Task<HttpResponseMessage> SendPostRequest<T>(string relativeUrl, T obj) // Ova metoda šalje POST zahtev i vraća HttpResponseMessage
                                                                                             // Ova metoda se koristi kada ne želimo da deserijalizujemo odgovor u objekat, već samo želimo da dobijemo status i eventualno telo odgovora.
                                                                                             // Pravi se ovde zbog AccountControllera
        {
            var httpClient = httpClientFactory.CreateClient(apiName); // Kreira HTTP klijent
            SetAuthorizationHeader(httpClient); // Postavlja JWT token u header

            // Šalje POST zahtev sa JSON-om
            var response = await httpClient.PostAsJsonAsync<T>(relativeUrl, obj);
            return response;
        }

        public async Task<T?> ProcessResponse<T>(HttpResponseMessage response)
        {
            // Proverava greške u odgovoru (ako su definisane)
            await HandlePotentialError(response);

            // Deserializuje JSON sadržaj u objekat tipa T
            return await response.Content.ReadFromJsonAsync<T>();
        }

        // Ova metoda se koristi za dodavanje JWT tokena u Authorization header HTTP zahteva.
        public void SetAuthorizationHeader(HttpClient httpClient) // HttpClient se koristi za slanje HTTP zahteva i primanje odgovora.
        {
            var token = httpContextAccessor.HttpContext?.Session.GetString("JwtToken"); // Dobija JWT token iz sesije korisnika. Ako je token prazan ili ne postoji, baca izuzetak.

            if (string.IsNullOrEmpty(token)) // Proverava da li je token prazan ili ne postoji
            {
                Console.WriteLine("Token is missing or expired.");
                throw new Exception("JWT token is missing or expired. Please login again.");
            }

            Console.WriteLine($"Token: {token}");

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); // Postavlja Authorization header sa tipom "Bearer" i vrednošću tokena.
                                                                                                             // Ovaj header se koristi za autentifikaciju prilikom slanja zahteva ka API-ju.
                                                                                                             // Bearer token je standardni način slanja JWT tokena u HTTP headeru.
        }

    }
}
