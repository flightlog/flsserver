using FLS.Common.Validators;
using FLS.Data.WebApi;
using TrackerEnabledDbContext.Common.Configuration;

namespace FLS.Server.Data.Extensions
{
    public static class EntityTrackerExtensions
    {
        public static ExceptResponse<T> ExceptMetadata<T>(this TrackAllResponse<T> trackAllResponse) where T : IFLSMetaData
        {
            trackAllResponse.ArgumentNotNull("trackAllResponse");

            //TODO: How can this be implemented (throws exception that response is not of correct type)
            var response = trackAllResponse.Except(x => x.Id)
                .And(x => x.CreatedByUserId)
                .And(x => x.CreatedOn)
                .And(x => x.ModifiedByUserId)
                .And(x => x.ModifiedOn)
                .And(x => x.DeletedByUserId)
                .And(x => x.DeletedOn)
                .And(x => x.OwnerId)
                .And(x => x.OwnershipType)
                .And(x => x.RecordState);

            return response;
        }

        
    }
}
