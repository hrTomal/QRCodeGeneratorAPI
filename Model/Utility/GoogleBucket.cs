using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Utility
{
    public class GoogleBucket
    {
        public string Type { get; set; } = "";
        public string ProjectId { get; set; } = "";
        public string PrivateKeyId { get; set; } = "";
        public string PrivateKey { get; set; } = ""
        public string ClientId { get; set; } = "";
        public string AuthUri { get; set; } = "";
        public string TokenUri { get; set; } = "";
        public string AuthProviderCertUrl { get; set; } = "";
        public string ClientCertUrl { get; set; } = "";
        public string UniverseDomain { get; set; } = "";
    }
}
