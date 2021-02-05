using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TranslationAPI.Structs
{
    /// <summary>
    /// Json deserialize object for language detect request
    /// </summary>
    public class LanguageDetectResponse
    {
        /// <summary>
        /// Languages detected container
        /// </summary>
        [JsonProperty("languages")]
        public LanguageItem[] LanguageItems;
    }

    /// <summary>
    /// Detected languages container item
    /// </summary>
    public class LanguageItem
    {
        /// <summary>
        /// Detected language code
        /// </summary>
        [JsonProperty("languageCode")]
        public string LanguageCode;

        /// <summary>
        /// Detected language confidence
        /// </summary>
        [JsonProperty("confidence")]
        public double Confidence;
    }
}
