

---- Set Lookup Textblock for entities
delete from EntityTypePresentationProperties 
delete from EntityTypeAttributeCaches
delete from PropertyValueOptions
delete from PropertyValues

insert into EntityTypePresentationProperties(ViewPropertyPresentationPropertyTypeId, PresentationThemeId,ViewTypeId,EntityTypeAttributeId)
SELECT        [ConfigurationPropertyPresentations].ViewPropertyPresentationPropertyTypeId, [ConfigurationPropertyPresentations].PresentationThemeId,  
                         [ConfigurationPropertyPresentations].ViewTypeId, t.ChildAttributeId
FROM            [ConfigurationPropertyPresentations] CROSS JOIN
                         (SELECT        ChildAttributeId
FROM          EntityRelationsView
WHERE        (Relationship = 'One-Many') ) t
where [ConfigurationPropertyPresentations].Id = 1

insert into PropertyValueOptions(Id, ValueOptionId)
SELECT        EntityTypePresentationProperties.Id, ConfigurationPropertyPresentations.ValueOptionId
FROM            EntityTypePresentationProperties INNER JOIN
                         ConfigurationPropertyPresentations ON EntityTypePresentationProperties.ViewPropertyPresentationPropertyTypeId = ConfigurationPropertyPresentations.ViewPropertyPresentationPropertyTypeId AND 
                         EntityTypePresentationProperties.ViewTypeId = ConfigurationPropertyPresentations.ViewTypeId
WHERE        (EntityTypePresentationProperties.ViewPropertyPresentationPropertyTypeId = 9) AND (EntityTypePresentationProperties.ViewTypeId = 1)

------- set ID Label invisible
insert into EntityTypePresentationProperties(ViewPropertyPresentationPropertyTypeId, PresentationThemeId,ViewTypeId,EntityTypeAttributeId)
SELECT        [ConfigurationPropertyPresentations].ViewPropertyPresentationPropertyTypeId, [ConfigurationPropertyPresentations].PresentationThemeId, 
                         [ConfigurationPropertyPresentations].ViewTypeId, t.Id
FROM            [ConfigurationPropertyPresentations] CROSS JOIN
                         (SELECT        EntityTypeAttributes.Id
FROM            EntityTypeAttributes INNER JOIN
                         Attributes ON EntityTypeAttributes.AttributeId = Attributes.Id
WHERE        (Attributes.Name = 'Id')) t
where [ConfigurationPropertyPresentations].Id = 2


insert into PropertyValueOptions(Id, ValueOptionId)
SELECT        EntityTypePresentationProperties.Id, ConfigurationPropertyPresentations.ValueOptionId
FROM            EntityTypePresentationProperties INNER JOIN
                         ConfigurationPropertyPresentations ON EntityTypePresentationProperties.ViewPropertyPresentationPropertyTypeId = ConfigurationPropertyPresentations.ViewPropertyPresentationPropertyTypeId AND 
                         EntityTypePresentationProperties.ViewTypeId = ConfigurationPropertyPresentations.ViewTypeId
WHERE        (ConfigurationPropertyPresentations.Id= 2) AND (EntityTypePresentationProperties.ViewTypeId = 1)

------- set ID Label invisible
insert into EntityTypePresentationProperties(ViewPropertyPresentationPropertyTypeId, PresentationThemeId,ViewTypeId,EntityTypeAttributeId)
SELECT        [ConfigurationPropertyPresentations].ViewPropertyPresentationPropertyTypeId, [ConfigurationPropertyPresentations].PresentationThemeId, 
                         [ConfigurationPropertyPresentations].ViewTypeId, t.Id
FROM            [ConfigurationPropertyPresentations] CROSS JOIN
                         (SELECT        EntityTypeAttributes.Id
FROM            EntityTypeAttributes INNER JOIN
                         Attributes ON EntityTypeAttributes.AttributeId = Attributes.Id
WHERE        (Attributes.Name = 'Id')) t
where [ConfigurationPropertyPresentations].Id = 3

insert into PropertyValueOptions(Id, ValueOptionId)
SELECT        EntityTypePresentationProperties.Id, ConfigurationPropertyPresentations.ValueOptionId
FROM            EntityTypePresentationProperties INNER JOIN
                         ConfigurationPropertyPresentations ON EntityTypePresentationProperties.ViewPropertyPresentationPropertyTypeId = ConfigurationPropertyPresentations.ViewPropertyPresentationPropertyTypeId AND 
                         EntityTypePresentationProperties.ViewTypeId = ConfigurationPropertyPresentations.ViewTypeId
