


--DBCC CHECKIDENT ([GenSoft-Creator.dbo.ProcessStepRelationshipPath], RESEED, 0)
--DBCC CHECKIDENT ([GenSoft-Creator.dbo.EntityRelationships], RESEED, 0)
--DBCC CHECKIDENT ([GenSoft-Creator.dbo.Attributes], RESEED, 0)

--DBCC CHECKIDENT ([GenSoft-Creator.dbo.EntityTypeAttributes], RESEED, 0)

--DBCC CHECKIDENT ([GenSoft-Creator.dbo.Type], RESEED, 62)


declare @AppId int, @appName varchar(50)

set @appName = (SELECT DB_NAME())
select @appName
set @AppId = (SELECT        [GenSoft-Creator].dbo.Application.Id
				FROM            [GenSoft-Creator].dbo.Application INNER JOIN
											[GenSoft-Creator].dbo.DatabaseInfo ON Application.Id = DatabaseInfo.Id
				WHERE        (DatabaseInfo.DBName = @appName))

if(@AppId is null)
begin
	
	set @AppId = (SELECT        [GenSoft-Creator].dbo.Application.Id
					FROM            [GenSoft-Creator].dbo.Application 
					WHERE        ([GenSoft-Creator].dbo.Application.Name =  @appName))
end




select @AppId



if(@AppId is null)
begin
	 print 'Manually Add this Database to GenCreator or Wrong Database'
	 return
end

----delete  from [GenSoft-Creator].dbo.EntityRelationships 
----delete from [GenSoft-Creator].dbo.Attributes
----delete from [GenSoft-Creator].dbo.CalculatedProperties
----delete from [GenSoft-Creator].dbo.EntityTypeAttributes
--delete from [GenSoft-Creator].dbo.EntityType
--delete from [GenSoft-Creator].dbo.Type 
--where id not in(select id from [GenSoft-Creator].dbo.DataType)



