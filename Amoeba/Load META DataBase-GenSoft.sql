


--DBCC CHECKIDENT ([GenSoft-Creator.dbo.ProcesStepRelationshipPath], RESEED, 0)
--DBCC CHECKIDENT ([GenSoft-Creator.dbo.EntityRelationships], RESEED, 0)
--DBCC CHECKIDENT ([GenSoft-Creator.dbo.Attributes], RESEED, 0)

--DBCC CHECKIDENT ([GenSoft-Creator.dbo.EntityTypeAttributes], RESEED, 0)

--DBCC CHECKIDENT ([GenSoft-Creator.dbo.Types], RESEED, 62)


declare @AppId int, @appName varchar(50)

set @appName = (SELECT DB_NAME())
select @appName
set @AppId = (SELECT        [GenSoft-Creator].dbo.Applications.Id
				FROM            [GenSoft-Creator].dbo.Applications INNER JOIN
											[GenSoft-Creator].dbo.DatabaseInfos ON Applications.Id = DatabaseInfos.Id
				WHERE        (DatabaseInfos.DBName = @appName))

if(@AppId is null)
begin
	
	set @AppId = (SELECT        [GenSoft-Creator].dbo.Applications.Id
					FROM            [GenSoft-Creator].dbo.Applications 
					WHERE        ([GenSoft-Creator].dbo.Applications.Name =  @appName))
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
--delete from [GenSoft-Creator].dbo.EntityTypes
--delete from [GenSoft-Creator].dbo.Types 
--where id not in(select id from [GenSoft-Creator].dbo.DataTypes)



