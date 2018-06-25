USE [FLSTest]
GO

DELETE FROM [dbo].[LanguageTranslations]
GO

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'MASTERDATA', 'Stammdaten', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'AIRCRAFTRESERVATION', 'Flugzeug Reservation', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'RESERVATIONS', 'Reservationen', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'PLANNING', 'Planung', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FLIGHTS', 'Flüge', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'STARTLIST', 'Startliste', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FLIGHT_REPORT_SUMMARY', 'Zusammenfassung', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'LOCATIONS', 'Flugplätze', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'CLUBS', 'Vereine', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'PERSONS', 'Personen', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'AIRCRAFTS', 'Flugzeuge', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'START_LOCATION', 'Startflugplatz', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'LANDING_LOCATION', 'Landeflugplatz', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'LOGOUT', 'Logout', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'DATE', 'Datum', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'LOCATION', 'Flugplatz', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'REMARKS', 'Bemerkungen', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'COMMENT', 'Kommentar', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'TOWING_PILOT', 'Schleppilot', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'TOWING_PLANE', 'Schleppflugzeug', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'GLIDER_PILOT', 'Segelflugpilot', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'CO_PILOT', 'Co-Pilot', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'GLIDER_PLANE', 'Segelflugzeug', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FLIGHT_OPERATOR', 'Segelflugleiter', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'TAKEOFF', 'Start', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'LANDING', 'Landung', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'DURATION', 'Dauer', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'TOW_LANDING', 'Schlepp Landung', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'GLIDER_LANDING', 'Segelflieger Landung', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'START', 'Start', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'END', 'Ende', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'DAY', 'Tag', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'ALL_DAY', 'Ganztags', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'IMMATRICULATION', 'Immatrikulation', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'AIRCRAFT_MODEL', 'Modell', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'PILOT_NAME', 'Pilot', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'INSTRUCTOR_NAME', 'Instruktor', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'RESERVATION_TYPE', 'Reservationstyp', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'SAVE', 'Speichern', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'OK', 'Ok', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'CANCEL', 'Abbrechen', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'SETUP', 'Vorbereitung', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'AIRCRAFT', 'Flugzeug', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'INSTRUCTOR', 'Instruktor', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'OBSERVER', 'Überwachender Pilot / Instruktor', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'PILOT', 'Pilot', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'PASSENGER', 'Passagier', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'SECOND_CREW_MEMBER', 'Mit an Bord', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'NUM_RESERVATIONS', 'Anzahl Reservationen', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'NEW', 'Neu', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'NEW_PURGED', 'Neu (Leer)', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'START_TYPE', 'Startart', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'START_TYPES', 'Startarten', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FLIGHT_TYPE', 'Flugtyp', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'DLG_ADD_NEW_PILOT', 'Neuer Pilot', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'DLG_ADD_NEW_PERSON', 'Neue Person', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FIRST_NAME', 'Vorname', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'LAST_NAME', 'Nachname', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'ADDRESS_LINE1', 'Strasse/Nr.', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'ADDRESS_LINE2', 'Zusatz', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'ZIP_CODE', 'PLZ', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'CITY', 'Stadt', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'COUNTRY', 'Land', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'HAS_GLIDER_INSTRUCTOR_LICENCE', 'Segelfluglehrer Lizenz', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'HAS_GLIDER_PILOT_LICENCE', 'Segelflug Lizenz', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'HAS_GLIDER_TRAINEE_LICENCE', 'Segelflugschüler Lizenz', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'HAS_MOTOR_PILOT_LICENCE', 'Motorflug Lizenz', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'HAS_MOTOR_INSTRUCTOR_LICENCE', 'Motorfluglehrer Lizenz', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'HAS_TOW_PILOT_LICENCE', 'Schlepp Lizenz', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'HAS_GLIDER_PASSENGER_LICENCE', 'Segelflug Passagier Lizenz', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'HAS_TMG_LICENCE', 'TMG Lizenz', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'HAS_WINCH_OPERATOR_LICENCE', 'Windenführer Lizenz', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'COST_BALANCE_TYPE', 'Kostenverteilung', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'INVOICE_RECEIPIENT', 'Rechnungsempfänger', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'IS_SOLO_FLIGHT', 'Soloflug', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'NR_LANDINGS', 'Anz. Ldg', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'OUTBOUND_ROUTE', 'Outbound', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'INBOUND_ROUTE', 'Inbound', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FROM_DATE', 'Von', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'TO_DATE', 'Bis', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'APPLY_FILTER', 'Filter Anwenden', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'LOAD_FLIGHTS', 'Flüge laden', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'ALL_TIME', 'Gesamter Zeitraum', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), '12_MONTHS', '12 Monate', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), '24_MONTHS', '24 Monate', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'TODAY', 'Heute', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'MY_PROFILE', 'Mein Profil', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'USER', 'Benutzer', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'USERNAME', 'Benutzername', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'PASSWORD', 'Passwort', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'OLD_PASSWORD', 'Aktuelles Passwort', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'NEW_PASSWORD', 'Neues Passwort', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'NEW_PASSWORD_CONFIRM', 'Neues Passwort bestätigen', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'UPDATE_PASSWORD', 'Passwort aktualisieren', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'UPDATE_PERSON_DATA', 'Benutzerdaten aktualisieren', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'EMAIL_ADDRESS', 'Email Adresse', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'MANUFACTURER_NAME', 'Hersteller', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'COMPETITION_SIGN', 'Wettbewerbszeichen', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'NUMBER_OF_SEATS', 'Anzahl Sitzplätze', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'IS_TOWING_AIRCRAFT', 'Schleppflugzeug', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'IS_TOWING_OR_WINCH_REQUIRED', 'benötigt Schlepp', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'IS_TOWING_START_ALLOWED', 'Flugzeugschlepp erlaubt', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'IS_WINCH_START_ALLOWED', 'Windenstart erlaubt', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'COUPON_NUMBER', 'Gutschein-Nr.', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'RELOAD_MASTERDATA', 'Stammdaten neu laden', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'LOCATION_NAME', 'Name', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'LOCATION_TYPE', 'Typ', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'ICAO_CODE', 'ICAO Code', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'LENGTH_UNIT', 'Längeneinheit', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'ELEVATION_UNIT', 'Höheneinheit', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'RUNWAY_LENGTH', 'Pistenlänge', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'RUNWAY_DIRECTION', 'Pistenrichtung (Grad Himmelsrichtung)', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'RUNWAY_ELEVATION', 'Pistenhöhe', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'DESCRIPTION', 'Beschreibung', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'LATITUDE', 'Breitengrad (e.g. 47.37639N)', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'LONGITUDE', 'Längengrad (e.g. 8.7575E)', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'WARN_TOWFLIGHT_LONGER_THAN_GLIDERFLIGHT', 'ACHTUNG: Der eingegebene Schleppflug ist länger als der Segelflug!', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'CLUB_NAME', 'Club Name', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'CLUB_KEY', 'Club Schlüssel', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'CLUB_HOME_BASE', 'Club Flugplatz', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'HOME_BASE', 'Heim-Flugplatz', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'HOME_BASE_OF_AIRCRAFT', 'Heim-Flugplatz des Flugzeuges', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'USERS', 'Benutzer', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'USER_NAME', 'Benutzername', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FRIENDLY_USER_NAME', 'Benutzer Bezeichnung', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'PERSON_NAME', 'Person Name', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'ROLES', 'Rollen', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'NOTIFICATION_EMAIL', 'Email Adresse für Benachrichtigungen', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'CONTACT', 'Kontakt', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'ADDRESS', 'Adresse', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'EMAIL', 'Email Adresse', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'PHONE_NUMBER', 'Telefonnummer', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FAX_NUMBER', 'Fax', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'WEB_PAGE', 'Webseite', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'DEFAULT_SETTINGS', 'Default Einstellungen', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'IS_INBOUND_ROUTE_REQUIRED', 'Inbound Route erforderlich', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'IS_OUTBOUND_ROUTE_REQUIRED', 'Outbound Route erforderlich', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'DEFAULT_GLIDER_FLIGHT_TYPE', 'Default Segelflug Typ', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'DEFAULT_TOWING_FLIGHT_TYPE', 'Default Schleppflug Typ', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'DEFAULT_MOTOR_FLIGHT_TYPE', 'Default Motorflug Typ', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'THIS_MONTH', 'Laufender Monat', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'LAST_MONTH', 'Letzer Monat', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'YESTERDAY', 'Gestern', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'CODE', 'Code', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'NAME', 'Name', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'IS_INSTRUCTOR_REQUIRED', 'Instruktor nötig', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'IS_OBSERVERPILOT_OR_INSTRUCTOR_REQUIRED', 'Instruktor oder Einweisungspilot nötig', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'IS_CHECKFLIGHT', 'Checkflug', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'IS_PASSENGERFLIGHT', 'Passagierflug', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'IS_SOLOFLIGHT', 'Soloflug', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'IS_FLIGHTCOSTBALANCE_SELECTABLE', 'Kostenverteilung wählbar', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'IS_FOR_GLIDERFLIGHTS', 'für Segelflüge', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'IS_FOR_MOTORFLIGHTS', 'für Motorflüge', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'IS_FOR_TOWFLIGHTS', 'für Schleppflüge', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'IS_SYSTEMFLIGHT', 'System Flug', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'IS_COUPONNUMBER_REQUIRED', 'Coupon nötig', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FLIGHT_TYPES', 'Flugtypen', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'LOST_PASSWORD', 'Passwort vergessen', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'LOST_PASSWORD_FORM_EXPLANATION', 'Bitte geben Sie entweder Ihren Benutzernamen oder die Email Adresse an um ein neues Passwort generieren zu lassen. Beachten Sie dass falls Sie mehrere Benutzer besitzen, und Ihre Angaben nicht eindeutig sind, alle gefundenen Benutzer ein neues Passwort bekommen werden.', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'GENERATE_NEW_PASSWORD', 'Neues Passwort generieren', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'USERNAME_OR_EMAIL', 'Benutzername oder Email Adresse', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'GENERATE_NEW_PASSWORD_SUCCEEDED', 'Das Passwort wurde zurückgesetzt. Sie sollten in den nächsten Minuten ein Email mit dem entsprchenden Link erhalten.', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'AIRCRAFT_TYPE', 'Flugzeug Typ', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'AIRCRAFT_OWNER_CLUB', 'Inhaber (Club)', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'AIRCRAFT_OWNER_PERSON', 'Inhaber (Privat)', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'AIRCRAFT_SERIAL_NUMBER', 'Seriennummer', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'AIRCRAFT_YEAR_OF_MANUFACTURE', 'Herstellungsjahr', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FLARM_ID', 'FLARM ID', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'DEAC_INDEX', 'DEAC Index', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'NOISE_CLASS', 'Lärmklasse', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'NOISE_LEVEL', 'Noise Level', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'MAXIMUM_TAKEOFF_MASS', 'Maximum Takeoff Mass (kg)', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'IS_CLUB_OWNED', 'Verein Inhaber', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'IS_PRIVATELY_OWNED', 'Privater Inhaber', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'MOBILE_NUMBER', 'Mobilnummer', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'COMPANY', 'Firma', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'BUSINESS_EMAIL_ADDRESS', 'Email Adresse (geschäftlich)', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'BUSINESS_PHONE_NUMBER', 'Telefonnummer (geschäftlich)', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'LICENSE_NUMBER', 'Lizenznummer', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'SPOT_LINK', 'Spot URL', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'TEST_LINK', 'Link testen', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'RECEIVE_OWNED_AIRCRAFT_STATISTIC_REPORT', 'Statistiken zu den eigenen Flugzeugen', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'CLUB_MEMBER_NUMBER', 'Verein Mitgliednummer', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'CLUB_MEMBER_KEY', 'Verein Mitgliedschlüssel', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'IS_GLIDER_INSTRUCTOR', 'Segelfluglehrer', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'IS_GLIDER_PILOT', 'Segelflugpilot', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'IS_GLIDER_TRAINEE', 'Segelflugschüler', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'IS_MOTOR_PILOT', 'Motorflugpilot', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'IS_MOTOR_INSTRUCTOR', 'Motorfluglehrer', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'IS_PASSENGER', 'Passagier', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'IS_TOW_PILOT', 'Schleppilot', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'IS_WINCH_OPERATOR', 'Windenführer', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'RECEIVE_FLIGHT_REPORTS', 'Flug Statistiken', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'RECEIVE_AIRCRAFT_RESERVATION_NOTIFICATIONS', 'Notifikationen zu Reservationen', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'RECEIVE_PLANNING_DAY_ROLE_REMINDER', 'Erinnerungen zu Planung der Einteilungen', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'REGION', 'Region / Kanton', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'MID_NAME', 'Zwischenname', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'USER_RELATED_PERSON', 'Zugehörige Person', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'USER_ACCOUNT_STATE', 'Benutzerkonto Status', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'CLUB', 'Verein', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'STATUS', 'Status', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FLIGHT_STATUS', 'Flug Status', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'PROCESS_STATUS', 'Verarbeitung', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'MY_DASHBOARD', 'Mein Dashboard', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'MY_FLIGHTS', 'Meine Flüge', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'MY_SAFETY_STATUS', 'Mein Safety Status', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'MY_MEDICAL', 'Mein Medical', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'EXPIRES_AT', 'Läuft aus am', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'MY_START_PERMISSIONS', 'Meine Starterlaubnis', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'MY_NEXT_RESERVATIONS', 'Meine nächsten Reservationen', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'SAFETY_GREEN_TITLE', 'Grüner Bereich', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'SAFETY_GREEN_SUBTITLE', 'Der Übungsstand ist gut - trotzdem Vorsicht!', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'SAFETY_GREEN_DESCRIPTION', 'Geübte Piloten machten folgende Fehler <ul> <li>Segelflugzeug fehlerhaft ausgerüstet</li><li>mangelhafter Cockpitcheck</li><li>Fehlverhalten bei Startunterbrechungen</li><li>Fehler bei der Landeeinteilung (vor allem bei Außenlandungen)</li></ul>', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'SAFETY_YELLOW_TITLE', 'Gelber Bereich', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'SAFETY_YELLOW_SUBTITLE', 'Mehr Übung könnte nicht schaden - Unerwartete Ereignisse können gefährlich werden!', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'SAFETY_YELLOW_DESCRIPTION', 'Vorsicht ist geboten beim Start <ul> <li>in unbekannten Landschaftsregionen (z.B. Alpen)</li> <li>auf unbekannten Fluggeländen</li><li>auf selten geflogenen Flugzeugmustern</li><li>in einer selten durchgeführten Startart</li> </ul>', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'SAFETY_RED_TITLE', 'Roter Bereich', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'SAFETY_RED_SUBTITLE', 'Übung tut Not - Fliegen kann zum Risiko werden!', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'SAFETY_RED_DESCRIPTION', 'Für ungeübte Piloten gilt: <ul> <li>die ersten Starts nach einer längeren Pause nur mit vertrauten Mustern und bei unkritischen Wetterlagen durchführen</li> <li>falls der letzte Start mehr als drei Monate zurück liegt, ist Training mit einem Fluglehrer der beste Weg zu einem guten Übungsstand</li> </ul>', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'AIR_MOVEMENTS', 'Luftbewegungen', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'GLIDER_STARTLIST', 'Segelflug Startliste', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'MOTOR_PLANE', 'Motorflugzeug', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'SYSTEM', 'System', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'LOGS', 'Logs', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'AIRPLANE', 'Flugzeug', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'MOTOR_MOVEMENTS', 'Motorflug Bewegungen', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'COMMUNICATION', 'Kommunikation', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'LICENSE', 'Lizenz', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'HAS_GLIDER_TOWING_START_PERMISSION', 'Schleppstart Zulassung', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'HAS_GLIDER_SELF_START_PERMISSION', 'Eigenstart Zulassung', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'HAS_GLIDER_WINCH_START_PERMISSION', 'Windenstart Zulassung', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'CLUB_SETTINGS', 'Club Einstellungen', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'NOTIFICATION_SETTINGS', 'Notifikationseinstellungen', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'MEDICAL_CLASS1_EXPIRE_DATE', 'Medical Class 1 Gültigkeit bis', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'MEDICAL_CLASS2_EXPIRE_DATE', 'Medical Class 2 Gültigkeit bis', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'MEDICAL_LAPL_EXPIRE_DATE', 'Medical LAPL Gültigkeit bis', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'BLOCK_TIME_BEGIN', 'Beginn Blockzeit', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'BLOCK_TIME_END', 'Ende Blockzeit', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'BLOCK_TIME_DURATION', 'Blockzeit', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'ENGINE_BEGIN', 'Beginn Motorzählerst.', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'ENGINE_END', 'Ende Motorzählerst.', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'ENGINE_DURATION', 'Motorlaufzeit', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'LAST_ENGINE_TIME', 'Letzer Zählerstand', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'ERROR_NO_CORRESPONDING_PERSON', 'Der Benutzer "{{user}}" hat keine zugewiesene Person, weshalb keine Statistiken angezeigt werden.', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'SEND_AIRCRAFT_STATISTIC_REPORT_TO', 'Flugzeug Statistik an (Email)', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'SEND_PLANNING_DAY_INFO_MAIL_TO', 'Planungstage Informationen an (Email)', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'SEND_DELIVERY_MAIL_EXPORT_TO', 'Rechnungs-Informationen an (Email)', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'TECHNICAL_DATA', 'Technische Daten', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'OPERATIONAL_DATA', 'Betriebliche Daten', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'HISTORY_OF', 'Historie von ', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'MODIFICATION', 'Veränderung', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'CLOSE', 'Schliessen', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'PLANNINGDAY', 'Planungs-Tag', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FLIGHT', 'Flug', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'PERSON', 'Person', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FLIGHTTYPE', 'Flugtyp', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'RESERVATION', 'Reservation', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'TIME', 'Zeit', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'GLIDER_INSTRUCTOR_LICENCE_EXPIRE_DATE', 'Segelfluglehrer Lizenz Gültigkeit bis', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'WINCH_OPERATOR', 'Windenführer', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'CHOOSE_PASSWORD', 'Passwort wählen', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'PASSWORD_CONFIRM', 'Passwort bestätigen', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'SET_PASSWORD', 'Passwort setzen', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'CHOOSE_PASSWORD_FORM_EXPLANATION', 'Bitte geben sie ihr neues Passwort ein, um den Benutzer zu aktivieren.', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'PASSWORD_MISMATCH', 'Die eingegebenen Passwörter stimmen nicht überein.', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'LANDED', 'gelandet', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'AIRBORN', 'in der Luft', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'WAITING', 'wartend', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'COPY', 'Kopieren', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'SHOW_HISTORY', 'Historie anzeigen', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'DELETE', 'Löschen', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'EDIT', 'Bearbeiten', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'NR_OF_PASSENGERS', 'Anzahl Passagiere', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'SEATS_INSUFFICIENT', 'Zu wenig Sitzplätze', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'MINIMUM_NUMBER_OF_SEATS_REQUIRED', 'Mninimale Anzahl Sitzplätze', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'BIRTHDAY', 'Geburtstag', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'MY_LICENSE_STATE', 'Mein Lizenz-Stand', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'NO_MEDICAL_INFO', 'Hier würde das Ablaufdatum des Medicals angezeigt werden. Das Datum kann unter folgendem Link ganz einfach in der Rubrik "Lizenz" erfasst werden:', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'RESET', 'Reset', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FILTER_USER_STATE_Aktiv', 'Aktive Benutzer', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FILTER_USER_STATE_Deaktiviert', 'Deaktivierte Benutzer', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FILTER_USER_STATE_Gesperrt', 'Gesperrte Benutzer', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FILTER_PERSON_FLAG_HasGliderInstructorLicence', 'Segelflug Lehrer', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FILTER_PERSON_FLAG_HasGliderPilotLicence', 'Segelflug Piloten', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FILTER_PERSON_FLAG_HasGliderTraineeLicence', 'Segelflug Schüler', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FILTER_PERSON_FLAG_HasMotorPilotLicence', 'Motorflug Piloten', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FILTER_PERSON_FLAG_HasTowPilotLicence', 'Schlepp Piloten', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FILTER_PERSON_FLAG_HasMotorInstructorLicence', 'Motorflug Lehrer', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'ALL', 'Alle', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FLIGHT_PERMISSION', 'Flugerlaubnis', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'MEMBER_STATE', 'Mitgliederstatus', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FLIGHT_COUNTER_UNIT_TYPE', 'Flug Zähler Einheit', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'ENGINE_COUNTER_UNIT_TYPE', 'Motor Zähler Einheit', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'TOW_DURATION', 'Schleppdauer', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'GLIDER_FLIGHT_DURATION', 'Segelflug Dauer', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FLIGHT_DURATION', 'Flugdauer', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'ACCOUNTING_RULE_FILTER_TARGET', 'Verrechnungsregel Ziel', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'ACCOUNTING_RULE_FILTER_TYPE', 'Regel-Typ', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'ACCOUNTING_RULE_FILTER_NAME', 'Filter-Name', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'ARTICLE', 'Artikel', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'ACCOUNTING_RULE_FILTERS', 'Verrechnungs-Regeln', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FOR_SELF_STARTER_GLIDER_FLIGHTS', 'Für Selbstarter-Segelflugzeug Flüge', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FOR_GLIDER_FLIGHTS', 'Für Segelflüge', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FOR_TOWING_FLIGHTS', 'Für Schleppflüge', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FOR_MOTOR_FLIGHTS', 'Für Motorflüge', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'ACTIVE', 'Aktiv', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'START_LOCATIONS', 'Startplätze', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'LANDING_LOCATIONS', 'Landeplätze', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'DELIVERY_LINE_TEXT', 'Buchungszeile', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'RECIPIENT', 'Empfänger', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'RECIPIENT_NAME', 'Empfänger Name', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'NR_LANDINGS_AT_START_LOCATION', 'Anz. Ldg am Startort', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'NR_LANDINGS_AT_LANDING_LOCATION', 'Anz. Ldg am Landeort', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'CLUB_MEMBERS', 'Clubmitglieder', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'IS_CHARGED_TO_CLUB_INTERNAL', 'Intern abbuchen (nicht verrechnen)', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'OR_FLIGHT_DURATION_BETWEEN', 'oder Flugdauer zwischen minimum:', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'AND_MAXIMUM_INCL', 'und maximum (inkl.): ', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'MINUTES', 'Minuten', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'SECONDS', 'Sekunden', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FLIGHT_DURATION_UNLIMITED', 'Flugdauer unbegrenzt', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'THREASHOLD_TEXT', 'Threshold-Text für Buchungstext', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'INCLUDE_FLIGHT_TYPE_IN_BOOKING_TEXT', 'Flugart-Bezeichnung in Buchungstext integrieren', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'NO_LANDING_TAX_FOR_GLIDER', 'Keine Landetaxen für Segelflugzeug', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'NO_LANDING_TAX_FOR_TOWING_AIRCRAFT', 'Keine Landetaxen für Schleppflugzeug', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'NO_LANDING_TAX_FOR_AIRCRAFT', 'Keine Landetaxen für Motorflugzeug', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FLIGHT_CREW_TYPES', 'Crew Typen', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'TRY_FLIGHT_APPLICATION', 'Schnupperflug Anmeldung', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'TRY_FLIGHT_FORM_EXPLANATION', 'Schnupperflug-Kandidat:', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'PRIVATE_PHONE', 'Tel. Privat', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'BUSINESS_PHONE', 'Tel. Geschäft', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'INVOICE_TO', 'Rechnung an', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'CHOOSE_TRY_FLIGHT_DAY', 'Schnuppertage (bitte passenden Tag ankreuzen)', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'SAME_ADDRESS_AS_PARTICIPANT', 'gleiche Adresse wie Schnupperflug-Kandidat', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'APPLY_FOR_TRY_FLIGHT', 'Anmeldung abschicken', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'APPLICATION_FOR_TRY_FLIGHT_WAS_SUCCESSFUL', 'Vielen Dank für Ihre Anmeldung. Wir freuen uns auf einen erfolgreichen Schnupperflugtag.', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'BACK_TO_HOMEPAGE', 'Zurück zur Homepage', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FLIGHT_STATES', 'Flug Status', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'MOTOR_STATES', 'Motorflug Status', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FLIGHT_READY', 'Flugzeug bereit', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FLIGHT_IN_AIR', 'Flugzeug in der Luft', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FLIGHT_LANDED', 'Flugzeug gelandet', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FLIGHT_INVALID', 'Flug unvollständig', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FLIGHT_VALID', 'Flug gültig', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FLIGHT_LOCKED', 'Flug gesperrt', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FLIGHT_DELIVERED', 'Flug verrechnet', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FLIGHT_DELIVERYPREPARATIONERROR', 'Lieferschein für Flug fehlerhaft', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FLIGHT_DELIVERYPREPARED', 'Lieferschein für Flug erstellt', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FLIGHT_DELIVERYBOOKED', 'Lieferschein verbucht', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FLIGHT_EXCLUDEDFROMDELIVERYPROCESS', 'Flug aus der Verrechnung ausgeschlossen', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'DELIVERY_PREPARATION_ERROR', 'Lieferschein für Flug fehlerhaft', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'DELIVERY_PREPARED', 'Lieferschein für Flug erstellt', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'DELIVERY_BOOKED', 'Lieferschein verbucht', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'EXCLUDED_FROM_DELIVERY_PROCESS', 'Flug aus der Verrechnung ausgeschlossen', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'COPY_FROM_LAST_FLIGHT', 'Vom letzten Flug kopieren', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'NO_TIME_INFORMATION', 'Keine Zeitangabe', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'VALIDATION_FAILED', 'Validierung fehlerhaft', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'DELIVERED', 'verrechnet', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'SEND_COUPON_TO', 'Gutschein', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'SEND_COUPON_TO_INVOICE_RECIPIENT', 'An Rechnungsempfänger schicken', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'SEND_COUPON_TO_CANDIDATE', 'Direkt an Schnupperflug-Kandidat schicken', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'DELIVERY_CREATION_TESTS', 'Verrechnungs-Regel-Tests', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'DELIVERY_CREATION_TEST_NAME', 'Test-Name', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FLIGHT_ID', 'Flug-ID', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'EXPECTED_DELIVERY_DETAILS', 'Erwartete Details', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'CREATE_TEST_DELIVERY', 'Test-Delivery erzeugen', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'RUN_ALL_TESTS', 'Alle Tests ausführen', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'RUN', 'Ausführen', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'RESEND_EMAIL_TOKEN', 'Email nicht bestätigt. Bestätigung erneut senden', 1, SYSDATETIME())



INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'VALIDATION_ERROR_No_towing_flight_referenced_for_towed_glider_flight', 'Kein Schleppflug erfasst für Segelflug mit Flugzeugschlepp', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'VALIDATION_ERROR_Towing_flight_referenced_for_externally_started_glider_flight', 'Schleppflug erfasst für extern gestarteter Segelflug', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'VALIDATION_ERROR_No_winch_operator_set_for_winch_started_glider_flight', 'Kein Windenführer erfasst für Windenstart', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'VALIDATION_ERROR_No_flight_date_set', 'Kein Flugdatum für Flug erfasst', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'VALIDATION_ERROR_No_aircraft_set', 'Kein Flugzeug für Flug erfasst', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'VALIDATION_ERROR_No_pilot_set', 'Kein Pilot für Flug erfasst', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'VALIDATION_ERROR_No_start_time_information_set', 'Keine Startzeit für Flug erfasst', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'VALIDATION_ERROR_No_landing_time_information_set', 'Keine Landezeit für Flug erfasst', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'VALIDATION_ERROR_No_start_location_set', 'Kein Startort für Flug erfasst', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'VALIDATION_ERROR_No_landing_location_set', 'Kein Landeort für Flug erfasst', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'VALIDATION_ERROR_No_start_type_set', 'Keine Startart für Flug ausgewählt', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'VALIDATION_ERROR_No_flight_type_set', 'Keine Flugart für Flug ausgewählt', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'VALIDATION_ERROR_Number_of_landings_not_set', 'Anzahl Landungen für Flug nicht erfasst', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'VALIDATION_ERROR_Number_of_landings_is_less_then_1', 'Anzahl Landungen ist kleiner als 1', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'SAVE_AND_COPY', 'Speichern und Kopieren', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'VALIDATION', 'Validierung', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'CONFIRM_VALIDATE', 'Mit dieser Funktion werden alle Flüge validiert. Um Fehler in der Validierung zu vermeiden, sollte die Validierung erst am Ende des Flugtages durchgeführt werden. Soll die Validierung gestartet werden?', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'PERSON_CATEGORY', 'Personen Kategorie', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'PERSON_CATEGORIES', 'Personen Kategorien', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'STOP_RULE_ENGINE_WHEN_RULE_APPLIED', 'Bei Regel-Treffer nachfolgende Regeln überspringen', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'ACCOUNTING_UNIT_TYPE', 'Accounting Einheit', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'ADD_AIRCRAFT', 'Flugzeug hinzufügen', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'Error_DbEntityValidationException', 'Datenbank-Validierungs-Fehler. Datensatz konnte nicht gespeichert oder aktualisiert werden!', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'Error_GeneralDatabaseException', 'Genereller Datenbank-Fehler. Datensatz konnte nicht gespeichert oder aktualisiert werden!', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'Error_UserNotAuthenticated', 'Benutzer ist nicht authentiziert.', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'Error_SoftDeleteDatabaseException', 'Datenbank-Fehler. Datensatz konnte nicht gelöscht bzw. aktualisiert werden.', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'Error_ArgumentOutOfRange', 'Das Argument ist ausserhalb des gültigen Wertebereichs. Argument: {{ArgumentName}}', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'Error_ReservationTypeNotFound', 'Reservierungs-Typ nicht gefunden!', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'Error_StartLocationsOfGliderAndTowFlightsNotEqual', 'Start-Flugplatz ist nicht gleich zwischen Segelflug und Schleppflug!', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'Error_StartTimeOfGliderAndTowFlightsNotEqual', 'Start-Zeit ist nicht gleich zwischen Segelflug und Schleppflug!', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'Error_TowFlightWithoutGliderFlightIsNotValid', 'Schleppflug ohne Segelflug ist nicht gültig und kann nicht abgespeichert werden.', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'Error_InternalServerException', 'Ein interner Server-Fehler ist aufgetreten. Das Entwickler-Team wurde informiert.', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'Error_NotInRole_ClubAdmin', 'Keine Berechtigung! Zum Ausführen dieser Aktion musst du Club-Administrator sein.', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'Error_InvalidCastException', 'Fehler bei der Konvertierung eines Wertes.', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'Error_CantDeleteClubDueToActiveUsers', 'Club kann nicht gelöscht werden, da er noch aktivierte Benutzer hat.', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'Error_NotInSameClub', 'Keine Berechtigung zum Ausführen dieser Aktion.', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'MOTOR_INSTRUCTOR_LICENCE_EXPIRE_DATE', 'Motorfluglehrer Lizenz Gültigkeit bis', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'PART_M_LICENCE_EXPIRE_DATE', 'Part-M Lizenz Gültigkeit bis', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'HAS_PART_M_LICENCE', 'Part-M Lizenz', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'SAVE_USER_SETTINGS', 'Benutzerdaten speichern', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FLIGHTREPORTS', 'Reports', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'REPORT_MY_FLIGHTS_TODAY', 'Heute', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'REPORT_MY_FLIGHTS_YESTERDAY', 'Gestern', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'REPORT_MY_FLIGHTS_LAST_7_DAYS', 'Letzte 7 Tage', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'REPORT_MY_FLIGHTS_LAST_30_DAYS', 'Letzte 30 Tage', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'REPORT_MY_FLIGHTS_LAST_12_MONTHS', 'Letzte 12 Monate', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'REPORT_MY_FLIGHTS_LAST_24_MONTHS', 'Letzte 24 Monate', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'REPORT_MY_FLIGHTS_THIS_YEAR', 'In diesem Jahr', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'REPORT_MY_FLIGHTS_PREVIOUS_YEAR', 'Im Vorjahr', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'LOCATION_REPORTS', 'Flüge auf Heimflugplatz', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'REPORT_LOCATION_FLIGHTS_TODAY', 'Heute', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'REPORT_LOCATION_FLIGHTS_YESTERDAY', 'Gestern', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'REPORT_LOCATION_FLIGHTS_THIS_YEAR', 'In diesem Jahr', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'REPORT_LOCATION_FLIGHTS_PREVIOUS_YEAR', 'Im Vorjahr', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'MY_REPORTS', 'Meine Flüge', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FLIGHT_REPORT_FILTER_CRITERIA', 'Report Kriterien', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'YES', 'Ja', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'NO', 'Nein', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'GLIDER_FLIGHTS', 'Segelflüge', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'MOTOR_FLIGHTS', 'Motorflüge', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'TOW_FLIGHTS', 'Schleppflüge', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'LANGUAGE', 'Sprache', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'GROUP_BY', 'Gruppiert nach', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'TOTAL_STARTS', 'Total Starts', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'TOTAL_FLIGHT_DURATION', 'Total Flugzeit', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'TOTAL_FLIGHTS', 'Total Flüge', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'TOTAL_LANDINGS', 'Total Landungen', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'LAST_MATCHED_RULE_FILTERS', 'Übereinstimmende Verrechnungs-Regeln beim Test', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'LAST_DELIVERY_DETAILS', 'Letzte Lieferschein-Details beim Test', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'NONE', 'Keine', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'INVERT', 'Invertieren', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'PASSENGER_FLIGHT_APPLICATION', 'Passagierflug Anmeldung', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'PASSENGER_FLIGHT_FORM_EXPLANATION', 'Angaben zum Passagier:', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'SAME_ADDRESS_AS_PASSENGER', 'gleiche Adresse wie Passagier', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'APPLY_FOR_PASSENGER_FLIGHT', 'Registrierung abschicken', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'APPLICATION_FOR_PASSENGER_FLIGHT_WAS_SUCCESSFUL', 'Vielen Dank für Ihre Registrierung. Wir freuen uns auf einen unvergesslichen Passagierflug.', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'SEND_COUPON_TO_PASSENGER', 'Direkt an Passagier schicken', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'DELIVERIES', 'Lieferscheine', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'DELIVERED_ON', 'LS erstellt am', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'DELIVERY_NUMBER', 'Lieferscheinnummer', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'BATCH_ID', 'Batch Id', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'DELIVERY_INFORMATION', 'LS Informationen', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'DELIVERY_RECIPIENTNAME', 'LS Empfänger', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'DELIVERY_FLIGHT_INFORMATION_IMMATRICULATION', 'Flugzeug', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'DELIVERY_FLIGHT_INFORMATION_START_DATETIME', 'Startzeit', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'POSITION', 'Position', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'ARTICLE_NUMBER', 'Artikelnummer', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'ITEM_TEXT', 'Artikelbezeichnung', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'QUANTITY', 'Menge', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'UNIT_TYPE', 'Einheit', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'IS_FURTHER_PROCESSED', 'Weiter verarbeitet', 1, SYSDATETIME())

INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'DELIVERY_ITEMS', 'Lieferpositionen', 1, SYSDATETIME())
	 
INSERT INTO [dbo].[LanguageTranslations] ([LanguageTranslationId], [TranslationKey], [TranslationValue], [LanguageId], [CreatedOn])
     VALUES (NEWID(), 'FLIGHT_INFORMATION', 'Flug-Informationen', 1, SYSDATETIME())

GO