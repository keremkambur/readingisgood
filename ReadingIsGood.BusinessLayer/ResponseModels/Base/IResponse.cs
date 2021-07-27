namespace ReadingIsGood.BusinessLayer.ResponseModels.Base
{
    public interface IResponse
    {
        /// <summary>
        /// Request specific id for debugging and logging
        /// </summary>
        string RequestId { get; set; }

        /// <summary>
        /// HttpStatus Code
        /// </summary>
        int Status { get; set; }

        /// <summary>
        /// Indicates if an error occurred.
        /// </summary>
        bool DidError { get; }

        /// <summary>
        /// Error Object with further information
        /// </summary>
        IErrorResponse Error { get; set; }
    }
}
