# Flight Logging System Server (FLS Server)
The Flight Logging System Server is the server part of the Flight Logging System. It covers the Web Api, business logic and database part of the FLS. The Angular web client is maintained in a separate project https://github.com/flightlog/flsweb.

The FLS manages glider and motor flights of clubs from the reservation until exporting to the invoicing system. The main features of the server are:
* Maintaining base data like:
  * Clubs (Tenant)
  * Users
  * Aircrafts
  * Persons with relations to the club(s)
  * Club-related flight types
  * Locations
  * Club-related accounting rules
  * Club-related member states and person categories
* Edit and validate glider- and motor flights
* Aircraft reservations
* Planning of flight days with flight instructor, tow-pilot and flight operator
* System- and club-related email template handling
* Workflow engine for several automated processes like email notification, invoice exports, etc.

## Additional extensions to the FLS
### Proffix Sync Interface
The Proffix Sync Interface synchronizes addresses, flights, deliveries and articles between Proffix and FLS Server. It automates the invoicing process of flights in Proffix. For more information see also: https://github.com/arminstutz/PROFFIX-FLS-Sync

### OGN Analyser
The OGN Analyser analyses flight GPS data from Open Glider Net and creates starts and landing events for FLS. See also: https://github.com/sgacond/OGNAnalyser

## Versioning

We use [SemVer](http://semver.org/) for versioning.

## License

This project is licensed under the MIT License.