WHERE        (ConfigurationPropertyPresentations.Id= 3) AND (EntityTypePresentationProperties.ViewTypeId = 1)

------- set ID Label invisible
insert into EntityTypePresentationProperties(ViewPropertyPresentationPropertyTypeId, PresentationThemeId,ViewTypeId,EntityTypeAttributeId)
SELECT        [ConfigurationPropertyPresentations].ViewPropertyPresentationPropertyTypeId, [ConfigurationPropertyPresentations].PresentationThemeId, 
                         [ConfigurationPropertyPresentations].ViewTypeId, t.Id
FROM            [ConfigurationPropertyPresentations] CROSS JOIN
                         (SELECT        EntityTypeAttributes.Id
FROM            EntityTypeAttributes INNER JOIN
                         Attributes ON EntityTypeAttributes.AttributeId = Attributes.Id
WHERE        (Attributes.Name = 'Id')) t
where [ConfigurationPropertyPresentations].Id = 4

insert into PropertyValueOptions(Id, ValueOptionId)
SELECT        EntityTypePresentationProperties.Id, ConfigurationPropertyPresentations.ValueOptionId
FROM            EntityTypePresentationProperties INNER JOIN
                         ConfigurationPropertyPresentations ON EntityTypePresentationProperties.ViewPropertyPresentationPropertyTypeId = ConfigurationPropertyPresentations.ViewPropertyPresentationPropertyTypeId AND 
                         EntityTypePresentationProperties.ViewTypeId = ConfigurationPropertyPresentations.ViewTypeId
WHERE        (ConfigurationPropertyPresentations.Id= 4)  AND (EntityTypePresentationProperties.ViewTypeId = 2)

------- set ID Label invisible
insert into EntityTypePresentationProperties(ViewPropertyPresentationPropertyTypeId, PresentationThemeId,ViewTypeId,EntityTypeAttributeId)
SELECT        [ConfigurationPropertyPresentations].ViewPropertyPresentationPropertyTypeId, [ConfigurationPropertyPresentations].PresentationThemeId,
                         [ConfigurationPropertyPresentations].ViewTypeId, t.Id
FROM            [ConfigurationPropertyPresentations] CROSS JOIN
                         (SELECT        EntityTypeAttributes.Id
FROM            EntityTypeAttributes INNER JOIN
                         Attributes ON EntityTypeAttributes.AttributeId = Attributes.Id
WHERE        (Attributes.Name = 'Id')) t
where [ConfigurationPropertyPresentations].Id = 5

insert into PropertyValueOptions(Id, ValueOptionId)
SELECT        EntityTypePresentationProperties.Id, ConfigurationPropertyPresentations.ValueOptionId
FROM            EntityTypePresentationProperties INNER JOIN
                         ConfigurationPropertyPresentations ON EntityTypePresentationProperties.ViewPropertyPresentationPropertyTypeId = ConfigurationPropertyPresentations.ViewPropertyPresentationPropertyTypeId AND 
                         EntityTypePresentationProperties.ViewTypeId = ConfigurationPropertyPresentations.ViewTypeId
WHERE        (ConfigurationPropertyPresentations.Id= 5)  AND (EntityTypePresentationProperties.ViewTypeId = 2)

insert into EntityTypeAttributeCaches(Id)
SELECT   distinct     EntityTypeAttributes.Id
FROM            EntityTypeAttributes INNER JOIN
                         Attributes ON EntityTypeAttributes.AttributeId = Attributes.Id
WHERE        (Attributes.Name = 'Name')


------- set ID Label DisplayText
insert into EntityTypePresentationProperties(ViewPropertyPresentationPropertyTypeId, PresentationThemeId,ViewTypeId,EntityTypeAttributeId)
SELECT        [ConfigurationPropertyPresentations].ViewPropertyPresentationPropertyTypeId, [ConfigurationPropertyPresentations].PresentationThemeId, 
                         [ConfigurationPropertyPresentations].ViewTypeId, t.Id
FROM            [ConfigurationPropertyPresentations] CROSS JOIN
                         (SELECT        EntityTypeAttributes.Id, Attributes.Name
FROM            EntityTypeAttributes INNER JOIN
                         Attributes ON EntityTypeAttributes.AttributeId = Attributes.Id
WHERE        (Attributes.Name LIKE '%Id') AND (Attributes.Name <> 'Id')) t
where [ConfigurationPropertyPresentations].Id = 6  

