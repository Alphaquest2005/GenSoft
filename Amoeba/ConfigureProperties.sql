

---- Set Lookup Textblock for entities
delete from EntityTypePresentationProperty 
delete from EntityTypeAttributeCache
delete from PropertyValueOption
delete from PropertyValue

insert into EntityTypePresentationProperty(ViewPropertyPresentationPropertyTypeId, PresentationThemeId,ViewTypeId,EntityTypeAttributeId)
SELECT        [ConfigurationPropertyPresentation].ViewPropertyPresentationPropertyTypeId, [ConfigurationPropertyPresentation].PresentationThemeId,  
                         [ConfigurationPropertyPresentation].ViewTypeId, t.ChildAttributeId
FROM            [ConfigurationPropertyPresentation] CROSS JOIN
                         (SELECT        ChildAttributeId
FROM          EntityRelationsView
WHERE        (Relationship = 'One-Many') ) t
where [ConfigurationPropertyPresentation].Id = 1

insert into PropertyValueOption(Id, ValueOptionId)
SELECT        EntityTypePresentationProperty.Id, ConfigurationPropertyPresentation.ValueOptionId
FROM            EntityTypePresentationProperty INNER JOIN
                         ConfigurationPropertyPresentation ON EntityTypePresentationProperty.ViewPropertyPresentationPropertyTypeId = ConfigurationPropertyPresentation.ViewPropertyPresentationPropertyTypeId AND 
                         EntityTypePresentationProperty.ViewTypeId = ConfigurationPropertyPresentation.ViewTypeId
WHERE        (EntityTypePresentationProperty.ViewPropertyPresentationPropertyTypeId = 9) AND (EntityTypePresentationProperty.ViewTypeId = 1)

------- set ID Label invisible
insert into EntityTypePresentationProperty(ViewPropertyPresentationPropertyTypeId, PresentationThemeId,ViewTypeId,EntityTypeAttributeId)
SELECT        [ConfigurationPropertyPresentation].ViewPropertyPresentationPropertyTypeId, [ConfigurationPropertyPresentation].PresentationThemeId, 
                         [ConfigurationPropertyPresentation].ViewTypeId, t.Id
FROM            [ConfigurationPropertyPresentation] CROSS JOIN
                         (SELECT        EntityTypeAttributes.Id
FROM            EntityTypeAttributes INNER JOIN
                         Attributes ON EntityTypeAttributes.AttributeId = Attributes.Id
WHERE        (Attributes.Name = 'Id')) t
where [ConfigurationPropertyPresentation].Id = 2


insert into PropertyValueOption(Id, ValueOptionId)
SELECT        EntityTypePresentationProperty.Id, ConfigurationPropertyPresentation.ValueOptionId
FROM            EntityTypePresentationProperty INNER JOIN
                         ConfigurationPropertyPresentation ON EntityTypePresentationProperty.ViewPropertyPresentationPropertyTypeId = ConfigurationPropertyPresentation.ViewPropertyPresentationPropertyTypeId AND 
                         EntityTypePresentationProperty.ViewTypeId = ConfigurationPropertyPresentation.ViewTypeId
WHERE        (ConfigurationPropertyPresentation.Id = 2) AND (EntityTypePresentationProperty.ViewTypeId = 1)

------- set ID Label invisible
insert into EntityTypePresentationProperty(ViewPropertyPresentationPropertyTypeId, PresentationThemeId,ViewTypeId,EntityTypeAttributeId)
SELECT        [ConfigurationPropertyPresentation].ViewPropertyPresentationPropertyTypeId, [ConfigurationPropertyPresentation].PresentationThemeId, 
                         [ConfigurationPropertyPresentation].ViewTypeId, t.Id
FROM            [ConfigurationPropertyPresentation] CROSS JOIN
                         (SELECT        EntityTypeAttributes.Id
FROM            EntityTypeAttributes INNER JOIN
                         Attributes ON EntityTypeAttributes.AttributeId = Attributes.Id
WHERE        (Attributes.Name = 'Id')) t
where [ConfigurationPropertyPresentation].Id = 3

insert into PropertyValueOption(Id, ValueOptionId)
SELECT        EntityTypePresentationProperty.Id, ConfigurationPropertyPresentation.ValueOptionId
FROM            EntityTypePresentationProperty INNER JOIN
                         ConfigurationPropertyPresentation ON EntityTypePresentationProperty.ViewPropertyPresentationPropertyTypeId = ConfigurationPropertyPresentation.ViewPropertyPresentationPropertyTypeId AND 
                         EntityTypePresentationProperty.ViewTypeId = ConfigurationPropertyPresentation.ViewTypeId
