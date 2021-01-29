using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TranslationAPI;
using TranslationAPI.Structs;

namespace Example
{
    class Program
    {
        public static TranslationClient GoogleClient = new TranslationClient();

        static void Main(string[] args) => new Program().Start().GetAwaiter().GetResult();

        async Task Start()
        {
            LoginCredential credential = new LoginCredential
            {
                P12Path = Application.StartupPath + "/Credentials/myprojectid-8538c731jz3h.p12",
                P12Password = "notasecret",
                ServiceAccountPrivateKeyId = "6747l853fe4h5678db01tu8caa08768ce30796nm",
                ServiceAccountEmail = "myserviceaccount@projectid.iam.gserviceaccount.com",
                AuthenticationTokenURI = "https://oauth2.googleapis.com/token",
                AuthenticationScope = "https://www.googleapis.com/auth/cloud-translation",
                ProjectID = "projectid",
                Endpoint = "https://translation.googleapis.com"
            };

            await GoogleClient.LoginAsync(credential);
            LanguageDetectResponse detectResponse = await GoogleClient.DetectLanguageAsync("こんにちは、元気ですか");
            SupportedLanguagesResponse languagesResponse = await GoogleClient.GetSupportedLanguagesAsync();
            TranslatedTextResponse textResponse = await GoogleClient.TranslateTextAsync("こんにちは、元気ですか");

            Console.WriteLine(GoogleClient.AccessToken);

            for (int i = 0; i < detectResponse.LanguageItems.Length; i++)
            {
                Console.WriteLine("Language code detected: " + detectResponse.LanguageItems[i].LanguageCode);
            }

            for (int i = 0; i < languagesResponse.CompatibleLanguages.Length; i++)
            {
                Console.WriteLine("Supported languages: " + languagesResponse.CompatibleLanguages[i].LanguageCode);
            }

            for (int i = 0; i < textResponse.TranslatedTexts.Length; i++)
            {
                Console.WriteLine("Translated: " + textResponse.TranslatedTexts[i].TranslatedText);
            }
            await Task.Delay(-1);
        }
    }
}
