using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using Jose;
using TranslationAPI.Structs;

namespace TranslationAPI
{
    /// <summary>
    /// Client for making all google api translation requests
    /// </summary>
    public class TranslationClient
    {
        /*Info*/
        /// <summary>
        /// Client for POST and GET requests
        /// </summary>
        public HttpClient Client = new HttpClient();
        /// <summary>
        /// Token for accessing google api, is temporary and expires after a certain amount of time
        /// </summary>
        public string AccessToken { get; private set; }

        /*Cache*/
        /// <summary>
        /// Client credentials
        /// </summary>
        private LoginCredential Credential = new LoginCredential();
        /// <summary>
        /// Token expiration timestamp
        /// </summary>
        private double ExpireTick = 0;

        #region Main functions
        /// <summary>
        /// Initialize the client and request an access token
        /// </summary>
        /// <param name="credential">Credentials used to initialize the client and to run requests</param>
        public async Task LoginAsync(LoginCredential credential)
        {
            Credential = credential;
            await RequestAccessToken();
        }

        /// <summary>
        /// Detects the language being used
        /// </summary>
        /// <param name="text">The language to detect</param>
        /// <param name="mimeType">The format of the text being detected, can be either "text/plain"(default) or "text/html"</param>
        public async Task<LanguageDetectResponse> DetectLanguageAsync(string text, string mimeType = "text/plain")
        {
            if (!await CheckValidClient()) return null;

            Dictionary<string, string> content = new Dictionary<string, string>
            {
                { "content", text },
                { "mimeType", mimeType }
            };

            HttpResponseMessage response = await Client.PostAsync($"{Credential.Endpoint}/v3/projects/{Credential.ProjectID}:detectLanguage", new FormUrlEncodedContent(content));
            string responseContent = await response.Content.ReadAsStringAsync();
            LanguageDetectResponse languageDetectResponse = JsonConvert.DeserializeObject<LanguageDetectResponse>(responseContent);
            return languageDetectResponse;
        }

        /// <summary>
        /// Gets an array of supported languages used by the translation api
        /// </summary>
        public async Task<SupportedLanguagesResponse> GetSupportedLanguagesAsync()
        {
            if (!await CheckValidClient()) return null;

            HttpResponseMessage response = await Client.GetAsync($"{Credential.Endpoint}/v3/projects/{Credential.ProjectID}/supportedLanguages");
            string responseContent = await response.Content.ReadAsStringAsync();
            SupportedLanguagesResponse supportedLanguagesResponse = JsonConvert.DeserializeObject<SupportedLanguagesResponse>(responseContent);
            return supportedLanguagesResponse;
        }

        /// <summary>
        /// Detects and translates text to another language
        /// </summary>
        /// <param name="text">The language to translate</param>
        /// <param name="languageCode">The language to translate to(default is "en")</param>
        /// <param name="mimeType">The format of the text being translated, can be either "text/plain"(default) or "text/html"</param>
        public async Task<TranslatedTextResponse> TranslateTextAsync(string text, string languageCode = "en", string mimeType = "text/plain")
        {
            if (!await CheckValidClient()) return null;

            Dictionary<string, string> content = new Dictionary<string, string>
            {
                { "contents", text },
                { "targetLanguageCode", languageCode },
                { "mimeType", mimeType }
            };

            HttpResponseMessage response = await Client.PostAsync($"{Credential.Endpoint}/v3/projects/{Credential.ProjectID}:translateText", new FormUrlEncodedContent(content));
            string responseContent = await response.Content.ReadAsStringAsync();
            TranslatedTextResponse translatedTextResponse = JsonConvert.DeserializeObject<TranslatedTextResponse>(responseContent);
            return translatedTextResponse;
        }
        #endregion

        #region Functions
        /// <summary>
        /// Checks to see if client has logged in
        /// </summary>
        async Task<bool> CheckValidClient()
        {
            DateTime utc0 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            DateTime issueTime = DateTime.Now.ToUniversalTime();
            if (ExpireTick <= (int)issueTime.Subtract(utc0).TotalSeconds)
            {
                bool success = await RequestAccessToken();
                if(!success) Console.WriteLine($"Client has not successfully logged in! Call Login() before calling any other client methods @TranslationClient");
                return success;
            }
            return true;
        }

        /// <summary>
        /// Request/renew a new access token
        /// </summary>
        async Task<bool> RequestAccessToken()
        {
            try
            {
                /*JWT Generation*/
                //Load p12
                X509Certificate2 certificate = new X509Certificate2(Credential.P12Path, Credential.P12Password);

                //Encoder values
                DateTime utc0 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                DateTime issueTime = DateTime.Now.ToUniversalTime();
                double iat = (int)issueTime.Subtract(utc0).TotalSeconds;
                double exp = (int)issueTime.AddMinutes(60).Subtract(utc0).TotalSeconds;

                //Header construction
                Dictionary<string, object> header = new Dictionary<string, object>
                {
                    { "typ", "JWT"},
                    { "kid", Credential.ServiceAccountPrivateKeyId }
                };

                //Payload construction
                Dictionary<string, object> payload = new Dictionary<string, object>
                {
                    { "iss", Credential.ServiceAccountEmail },
                    { "sub", Credential.ServiceAccountEmail },
                    { "aud", Credential.AuthenticationTokenURI },
                    { "iat", iat },
                    { "exp", exp },
                    { "scope", Credential.AuthenticationScope }
                };

                //Generate signed JWT
                RSA privateKey = new X509Certificate2(Credential.P12Path, Credential.P12Password, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet).GetRSAPrivateKey();
                string signedJwtToken = Jose.JWT.Encode(payload, privateKey, JwsAlgorithm.RS256, header);

                /*Request new access token*/
                Dictionary<string, string> content = new Dictionary<string, string>
                {
                    { "grant_type", "urn:ietf:params:oauth:grant-type:jwt-bearer" },
                    { "assertion", signedJwtToken },
                };
                HttpResponseMessage response = await Client.PostAsync("https://oauth2.googleapis.com/token", new FormUrlEncodedContent(content));
                string responseContent = await response.Content.ReadAsStringAsync();
                TokenResponse tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);

                //Results
                if (!string.IsNullOrEmpty(tokenResponse.AccessToken))
                {
                    AccessToken = tokenResponse.AccessToken;
                    ExpireTick = exp;
                    Client.DefaultRequestHeaders.Clear();
                    Client.DefaultRequestHeaders.Add("Authorization", $"Bearer {AccessToken}");
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
