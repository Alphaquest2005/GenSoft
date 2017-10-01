delete  from EntityRelationship
delete from Attributes
delete from CalculatedProperties
delete from EntityTypeAttributes
delete from EntityType
delete from Type 
where id not in(select id from DataType)


DBCC CHECKIDENT (ProcessStepRelationship, RESEED, 0)
DBCC CHECKIDENT (EntityRelationship, RESEED, 0)
DBCC CHECKIDENT (Attributes, RESEED, 0)

DBCC CHECKIDENT (EntityTypeAttributes, RESEED, 0)

DBCC CHECKIDENT ([Type], RESEED, 62)
