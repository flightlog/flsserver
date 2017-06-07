USE [FLSTest]
GO

DECLARE @buochs as uniqueidentifier
SET @buochs = 'C11CA58F-615D-469D-9D25-8729E7CA432C'

INSERT INTO [dbo].[InOutboundPoints]
           ([InOutboundPointId],[LocationId],[InOutboundPointName],[IsInboundPoint],[IsOutboundPoint],[CreatedOn],[CreatedByUserId],[RecordState],[OwnerId],[OwnershipType],[IsDeleted])
     VALUES
           (NEWID(),@buochs,'07N',1,0,SYSUTCDATETIME(),'13731EE2-C1D8-455C-8AD1-C39399893FFF',1,'A1DDE2CB-6326-4BB2-897D-7CFC118E842B',2,0)

INSERT INTO [dbo].[InOutboundPoints]
           ([InOutboundPointId],[LocationId],[InOutboundPointName],[IsInboundPoint],[IsOutboundPoint],[CreatedOn],[CreatedByUserId],[RecordState],[OwnerId],[OwnershipType],[IsDeleted])
     VALUES
           (NEWID(),@buochs,'07E',1,0,SYSUTCDATETIME(),'13731EE2-C1D8-455C-8AD1-C39399893FFF',1,'A1DDE2CB-6326-4BB2-897D-7CFC118E842B',2,0)

INSERT INTO [dbo].[InOutboundPoints]
           ([InOutboundPointId],[LocationId],[InOutboundPointName],[IsInboundPoint],[IsOutboundPoint],[CreatedOn],[CreatedByUserId],[RecordState],[OwnerId],[OwnershipType],[IsDeleted])
     VALUES
           (NEWID(),@buochs,'07S',1,0,SYSUTCDATETIME(),'13731EE2-C1D8-455C-8AD1-C39399893FFF',1,'A1DDE2CB-6326-4BB2-897D-7CFC118E842B',2,0)

INSERT INTO [dbo].[InOutboundPoints]
           ([InOutboundPointId],[LocationId],[InOutboundPointName],[IsInboundPoint],[IsOutboundPoint],[CreatedOn],[CreatedByUserId],[RecordState],[OwnerId],[OwnershipType],[IsDeleted])
     VALUES
           (NEWID(),@buochs,'07W',1,0,SYSUTCDATETIME(),'13731EE2-C1D8-455C-8AD1-C39399893FFF',1,'A1DDE2CB-6326-4BB2-897D-7CFC118E842B',2,0)

INSERT INTO [dbo].[InOutboundPoints]
           ([InOutboundPointId],[LocationId],[InOutboundPointName],[IsInboundPoint],[IsOutboundPoint],[CreatedOn],[CreatedByUserId],[RecordState],[OwnerId],[OwnershipType],[IsDeleted])
     VALUES
           (NEWID(),@buochs,'25N',1,0,SYSUTCDATETIME(),'13731EE2-C1D8-455C-8AD1-C39399893FFF',1,'A1DDE2CB-6326-4BB2-897D-7CFC118E842B',2,0)

INSERT INTO [dbo].[InOutboundPoints]
           ([InOutboundPointId],[LocationId],[InOutboundPointName],[IsInboundPoint],[IsOutboundPoint],[CreatedOn],[CreatedByUserId],[RecordState],[OwnerId],[OwnershipType],[IsDeleted])
     VALUES
           (NEWID(),@buochs,'25E',1,0,SYSUTCDATETIME(),'13731EE2-C1D8-455C-8AD1-C39399893FFF',1,'A1DDE2CB-6326-4BB2-897D-7CFC118E842B',2,0)

INSERT INTO [dbo].[InOutboundPoints]
           ([InOutboundPointId],[LocationId],[InOutboundPointName],[IsInboundPoint],[IsOutboundPoint],[CreatedOn],[CreatedByUserId],[RecordState],[OwnerId],[OwnershipType],[IsDeleted])
     VALUES
           (NEWID(),@buochs,'25S',1,0,SYSUTCDATETIME(),'13731EE2-C1D8-455C-8AD1-C39399893FFF',1,'A1DDE2CB-6326-4BB2-897D-7CFC118E842B',2,0)

