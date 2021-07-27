using System;
using ReadingIsGood.DataLayer.Contracts;

namespace ReadingIsGood.BusinessLayer.Contracts
{
    public interface IBusinessObject : IDisposable
    {
        Guid Id { get; }

        bool BulkModeIsEnabled { get; }

        IDatabaseRepository DatabaseRepository { get; }

        IAuthRepository AuthRepository { get; }

        void EnableBulkMode();
        void DisableBulkMode(bool saveChanges = false);
    }
}