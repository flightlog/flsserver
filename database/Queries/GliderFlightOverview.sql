SELECT  Flights.FlightId,
		(SELECT     Clubs.Clubname
		FROM         Flights AS F INNER JOIN
                      FlightTypes ON F.FlightTypeId = FlightTypes.FlightTypeId INNER JOIN
                      Clubs ON Clubs.ClubId = FlightTypes.ClubId
		WHERE F.FlightId = Flights.FlightId) AS Clubname,   

		CONVERT(DATE, Flights.StartDateTime) AS FlightDate, 
		Aircrafts.Immatriculation, 
		CONCAT (StartTypes.StartTypeName, ' (', StartTypes.StartTypeId, ')') AS StartType,
		CONVERT(TIME, Flights.StartDateTime) AS StartTime, 
		CONVERT(TIME, Flights.LdgDateTime) AS LdgTimeGlider, 
		CONVERT(varchar(5), DATEADD(minute, DATEDIFF(minute, Flights.StartDateTime, Flights.LdgDateTime), 0), 114) AS GliderFlightDuration,
		Flights.EngineTime,
		Flights.IsSoloFlight,
		
		(SELECT     CONCAT(Persons.Lastname, ' ', Persons.Firstname) AS GliderPilot
        FROM          FlightCrew INNER JOIN
                                Persons ON FlightCrew.PersonId = Persons.PersonId
        WHERE      (FlightCrew.FlightId = Flights.FlightId) AND (FlightCrew.FlightCrewType = 1)) AS GliderPilot,
		
		(SELECT     PersonClub.MemberNumber
		FROM         Flights AS F INNER JOIN
                      FlightCrew ON F.FlightId = FlightCrew.FlightId INNER JOIN
                      Persons ON FlightCrew.PersonId = Persons.PersonId INNER JOIN
                      FlightTypes ON F.FlightTypeId = FlightTypes.FlightTypeId INNER JOIN
                      PersonClub ON Persons.PersonId = PersonClub.PersonId
		WHERE F.FlightId = Flights.FlightId AND (FlightCrew.FlightCrewType = 1) and PersonClub.ClubId = FlightTypes.ClubId) AS PilotsMemberNumber,
		
		(SELECT     PersonClub.MemberKey
		FROM         Flights AS F INNER JOIN
                      FlightCrew ON F.FlightId = FlightCrew.FlightId INNER JOIN
                      Persons ON FlightCrew.PersonId = Persons.PersonId INNER JOIN
                      FlightTypes ON F.FlightTypeId = FlightTypes.FlightTypeId INNER JOIN
                      PersonClub ON Persons.PersonId = PersonClub.PersonId
		WHERE F.FlightId = Flights.FlightId AND (FlightCrew.FlightCrewType = 1) and PersonClub.ClubId = FlightTypes.ClubId) AS PilotsMemberKey,

		(SELECT     CONCAT(Persons_4.Lastname, ' ', Persons_4.Firstname) AS GliderCoPilot
        FROM          FlightCrew AS FlightCrew_4 INNER JOIN
                                Persons AS Persons_4 ON FlightCrew_4.PersonId = Persons_4.PersonId
        WHERE      (FlightCrew_4.FlightId = Flights.FlightId) AND (FlightCrew_4.FlightCrewType = 2)) AS GliderCoPilot,
        
		(SELECT     CONCAT(Persons_3.Lastname, ' ', Persons_3.Firstname) AS GliderInstructor
        FROM          FlightCrew AS FlightCrew_3 INNER JOIN
                                Persons AS Persons_3 ON FlightCrew_3.PersonId = Persons_3.PersonId
        WHERE      (FlightCrew_3.FlightId = Flights.FlightId) AND (FlightCrew_3.FlightCrewType = 3)) AS GliderInstructor,
		
		(SELECT     CONCAT(Persons_3.Lastname, ' ', Persons_3.Firstname) AS GliderPAX
        FROM          FlightCrew AS FlightCrew_3 INNER JOIN
                                Persons AS Persons_3 ON FlightCrew_3.PersonId = Persons_3.PersonId
        WHERE      (FlightCrew_3.FlightId = Flights.FlightId) AND (FlightCrew_3.FlightCrewType = 4)) AS GliderPAX,
	
		(SELECT     CONCAT(Persons_3.Lastname, ' ', Persons_3.Firstname) AS InvoiceRecipient
        FROM          FlightCrew AS FlightCrew_3 INNER JOIN
                                Persons AS Persons_3 ON FlightCrew_3.PersonId = Persons_3.PersonId
        WHERE      (FlightCrew_3.FlightId = Flights.FlightId) AND (FlightCrew_3.FlightCrewType = 10)) AS InvoiceRecipient,

	CONCAT(Locations.LocationName, ' (', Locations.IcaoCode, ')') AS StartLocation, 
	CONCAT(Locations_1.LocationName, ' (', Locations_1.IcaoCode, ')') AS LdgLocation, 
    FlightTypes.FlightTypeName, 
	FlightTypes.FlightCode,  
	Flights.NrOfLdgs, 
	Flights.FlightState, 
	Flights.FlightAircraftType, 
	Flights.Comment, 
    Flights.IncidentComment, 
	Flights.CouponNumber, 
	Flights.FlightCostBalanceType, 
	Flights.InvoicedOn, 
	Flights.InvoiceNumber, 
	Flights.CreatedOn, 
    Flights.ModifiedOn,
	Aircrafts_1.Immatriculation AS TowAircraft, 
	CONVERT(TIME, Flights_1.StartDateTime) AS TowStartTime, 
	CONVERT(TIME, Flights_1.LdgDateTime) AS TowLdgTime, 
	CONVERT(varchar(5), DATEADD(minute, DATEDIFF(minute, Flights_1.StartDateTime, Flights_1.LdgDateTime), 0), 114) AS TowFlightDuration,
	(SELECT     CONCAT(Persons_3.Lastname, ' ', Persons_3.Firstname) AS TowPilot
        FROM          FlightCrew AS FlightCrew_3 INNER JOIN
                                Persons AS Persons_3 ON FlightCrew_3.PersonId = Persons_3.PersonId
        WHERE      (FlightCrew_3.FlightId = Flights_1.FlightId) AND (FlightCrew_3.FlightCrewType = 1)) AS TowPilot,
    CONCAT(Locations_2.LocationName, ' (', Locations_2.IcaoCode, ')') AS TowStartLocation, 
	CONCAT(Locations_3.LocationName, ' (', Locations_3.IcaoCode, ')') AS TowLdgLocation, 
    FlightTypes_1.FlightTypeName, 
	FlightTypes_1.FlightCode, 
	Flights_1.NrOfLdgs AS NrOfLdgsTowFlight, 
	Flights_1.FlightState AS FlightStateTowFlight, 
	Flights_1.CreatedOn AS CreatedOnTowFlight, 
    Flights_1.ModifiedOn AS ModifiedOnTowFlight,
	(SELECT     CONCAT(Persons_3.Lastname, ' ', Persons_3.Firstname) AS WinchOperator
        FROM          FlightCrew AS FlightCrew_3 INNER JOIN
                                Persons AS Persons_3 ON FlightCrew_3.PersonId = Persons_3.PersonId
        WHERE      (FlightCrew_3.FlightId = Flights.FlightId) AND (FlightCrew_3.FlightCrewType = 5)) AS WinchOperator
