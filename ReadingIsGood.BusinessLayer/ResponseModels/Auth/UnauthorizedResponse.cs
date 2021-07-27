using System.Net;
using ReadingIsGood.BusinessLayer.ResponseModels.Base;

namespace ReadingIsGood.BusinessLayer.ResponseModels.Auth
{
    public class UnauthorizedResponse : Response
    {
        public UnauthorizedResponse(string requestId)
        {
            Status = (int) HttpStatusCode.Unauthorized;
            RequestId = requestId;
        }
    }
}