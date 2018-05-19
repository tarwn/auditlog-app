/* SQL Core Updates - Updated 2018-05-19 14:07 */
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

/* File: TestData.sql */
IF NOT EXISTS (SELECT 1 FROM UpdateTracking WHERE Name = 'TestData')
BEGIN
	Print 'Applying Update: TestData'
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
		
			INSERT INTO dbo.Users(CustomerId, DisplayName) VALUES(@CustomerId, ''Sample User'');
			SET @UserId = SCOPE_IDENTITY();
		
		END
	');
	INSERT INTO UpdateTracking(Name, Applied) SELECT 'TestData', GETUTCDATE();
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