insert into PropertyValueOptions(Id, ValueOptionId)
SELECT        EntityTypePresentationProperties.Id, ConfigurationPropertyPresentations.ValueOptionId
FROM            EntityTypePresentationProperties INNER JOIN
                         ConfigurationPropertyPresentations ON EntityTypePresentationProperties.ViewPropertyPresentationPropertyTypeId = ConfigurationPropertyPresentations.ViewPropertyPresentationPropertyTypeId AND 
                         EntityTypePresentationProperties.ViewTypeId = ConfigurationPropertyPresentations.ViewTypeId
WHERE        (ConfigurationPropertyPresentations.Id= 6)  AND (EntityTypePresentationProperties.ViewTypeId = 1)

insert into EntityTypePresentationProperties(ViewPropertyPresentationPropertyTypeId, PresentationThemeId,ViewTypeId,EntityTypeAttributeId)
SELECT        [ConfigurationPropertyPresentations].ViewPropertyPresentationPropertyTypeId, [ConfigurationPropertyPresentations].PresentationThemeId, 
                         [ConfigurationPropertyPresentations].ViewTypeId, t.Id
FROM            [ConfigurationPropertyPresentations] CROSS JOIN
                         (SELECT        EntityTypeAttributes.Id, Attributes.Name
FROM            EntityTypeAttributes INNER JOIN
                         Attributes ON EntityTypeAttributes.AttributeId = Attributes.Id
WHERE        (Attributes.Name LIKE '%Id') AND (Attributes.Name <> 'Id')) t
where [ConfigurationPropertyPresentations].Id = 7

insert into PropertyValueOptions(Id, ValueOptionId)
SELECT        EntityTypePresentationProperties.Id, ConfigurationPropertyPresentations.ValueOptionId
FROM            EntityTypePresentationProperties INNER JOIN
                         ConfigurationPropertyPresentations ON EntityTypePresentationProperties.ViewPropertyPresentationPropertyTypeId = ConfigurationPropertyPresentations.ViewPropertyPresentationPropertyTypeId AND 
                         EntityTypePresentationProperties.ViewTypeId = ConfigurationPropertyPresentations.ViewTypeId
WHERE        (ConfigurationPropertyPresentations.Id= 7)  AND (EntityTypePresentationProperties.ViewTypeId = 2)


------- set ID Label DisplayText
insert into EntityTypePresentationProperties(ViewPropertyPresentationPropertyTypeId, PresentationThemeId,ViewTypeId,EntityTypeAttributeId)
SELECT        [ConfigurationPropertyPresentations].ViewPropertyPresentationPropertyTypeId, [ConfigurationPropertyPresentations].PresentationThemeId, 
                         [ConfigurationPropertyPresentations].ViewTypeId, t.Id
FROM            [ConfigurationPropertyPresentations] CROSS JOIN
                         (SELECT        EntityTypeAttributes.Id, Attributes.Name
FROM            EntityTypeAttributes INNER JOIN
                         Attributes ON EntityTypeAttributes.AttributeId = Attributes.Id
WHERE        (Attributes.Name not LIKE '%Id')) t
where [ConfigurationPropertyPresentations].Id = 6

insert into EntityTypePresentationProperties(ViewPropertyPresentationPropertyTypeId, PresentationThemeId,ViewTypeId,EntityTypeAttributeId)
SELECT        [ConfigurationPropertyPresentations].ViewPropertyPresentationPropertyTypeId, [ConfigurationPropertyPresentations].PresentationThemeId, 
                         [ConfigurationPropertyPresentations].ViewTypeId, t.Id
FROM            [ConfigurationPropertyPresentations] CROSS JOIN
                         (SELECT        EntityTypeAttributes.Id, Attributes.Name
FROM            EntityTypeAttributes INNER JOIN
                         Attributes ON EntityTypeAttributes.AttributeId = Attributes.Id
WHERE        (Attributes.Name not LIKE '%Id')) t
where [ConfigurationPropertyPresentations].Id = 7


insert into PropertyValues(Id, [Value])
SELECT      EntityTypePresentationProperties.Id,  Replace(dbo.SpaceBeforeCap(Attributes.Name),' Id','') as displaytxt
FROM            EntityTypePresentationProperties INNER JOIN
                         EntityTypeAttributes ON EntityTypePresentationProperties.EntityTypeAttributeId = EntityTypeAttributes.Id INNER JOIN
                         Attributes ON EntityTypeAttributes.AttributeId = Attributes.Id
