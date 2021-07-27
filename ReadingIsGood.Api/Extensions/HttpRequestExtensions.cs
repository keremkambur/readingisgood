using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using ReadingIsGood.BusinessLayer.Exceptions;
using ReadingIsGood.Utils;

namespace ReadingIsGood.Api.Extensions
{
    public static class HttpRequestExtensions
    {
        public static async Task<TModel> ToModel<TModel>(this HttpRequest request) where TModel : class
        {
            if (request == null)
            {
                throw new ApiException("Request is empty");
            }

            if (request.Body.CanSeek && request.Body.Position != 0L)
            {
                request.Body.Seek(0, SeekOrigin.Begin);
            }

            using var streamReader = new StreamReader(request.Body);
            var requestBody = await streamReader.ReadToEndAsync().ConfigureAwait(false);
            var data =  System.Text.Json.JsonSerializer.Deserialize<TModel>(requestBody);

            if (data == null)
            {
                throw new ApiException("Request does not have any data.");
            }

            return data;
        }

        public static string GetUserAgent(this HttpRequest request, string fallback = "")
        {
            return request.GetHeaderValue(Constants.HttpHeaders.UserAgent, fallback);
        }

        /// <summary>
        /// Tries to get value from Authorization header and read Bearer token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetAuthorizationBearerToken(this HttpRequest request)
        {
            StringValues stringValue = StringValues.Empty;
            if (request?.Headers.TryGetValue(nameof(HttpRequestHeader.Authorization), out stringValue) != true)
            {
                throw new ForbiddenAccessException("No authorization header found.");
            }

            if (string.IsNullOrWhiteSpace(stringValue))
            {
                throw new ForbiddenAccessException("No authorization value found.");
            }

            var token = stringValue.ToString();

            if (!token.StartsWith(Constants.HttpHeaders.AuthorizationBearerPrefix))
            {
                throw new ForbiddenAccessException($"{Constants.HttpHeaders.AuthorizationBearerPrefix} token was not found. Please provide token as {Constants.HttpHeaders.AuthorizationBearerPrefix} token.");
            }
            
            var jwt = token.Replace(Constants.HttpHeaders.AuthorizationBearerPrefix, string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(jwt))
            {
                throw new ForbiddenAccessException("No authorization value found.");
            }

            return jwt;
        }

        public static string GetHeaderValue(this HttpRequest request, string header, string fallbackValue)
        {
            StringValues value = StringValues.Empty;
            return request.Headers?.TryGetValue(header, out  value) == true ? value.ToString() : fallbackValue;
        }

        public static string GetHeaderValue(this HttpRequest request, string header)
        {
            StringValues value = StringValues.Empty;
            return request.Headers?.TryGetValue(header, out value) == true ? value : default;
        }


        public static string[] GetHeaderValues(this HttpRequest request, string header, char separator = ',')
        {
            StringValues value = StringValues.Empty;
            return request.Headers?.TryGetValue(header, out value) == true ? value.ToString().Split(separator) : default;
        }
            
    }
}