Select Persons.PersonId, Persons.Lastname, Persons.Firstname, PersonClub.MemberNumber, Clubs.Clubname, PersonClub.MemberNumber, PersonClub.MemberStateId, MemberStates.MemberStateName,
                      PersonClub.IsMotorPilot, PersonClub.IsTowPilot, PersonClub.IsGliderInstructor, PersonClub.IsGliderPilot, 
                      PersonClub.IsGliderTrainee, PersonClub.IsPassenger, PersonClub.IsWinchOperator, PersonClub.CreatedOn, PersonClub.CreatedByUserId, PersonClub.ModifiedOn, 
                      PersonClub.ModifiedByUserId, PersonClub.DeletedOn, PersonClub.RecordState, PersonClub.OwnerId, PersonClub.IsDeleted  
From Persons
LEFT OUTER JOIN
                      PersonClub ON Persons.PersonId = PersonClub.PersonId FULL OUTER JOIN
                      Clubs ON PersonClub.ClubId = Clubs.ClubId
					  FULL OUTER JOIN MemberStates ON PersonClub.MemberStateId = MemberStates.MemberStateId
where Persons.IsDeleted = 0
order by Clubname, Lastname, Firstname