declare @attributeId int, @dataTypeId int, @Counter int = 0

			set @attributeId = (select id from [GenSoft-Creator].dbo.[Attributes] where Name = 'Id') 

		if(@attributeId is null)
		begin
			set @dataTypeId = (select id from [GenSoft-Creator].dbo.[Types] where [Name] = 'int32')
			if(@dataTypeId is not null)
			begin
				
					insert into [GenSoft-Creator].dbo.Attributes(DataTypeId, [Name]) values (@dataTypeId, 'Id')
					set @attributeId = (select id from [GenSoft-Creator].dbo.Attributes where [name] = 'Id')
				
			end
			
		end

	--delete from [GenSoft-Creator].dbo.[Types] where id in (SELECT        Types.Id
	--							FROM            [GenSoft-Creator].dbo.Types INNER JOIN
	--													 [GenSoft-Creator].dbo.DBTypes ON Types.Id = DBTypes.Id)

	DECLARE @entity Table (Id int identity,TableName varchar(50), EntitySetName varchar(50), [Schema] varchar(50))

	insert into @entity(TableName, EntitySetName, [Schema])
		SELECT    AmoebaDB.dbo.Singularize(TABLE_NAME), TABLE_NAME AS EntitySetName, TABLE_SCHEMA as [Schema] --top 60
		FROM            INFORMATION_SCHEMA.TABLES
		WHERE        (TABLE_Type = N'BASE TABLE' and TABLE_NAME not in (/*'ApplicationSettings',*/'sysdiagrams', '__EFMigrationsHistory'))
		order by TABLE_NAME

	DELETE FROM [GenSoft-Creator].dbo.DBTypes
		WHERE        (Id IN
									 (SELECT        Types.Id
		FROM            [GenSoft-Creator].dbo.Types INNER JOIN
								 [GenSoft-Creator].dbo.EntityTypes ON Types.Id = EntityTypes.TypeId INNER JOIN
								 [GenSoft-Creator].dbo.DBTypes AS DBTypes_1 ON EntityTypes.Id = DBTypes_1.Id
		WHERE        (EntityTypes.ApplicationId = @AppId) AND (NOT (Types.Name IN (select TableName from @entity)))))

			DELETE FROM [GenSoft-Creator].dbo.ParentEntityTypes
		WHERE        (Id IN
									 (SELECT        Types.Id
		FROM            [GenSoft-Creator].dbo.Types INNER JOIN
								 [GenSoft-Creator].dbo.EntityTypes ON Types.Id = EntityTypes.TypeId 
		WHERE        (EntityTypes.ApplicationId = @AppId) AND (NOT (Types.Name IN (select TableName from @entity)))))

	WHILE (1=1)
	BEGIN 
		set @Counter +=1; 
		
		--Do something with Id here
		declare @EntityIds int, @TypeId int, @rEntityIds int,  @TableName varchar(50), @schema varchar(50)
		set @TableName = null; --initialize with null

		select @TableName = EntitySetName,@schema = [Schema] 
		from @entity where Id = @Counter

		if @TableName is null
	    break;
		
		set @EntityIds = null

		set @EntityIds = (SELECT        EntityTypes.Id
							FROM            [GenSoft-Creator].dbo.EntityTypes INNER JOIN
													 [GenSoft-Creator].dbo.Types ON EntityTypes.TypeId = Types.Id
							WHERE        (EntityTypes.ApplicationId = @AppId) AND (Types.Name = @TableName))

		if (@EntityIds is null)
		begin
			
			Declare @TypeIdTable Table(Id int)

			Insert into [GenSoft-Creator].dbo.[Types] ([Name])
			OutPut INSERTED.Id into @TypeIdTable 
			 values (@TableName)		
			
			select @TypeId = id from @TypeIdTable	

			
			Declare @entityTypeIdTable Table(Id int)

			Insert into [GenSoft-Creator].dbo.EntityTypes(TypeId, ApplicationId, EntitySet)
			OutPut INSERTED.Id into @entityTypeIdTable 
			values (@TypeId, @AppId, @TableName)	
			
			select @EntityIds = id from @entityTypeIdTable	


			insert into [GenSoft-Creator].dbo.DBTypes (ID,[Table], [Schema]) values (@EntityIds,@TableName, @schema)
			--insert into [GenSoft-Creator].dbo.EntityIds(ID,IsEntityIds) values (@attributeId,1)

			insert into [GenSoft-Creator].dbo.EntityTypeAttributes(EntityTypeId,AttributeId) values (@EntityIds, @attributeId)
		end
		
		


		
		INSERT INTO [GenSoft-Creator].dbo.Attributes
								 (Name, DataTypeId)
		SELECT DISTINCT INFORMATION_SCHEMA.COLUMNS.COLUMN_NAME, MIN(DataTypes.Id) AS DataTypeId
		FROM            INFORMATION_SCHEMA.COLUMNS INNER JOIN
								 AmoebaDB.dbo.DataTypes ON INFORMATION_SCHEMA.COLUMNS.DATA_Type = AmoebaDB.dbo.DataTypes.DBType INNER JOIN
								 [GenSoft-Creator].dbo.DataTypes as Dt1 INNER JOIN
								 [GenSoft-Creator].dbo.Types ON Dt1.Id = Types.Id ON Types.Name LIKE '%' + AmoebaDB.dbo.DataTypes.Name + '%' LEFT OUTER JOIN
								 [GenSoft-Creator].dbo.Attributes AS Attributes_1 ON INFORMATION_SCHEMA.COLUMNS.COLUMN_NAME = Attributes_1.Name
		WHERE        (INFORMATION_SCHEMA.COLUMNS.TABLE_NAME = @TableName) AND (Attributes_1.Name IS NULL)
		GROUP BY INFORMATION_SCHEMA.COLUMNS.COLUMN_NAME

				DELETE FROM [GenSoft-Creator].dbo.EntityRelationships
				FROM            [GenSoft-Creator].dbo.EntityTypes INNER JOIN
										 [GenSoft-Creator].dbo.EntityTypeAttributes ON EntityTypes.Id = EntityTypeAttributes.EntityTypeId INNER JOIN
										 [GenSoft-Creator].dbo.EntityRelationships ON EntityTypeAttributes.Id = EntityRelationships.ChildEntityId
				WHERE        (EntityTypes.Id = @EntityIds)

				DELETE FROM [GenSoft-Creator].dbo.EntityRelationships
				FROM            [GenSoft-Creator].dbo.EntityTypes INNER JOIN
										 [GenSoft-Creator].dbo.EntityTypeAttributes ON EntityTypes.Id = EntityTypeAttributes.EntityTypeId INNER JOIN
										 [GenSoft-Creator].dbo.ParentEntities ON EntityTypeAttributes.Id = ParentEntities.ParentEntityId INNER JOIN
										 [GenSoft-Creator].dbo.EntityRelationships ON ParentEntities.Id = EntityRelationships.Id
				WHERE        (EntityTypes.Id = @EntityIds)

		delete from [GenSoft-Creator].dbo.EntityTypeAttributes where /*AttributeId <> @attributeId and */EntityTypeId = @EntityIds

		INSERT INTO [GenSoft-Creator].dbo.EntityTypeAttributes
								 (AttributeId, EntityTypeId)
		SELECT   distinct     Attributes_1.Id, @EntityIds
		FROM            INFORMATION_SCHEMA.COLUMNS INNER JOIN
								 [GenSoft-Creator].dbo.Attributes AS Attributes_1 ON INFORMATION_SCHEMA.COLUMNS.COLUMN_NAME = Attributes_1.Name
		WHERE        (INFORMATION_SCHEMA.COLUMNS.TABLE_NAME = @TableName/* and Attributes_1.Id <> @attributeId*/)

		
	    declare @nameCount int = 0

		set @nameCount = (SELECT        COUNT(Attributes.Name) AS Expr1
						FROM            [GenSoft-Creator].dbo.EntityTypeAttributes INNER JOIN
												 [GenSoft-Creator].dbo.Attributes ON EntityTypeAttributes.AttributeId = Attributes.Id INNER JOIN
												 [GenSoft-Creator].dbo.EntityTypes ON EntityTypeAttributes.EntityTypeId = EntityTypes.Id INNER JOIN
												 [GenSoft-Creator].dbo.Types ON EntityTypes.Id = Types.Id
						where (Attributes.Name like '%Name%')
						GROUP BY EntityTypeAttributes.EntityTypeId, Types.Name
						HAVING        (EntityTypeAttributes.EntityTypeId = @EntityIds))

			delete from [GenSoft-Creator].dbo.EntityTypeViewModelCommands where EntityTypeId = @EntityIds
			insert into [GenSoft-Creator].dbo.EntityTypeViewModelCommands(EntityTypeId,ViewModelCommandId) Values(@EntityIds, 2)
			insert into [GenSoft-Creator].dbo.EntityTypeViewModelCommands(EntityTypeId,ViewModelCommandId) Values(@EntityIds, 3)
		
	END
	
	drop table #Relationships

