namespace ReadingIsGood.EntityLayer.Enum
{
    public enum ErrorCodes
    {
        Unknown

        , Internal
        , RequestCanceled
        , BadRequest
        , Unauthorized
        , Forbidden

        , SeriesNotFound
        , SeriesHasNoDefaultImage
        , EpisodeNotCreated
        , EpisodeNotFound
        , EpisodeNotPublished
        , TenantNotFound
        , UserNotFound
        , FailedToUnPublishUser
        , UserCommentNotFound
        , UserNameAlreadyTaken
        , MobileNumberInvalid
        , MailInvalid
        , UnableToDeleteSuperAdmin
        , UnableToDeleteYourself
        , BlobMissingContentType
        , BlobNotFound
        , MissingSevenZipExecutable
        , SevenZipError
        , IntroOutroContentNotFound
        , UserHasNoMobileNumber
        , InvalidLogin
        , FailedToAuthenticateUser
        , InvalidOtp
        , UnableToRestoreItemIsNotDeleted
        , UnableToRestoreItemEpisodeIsDeleted
        , EpisodeNotArchived
        , EditorialCommentNotArchived
    }
}