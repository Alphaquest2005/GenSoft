delete  from EntityRelationships
delete from CalculatedProperties

delete from Types 
where id > 62


--DBCC CHECKIDENT (ProcessStepRelationship, RESEED, 0)
DBCC CHECKIDENT (EntityRelationships, RESEED, 0)
DBCC CHECKIDENT (EntityTypeViewModelCommands, RESEED, 1)
DBCC CHECKIDENT (EntityTypes, RESEED, 1)
DBCC CHECKIDENT (Entities, RESEED, 1)
DBCC CHECKIDENT (EntityAttributes, RESEED, 2)
DBCC CHECKIDENT (EntityTypeAttributes, RESEED, 2)
--dbcc checkident (ConfigurationPropertyPresentations, Reseed, 8)

DBCC CHECKIDENT ([Types], RESEED, 62)
