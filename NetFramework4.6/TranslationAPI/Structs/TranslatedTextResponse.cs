using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TranslationAPI.Structs
{
    /// <summary>
    /// Json deserialize object for translate text request
    /// </summary>
    public class TranslatedTextResponse
    {
        /// <summary>
        /// Translated text container
        /// </summary>
        [JsonProperty("translations")]
        public Translated[] TranslatedTexts;
    }

    /// <summary>
    /// Translated container item
    /// </summary>
    public class Translated
    {
        /// <summary>
        /// Translated text
        /// </summary>
        [JsonProperty("translatedText")]
        public string TranslatedText;

        /// <summary>
        /// Detected language code for translated text
        /// </summary>
        [JsonProperty("detectedLanguageCode")]
        public string DetectedLanguageCode;
    }
}
