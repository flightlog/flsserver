SELECT Aircrafts.Immatriculation,
--Month(Flights.StartDateTime) AS Monat,
CONCAT((SUM(DATEDIFF(minute, Flights.StartDateTime, Flights.LdgDateTime)) /60), 
':',
(FORMAT(SUM(DATEDIFF(minute, Flights.StartDateTime, Flights.LdgDateTime)) % 60, '00'))) AS AnzahlStundenMinuten,
SUM(Flights.NrOfLdgs) AS AnzahlLandungen
FROM            Flights INNER JOIN
                         Aircrafts ON Flights.AircraftId = Aircrafts.AircraftId INNER JOIN
                         StartTypes ON Flights.StartType = StartTypes.StartTypeId
WHERE        
Flights.IsDeleted = 0 and 
Flights.FlightState <> 28
and Flights.StartDateTime >= CONCAT(YEAR(SYSDATETIME()), '-01-01') and Flights.LdgDateTime <= CONCAT(YEAR(SYSDATETIME()), '-12-31')
and (Aircrafts.Immatriculation = 'HB-3256' 
or Aircrafts.Immatriculation = 'HB-3407'
or Aircrafts.Immatriculation = 'HB-1824'
or Aircrafts.Immatriculation = 'HB-1841'
or Aircrafts.Immatriculation = 'HB-2464'
or Aircrafts.Immatriculation = 'HB-KCB')
group by Aircrafts.Immatriculation--, Month(Flights.StartDateTime)