ALTER TABLE dbo.EventActors ADD IsForgotten bit NULL;

Go

UPDATE dbo.EventActors SET IsForgotten = 0;

Go 

ALTER TABLE dbo.EventActors ALTER COLUMN IsForgotten bit NOT NULL;