select ROW_NUMBER() OVER (ORDER BY ParentAttributeId) as Id, ParentAttributeId, ChildAttributeId, RelationshipTypeId
into #Relationships
from
(
SELECT DISTINCT ParentEntityTypeAttributes.Id AS ParentAttributeId, ChildEntityTypeAttributes.Id AS ChildAttributeId, RelationshipTypes.Id AS RelationshipTypeId
FROM            [GenSoft-Creator].dbo.Types AS ChildTypes INNER JOIN
                             (SELECT        KEY_COLUMN_USAGE_1.TABLE_NAME AS ParentTable, KEY_COLUMN_USAGE_1.COLUMN_NAME AS ParentColumn, INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE.TABLE_NAME AS ChildTable, 
                                                         INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE.COLUMN_NAME AS ChildColumn
                               FROM            INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE INNER JOIN
                                                         INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS ON 
                                                         INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE.CONSTRAINT_NAME = INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS.CONSTRAINT_NAME INNER JOIN
                                                         INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KEY_COLUMN_USAGE_1 ON 
                                                         INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS.UNIQUE_CONSTRAINT_NAME = KEY_COLUMN_USAGE_1.CONSTRAINT_NAME INNER JOIN
                                                         INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ON 
                                                         INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE.CONSTRAINT_NAME = INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE.CONSTRAINT_NAME) AS RelView ON 
                         ChildTypes.Name = RelView.ChildTable INNER JOIN
                         [GenSoft-Creator].dbo.Attributes AS ChildAttributes INNER JOIN
                         [GenSoft-Creator].dbo.EntityTypeAttributes AS ChildEntityTypeAttributes ON ChildAttributes.Id = ChildEntityTypeAttributes.AttributeId INNER JOIN
                         [GenSoft-Creator].dbo.EntityTypes AS ChildEntityTypes ON ChildEntityTypeAttributes.EntityTypeId = ChildEntityTypes.Id ON RelView.ChildColumn = ChildAttributes.Name AND ChildTypes.Id = ChildEntityTypes.TypeId LEFT OUTER JOIN
                         [GenSoft-Creator].dbo.EntityTypeAttributes AS ParentEntityTypeAttributes INNER JOIN
                         [GenSoft-Creator].dbo.EntityTypes AS ParentEntityTypes ON ParentEntityTypeAttributes.EntityTypeId = ParentEntityTypes.Id INNER JOIN
                         [GenSoft-Creator].dbo.Attributes AS ParentAttributes ON ParentEntityTypeAttributes.AttributeId = ParentAttributes.Id INNER JOIN
                         [GenSoft-Creator].dbo.Types AS ParentTypes ON ParentEntityTypes.TypeId = ParentTypes.Id ON RelView.ParentTable = ParentTypes.Name AND RelView.ParentColumn = ParentAttributes.Name LEFT OUTER JOIN
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
                         ChildAttributes.Name = ChildKeyData.COLUMN_NAME AND ChildTypes.Name = ChildKeyData.TABLE_NAME LEFT OUTER JOIN
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
                                                                                   COLUMNS_1.COLUMN_NAME = PrimaryKeys_1.columnName) AS t_1) AS ParentKeyData ON ParentTypes.Name = ParentKeyData.TABLE_NAME AND 
                         ParentAttributes.Name = ParentKeyData.COLUMN_NAME LEFT OUTER JOIN
                         [GenSoft-Creator].dbo.RelationshipTypes AS RelationshipTypes ON CASE WHEN ParentKeyData.isPrimaryKey = 1 THEN 1 ELSE 2 END = RelationshipTypes.ParentOrdinalityId AND 
                         CASE WHEN ChildKeyData.isPrimaryKey = 1 THEN 1 ELSE 2 END = RelationshipTypes.ChildOrdinalityId
