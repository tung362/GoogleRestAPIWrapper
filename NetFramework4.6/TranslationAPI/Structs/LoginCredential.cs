using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslationAPI.Structs
{
    /// <summary>
    /// Client login information
    /// </summary>
    public struct LoginCredential
    {
        /// <summary>
        /// File path to the .p12 credentials file
        /// </summary>
        public string P12Path;
        /// <summary>
        /// password for the .p12 credentials file
        /// </summary>
        public string P12Password;
        /// <summary>
        /// Service account private key id
        /// </summary>
        public string ServiceAccountPrivateKeyId;
        /// <summary>
        /// Service account email address
        /// </summary>
        public string ServiceAccountEmail;
        /// <summary>
        /// URI to the authentication token address
        /// </summary>
        public string AuthenticationTokenURI;
        /// <summary>
        /// Url to the authentication scope for the specific google api
        /// </summary>
        public string AuthenticationScope;
        /// <summary>
        /// Project id registered on your google console
        /// </summary>
        public string ProjectID;
        /// <summary>
        /// Url to the specific google api end point where you make your POST and GET requests
        /// </summary>
        public string Endpoint;
    }
}
