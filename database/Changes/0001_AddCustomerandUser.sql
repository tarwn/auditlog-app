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