declare @attributeId int, @datatypeId int, @Counter int = 0

			set @attributeId = (select id from [GenSoft-Creator].dbo.[Attributes] where Name = 'Id') 

		if(@attributeId is null)
		begin
			set @datatypeId = (select id from [GenSoft-Creator].dbo.[Type] where [Name] = 'int32')
			if(@datatypeId is not null)
			begin
				
					insert into [GenSoft-Creator].dbo.Attributes(DataTypeId, [Name]) values (@datatypeId, 'Id')
					set @attributeId = (select id from [GenSoft-Creator].dbo.Attributes where [name] = 'Id')
				
			end
			
		end

	--delete from [GenSoft-Creator].dbo.[Type] where id in (SELECT        Type.Id
	--							FROM            [GenSoft-Creator].dbo.Type INNER JOIN
	--													 [GenSoft-Creator].dbo.DBType ON Type.Id = DBType.Id)

	DECLARE @entity Table (Id int identity,TableName varchar(50), EntitySetName varchar(50), [Schema] varchar(50))

	insert into @entity(TableName, EntitySetName, [Schema])
		SELECT    TABLE_NAME, TABLE_NAME AS EntitySetName, TABLE_SCHEMA as [Schema] --top 60
		FROM            INFORMATION_SCHEMA.TABLES
		WHERE        (TABLE_Type = N'BASE TABLE' and TABLE_NAME not in (/*'ApplicationSettings',*/'sysdiagrams', '__EFMigrationsHistory'))
		order by TABLE_NAME

	DELETE FROM [GenSoft-Creator].dbo.DBType
		WHERE        (Id IN
									 (SELECT        Type.Id
		FROM            [GenSoft-Creator].dbo.Type INNER JOIN
								 [GenSoft-Creator].dbo.EntityType ON Type.Id = EntityType.Id INNER JOIN
								 [GenSoft-Creator].dbo.DBType AS DBType_1 ON EntityType.Id = DBType_1.Id
		WHERE        (EntityType.ApplicationId = @AppId) AND (NOT (Type.Name IN (select TableName from @entity)))))

			DELETE FROM [GenSoft-Creator].dbo.ParentEntityType
		WHERE        (Id IN
									 (SELECT        Type.Id
		FROM            [GenSoft-Creator].dbo.Type INNER JOIN
								 [GenSoft-Creator].dbo.EntityType ON Type.Id = EntityType.Id 
		WHERE        (EntityType.ApplicationId = @AppId) AND (NOT (Type.Name IN (select TableName from @entity)))))

	WHILE (1=1)
	BEGIN 
		set @Counter +=1; 
		
		--Do something with Id here
		declare @entityId int, @rEntityId int, @EntitySet varchar(50), @TableName varchar(50), @schema varchar(50)
		set @TableName = null; --initialize with null

		select @TableName = TableName, @EntitySet = EntitySetName,@schema = [Schema] 
		from @entity where Id = @Counter

		if @TableName is null
	    break;
		
		set @entityId = null

		set @entityId = (SELECT        EntityType.Id
							FROM            [GenSoft-Creator].dbo.EntityType INNER JOIN
													 [GenSoft-Creator].dbo.Type ON EntityType.Id = Type.Id
							WHERE        (EntityType.ApplicationId = @AppId) AND (Type.Name = @TableName))

		if (@entityId is null)
		begin
			
			Declare @typeIdTable Table(Id int)

			Insert into [GenSoft-Creator].dbo.[Type] ([Name])
			OutPut INSERTED.Id into @typeIdTable 
			 values (@TableName)		
			
			select @entityId = id from @typeIdTable	

			--Insert into [GenSoft-Creator].dbo.[Type] ([Name]) values ('r'+@TableName)
			--set @rEntityId = null
			--set @rEntityId = (select id from [GenSoft-Creator].dbo.[Type] where [Name] = 'r'+@TableName)

			


			insert into [GenSoft-Creator].dbo.EntityType(ID, ApplicationId, EntitySet) values (@entityId, @AppId, @EntitySet)
			

			--insert into [GenSoft-Creator].dbo.EntityType(ID, EntitySetName) values (@rEntityId, 'r'+@EntitySet)
			--insert into [GenSoft-Creator].dbo.DomainEntityType(ID) values (@rEntityId)

			insert into [GenSoft-Creator].dbo.DBType (ID,[Table], [Schema]) values (@EntityId,@TableName, @schema)
			insert into [GenSoft-Creator].dbo.EntityId(ID,IsEntityId) values (@attributeId,1)

			insert into [GenSoft-Creator].dbo.EntityTypeAttributes(EntityTypeId,AttributeId) values (@entityId, @attributeId)
		end
		
		


		
		INSERT INTO [GenSoft-Creator].dbo.Attributes
								 (Name, DataTypeId)
		SELECT DISTINCT INFORMATION_SCHEMA.COLUMNS.COLUMN_NAME, MIN(DataType.Id) AS DataTypeId
		FROM            INFORMATION_SCHEMA.COLUMNS INNER JOIN
								 AmoebaDB.dbo.DataTypes ON INFORMATION_SCHEMA.COLUMNS.DATA_TYPE = AmoebaDB.dbo.DataTypes.DBType INNER JOIN
								 [GenSoft-Creator].dbo.DataType INNER JOIN
								 [GenSoft-Creator].dbo.Type ON DataType.Id = Type.Id ON Type.Name LIKE '%' + AmoebaDB.dbo.DataTypes.Name + '%' LEFT OUTER JOIN
								 [GenSoft-Creator].dbo.Attributes AS Attributes_1 ON INFORMATION_SCHEMA.COLUMNS.COLUMN_NAME = Attributes_1.Name
		WHERE        (INFORMATION_SCHEMA.COLUMNS.TABLE_NAME = @TableName) AND (Attributes_1.Name IS NULL)
		GROUP BY INFORMATION_SCHEMA.COLUMNS.COLUMN_NAME

				DELETE FROM [GenSoft-Creator].dbo.EntityRelationship
				FROM            [GenSoft-Creator].dbo.EntityType INNER JOIN
										 [GenSoft-Creator].dbo.EntityTypeAttributes ON EntityType.Id = EntityTypeAttributes.EntityTypeId INNER JOIN
										 [GenSoft-Creator].dbo.EntityRelationship ON EntityTypeAttributes.Id = EntityRelationship.ChildEntityId
				WHERE        (EntityType.Id = @entityId)

		delete from [GenSoft-Creator].dbo.EntityTypeAttributes where AttributeId <> @attributeId and EntityTypeId = @entityId

		INSERT INTO [GenSoft-Creator].dbo.EntityTypeAttributes
								 (AttributeId, EntityTypeId)
		SELECT   distinct     Attributes_1.Id, @entityId
		FROM            INFORMATION_SCHEMA.COLUMNS INNER JOIN
								 [GenSoft-Creator].dbo.Attributes AS Attributes_1 ON INFORMATION_SCHEMA.COLUMNS.COLUMN_NAME = Attributes_1.Name
		WHERE        (INFORMATION_SCHEMA.COLUMNS.TABLE_NAME = @TableName and Attributes_1.Id <> @attributeId)

			
		--insert into ProcessStateDomainEntityTypes(ProcessStateId, DomainEntityTypeId) Values (6, @entityId)
		
		--declare @processDomainEntityId int
		--set @processDomainEntityId = (select Id from ProcessStateDomainEntityTypes where ProcessStateId = 6 and DomainEntityTypeId = @entityId)

	    declare @nameCount int = 0

		set @nameCount = (SELECT        COUNT(Attributes.Name) AS Expr1
						FROM            [GenSoft-Creator].dbo.EntityTypeAttributes INNER JOIN
												 [GenSoft-Creator].dbo.Attributes ON EntityTypeAttributes.AttributeId = Attributes.Id INNER JOIN
												 [GenSoft-Creator].dbo.EntityType ON EntityTypeAttributes.EntityTypeId = EntityType.Id INNER JOIN
												 [GenSoft-Creator].dbo.Type ON EntityType.Id = Type.Id
						where (Attributes.Name like '%Name%')
						GROUP BY EntityTypeAttributes.EntityTypeId, Type.Name
						HAVING        (EntityTypeAttributes.EntityTypeId = @entityId))

		--declare @entityViewModelId int
		--if(@nameCount > 0)
		--begin
			
		--	insert into EntityTypeViewModel(ProcessDomainEntityTypeId, [Priority], Symbol, [Description], ViewModelTypeId ) Values (@processDomainEntityId,0, Left(@TableName, 1), @TableName, 3)
		--	insert into EntityTypeViewModel(ProcessDomainEntityTypeId, [Priority], Symbol, [Description], ViewModelTypeId ) Values (@processDomainEntityId,0, Left(@TableName, 1), @TableName, 4)
							
			

		--	set @entityViewModelId = (select id from EntityTypeViewModel where ProcessDomainEntityTypeId = @processDomainEntityId and ViewModelTypeId = 3)

			
		--end
		--else
		--begin
		--	insert into EntityTypeViewModel(ProcessDomainEntityTypeId, [Priority], Symbol, [Description], ViewModelTypeId ) Values (@processDomainEntityId,0, Left(@TableName, 1), @TableName, 2)
		--	set @entityViewModelId = (select id from EntityTypeViewModel where ProcessDomainEntityTypeId = @processDomainEntityId and ViewModelTypeId = 2)
		--end

			insert into [GenSoft-Creator].dbo.EntityTypeViewModelCommand(EntityTypeId,ViewModelCommandId) Values(@entityId, 2)
			insert into [GenSoft-Creator].dbo.EntityTypeViewModelCommand(EntityTypeId,ViewModelCommandId) Values(@entityId, 3)
		
	END
	
	drop table #Relationships

