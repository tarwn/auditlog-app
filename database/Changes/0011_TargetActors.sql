ALTER TABLE EventEntries ADD TargetEventActorId uniqueidentifier NULL;
ALTER TABLE EventEntries ADD CONSTRAINT FK_EventEntries_EventActors_2 FOREIGN KEY (TargetEventActorId) REFERENCES dbo.EventActors(Id);

