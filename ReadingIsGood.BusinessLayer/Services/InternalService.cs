using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ReadingIsGood.BusinessLayer.Contracts;

namespace ReadingIsGood.BusinessLayer.Services
{
    public abstract class InternalService<TOptions> : InternalService where TOptions : Options.Options, new()
    {
        protected TOptions Options;

        protected InternalService(
            ILogger logger,
            IBusinessObject businessObject,
            IOptions<TOptions> options
        )
            : base(logger, businessObject)
        {
            Options = options.Value;
        }

        public void Dispose()
        {
        }
    }

    public abstract class InternalService : IInternalService
    {
        protected IBusinessObject BusinessObject;
        protected Guid ServiceId;

        protected InternalService(
            ILogger logger,
            IBusinessObject businessObject
        )
        {
            ServiceId = Guid.NewGuid();
            BusinessObject = businessObject;
        }

        public void Dispose()
        {
        }
    }
}