WHERE        (ChildEntityTypes.ApplicationId = @AppId) AND (ParentEntityTypes.ApplicationId = @AppId)) as relsource

select * from #Relationships




set @Counter = 0
	WHILE (1=1)
	BEGIN 
		set @Counter +=1; 

		declare @ParentEntityIds int, @ChildEntityIds int, @RelationshipTypeId int, @RelId int;
		set @ParentEntityIds = null;
		select @ParentEntityIds = ParentAttributeId,  @ChildEntityIds = ChildAttributeId, @RelationshipTypeId = RelationshipTypeId
		from #Relationships where id = @Counter;

		if(@ParentEntityIds is null) break;


		Declare @relIdTable Table(Id int)

			INSERT INTO [GenSoft-Creator].dbo.EntityRelationships
                         (ChildEntityId, RelationshipTypeId)
			OutPut INSERTED.Id into @relIdTable 
			values (@ChildEntityIds, @RelationshipTypeId)
			
			select @RelId = id from @relIdTable	
			
			INSERT INTO [GenSoft-Creator].dbo.ParentEntities
                         (Id,ParentEntityId) Values (@RelId, @ParentEntityIds)	 
   End

   drop table #ParentClas

select ROW_NUMBER() OVER (ORDER BY ParentEntityTypeId) as Id, ParentEntityTypeId, ChildEntityTypeId
into #ParentClas
from
(
SELECT DISTINCT 
                         ParentEntityTypeAttributes.Id AS ParentAttributeId, ChildEntityTypeAttributes.Id AS ChildAttributeId, RelationshipTypes.Id AS RelationshipTypeId, ParentEntityTypes.Id AS ParentEntityTypeId, 
                         ChildEntityTypes.Id AS ChildEntityTypeId
FROM            [GenSoft-Creator].dbo.Types AS ChildTypes INNER JOIN
                             (SELECT        KEY_COLUMN_USAGE_1.TABLE_NAME AS ParentTable, KEY_COLUMN_USAGE_1.COLUMN_NAME AS ParentColumn, INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE.TABLE_NAME AS ChildTable, 
                                                         INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE.COLUMN_NAME AS ChildColumn
                               FROM            INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE INNER JOIN
                                                         INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS ON 
                                                         INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE.CONSTRAINT_NAME = INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS.CONSTRAINT_NAME INNER JOIN
                                                         INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KEY_COLUMN_USAGE_1 ON 
                                                         INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS.UNIQUE_CONSTRAINT_NAME = KEY_COLUMN_USAGE_1.CONSTRAINT_NAME INNER JOIN
                                                         INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE ON 
                                                         INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE.CONSTRAINT_NAME = INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE.CONSTRAINT_NAME
                               WHERE        (KEY_COLUMN_USAGE_1.COLUMN_NAME = N'Id') AND (INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE.COLUMN_NAME = N'Id')) AS RelView ON ChildTypes.Name = RelView.ChildTable INNER JOIN
                         [GenSoft-Creator].dbo.Attributes AS ChildAttributes INNER JOIN
                         [GenSoft-Creator].dbo.EntityTypeAttributes AS ChildEntityTypeAttributes ON ChildAttributes.Id = ChildEntityTypeAttributes.AttributeId INNER JOIN
                         [GenSoft-Creator].dbo.EntityTypes AS ChildEntityTypes ON ChildEntityTypeAttributes.EntityTypeId = ChildEntityTypes.Id ON RelView.ChildColumn = ChildAttributes.Name AND ChildTypes.Id = ChildEntityTypes.TypeId LEFT OUTER JOIN
                         [GenSoft-Creator].dbo.EntityTypeAttributes AS ParentEntityTypeAttributes INNER JOIN
                         [GenSoft-Creator].dbo.EntityTypes AS ParentEntityTypes ON ParentEntityTypeAttributes.EntityTypeId = ParentEntityTypes.Id INNER JOIN
                         [GenSoft-Creator].dbo.Attributes AS ParentAttributes ON ParentEntityTypeAttributes.AttributeId = ParentAttributes.Id INNER JOIN
                         [GenSoft-Creator].dbo.Types AS ParentTypes ON ParentEntityTypes.TypeId = ParentTypes.Id ON RelView.ParentTable = ParentTypes.Name AND RelView.ParentColumn = ParentAttributes.Name LEFT OUTER JOIN
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
                         ChildAttributes.Name = ChildKeyData.COLUMN_NAME AND ChildTypes.Name = ChildKeyData.TABLE_NAME LEFT OUTER JOIN
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
                                                                                   COLUMNS_1.COLUMN_NAME = PrimaryKeys_1.columnName) AS t_1) AS ParentKeyData ON ParentTypes.Name = ParentKeyData.TABLE_NAME AND 
                         ParentAttributes.Name = ParentKeyData.COLUMN_NAME LEFT OUTER JOIN
                         [GenSoft-Creator].dbo.RelationshipTypes AS RelationshipTypes ON CASE WHEN ParentKeyData.isidentity = 1 THEN 1 ELSE 2 END = RelationshipTypes.ParentOrdinalityId AND 
                         CASE WHEN ChildKeyData.isPrimaryKey = 1 THEN 1 ELSE 2 END = RelationshipTypes.ChildOrdinalityId
