namespace ReadingIsGood.BusinessLayer.Exceptions
{
    public class RequestParameterErrorPayload
    {
        public string[] ParameterNames { get; set; }

        public string Scope { get; set; }
    }
}
