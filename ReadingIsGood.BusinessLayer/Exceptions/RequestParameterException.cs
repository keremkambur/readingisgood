using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ReadingIsGood.BusinessLayer.Exceptions
{
    public class RequestParameterException : ApiException
    {
        public Dictionary<string, string> Parameters { get; set; }

        public string SingleMessage { get; set; }

        public string Scope { get; set; }

        public RequestParameterException(string parameterName, string message, string scope = null) : base(HttpStatusCode.BadRequest)
        {
            this.Scope = scope;
            this.Parameters = new Dictionary<string, string>
            {
                { parameterName, message }
            };
        }

        public RequestParameterException(string scope, params KeyValuePair<string, string>[] parameters) : base(HttpStatusCode.BadRequest)
        {
            this.Scope = scope;
            this.Parameters = new Dictionary<string, string>(parameters);
        }

        public RequestParameterException(IEnumerable<string> parameterNames, string message, string scope = null) : base(HttpStatusCode.BadRequest)
        {
            this.Scope = scope;
            this.Parameters = new Dictionary<string, string>(parameterNames.Select(x => new KeyValuePair<string, string>(x, null)));
            this.SingleMessage = message;
        }

        public string AggregatedErrorMessage => !string.IsNullOrWhiteSpace(this.SingleMessage) 
            ? $"Parameters: [{string.Join(Environment.NewLine, this.Parameters.Select(x => x.Key))}]; Message: [{this.SingleMessage}]" 
            : string.Join(Environment.NewLine, this.Parameters.Select(pair => $"Parameter [{pair.Key}]; Message: [{pair.Value}]"))
        ;

        public RequestParameterErrorPayload AggregatedErrorPayload 
            => new RequestParameterErrorPayload
            {
                Scope = this.Scope,
                ParameterNames = this.Parameters.Select(pair => pair.Key).ToArray()
            };
    }
}
