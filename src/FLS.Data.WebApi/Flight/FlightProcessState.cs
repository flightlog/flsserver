
namespace FLS.Data.WebApi.Flight
{
    /// <summary>
    /// Defines the values of the flight status.
    /// </summary>
    /// <remarks>
    /// After the flight has been validated, it new status will be <code>Valid</code> or <code>Invalid</code>.
    /// An invalid flight must be edited from an user to be re-validated during the next process workflow job and may result in a <code>Valid</code> process state.
    /// A valid flight will be locked after two days of creation.
    /// Only locked flights can be further processed for delivery.
    /// The delivery preparation process picks all the locked flights and creates the delivery for the external finance system and sets it to process state <code>DeliveryPrepared</code>.
    /// If the delivery creation fails (like no delivery item positions have been created because no rule has been matched), the process state <code>DeliveryPreparationError</code> will bet set.
    /// Depending on the process configuration, the external finance system will pick the delivery and mark the flight delivery as booked and therefore set the process state to <code>DeliveryBooked</code>.
    /// A flight in process state <code>DeliveryPrepared</code> can be reset to the state <code>Locked</code>. The delivery will be deleted. In this case, it is possible to re-define some AccountingRuleFilters for processing this flight again.
    /// A flight in process state <code>DeliveryBooked</code> can no longer be edited.
    /// A flight in process state <code>ExcludedFromDeliveryProcess</code> is excluded from delivery creation. It can be set to this state from valid, locked or any delivery states except <code>DeliveryBooked</code>.
    /// From the state <code>ExcludedFromDeliveryProcess</code> it is possible to set the flight back to the state <code>Locked</code>.
    /// </remarks>
    public enum FlightProcessState
    {
        NotProcessed = 0,
        Invalid = 28,
        Valid = 30,
        Locked = 40,
        DeliveryPreparationError = 45,
        DeliveryPrepared = 50,
        DeliveryBooked = 60,
        ExcludedFromDeliveryProcess = 99
    }
}