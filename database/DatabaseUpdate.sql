/* SQL Core Updates - Updated 2018-06-03 13:34 */
BEGIN TRY
BEGIN TRANSACTION

/* File: 0001_AddCustomerandUser.sql */
IF NOT EXISTS (SELECT 1 FROM UpdateTracking WHERE Name = '0001_AddCustomerandUser')
BEGIN
	Print 'Applying Update: 0001_AddCustomerandUser'
	EXEC('
		CREATE TABLE dbo.Customers (
			[Id] int IDENTITY(1,1) NOT NULL,
			[DisplayName] varchar(80) NOT NULL,
		
			CONSTRAINT PK_Customers PRIMARY KEY CLUSTERED(Id ASC)
		);
		
		CREATE TABLE dbo.Users (
			[Id] int IDENTITY(1,1) NOT NULL,
			[CustomerId] int NOT NULL,
			[DisplayName] varchar(80) NOT NULL,
		
			CONSTRAINT PK_Users PRIMARY KEY CLUSTERED(Id ASC),
			CONSTRAINT FK_Users_Customers FOREIGN KEY (CustomerId) REFERENCES dbo.Customers(Id)
				ON DELETE CASCADE
		);
	');
	INSERT INTO UpdateTracking(Name, Applied) SELECT '0001_AddCustomerandUser', GETUTCDATE();
END

/* File: 0002_Authentication.sql */
IF NOT EXISTS (SELECT 1 FROM UpdateTracking WHERE Name = '0002_Authentication')
BEGIN
	Print 'Applying Update: 0002_Authentication'
	EXEC('
		
		-- expand the display name size
		ALTER TABLE dbo.Users ALTER COLUMN DisplayName varchar(80) NOT NULL;
		-- add username as unique column
		ALTER TABLE dbo.Users ADD Username varchar(80) NOT NULL;
		ALTER TABLE dbo.Users ADD CONSTRAINT AK_Username UNIQUE(Username);
		-- add email address
		ALTER TABLE dbo.Users ADD EmailAddress varchar(80) NOT NULL;
		ALTER TABLE dbo.Users ADD PasswordResetKey uniqueidentifier NULL;
		ALTER TABLE dbo.Users ADD PasswordResetGoodUntil DateTime2(7) NULL;
		ALTER TABLE dbo.Users ADD IsEnabled bit NOT NULL;
		
		-- add the authentication table for users
		CREATE TABLE dbo.UserAuthenticationMethods (
			[Id] uniqueidentifier NOT NULL,
			[UserId] int NOT NULL,
			[CredentialType] int NOT NULL,
			[Secret] varchar(100) NOT NULL,
			[DisplayName] varchar(80) NOT NULL,
			[CreationTime] datetime2(7) NOT NULL,
			[IsRevoked] bit NOT NULL,
			[RevokeTime] datetime2(7) NULL,
		
			CONSTRAINT PK_UserAuthenticationMethods PRIMARY KEY CLUSTERED(Id ASC),
			CONSTRAINT FK_UserAuthenticationMethods_Users FOREIGN KEY (UserId) REFERENCES dbo.Users(Id)
				ON DELETE CASCADE
		);
		
		-- add sessions
		CREATE TABLE dbo.UserSessions (
			[Id] uniqueidentifier NOT NULL,
			[UserId] int NOT NULL,
			[CreationTime] datetime2(7) NOT NULL,
			[LogoutTime] datetime2(7) NULL,
		
			CONSTRAINT PK_UserSessions PRIMARY KEY CLUSTERED(Id ASC),
			CONSTRAINT FK_UserSessions_Users FOREIGN KEY (UserId) REFERENCES dbo.Users(Id)
				ON DELETE CASCADE
		);
	');
	INSERT INTO UpdateTracking(Name, Applied) SELECT '0002_Authentication', GETUTCDATE();
END

/* File: 0004_CustomerAuthentication.sql */
IF NOT EXISTS (SELECT 1 FROM UpdateTracking WHERE Name = '0004_CustomerAuthentication')
BEGIN
	Print 'Applying Update: 0004_CustomerAuthentication'
	EXEC('
		CREATE TABLE dbo.CustomerAuthenticationMethods (
			[Id] uniqueidentifier NOT NULL,
			[CustomerId] int NOT NULL,
			[CredentialType] int NOT NULL,
			[Secret] varchar(100) NOT NULL,
			[DisplayName] varchar(80) NOT NULL,
			[CreationTime] datetime2(7) NOT NULL,
			[CreatedBy] int NOT NULL,
			[IsRevoked] bit NOT NULL,
			[RevokeTime] datetime2(7) NULL,
		
			CONSTRAINT PK_CustomerAuthenticationMethods PRIMARY KEY CLUSTERED(Id ASC),
			CONSTRAINT FK_CustomerAuthenticationMethods_Customers FOREIGN KEY ([CustomerId]) REFERENCES dbo.Customers(Id)
				ON DELETE CASCADE,
			CONSTRAINT FK_CustomerAuthenticationMethods_Users FOREIGN KEY ([CreatedBy]) REFERENCES dbo.Users(Id)
		);
	');
	INSERT INTO UpdateTracking(Name, Applied) SELECT '0004_CustomerAuthentication', GETUTCDATE();
END

/* File: 0005_CustomerCreated.sql */
IF NOT EXISTS (SELECT 1 FROM UpdateTracking WHERE Name = '0005_CustomerCreated')
BEGIN
	Print 'Applying Update: 0005_CustomerCreated'
	EXEC('
		ALTER TABLE dbo.Customers ADD CreationTime datetime2(7) NOT NULL;
	');
	INSERT INTO UpdateTracking(Name, Applied) SELECT '0005_CustomerCreated', GETUTCDATE();
END

/* File: 0006_TestData.sql */
IF NOT EXISTS (SELECT 1 FROM UpdateTracking WHERE Name = '0006_TestData')
BEGIN
	Print 'Applying Update: 0006_TestData'
	EXEC('
		TRUNCATE TABLE dbo.UserAuthenticationMethods;
		DELETE FROM dbo.Users;
		DELETE FROM dbo.CUstomers;
		');



	EXEC('
		
		IF EXISTS (SELECT * FROM sys.identity_columns WHERE OBJECT_NAME(OBJECT_ID) = ''Customers'' AND last_value IS NOT NULL) 
			DBCC CHECKIDENT (''[Customers]'', RESEED, 0);
		IF EXISTS (SELECT * FROM sys.identity_columns WHERE OBJECT_NAME(OBJECT_ID) = ''Users'' AND last_value IS NOT NULL) 
			DBCC CHECKIDENT (''[Users]'', RESEED, 0);
		');



	EXEC('
		
		BEGIN
			DECLARE @PasswordHashCredentialType bit = 1;
		
			DECLARE @CustomerId int,
					@UserId int;
		
			INSERT INTO dbo.Customers(DisplayName, CreationTime) VALUES(''Sample Customer'', GetUtcDate());
			SET @CustomerId = SCOPE_IDENTITY();
		
			INSERT INTO dbo.Users(CustomerId, Username, DisplayName, EmailAddress, IsEnabled) VALUES(@CustomerId, ''Sample User'', ''Sample User'', ''Sample@User.Text'', 1);
			SET @UserId = SCOPE_IDENTITY();
		
			INSERT INTO dbo.UserAuthenticationMethods(Id, UserId, CredentialType, Secret, DisplayName, CreationTime, IsRevoked)
			VALUES(''62A4C043-09E8-44FA-BA60-5702E5E32047'', @UserId, @PasswordHashCredentialType, ''secret'', ''display name'', GetUtcDate(), 0),
				  (''9E7512C5-CE29-4802-9D95-84CC643C805D'', @UserId, @PasswordHashCredentialType, ''secret 2'', ''display name 2'', GetUtcDate(), 0);
		END
	');
	INSERT INTO UpdateTracking(Name, Applied) SELECT '0006_TestData', GETUTCDATE();
END

/* File: 0007_AuditEventData.sql */
IF NOT EXISTS (SELECT 1 FROM UpdateTracking WHERE Name = '0007_AuditEventData')
BEGIN
	Print 'Applying Update: 0007_AuditEventData'
	EXEC('
		-- NOTE: No constraint on CutsomerId + UUID
		-- There is not a unique constraint on EventActors by CustomerId/UUID
		-- because the goal is to ensure actor info is kept seperate from the
		-- event data and up to date, rather than ensuring perfect uniqueness.
		-- If there are dupes, they will become consistent over time.
		
		CREATE TABLE dbo.EventActors (
			Id uniqueidentifier NOT NULL,
			CustomerId int NOT NULL,
			UUID varchar(80) NOT NULL,
			[Name] varchar(120) NULL,
			[Email] varchar(120) NULL,
		
			CONSTRAINT PK_EventActors PRIMARY KEY CLUSTERED(Id ASC),
			CONSTRAINT FK_EventActors_Customers FOREIGN KEY ([CustomerId]) REFERENCES dbo.Customers(Id)
		);
		
		CREATE TABLE dbo.EventEntries (
			-- Our Fields
			Id uniqueidentifier NOT NULL,
			CustomerId int NOT NULL,
			ReceptionTime datetime2(7) NOT NULL,
		
			-- Their Fields
			UUID varchar(80) NOT NULL,
			Client_Id varchar(120) NULL,
			Client_Name varchar(120) NULL,
			EventTime datetime2(7) NOT NULL,
			[Action] varchar(40) NOT NULL,
			[Description] varchar(120) NULL,
			[URL] varchar(400) NULL,
			EventActorId uniqueidentifier NOT NULL,
			Context_Client_IP varchar(15) NULL,
			Context_Client_BrowserAgent varchar(4096) NULL,
			Context_Server_ServerId varchar(240) NOT NULL,
			Context_Server_Version varchar(80) NOT NULL,
			Target_Type varchar(40) NULL,
			Target_UUID varchar(80) NULL,
			Target_Label varchar(120) NULL,
			Target_URL varchar(400) NULL,
		
			CONSTRAINT PK_EventEntries PRIMARY KEY CLUSTERED(Id ASC),
			CONSTRAINT FK_EventEntries_Customers FOREIGN KEY ([CustomerId]) REFERENCES dbo.Customers(Id)
				ON DELETE CASCADE,
			CONSTRAINT FK_EventEntries_EventActors FOREIGN KEY (EventActorId) REFERENCES dbo.EventActors(Id)
				ON DELETE CASCADE
		);
	');
	INSERT INTO UpdateTracking(Name, Applied) SELECT '0007_AuditEventData', GETUTCDATE();
END

/* File: 0008_ForgettableActors.sql */
IF NOT EXISTS (SELECT 1 FROM UpdateTracking WHERE Name = '0008_ForgettableActors')
BEGIN
	Print 'Applying Update: 0008_ForgettableActors'
	EXEC('
		ALTER TABLE dbo.EventActors ADD IsForgotten bit NULL;
		
		');



	EXEC('
		
		UPDATE dbo.EventActors SET IsForgotten = 1;
		
		');



	EXEC('
		
		ALTER TABLE dbo.EventActors ALTER COLUMN IsForgotten bit NOT NULL;
	');
	INSERT INTO UpdateTracking(Name, Applied) SELECT '0008_ForgettableActors', GETUTCDATE();
END
COMMIT TRANSACTION
END TRY BEGIN CATCH
ROLLBACK TRANSACTION

    -- copied from http://technet.microsoft.com/en-us/library/ms179296%28v=sql.105%29.aspx
    DECLARE 
        @ErrorMessage    NVARCHAR(4000),
        @ErrorNumber     INT,
        @ErrorSeverity   INT,
        @ErrorState      INT,
        @ErrorLine       INT,
        @ErrorProcedure  NVARCHAR(200);

    -- Assign variables to error-handling functions that 
    -- capture information for RAISERROR.
    SELECT 
        @ErrorNumber = ERROR_NUMBER(),
        @ErrorSeverity = ERROR_SEVERITY(),
        @ErrorState = ERROR_STATE(),
        @ErrorLine = ERROR_LINE(),
        @ErrorProcedure = ISNULL(ERROR_PROCEDURE(), '-');

    -- Build the message string that will contain original
    -- error information.
    SELECT @ErrorMessage = 
        N'Error %d, Level %d, State %d, Procedure %s, Line %d, ' + 
            'Message: '+ ERROR_MESSAGE();

    -- Print error information. 
    PRINT 'Error ' + CONVERT(varchar(50), ERROR_NUMBER()) +
          ', Severity ' + CONVERT(varchar(5), ERROR_SEVERITY()) +
          ', State ' + CONVERT(varchar(5), ERROR_STATE()) + 
          ', Procedure ' + ISNULL(ERROR_PROCEDURE(), '-') + 
          ', Line ' + CONVERT(varchar(5), ERROR_LINE());
    PRINT 'Error Message: ' + ERROR_MESSAGE();

    -- Raise an error: msg_str parameter of RAISERROR will contain
    -- the original error information.
    RAISERROR 
        (
        @ErrorMessage, 
        @ErrorSeverity, 
        1,               
        @ErrorNumber,    -- parameter: original error number.
        @ErrorSeverity,  -- parameter: original error severity.
        @ErrorState,     -- parameter: original error state.
        @ErrorProcedure, -- parameter: original error procedure name.
        @ErrorLine       -- parameter: original error line number.
        );

END CATCH