WHERE        (ConfigurationPropertyPresentation.Id = 3) AND (EntityTypePresentationProperty.ViewTypeId = 1)

------- set ID Label invisible
insert into EntityTypePresentationProperty(ViewPropertyPresentationPropertyTypeId, PresentationThemeId,ViewTypeId,EntityTypeAttributeId)
SELECT        [ConfigurationPropertyPresentation].ViewPropertyPresentationPropertyTypeId, [ConfigurationPropertyPresentation].PresentationThemeId, 
                         [ConfigurationPropertyPresentation].ViewTypeId, t.Id
FROM            [ConfigurationPropertyPresentation] CROSS JOIN
                         (SELECT        EntityTypeAttributes.Id
FROM            EntityTypeAttributes INNER JOIN
                         Attributes ON EntityTypeAttributes.AttributeId = Attributes.Id
WHERE        (Attributes.Name = 'Id')) t
where [ConfigurationPropertyPresentation].Id = 4

insert into PropertyValueOption(Id, ValueOptionId)
SELECT        EntityTypePresentationProperty.Id, ConfigurationPropertyPresentation.ValueOptionId
FROM            EntityTypePresentationProperty INNER JOIN
                         ConfigurationPropertyPresentation ON EntityTypePresentationProperty.ViewPropertyPresentationPropertyTypeId = ConfigurationPropertyPresentation.ViewPropertyPresentationPropertyTypeId AND 
                         EntityTypePresentationProperty.ViewTypeId = ConfigurationPropertyPresentation.ViewTypeId
WHERE        (ConfigurationPropertyPresentation.Id = 4)  AND (EntityTypePresentationProperty.ViewTypeId = 2)

------- set ID Label invisible
insert into EntityTypePresentationProperty(ViewPropertyPresentationPropertyTypeId, PresentationThemeId,ViewTypeId,EntityTypeAttributeId)
SELECT        [ConfigurationPropertyPresentation].ViewPropertyPresentationPropertyTypeId, [ConfigurationPropertyPresentation].PresentationThemeId,
                         [ConfigurationPropertyPresentation].ViewTypeId, t.Id
FROM            [ConfigurationPropertyPresentation] CROSS JOIN
                         (SELECT        EntityTypeAttributes.Id
FROM            EntityTypeAttributes INNER JOIN
                         Attributes ON EntityTypeAttributes.AttributeId = Attributes.Id
WHERE        (Attributes.Name = 'Id')) t
where [ConfigurationPropertyPresentation].Id = 5

insert into PropertyValueOption(Id, ValueOptionId)
SELECT        EntityTypePresentationProperty.Id, ConfigurationPropertyPresentation.ValueOptionId
FROM            EntityTypePresentationProperty INNER JOIN
                         ConfigurationPropertyPresentation ON EntityTypePresentationProperty.ViewPropertyPresentationPropertyTypeId = ConfigurationPropertyPresentation.ViewPropertyPresentationPropertyTypeId AND 
                         EntityTypePresentationProperty.ViewTypeId = ConfigurationPropertyPresentation.ViewTypeId
WHERE        (ConfigurationPropertyPresentation.Id = 5)  AND (EntityTypePresentationProperty.ViewTypeId = 2)

insert into EntityTypeAttributeCache(Id)
SELECT   distinct     EntityTypeAttributes.Id
FROM            EntityTypeAttributes INNER JOIN
                         Attributes ON EntityTypeAttributes.AttributeId = Attributes.Id
WHERE        (Attributes.Name = 'Name')


------- set ID Label DisplayText
insert into EntityTypePresentationProperty(ViewPropertyPresentationPropertyTypeId, PresentationThemeId,ViewTypeId,EntityTypeAttributeId)
SELECT        [ConfigurationPropertyPresentation].ViewPropertyPresentationPropertyTypeId, [ConfigurationPropertyPresentation].PresentationThemeId, 
                         [ConfigurationPropertyPresentation].ViewTypeId, t.Id
FROM            [ConfigurationPropertyPresentation] CROSS JOIN
                         (SELECT        EntityTypeAttributes.Id, Attributes.Name
FROM            EntityTypeAttributes INNER JOIN
                         Attributes ON EntityTypeAttributes.AttributeId = Attributes.Id
WHERE        (Attributes.Name LIKE '%Id') AND (Attributes.Name <> 'Id')) t
where [ConfigurationPropertyPresentation].Id = 6  

