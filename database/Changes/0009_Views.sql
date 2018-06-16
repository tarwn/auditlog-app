CREATE TABLE dbo.Views (
	Id uniqueidentifier NOT NULL,
	CustomerId int NOT NULL,
	AccessKey varchar(MAX) NOT NULL,
	Content varchar(MAX) NOT NULL,

	CONSTRAINT PK_Views PRIMARY KEY CLUSTERED(Id ASC),
	CONSTRAINT FK_Views_Customers FOREIGN KEY ([CustomerId]) REFERENCES dbo.Customers(Id)
		ON DELETE CASCADE
)