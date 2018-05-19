/* SQL Core Updates - Updated 2018-05-21 21:27 */
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

/* File: 0003_TestData.sql */
IF NOT EXISTS (SELECT 1 FROM UpdateTracking WHERE Name = '0003_TestData')
BEGIN
	Print 'Applying Update: 0003_TestData'
	EXEC('
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
			DECLARE @CustomerId int,
					@UserId int;
		
			INSERT INTO dbo.Customers(DisplayName) VALUES(''Sample Customer'');
			SET @CustomerId = SCOPE_IDENTITY();
		
			INSERT INTO dbo.Users(CustomerId, Username, DisplayName, EmailAddress, IsEnabled) VALUES(@CustomerId, ''Sample User'', ''Sample User'', ''Sample@User.Text'', 1);
			SET @UserId = SCOPE_IDENTITY();
		
		END
	');
	INSERT INTO UpdateTracking(Name, Applied) SELECT '0003_TestData', GETUTCDATE();
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
