namespace FLS.Server.Data.Enums
{
    /// <summary>
    /// defines the possible values for a record state in an entity
    /// </summary>
    public enum EntityRecordState
    {
        Inactive = 0,
        Active = 1,
        ConfirmationNeeded = 2,
        Deleted = 99,
        System = 100
    }
}
