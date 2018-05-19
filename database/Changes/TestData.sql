DELETE FROM dbo.Users;
DELETE FROM dbo.CUstomers;
GO

IF EXISTS (SELECT * FROM sys.identity_columns WHERE OBJECT_NAME(OBJECT_ID) = 'Customers' AND last_value IS NOT NULL) 
	DBCC CHECKIDENT ('[Customers]', RESEED, 0);
IF EXISTS (SELECT * FROM sys.identity_columns WHERE OBJECT_NAME(OBJECT_ID) = 'Users' AND last_value IS NOT NULL) 
	DBCC CHECKIDENT ('[Users]', RESEED, 0);
GO

BEGIN
	DECLARE @CustomerId int,
			@UserId int;

	INSERT INTO dbo.Customers(DisplayName) VALUES('Sample Customer');
	SET @CustomerId = SCOPE_IDENTITY();

	INSERT INTO dbo.Users(CustomerId, DisplayName) VALUES(@CustomerId, 'Sample User');
	SET @UserId = SCOPE_IDENTITY();

END