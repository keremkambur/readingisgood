namespace ReadingIsGood.BusinessLayer.ResponseModels.Base
{
    public abstract class Response : IResponse
    {
        /// <inheritdoc />
        public string RequestId { get; set; }

        /// <inheritdoc />
        public int Status { get; set; }

        /// <inheritdoc />
        public bool DidError => this.Error != null;

        /// <inheritdoc />
        public IErrorResponse Error { get; set; }
    }
}