using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GraphApi
{
    class Program
    {

        private const string clientId = "";
        private const string instanceAD = "https://login.microsoftonline.com/{0}";
        private const string tennantId = "";
        private const string resource = "https://graph.microsoft.com";
        private const string appKey = "";

        static string authority = String.Format(CultureInfo.InvariantCulture, instanceAD, tennantId);

        private static HttpClient httpClient = new HttpClient();
        private static AuthenticationContext context = null;
        private static ClientCredential credential = null;

        static void Main(string[] args)
        {
            context = new AuthenticationContext(authority);
            credential = new ClientCredential(clientId, appKey);

            Task<string> token = GetToken();

            Console.WriteLine(token.Result);

            Console.WriteLine("-----------------------------------------------------------------------------------------------");

            Task<string> profile = GetMyProfile(token.Result);

            Console.WriteLine(profile.Result);

            var jobject = JsonConvert.DeserializeObject<JObject>(profile.Result);

            var mail = jobject["mail"].Value<string>();

            Console.ReadKey();
        }

        private static async Task<string> GetMyProfile(string result)
        {
            string profile = null;
            // string queryString = "api-version=1.6";

            string uri = "";

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result);

            var getResult = await httpClient.GetAsync(uri);

            if(getResult.Content != null)
            {
                profile = await getResult.Content.ReadAsStringAsync();
            }

            return profile;
        }

        private static async Task<string> GetToken()
        {
            AuthenticationResult result = null;
            string token = null;

            result = await context.AcquireTokenAsync(resource, credential);
            token = result.AccessToken;
            return token;
        }
    }
}
