SELECT Clubs.Clubname,
CONCAT(Persons.Lastname, ' ', Persons.Firstname) AS Pilot,
CONCAT(SUM(DATEDIFF(minute, Flights.StartDateTime, Flights.LdgDateTime)) /60, ':',
FORMAT(SUM(DATEDIFF(minute, Flights.StartDateTime, Flights.LdgDateTime)) % 60, '00')) AS Flugzeit,
SUM(Flights.NrOfLdgs) AS AnzahlLandungen
FROM            Flights INNER JOIN
                         
                         FlightCrew ON Flights.FlightId = FlightCrew.FlightId
						 INNER JOIN
                         Persons ON FlightCrew.PersonId = Persons.PersonId	
						 INNER JOIN PersonClub ON PersonClub.PersonId = Persons.PersonId
						 LEFT OUTER JOIN Clubs ON PersonClub.ClubId = Clubs.ClubId		 
WHERE        
Flights.IsDeleted = 0 and 
Flights.FlightState <> 28
and Flights.StartDateTime >= CONCAT(YEAR(SYSDATETIME()), '-01-01') and Flights.LdgDateTime <= CONCAT(YEAR(SYSDATETIME()), '-12-31')
and FlightCrew.FlightCrewType = 1
and Flights.FlightAircraftType = 1
group by Clubname, CONCAT(Persons.Lastname, ' ', Persons.Firstname)

--CONVERT(DATE, Flights.StartDateTime)