insert into PropertyValueOption(Id, ValueOptionId)
SELECT        EntityTypePresentationProperty.Id, ConfigurationPropertyPresentation.ValueOptionId
FROM            EntityTypePresentationProperty INNER JOIN
                         ConfigurationPropertyPresentation ON EntityTypePresentationProperty.ViewPropertyPresentationPropertyTypeId = ConfigurationPropertyPresentation.ViewPropertyPresentationPropertyTypeId AND 
                         EntityTypePresentationProperty.ViewTypeId = ConfigurationPropertyPresentation.ViewTypeId
WHERE        (ConfigurationPropertyPresentation.Id = 6)  AND (EntityTypePresentationProperty.ViewTypeId = 1)

insert into EntityTypePresentationProperty(ViewPropertyPresentationPropertyTypeId, PresentationThemeId,ViewTypeId,EntityTypeAttributeId)
SELECT        [ConfigurationPropertyPresentation].ViewPropertyPresentationPropertyTypeId, [ConfigurationPropertyPresentation].PresentationThemeId, 
                         [ConfigurationPropertyPresentation].ViewTypeId, t.Id
FROM            [ConfigurationPropertyPresentation] CROSS JOIN
                         (SELECT        EntityTypeAttributes.Id, Attributes.Name
FROM            EntityTypeAttributes INNER JOIN
                         Attributes ON EntityTypeAttributes.AttributeId = Attributes.Id
WHERE        (Attributes.Name LIKE '%Id') AND (Attributes.Name <> 'Id')) t
where [ConfigurationPropertyPresentation].Id = 7

insert into PropertyValueOption(Id, ValueOptionId)
SELECT        EntityTypePresentationProperty.Id, ConfigurationPropertyPresentation.ValueOptionId
FROM            EntityTypePresentationProperty INNER JOIN
                         ConfigurationPropertyPresentation ON EntityTypePresentationProperty.ViewPropertyPresentationPropertyTypeId = ConfigurationPropertyPresentation.ViewPropertyPresentationPropertyTypeId AND 
                         EntityTypePresentationProperty.ViewTypeId = ConfigurationPropertyPresentation.ViewTypeId
WHERE        (ConfigurationPropertyPresentation.Id = 7)  AND (EntityTypePresentationProperty.ViewTypeId = 2)


------- set ID Label DisplayText
insert into EntityTypePresentationProperty(ViewPropertyPresentationPropertyTypeId, PresentationThemeId,ViewTypeId,EntityTypeAttributeId)
SELECT        [ConfigurationPropertyPresentation].ViewPropertyPresentationPropertyTypeId, [ConfigurationPropertyPresentation].PresentationThemeId, 
                         [ConfigurationPropertyPresentation].ViewTypeId, t.Id
FROM            [ConfigurationPropertyPresentation] CROSS JOIN
                         (SELECT        EntityTypeAttributes.Id, Attributes.Name
FROM            EntityTypeAttributes INNER JOIN
                         Attributes ON EntityTypeAttributes.AttributeId = Attributes.Id
WHERE        (Attributes.Name not LIKE '%Id')) t
where [ConfigurationPropertyPresentation].Id = 6

insert into EntityTypePresentationProperty(ViewPropertyPresentationPropertyTypeId, PresentationThemeId,ViewTypeId,EntityTypeAttributeId)
SELECT        [ConfigurationPropertyPresentation].ViewPropertyPresentationPropertyTypeId, [ConfigurationPropertyPresentation].PresentationThemeId, 
                         [ConfigurationPropertyPresentation].ViewTypeId, t.Id
FROM            [ConfigurationPropertyPresentation] CROSS JOIN
                         (SELECT        EntityTypeAttributes.Id, Attributes.Name
FROM            EntityTypeAttributes INNER JOIN
                         Attributes ON EntityTypeAttributes.AttributeId = Attributes.Id
WHERE        (Attributes.Name not LIKE '%Id')) t
where [ConfigurationPropertyPresentation].Id = 7


insert into PropertyValue(Id, [Value])
SELECT      EntityTypePresentationProperty.Id,  dbo.SpaceBeforeCap(Attributes.Name) as displaytxt
FROM            EntityTypePresentationProperty INNER JOIN
                         EntityTypeAttributes ON EntityTypePresentationProperty.EntityTypeAttributeId = EntityTypeAttributes.Id INNER JOIN
                         Attributes ON EntityTypeAttributes.AttributeId = Attributes.Id
