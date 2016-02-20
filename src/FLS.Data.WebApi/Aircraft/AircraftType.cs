namespace FLS.Data.WebApi.Aircraft
{
    public enum AircraftType
    {
        Unknown = 0,
        Glider = 1,
        GliderWithMotor = 2,
        MotorGlider = 4,
        MotorAircraft = 8,
        MultiEngineAircraft = 16,
        Jet = 32,
        Helicopter = 64
    }
}