select ROW_NUMBER() OVER (ORDER BY ParentAttributeId) as Id, ParentAttributeId, ChildAttributeId, RelationshipTypeId
into #Relationships
from
(
SELECT DISTINCT ParentEntityTypeAttributes.Id AS ParentAttributeId, ChildEntityTypeAttributes.Id AS ChildAttributeId, RelationshipTypes.Id AS RelationshipTypeId
FROM            [GenSoft-Creator].dbo.Attributes AS ChildAttributes INNER JOIN
                         [GenSoft-Creator].dbo.EntityTypeAttributes AS ChildEntityTypeAttributes ON ChildAttributes.Id = ChildEntityTypeAttributes.AttributeId INNER JOIN
                         [GenSoft-Creator].dbo.Type AS ChildType INNER JOIN
                             (SELECT        KEY_COLUMN_USAGE_1.TABLE_NAME AS ParentTable, KEY_COLUMN_USAGE_1.COLUMN_NAME AS ParentColumn, INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE.TABLE_NAME AS ChildTable, 
                                                         INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE.COLUMN_NAME AS ChildColumn
                               FROM            INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE INNER JOIN
                                                         INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS ON 
                                                         INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE.CONSTRAINT_NAME = INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS.CONSTRAINT_NAME INNER JOIN
                                                         INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KEY_COLUMN_USAGE_1 ON 
                                                         INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS.UNIQUE_CONSTRAINT_NAME = KEY_COLUMN_USAGE_1.CONSTRAINT_NAME INNER JOIN
                                                         INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ON 
                                                         INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE.CONSTRAINT_NAME = INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE.CONSTRAINT_NAME) AS RelView ON 
                         ChildType.Name = RelView.ChildTable INNER JOIN
                         [GenSoft-Creator].dbo.EntityType AS ChildEntityType ON ChildType.Id = ChildEntityType.Id ON ChildEntityTypeAttributes.EntityTypeId = ChildEntityType.Id AND ChildAttributes.Name = RelView.ChildColumn LEFT OUTER JOIN
                         [GenSoft-Creator].dbo.EntityTypeAttributes AS ParentEntityTypeAttributes INNER JOIN
                         [GenSoft-Creator].dbo.EntityType AS ParentEntityType ON ParentEntityTypeAttributes.EntityTypeId = ParentEntityType.Id INNER JOIN
                         [GenSoft-Creator].dbo.Type AS ParentType ON ParentEntityType.Id = ParentType.Id INNER JOIN
                         [GenSoft-Creator].dbo.Attributes AS ParentAttributes ON ParentEntityTypeAttributes.AttributeId = ParentAttributes.Id ON RelView.ParentTable = ParentType.Name AND RelView.ParentColumn = ParentAttributes.Name LEFT OUTER JOIN
                             (SELECT        MaxLength, TABLE_NAME, COLUMN_NAME, isIdentity, isPrimaryKey
                               FROM            (SELECT        ISNULL(INFORMATION_SCHEMA.COLUMNS.CHARACTER_MAXIMUM_LENGTH, 0) AS MaxLength, INFORMATION_SCHEMA.COLUMNS.TABLE_NAME, INFORMATION_SCHEMA.COLUMNS.COLUMN_NAME, 
																		 ISNULL(PrimaryKeys.is_identity, 0) AS isIdentity, ISNULL(PrimaryKeys.is_primary_key, 0) AS isPrimaryKey
												FROM            INFORMATION_SCHEMA.KEY_COLUMN_USAGE INNER JOIN
																		 INFORMATION_SCHEMA.COLUMNS ON INFORMATION_SCHEMA.KEY_COLUMN_USAGE.COLUMN_NAME = INFORMATION_SCHEMA.COLUMNS.COLUMN_NAME AND 
																		 INFORMATION_SCHEMA.KEY_COLUMN_USAGE.TABLE_NAME = INFORMATION_SCHEMA.COLUMNS.TABLE_NAME LEFT OUTER JOIN
																			 (SELECT        OBJECT_NAME(i.object_id) AS tableName, i.name AS indexName, c.name AS columnName, c.is_identity, idc.seed_value, idc.increment_value, idc.last_value, idc.is_computed, i.is_primary_key, 
																										 i.is_unique_constraint
																			   FROM            sys.indexes AS i LEFT OUTER JOIN
																										 sys.index_columns AS ic ON ic.object_id = i.object_id AND ic.index_id = i.index_id LEFT OUTER JOIN
																										 sys.columns AS c ON c.object_id = ic.object_id AND c.column_id = ic.column_id LEFT OUTER JOIN
																										 sys.identity_columns AS idc ON idc.object_id = c.object_id AND idc.column_id = c.column_id) AS PrimaryKeys ON INFORMATION_SCHEMA.COLUMNS.TABLE_NAME = PrimaryKeys.tableName AND 
																		 INFORMATION_SCHEMA.COLUMNS.COLUMN_NAME = PrimaryKeys.columnName) as t) AS ChildKeyData ON ChildAttributes.Name = ChildKeyData.COLUMN_NAME AND ChildType.Name = ChildKeyData.TABLE_NAME LEFT OUTER JOIN
                             (SELECT        MaxLength, TABLE_NAME, COLUMN_NAME, isIdentity, isPrimaryKey
                               FROM            (SELECT        ISNULL(INFORMATION_SCHEMA.COLUMNS.CHARACTER_MAXIMUM_LENGTH, 0) AS MaxLength, INFORMATION_SCHEMA.COLUMNS.TABLE_NAME, INFORMATION_SCHEMA.COLUMNS.COLUMN_NAME, 
																		 ISNULL(PrimaryKeys.is_identity, 0) AS isIdentity, ISNULL(PrimaryKeys.is_primary_key, 0) AS isPrimaryKey
												FROM            INFORMATION_SCHEMA.KEY_COLUMN_USAGE INNER JOIN
																		 INFORMATION_SCHEMA.COLUMNS ON INFORMATION_SCHEMA.KEY_COLUMN_USAGE.COLUMN_NAME = INFORMATION_SCHEMA.COLUMNS.COLUMN_NAME AND 
																		 INFORMATION_SCHEMA.KEY_COLUMN_USAGE.TABLE_NAME = INFORMATION_SCHEMA.COLUMNS.TABLE_NAME LEFT OUTER JOIN
																			 (SELECT        OBJECT_NAME(i.object_id) AS tableName, i.name AS indexName, c.name AS columnName, c.is_identity, idc.seed_value, idc.increment_value, idc.last_value, idc.is_computed, i.is_primary_key, 
																										 i.is_unique_constraint
																			   FROM            sys.indexes AS i LEFT OUTER JOIN
																										 sys.index_columns AS ic ON ic.object_id = i.object_id AND ic.index_id = i.index_id LEFT OUTER JOIN
																										 sys.columns AS c ON c.object_id = ic.object_id AND c.column_id = ic.column_id LEFT OUTER JOIN
																										 sys.identity_columns AS idc ON idc.object_id = c.object_id AND idc.column_id = c.column_id) AS PrimaryKeys ON INFORMATION_SCHEMA.COLUMNS.TABLE_NAME = PrimaryKeys.tableName AND 
																		 INFORMATION_SCHEMA.COLUMNS.COLUMN_NAME = PrimaryKeys.columnName) as t) AS ParentKeyData ON ParentType.Name = ParentKeyData.TABLE_NAME AND 
                         ParentAttributes.Name = ParentKeyData.COLUMN_NAME LEFT OUTER JOIN
                         [GenSoft-Creator].dbo.RelationshipType as RelationshipTypes ON CASE WHEN ParentKeyData.isidentity = 1 THEN 1 ELSE 2 END = RelationshipTypes.ParentOrdinalityId AND 
                         CASE WHEN ChildKeyData.isPrimaryKey = 1 THEN 1 ELSE 2 END = RelationshipTypes.ChildOrdinalityId
						 
						 ) as relsource