WHERE        (EntityTypePresentationProperty.ViewPropertyPresentationPropertyTypeId = 10) --and (Attributes.Name not like '%Id')

-----------------set comboboxes ----------------------

insert into EntityTypePresentationProperty(ViewPropertyPresentationPropertyTypeId, PresentationThemeId,ViewTypeId,EntityTypeAttributeId)
SELECT        [ConfigurationPropertyPresentation].ViewPropertyPresentationPropertyTypeId, [ConfigurationPropertyPresentation].PresentationThemeId, 
                         [ConfigurationPropertyPresentation].ViewTypeId, t.Id
FROM            [ConfigurationPropertyPresentation] CROSS JOIN
                         (SELECT        EntityTypeAttributes.Id, Attributes.Name
FROM            EntityTypeAttributes INNER JOIN
                         Attributes ON EntityTypeAttributes.AttributeId = Attributes.Id INNER JOIN
                         Type ON Attributes.DataTypeId = Type.Id
WHERE        (Attributes.Name LIKE '%Id') AND (Attributes.Name <> 'Id') AND (Type.Name = N'System.int32')) t
where [ConfigurationPropertyPresentation].Id = 8

insert into PropertyValueOption(Id, ValueOptionId)
SELECT DISTINCT EntityTypePresentationProperty.Id, ConfigurationPropertyPresentation.ValueOptionId
FROM            Attributes INNER JOIN
                         Type ON Attributes.DataTypeId = Type.Id INNER JOIN
                         EntityTypeAttributes ON Attributes.Id = EntityTypeAttributes.AttributeId INNER JOIN
                         EntityTypePresentationProperty INNER JOIN
                         ConfigurationPropertyPresentation ON EntityTypePresentationProperty.ViewPropertyPresentationPropertyTypeId = ConfigurationPropertyPresentation.ViewPropertyPresentationPropertyTypeId AND 
                         EntityTypePresentationProperty.ViewTypeId = ConfigurationPropertyPresentation.ViewTypeId ON EntityTypeAttributes.Id = EntityTypePresentationProperty.EntityTypeAttributeId
WHERE        (EntityTypePresentationProperty.ViewPropertyPresentationPropertyTypeId = 9) AND (EntityTypePresentationProperty.ViewTypeId = 2) AND (Type.Name = N'System.int32') AND (ConfigurationPropertyPresentation.Id = 8)

---------------- do checkboxes
insert into EntityTypePresentationProperty(ViewPropertyPresentationPropertyTypeId, PresentationThemeId,ViewTypeId,EntityTypeAttributeId)
SELECT        [ConfigurationPropertyPresentation].ViewPropertyPresentationPropertyTypeId, [ConfigurationPropertyPresentation].PresentationThemeId, 
                         [ConfigurationPropertyPresentation].ViewTypeId, t.Id
FROM            [ConfigurationPropertyPresentation] CROSS JOIN
                         (SELECT        EntityTypeAttributes.Id, Attributes.Name
FROM            EntityTypeAttributes INNER JOIN
                         Attributes ON EntityTypeAttributes.AttributeId = Attributes.Id INNER JOIN
                         Type ON Attributes.DataTypeId = Type.Id
WHERE        (Type.Name = N'bool')) t
where [ConfigurationPropertyPresentation].Id = 9


insert into PropertyValueOption(Id, ValueOptionId)
SELECT DISTINCT EntityTypePresentationProperty.Id, ConfigurationPropertyPresentation.ValueOptionId
FROM            Attributes INNER JOIN
                         Type ON Attributes.DataTypeId = Type.Id INNER JOIN
                         EntityTypeAttributes ON Attributes.Id = EntityTypeAttributes.AttributeId INNER JOIN
                         EntityTypePresentationProperty INNER JOIN
                         ConfigurationPropertyPresentation ON EntityTypePresentationProperty.ViewPropertyPresentationPropertyTypeId = ConfigurationPropertyPresentation.ViewPropertyPresentationPropertyTypeId AND 
                         EntityTypePresentationProperty.ViewTypeId = ConfigurationPropertyPresentation.ViewTypeId ON EntityTypeAttributes.Id = EntityTypePresentationProperty.EntityTypeAttributeId
WHERE        (EntityTypePresentationProperty.ViewPropertyPresentationPropertyTypeId = 9) AND (EntityTypePresentationProperty.ViewTypeId = 2) AND (Type.Name = N'bool') AND (ConfigurationPropertyPresentation.Id = 9)


