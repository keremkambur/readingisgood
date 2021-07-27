using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingIsGood.Utils
{
    public static class Constants
    {
        public static class HttpHeaders
        {
            public const string UserAgent = "User-Agent";
            public const string AuthorizationBearerPrefix = "Bearer";
        }

        public static class Version
        {
            public const int CurrentApiMinorVersion = 0;
            public const int CurrentApiMajorVersion = 1;
            public const string CurrentApiVersion = "v1";
        }

        public static class MimeType
        {
            public const string Unknown = "application/octet-stream";
            public const string OctetStream = MimeType.Unknown;
            public const string Json = "application/json";
            public const string UrlFormEncoded = "application/x-www-form-urlencoded";
            public const string Text = "text/plain";
            public const string Html = "text/html";
            public const string Jpeg = "image/jpeg";
            public const string Png = "image/png";
            public const string Css = "text/css";
            public const string JavaScript = "text/javascript";
            public const string FontOtf = "application/x-otf";
            public const string Pem = "application/x-pem-file";
            public const string Pkcs12 = "application/x-pkcs12";
            public const string MultiPart = "multipart/form-data";
            public const string Wav = "audio/x-wav";
            public const string Zip = "application/zip";
        }
    }
}