INSERT INTO [dbo].[InOutboundPoints]
           ([InOutboundPointId],[LocationId],[InOutboundPointName],[IsInboundPoint],[IsOutboundPoint],[CreatedOn],[CreatedByUserId],[RecordState],[OwnerId],[OwnershipType],[IsDeleted])
     VALUES
           (NEWID(),@buochs,'25W',1,0,SYSUTCDATETIME(),'13731EE2-C1D8-455C-8AD1-C39399893FFF',1,'A1DDE2CB-6326-4BB2-897D-7CFC118E842B',2,0)

INSERT INTO [dbo].[InOutboundPoints]
           ([InOutboundPointId],[LocationId],[InOutboundPointName],[IsInboundPoint],[IsOutboundPoint],[CreatedOn],[CreatedByUserId],[RecordState],[OwnerId],[OwnershipType],[IsDeleted])
     VALUES
           (NEWID(),@buochs,'07N',0,1,SYSUTCDATETIME(),'13731EE2-C1D8-455C-8AD1-C39399893FFF',1,'A1DDE2CB-6326-4BB2-897D-7CFC118E842B',2,0)

INSERT INTO [dbo].[InOutboundPoints]
           ([InOutboundPointId],[LocationId],[InOutboundPointName],[IsInboundPoint],[IsOutboundPoint],[CreatedOn],[CreatedByUserId],[RecordState],[OwnerId],[OwnershipType],[IsDeleted])
     VALUES
           (NEWID(),@buochs,'07E',0,1,SYSUTCDATETIME(),'13731EE2-C1D8-455C-8AD1-C39399893FFF',1,'A1DDE2CB-6326-4BB2-897D-7CFC118E842B',2,0)

INSERT INTO [dbo].[InOutboundPoints]
           ([InOutboundPointId],[LocationId],[InOutboundPointName],[IsInboundPoint],[IsOutboundPoint],[CreatedOn],[CreatedByUserId],[RecordState],[OwnerId],[OwnershipType],[IsDeleted])
     VALUES
           (NEWID(),@buochs,'07S',0,1,SYSUTCDATETIME(),'13731EE2-C1D8-455C-8AD1-C39399893FFF',1,'A1DDE2CB-6326-4BB2-897D-7CFC118E842B',2,0)

INSERT INTO [dbo].[InOutboundPoints]
           ([InOutboundPointId],[LocationId],[InOutboundPointName],[IsInboundPoint],[IsOutboundPoint],[CreatedOn],[CreatedByUserId],[RecordState],[OwnerId],[OwnershipType],[IsDeleted])
     VALUES
           (NEWID(),@buochs,'07W',0,1,SYSUTCDATETIME(),'13731EE2-C1D8-455C-8AD1-C39399893FFF',1,'A1DDE2CB-6326-4BB2-897D-7CFC118E842B',2,0)

INSERT INTO [dbo].[InOutboundPoints]
           ([InOutboundPointId],[LocationId],[InOutboundPointName],[IsInboundPoint],[IsOutboundPoint],[CreatedOn],[CreatedByUserId],[RecordState],[OwnerId],[OwnershipType],[IsDeleted])
     VALUES
           (NEWID(),@buochs,'25N',0,1,SYSUTCDATETIME(),'13731EE2-C1D8-455C-8AD1-C39399893FFF',1,'A1DDE2CB-6326-4BB2-897D-7CFC118E842B',2,0)

INSERT INTO [dbo].[InOutboundPoints]
           ([InOutboundPointId],[LocationId],[InOutboundPointName],[IsInboundPoint],[IsOutboundPoint],[CreatedOn],[CreatedByUserId],[RecordState],[OwnerId],[OwnershipType],[IsDeleted])
     VALUES
           (NEWID(),@buochs,'25E',0,1,SYSUTCDATETIME(),'13731EE2-C1D8-455C-8AD1-C39399893FFF',1,'A1DDE2CB-6326-4BB2-897D-7CFC118E842B',2,0)

