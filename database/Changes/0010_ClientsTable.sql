CREATE TABLE dbo.EventClients (
	Id uniqueidentifier NOT NULL,
	CustomerId int NOT NULL,
	UUID varchar(80) NOT NULL,
	[Name] varchar(120) NULL

	CONSTRAINT PK_EventClients PRIMARY KEY CLUSTERED(Id ASC),
	CONSTRAINT FK_EventClients_Customers FOREIGN KEY ([CustomerId]) REFERENCES dbo.Customers(Id)
);

GO

TRUNCATE TABLE dbo.EventEntries;

GO

ALTER TABLE dbo.EventEntries ADD EventClientId uniqueidentifier NOT NULL;
ALTER TABLE dbo.EventEntries ADD CONSTRAINT FK_EventEntries_EventClients FOREIGN KEY (EventClientId) REFERENCES dbo.EventClients(Id)
		ON DELETE CASCADE;

ALTER TABLE dbo.EventEntries DROP COLUMN Client_Id;
ALTER TABLE dbo.EventEntries DROP COLUMN Client_Name;
