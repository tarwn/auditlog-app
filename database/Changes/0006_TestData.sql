TRUNCATE TABLE dbo.UserAuthenticationMethods;
DELETE FROM dbo.Users;
DELETE FROM dbo.CUstomers;
GO

IF EXISTS (SELECT * FROM sys.identity_columns WHERE OBJECT_NAME(OBJECT_ID) = 'Customers' AND last_value IS NOT NULL) 
	DBCC CHECKIDENT ('[Customers]', RESEED, 0);
IF EXISTS (SELECT * FROM sys.identity_columns WHERE OBJECT_NAME(OBJECT_ID) = 'Users' AND last_value IS NOT NULL) 
	DBCC CHECKIDENT ('[Users]', RESEED, 0);
GO

BEGIN
	DECLARE @PasswordHashCredentialType bit = 1;

	DECLARE @CustomerId int,
			@UserId int;

	INSERT INTO dbo.Customers(DisplayName, CreationTime) VALUES('Sample Customer', GetUtcDate());
	SET @CustomerId = SCOPE_IDENTITY();

	INSERT INTO dbo.Users(CustomerId, Username, DisplayName, EmailAddress, IsEnabled) VALUES(@CustomerId, 'Sample User', 'Sample User', 'Sample@User.Text', 1);
	SET @UserId = SCOPE_IDENTITY();

	INSERT INTO dbo.UserAuthenticationMethods(Id, UserId, CredentialType, Secret, DisplayName, CreationTime, IsRevoked)
	VALUES('62A4C043-09E8-44FA-BA60-5702E5E32047', @UserId, @PasswordHashCredentialType, 'secret', 'display name', GetUtcDate(), 0),
		  ('9E7512C5-CE29-4802-9D95-84CC643C805D', @UserId, @PasswordHashCredentialType, 'secret 2', 'display name 2', GetUtcDate(), 0);
END
