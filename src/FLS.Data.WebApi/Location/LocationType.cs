namespace FLS.Data.WebApi.Location
{
    /// <summary>
    /// The location type values corresponds to the waypoint ID in the CUP file format
    /// </summary>
    public enum LocationType
    {
        Waypoint = 1,
        AirfieldGrass = 2,
        Outlanding = 3,
        AirfieldGliderOnly = 4,
        AirfieldSolid = 5,
        MountainPass = 6,
        MountainTop = 7,
        Sender = 8,
        VorStation = 9,
        NdbStation = 10,
        CoolTower = 11,
        Dam = 12,
        Tunnel = 13,
        Bridge = 14,
        PowerPlant = 15,
        Castle = 16,
        Intersection = 17
    };
}