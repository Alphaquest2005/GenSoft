delete  from EntityRelationship
delete from CalculatedProperties
delete from Type 
where id > 62


DBCC CHECKIDENT (ProcessStepRelationship, RESEED, 0)
DBCC CHECKIDENT (EntityRelationship, RESEED, 0)
--DBCC CHECKIDENT (Attributes, RESEED, 3)

DBCC CHECKIDENT (Entity, RESEED, 1)
DBCC CHECKIDENT (EntityAttribute, RESEED, 2)
DBCC CHECKIDENT (EntityTypeAttributes, RESEED, 2)

DBCC CHECKIDENT ([Type], RESEED, 62)
