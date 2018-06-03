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