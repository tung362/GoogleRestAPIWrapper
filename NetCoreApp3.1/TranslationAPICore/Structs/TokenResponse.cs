using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TranslationAPI.Structs
{
    /// <summary>
    /// Json deserialize object for token request
    /// </summary>
    public class TokenResponse
    {
        /// <summary>
        /// Token used for accessing google api
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken;

        /// <summary>
        /// Expiration for the access token
        /// </summary>
        [JsonProperty("expires_in")]
        public double Expiration;

        /// <summary>
        /// Token type
        /// </summary>
        [JsonProperty("token_type")]
        public string TokenType;
    }
}
