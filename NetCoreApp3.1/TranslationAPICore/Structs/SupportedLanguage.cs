using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TranslationAPI.Structs
{
    /// <summary>
    /// Json deserialize object for supported languages request
    /// </summary>
    public class SupportedLanguagesResponse
    {
        /// <summary>
        /// Supported languages container
        /// </summary>
        [JsonProperty("languages")]
        public SupportedLanguage[] CompatibleLanguages;
    }

    /// <summary>
    /// Supported languages container item
    /// </summary>
    public class SupportedLanguage
    {
        /// <summary>
        /// Supported language code
        /// </summary>
        [JsonProperty("languageCode")]
        public string LanguageCode;

        /// <summary>
        /// Support source
        /// </summary>
        [JsonProperty("supportSource")]
        public bool SupportSource;

        /// <summary>
        /// Support target
        /// </summary>
        [JsonProperty("supportTarget")]
        public bool SupportTarget;
    }
}
