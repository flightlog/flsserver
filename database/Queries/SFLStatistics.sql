DECLARE @ClubKey nvarchar(10) = 'FGZO';

SELECT      CONCAT(Persons.Lastname, ' ', Persons.Firstname) AS Person, PlanningDays.Day
, (SELECT COUNT(*) AS NumberOfReservations FROM AircraftReservations WHERE (AircraftReservations.LocationId = PlanningDays.LocationId) AND (CAST(Start AS Date) = CAST(PlanningDays.Day AS Date))
	AND AircraftReservations.IsDeleted = 0) AS 'Anzahl Reservationen'
, (SELECT COUNT(*) AS NumberOfFlights FROM Flights WHERE (StartLocationId = PlanningDays.LocationId) AND (CAST(StartDateTime AS Date) = CAST(PlanningDays.Day AS Date))
	AND (Flights.FlightAircraftType = 1 OR Flights.FlightAircraftType = 3)
	AND Flights.IsDeleted = 0) AS 'Anzahl Flüge'

FROM            PlanningDays
INNER JOIN PlanningDayAssignments ON PlanningDayAssignments.AssignedPlanningDayId = PlanningDays.PlanningDayId
INNER JOIN Persons ON PlanningDayAssignments.AssignedPersonId = Persons.PersonId 

where PlanningDays.ClubId = (SELECT ClubId FROM Clubs WHERE ClubKey = @ClubKey) 
and PlanningDayAssignments.AssignmentTypeId = (SELECT PlanningDayAssignmentTypeId FROM PlanningDayAssignmentTypes where PlanningDayAssignmentTypes.AssignmentTypeName = 'Segelflugleiter' and PlanningDayAssignmentTypes.ClubId = (SELECT ClubId FROM Clubs WHERE ClubKey = @ClubKey))
and PlanningDayAssignments.IsDeleted = 0
and PlanningDays.IsDeleted = 0
and YEAR(PlanningDays.Day) = YEAR(SYSDATETIME())
GROUP BY  CONCAT(Persons.Lastname, ' ', Persons.Firstname), PlanningDays.LocationId, PlanningDays.Day