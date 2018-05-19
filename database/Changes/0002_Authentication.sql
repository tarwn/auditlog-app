
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