WHERE        (EntityTypePresentationProperties.ViewPropertyPresentationPropertyTypeId = 10) --and (Attributes.Name not like '%Id')

-----------------set comboboxes ----------------------

insert into EntityTypePresentationProperties(ViewPropertyPresentationPropertyTypeId, PresentationThemeId,ViewTypeId,EntityTypeAttributeId)
SELECT        [ConfigurationPropertyPresentations].ViewPropertyPresentationPropertyTypeId, [ConfigurationPropertyPresentations].PresentationThemeId, 
                         [ConfigurationPropertyPresentations].ViewTypeId, t.Id
FROM            [ConfigurationPropertyPresentations] CROSS JOIN
                         (SELECT        EntityTypeAttributes.Id, Attributes.Name
FROM            EntityTypeAttributes INNER JOIN
                         Attributes ON EntityTypeAttributes.AttributeId = Attributes.Id INNER JOIN
                         Types ON Attributes.DataTypeId = Types.Id
WHERE        (Attributes.Name LIKE '%Id') AND (Attributes.Name <> 'Id') AND (Types.Name = N'System.int32')) t
where [ConfigurationPropertyPresentations].Id = 8

insert into PropertyValueOptions(Id, ValueOptionId)
SELECT DISTINCT EntityTypePresentationProperties.Id, ConfigurationPropertyPresentations.ValueOptionId
FROM            Attributes INNER JOIN
                         Types ON Attributes.DataTypeId = Types.Id INNER JOIN
                         EntityTypeAttributes ON Attributes.Id = EntityTypeAttributes.AttributeId INNER JOIN
                         EntityTypePresentationProperties INNER JOIN
                         ConfigurationPropertyPresentations ON EntityTypePresentationProperties.ViewPropertyPresentationPropertyTypeId = ConfigurationPropertyPresentations.ViewPropertyPresentationPropertyTypeId AND 
                         EntityTypePresentationProperties.ViewTypeId = ConfigurationPropertyPresentations.ViewTypeId ON EntityTypeAttributes.Id = EntityTypePresentationProperties.EntityTypeAttributeId
WHERE        (EntityTypePresentationProperties.ViewPropertyPresentationPropertyTypeId = 9) AND (EntityTypePresentationProperties.ViewTypeId = 2) AND (Types.Name = N'System.int32') AND (ConfigurationPropertyPresentations.Id= 8)

---------------- do checkboxes
insert into EntityTypePresentationProperties(ViewPropertyPresentationPropertyTypeId, PresentationThemeId,ViewTypeId,EntityTypeAttributeId)
SELECT        [ConfigurationPropertyPresentations].ViewPropertyPresentationPropertyTypeId, [ConfigurationPropertyPresentations].PresentationThemeId, 
                         [ConfigurationPropertyPresentations].ViewTypeId, t.Id
FROM            [ConfigurationPropertyPresentations] CROSS JOIN
                         (SELECT        EntityTypeAttributes.Id, Attributes.Name
FROM            EntityTypeAttributes INNER JOIN
                         Attributes ON EntityTypeAttributes.AttributeId = Attributes.Id INNER JOIN
                         Types ON Attributes.DataTypeId = Types.Id
WHERE        (Types.Name = N'bool')) t
where [ConfigurationPropertyPresentations].Id = 9


insert into PropertyValueOptions(Id, ValueOptionId)
SELECT DISTINCT EntityTypePresentationProperties.Id, ConfigurationPropertyPresentations.ValueOptionId
FROM            Attributes INNER JOIN
                         Types ON Attributes.DataTypeId = Types.Id INNER JOIN
                         EntityTypeAttributes ON Attributes.Id = EntityTypeAttributes.AttributeId INNER JOIN
                         EntityTypePresentationProperties INNER JOIN
                         ConfigurationPropertyPresentations ON EntityTypePresentationProperties.ViewPropertyPresentationPropertyTypeId = ConfigurationPropertyPresentations.ViewPropertyPresentationPropertyTypeId AND 
                         EntityTypePresentationProperties.ViewTypeId = ConfigurationPropertyPresentations.ViewTypeId ON EntityTypeAttributes.Id = EntityTypePresentationProperties.EntityTypeAttributeId
WHERE        (EntityTypePresentationProperties.ViewPropertyPresentationPropertyTypeId = 9) AND (EntityTypePresentationProperties.ViewTypeId = 2) AND (Types.Name = N'bool') AND (ConfigurationPropertyPresentations.Id= 9)