WHERE        (ChildEntityTypes.ApplicationId = @AppId) AND (ParentEntityTypes.ApplicationId = @AppId)) as relsource

select * from #ParentClas




set @Counter = 0
	WHILE (1=1)
	BEGIN 
		set @Counter +=1; 

		declare @ParentEntityTypeId int, @ChildEntityTypeId int;
		set @ParentEntityTypeId = null;
		select @ParentEntityTypeId = ParentEntityTypeId,  @ChildEntityTypeId = ChildEntityTypeId
		from #ParentClas where id = @Counter;

		if(@ParentEntityTypeId is null) break;

			
			INSERT INTO [GenSoft-Creator].dbo.ParentEntityTypes
                         (Id,Parent_EntityTypeId) Values (@ChildEntityTypeId, @ParentEntityTypeId)	 
   End

INSERT INTO [GenSoft-Creator].dbo.MainEntities
                         (Id, EntityTypeId)
SELECT        ProcessSteps.Id AS Expr2, EntityTypes.Id AS Expr1
FROM            [GenSoft-Creator].dbo.EntityTypes INNER JOIN
                         [GenSoft-Creator].dbo.Types ON EntityTypes.TypeId = Types.Id INNER JOIN
                         [GenSoft-Creator].dbo.ProcessSteps ON Types.Name = ProcessSteps.Entity
WHERE        (EntityTypes.ApplicationId = @AppId)