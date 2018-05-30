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
