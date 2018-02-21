--------------------------------------Insert Test Data --------------------------------------------------
declare @AppId int, @appName varchar(50)

set @appName = (SELECT DB_NAME())

set @AppId = (SELECT        Application.Id
				FROM            [GenSoft-Creator].dbo.Application INNER JOIN
											[GenSoft-Creator].dbo.DatabaseInfo ON Application.Id = DatabaseInfo.Id
				WHERE        (DatabaseInfo.DBName = @appName) )

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


DECLARE @entityId int

DELETE FROM [GenSoft-Creator].dbo.Entity
FROM            [GenSoft-Creator].dbo.Entity INNER JOIN
                         [GenSoft-Creator].dbo.EntityType ON Entity.EntityTypeId = EntityType.Id INNER JOIN
                         [GenSoft-Creator].dbo.DBType ON EntityType.Id = DBType.Id
WHERE        (EntityType.ApplicationId = @AppId)
------------------------For Each Entity
DECLARE Entity_CURSOR CURSOR 
  LOCAL STATIC READ_ONLY FORWARD_ONLY
FOR 
			SELECT EntityType.id from [GenSoft-Creator].dbo.EntityType INNER JOIN
										[GenSoft-Creator].dbo.DBType ON EntityType.Id = DBType.Id
			WHERE        (EntityType.ApplicationId = @AppId)

OPEN Entity_CURSOR
FETCH NEXT FROM Entity_CURSOR INTO @entityId
WHILE @@FETCH_STATUS = 0
BEGIN 
	PRINT @entityId
    --Do something with Id here
	declare @entityName varchar(50)
	set @entityName = (select schemaName + '.[' + TableName + ']' from [GenSoft-Creator].dbo.DBType where id = @entityId)
	PRINT @entityName


	

--------------------------For Each Data Row	
	DECLARE @rowId int
	Exec('
	
	DECLARE Data_CURSOR CURSOR 
	Global STATIC READ_ONLY FORWARD_ONLY
	FOR 
	SELECT Id  
	FROM ' + @entityName)

		OPEN Data_CURSOR
		FETCH NEXT FROM Data_CURSOR INTO @rowId
		WHILE @@FETCH_STATUS = 0
		BEGIN 
				Declare @entityAttributeId int, @entityRowId int

				Declare @typeIdTable Table(Id int)

			Insert into [GenSoft-Creator].dbo.[Entity] (EntityTypeId)
			OutPut INSERTED.Id into @typeIdTable 
			 values (@entityId)		
			
			select @entityRowId = id from @typeIdTable	

				
-------------------------For Each Entity Property	
				
		
				DECLARE EntityProperty_CURSOR CURSOR 
				  LOCAL STATIC READ_ONLY FORWARD_ONLY
				FOR 
				SELECT DISTINCT Id
				FROM [GenSoft-Creator].dbo.EntityTypeAttributes where EntityTypeId = @entityId 

				OPEN EntityProperty_CURSOR
				FETCH NEXT FROM EntityProperty_CURSOR INTO @entityAttributeId
				WHILE @@FETCH_STATUS = 0
				BEGIN 
					PRINT @entityAttributeId

					--Declare @DataType varchar(50)
					--Set @DataType = (SELECT distinct  DataTypes.Name
					--					FROM            AmoebaDB.dbo.EntityProperties INNER JOIN
					--											 AmoebaDB.dbo.DataProperties ON EntityProperties.Id = DataProperties.EntityPropertyId INNER JOIN
					--											 AmoebaDB.dbo.DataTypes ON DataProperties.DataTypeId = DataTypes.Id
					--					WHERE        (EntityProperties.Id = @entityPropertyId))
					Declare @propertyName varchar(50), @AttributeId int
					SELECT  @AttributeId = Attributes.id,@propertyName= Attributes.Name
										FROM            [GenSoft-Creator].dbo.Attributes INNER JOIN
																 [GenSoft-Creator].dbo.EntityTypeAttributes ON Attributes.Id = EntityTypeAttributes.AttributeId
										WHERE        (EntityTypeAttributes.Id = @entityAttributeId)
					--Do something with Id here
					Declare @Sql nvarchar(1000)
					declare @param1 nvarchar(1000) 

					
							select @Sql = N'select @val = cast(' + @propertyName + N' as VarChar(Max)) from ' + @entityName + N' Where Id = ' + cast(@rowId as varchar(50))
							PRINT  'Sql = ' +@Sql
							set @param1 = '@val varchar(Max) OUTPUT'
							Declare @value nvarchar(Max)
					
							execute sp_executesql @sql, @param1, @value Output
							PRINT 'Value = ' + @value

							insert into [GenSoft-Creator].dbo.EntityAttribute(EntityId, AttributeId, [Value]) Values (@EntityRowId, @AttributeId,  @value)
															
					
					FETCH NEXT FROM EntityProperty_CURSOR INTO @entityAttributeId
				END
				CLOSE EntityProperty_CURSOR
				DEALLOCATE EntityProperty_CURSOR



			FETCH NEXT FROM Data_CURSOR INTO @rowId
		END
		CLOSE Data_CURSOR
		DEALLOCATE Data_CURSOR
	
    FETCH NEXT FROM Entity_CURSOR INTO @entityId
END
CLOSE Entity_CURSOR
DEALLOCATE Entity_CURSOR


select * from [GenSoft-Creator].dbo.EntityAttribute