INSERT INTO [dbo].[InOutboundPoints]
           ([InOutboundPointId],[LocationId],[InOutboundPointName],[IsInboundPoint],[IsOutboundPoint],[CreatedOn],[CreatedByUserId],[RecordState],[OwnerId],[OwnershipType],[IsDeleted])
     VALUES
           (NEWID(),@buochs,'25S',0,1,SYSUTCDATETIME(),'13731EE2-C1D8-455C-8AD1-C39399893FFF',1,'A1DDE2CB-6326-4BB2-897D-7CFC118E842B',2,0)

INSERT INTO [dbo].[InOutboundPoints]
           ([InOutboundPointId],[LocationId],[InOutboundPointName],[IsInboundPoint],[IsOutboundPoint],[CreatedOn],[CreatedByUserId],[RecordState],[OwnerId],[OwnershipType],[IsDeleted])
     VALUES
           (NEWID(),@buochs,'25W',0,1,SYSUTCDATETIME(),'13731EE2-C1D8-455C-8AD1-C39399893FFF',1,'A1DDE2CB-6326-4BB2-897D-7CFC118E842B',2,0)

UPDATE [dbo].[Flights]
   SET [OutboundRoute] = NULL
 WHERE StartLocationId <> @buochs

 UPDATE [dbo].[Flights]
   SET [InboundRoute] = NULL
 WHERE LdgLocationId <> @buochs

  UPDATE [dbo].[Flights]
   SET [OutboundRoute] = '07E'
 WHERE StartLocationId = @buochs
 and [OutboundRoute] in ('06','06 OST','06  SO','06E','06O','07','07O','6E','O','Ost')

   UPDATE [dbo].[Flights]
   SET [OutboundRoute] = '07W'
 WHERE StartLocationId = @buochs
 and [OutboundRoute] in ('04W','06 SW','06 W','06W','07W')

    UPDATE [dbo].[Flights]
   SET [OutboundRoute] = '07S'
 WHERE StartLocationId = @buochs
 and [OutboundRoute] in ('06+S','06S','07S')
 
    UPDATE [dbo].[Flights]
   SET [OutboundRoute] = '07N'
 WHERE StartLocationId = @buochs
 and [OutboundRoute] in ('06N','07N')
  
    UPDATE [dbo].[Flights]
   SET [OutboundRoute] = '25E'
 WHERE StartLocationId = @buochs
 and [OutboundRoute] in ('24E')




 UPDATE [dbo].[Flights]
   SET [InboundRoute] = '25W'
 WHERE LdgLocationId = @buochs
 and [InboundRoute] in ('024', '24', '24 West', '240', '25W', '25', 'W', 'West', '24W', '24 SW')

 UPDATE [dbo].[Flights]
   SET [InboundRoute] = '25E'
 WHERE LdgLocationId = @buochs
 and [InboundRoute] in ('24O', '25O', '24 O', '24 SO', '24E')
  
 UPDATE [dbo].[Flights]
   SET [InboundRoute] = '25N'
 WHERE LdgLocationId = @buochs
 and [InboundRoute] in ('24N')
 
 UPDATE [dbo].[Flights]
   SET [InboundRoute] = '25S'
 WHERE LdgLocationId = @buochs
 and [InboundRoute] in ('25S', '25S', '24S')

 UPDATE [dbo].[Flights]
   SET [InboundRoute] = '07W'
 WHERE LdgLocationId = @buochs
 and [InboundRoute] in ('05W', '06W', '07W')

 UPDATE [dbo].[Flights]
   SET [InboundRoute] = '07N'
 WHERE LdgLocationId = @buochs
 and [InboundRoute] in ('Nord', '07N', '06N')

 UPDATE [dbo].[Flights]
   SET [InboundRoute] = '07E'
 WHERE LdgLocationId = @buochs
 and [InboundRoute] in ('06', '060', '06E', '06O', '07', '07O', 'O', 'O6O', 'Ost')

  UPDATE [dbo].[Flights]
   SET [InboundRoute] = '07S'
 WHERE LdgLocationId = @buochs
 and [InboundRoute] in ('06S', '07S')

 GO