select * from #Relationships




set @Counter = 0
	WHILE (1=1)
	BEGIN 
		set @Counter +=1; 

		declare @ParentEntityId int, @ChildEntityId int, @RelationshipTypeId int, @RelId int;
		set @ParentEntityId = null;
		select @ParentEntityId = ParentAttributeId,  @ChildEntityId = ChildAttributeId, @RelationshipTypeId = RelationshipTypeId
		from #Relationships where id = @Counter;

		if(@ParentEntityId is null) break;


		Declare @relIdTable Table(Id int)

			INSERT INTO [GenSoft-Creator].dbo.EntityRelationship
                         (ChildEntityId, RelationshipTypeId)
			OutPut INSERTED.Id into @relIdTable 
			values (@ChildEntityId, @RelationshipTypeId)
			
			select @RelId = id from @relIdTable	
			
			INSERT INTO [GenSoft-Creator].dbo.ParentEntity
                         (Id,ParentEntityId) Values (@RelId, @ParentEntityId)	 
   End

   drop table #ParentClass

select ROW_NUMBER() OVER (ORDER BY ParentEntityTypeId) as Id, ParentEntityTypeId, ChildEntityTypeId
into #ParentClass
from
(
SELECT DISTINCT 
                         ParentEntityTypeAttributes.Id AS ParentAttributeId, ChildEntityTypeAttributes.Id AS ChildAttributeId, RelationshipTypes.Id AS RelationshipTypeId, ParentEntityType.Id AS ParentEntityTypeId, 
                         ChildEntityType.Id AS ChildEntityTypeId
FROM            Attributes AS ChildAttributes INNER JOIN
                         EntityTypeAttributes AS ChildEntityTypeAttributes ON ChildAttributes.Id = ChildEntityTypeAttributes.AttributeId INNER JOIN
                         Type AS ChildType INNER JOIN
                             (SELECT        KEY_COLUMN_USAGE_1.TABLE_NAME AS ParentTable, KEY_COLUMN_USAGE_1.COLUMN_NAME AS ParentColumn, INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE.TABLE_NAME AS ChildTable, 
                                                         INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE.COLUMN_NAME AS ChildColumn
                               FROM            INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE INNER JOIN
                                                         INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS ON 
                                                         INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE.CONSTRAINT_NAME = INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS.CONSTRAINT_NAME INNER JOIN
                                                         INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KEY_COLUMN_USAGE_1 ON 
                                                         INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS.UNIQUE_CONSTRAINT_NAME = KEY_COLUMN_USAGE_1.CONSTRAINT_NAME INNER JOIN
                                                         INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ON 
                                                         INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE.CONSTRAINT_NAME = INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE.CONSTRAINT_NAME
                               WHERE        (KEY_COLUMN_USAGE_1.COLUMN_NAME = N'Id') AND (INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE.COLUMN_NAME = N'Id')) AS RelView ON ChildType.Name = RelView.ChildTable INNER JOIN
                         EntityType AS ChildEntityType ON ChildType.Id = ChildEntityType.Id ON ChildEntityTypeAttributes.EntityTypeId = ChildEntityType.Id AND ChildAttributes.Name = RelView.ChildColumn LEFT OUTER JOIN
                         EntityTypeAttributes AS ParentEntityTypeAttributes INNER JOIN
                         EntityType AS ParentEntityType ON ParentEntityTypeAttributes.EntityTypeId = ParentEntityType.Id INNER JOIN
                         Type AS ParentType ON ParentEntityType.Id = ParentType.Id INNER JOIN
                         Attributes AS ParentAttributes ON ParentEntityTypeAttributes.AttributeId = ParentAttributes.Id ON RelView.ParentTable = ParentType.Name AND RelView.ParentColumn = ParentAttributes.Name LEFT OUTER JOIN
                             (SELECT        MaxLength, TABLE_NAME, COLUMN_NAME, isIdentity, isPrimaryKey
                               FROM            (SELECT        ISNULL(INFORMATION_SCHEMA.COLUMNS.CHARACTER_MAXIMUM_LENGTH, 0) AS MaxLength, INFORMATION_SCHEMA.COLUMNS.TABLE_NAME, 
                                                                                   INFORMATION_SCHEMA.COLUMNS.COLUMN_NAME, ISNULL(PrimaryKeys.is_identity, 0) AS isIdentity, ISNULL(PrimaryKeys.is_primary_key, 0) AS isPrimaryKey
                                                         FROM            INFORMATION_SCHEMA.KEY_COLUMN_USAGE INNER JOIN
                                                                                   INFORMATION_SCHEMA.COLUMNS ON INFORMATION_SCHEMA.KEY_COLUMN_USAGE.COLUMN_NAME = INFORMATION_SCHEMA.COLUMNS.COLUMN_NAME AND 
                                                                                   INFORMATION_SCHEMA.KEY_COLUMN_USAGE.TABLE_NAME = INFORMATION_SCHEMA.COLUMNS.TABLE_NAME LEFT OUTER JOIN
                                                                                       (SELECT        OBJECT_NAME(i.object_id) AS tableName, i.name AS indexName, c.name AS columnName, c.is_identity, idc.seed_value, idc.increment_value, idc.last_value, idc.is_computed, 
                                                                                                                   i.is_primary_key, i.is_unique_constraint
                                                                                         FROM            sys.indexes AS i LEFT OUTER JOIN
                                                                                                                   sys.index_columns AS ic ON ic.object_id = i.object_id AND ic.index_id = i.index_id LEFT OUTER JOIN
                                                                                                                   sys.columns AS c ON c.object_id = ic.object_id AND c.column_id = ic.column_id LEFT OUTER JOIN
                                                                                                                   sys.identity_columns AS idc ON idc.object_id = c.object_id AND idc.column_id = c.column_id) AS PrimaryKeys ON 
                                                                                   INFORMATION_SCHEMA.COLUMNS.TABLE_NAME = PrimaryKeys.tableName AND INFORMATION_SCHEMA.COLUMNS.COLUMN_NAME = PrimaryKeys.columnName) AS t) AS ChildKeyData ON 
                         ChildAttributes.Name = ChildKeyData.COLUMN_NAME AND ChildType.Name = ChildKeyData.TABLE_NAME LEFT OUTER JOIN
                             (SELECT        MaxLength, TABLE_NAME, COLUMN_NAME, isIdentity, isPrimaryKey
                               FROM            (SELECT        ISNULL(COLUMNS_1.CHARACTER_MAXIMUM_LENGTH, 0) AS MaxLength, COLUMNS_1.TABLE_NAME, COLUMNS_1.COLUMN_NAME, ISNULL(PrimaryKeys_1.is_identity, 0) AS isIdentity, 
                                                                                   ISNULL(PrimaryKeys_1.is_primary_key, 0) AS isPrimaryKey
                                                         FROM            INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KEY_COLUMN_USAGE_2 INNER JOIN
                                                                                   INFORMATION_SCHEMA.COLUMNS AS COLUMNS_1 ON KEY_COLUMN_USAGE_2.COLUMN_NAME = COLUMNS_1.COLUMN_NAME AND 
                                                                                   KEY_COLUMN_USAGE_2.TABLE_NAME = COLUMNS_1.TABLE_NAME LEFT OUTER JOIN
                                                                                       (SELECT        OBJECT_NAME(i.object_id) AS tableName, i.name AS indexName, c.name AS columnName, c.is_identity, idc.seed_value, idc.increment_value, idc.last_value, idc.is_computed, 
                                                                                                                   i.is_primary_key, i.is_unique_constraint
                                                                                         FROM            sys.indexes AS i LEFT OUTER JOIN
                                                                                                                   sys.index_columns AS ic ON ic.object_id = i.object_id AND ic.index_id = i.index_id LEFT OUTER JOIN
                                                                                                                   sys.columns AS c ON c.object_id = ic.object_id AND c.column_id = ic.column_id LEFT OUTER JOIN
                                                                                                                   sys.identity_columns AS idc ON idc.object_id = c.object_id AND idc.column_id = c.column_id) AS PrimaryKeys_1 ON COLUMNS_1.TABLE_NAME = PrimaryKeys_1.tableName AND 
                                                                                   COLUMNS_1.COLUMN_NAME = PrimaryKeys_1.columnName) AS t_1) AS ParentKeyData ON ParentType.Name = ParentKeyData.TABLE_NAME AND 
                         ParentAttributes.Name = ParentKeyData.COLUMN_NAME LEFT OUTER JOIN
                         RelationshipType AS RelationshipTypes ON CASE WHEN ParentKeyData.isidentity = 1 THEN 1 ELSE 2 END = RelationshipTypes.ParentOrdinalityId AND 
                         CASE WHEN ChildKeyData.isPrimaryKey = 1 THEN 1 ELSE 2 END = RelationshipTypes.ChildOrdinalityId) as relsource

select * from #ParentClass




set @Counter = 0
	WHILE (1=1)
	BEGIN 
		set @Counter +=1; 

		declare @ParentEntityTypeId int, @ChildEntityTypeId int;
		set @ParentEntityTypeId = null;
		select @ParentEntityTypeId = ParentEntityTypeId,  @ChildEntityTypeId = ChildEntityTypeId
		from #ParentClass where id = @Counter;

		if(@ParentEntityTypeId is null) break;

			
			INSERT INTO [GenSoft-Creator].dbo.ParentEntityType
                         (Id,ParentEntityTypeId) Values (@ChildEntityTypeId, @ParentEntityTypeId)	 
   End
