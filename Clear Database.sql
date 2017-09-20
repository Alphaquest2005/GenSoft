delete  from EntityRelationships
delete from EntityTypeViewModelAttributes
delete from Attributes
delete from CalculatedProperties
delete from EntityTypeAttributes
delete from EntityType
delete from Type 
where id not in(select id from DataType)

DBCC CHECKIDENT (EntityTypeViewModel, RESEED, 0)
DBCC CHECKIDENT (ProcessStateDomainEntityTypes, RESEED, 0)
DBCC CHECKIDENT (EntityRelationships, RESEED, 0)
DBCC CHECKIDENT (EntityTypeViewModelAttributes, RESEED, 0)
DBCC CHECKIDENT (Attributes, RESEED, 0)

DBCC CHECKIDENT (EntityTypeAttributes, RESEED, 0)

DBCC CHECKIDENT ([Type], RESEED, 62)