FROM Flights 
	INNER JOIN Locations ON Flights.StartLocationId = Locations.LocationId 
	INNER JOIN Aircrafts ON Flights.AircraftId = Aircrafts.AircraftId 
	INNER JOIN StartTypes ON StartTypes.StartTypeId = Flights.StartType 
	INNER JOIN Locations AS Locations_1 ON Flights.LdgLocationId = Locations_1.LocationId 
	INNER JOIN FlightTypes ON Flights.FlightTypeId = FlightTypes.FlightTypeId 
	left OUTER JOIN Flights as Flights_1 ON Flights_1.FlightId = Flights.TowFlightId
	full outer JOIN Aircrafts AS Aircrafts_1 ON Aircrafts_1.AircraftId = Flights_1.AircraftId 
	full outer JOIN FlightTypes AS FlightTypes_1 ON Flights_1.FlightTypeId = FlightTypes_1.FlightTypeId 
	full outer JOIN Locations AS Locations_2 ON Flights_1.StartLocationId = Locations_2.LocationId 
	full outer JOIN Locations AS Locations_3 ON Flights_1.LdgLocationId = Locations_3.LocationId 
WHERE Flights.IsDeleted = 0 and (Flights.TowFlightId is null OR (Flights.TowFlightId is not null and Flights_1.IsDeleted = 0))
	and Flights.FlightAircraftType = 1
ORDER BY Clubname, FlightDate desc, StartTime desc