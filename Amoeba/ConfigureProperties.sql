


SELECT        ChildEntityId
FROM            EntityRelationsView
WHERE        (Relationship = 'One-Many') AND (ChildEntity = 'BranchCurrentPayrollJob')

SELECT        Application, Id, Attribute, Type
FROM            EntityTypeAttributesView
WHERE        (Type = 'BranchCurrentPayrollJob')

---- Set Lookup Textblock for entities
delete from EntityTypePresentationProperty where ViewPropertyPresentationPropertyTypeId = 9

insert into EntityTypePresentationProperty(ViewPropertyPresentationPropertyTypeId, PresentationThemeId,ValueOptionId,ViewTypeId,EntityTypeAttributeId)
SELECT        [ConfigurationPropertyPresentation].ViewPropertyPresentationPropertyTypeId, [ConfigurationPropertyPresentation].PresentationThemeId, [ConfigurationPropertyPresentation].ValueOptionId, 
                         [ConfigurationPropertyPresentation].ViewTypeId, t.ChildEntityId
FROM            [ConfigurationPropertyPresentation] CROSS JOIN
                         (SELECT        ChildEntityId
FROM            EntityRelationsView
WHERE        (Relationship = 'One-Many')) t
where [ConfigurationPropertyPresentation].Id = 1

------- set ID Label invisible
insert into EntityTypePresentationProperty(ViewPropertyPresentationPropertyTypeId, PresentationThemeId,ValueOptionId,ViewTypeId,EntityTypeAttributeId)
SELECT        [ConfigurationPropertyPresentation].ViewPropertyPresentationPropertyTypeId, [ConfigurationPropertyPresentation].PresentationThemeId, [ConfigurationPropertyPresentation].ValueOptionId, 
                         [ConfigurationPropertyPresentation].ViewTypeId, t.Id
FROM            [ConfigurationPropertyPresentation] CROSS JOIN
                         (SELECT        EntityTypeAttributes.Id
FROM            EntityTypeAttributes INNER JOIN
                         Attributes ON EntityTypeAttributes.AttributeId = Attributes.Id
WHERE        (Attributes.Name = 'Id')) t
where [ConfigurationPropertyPresentation].Id = 2


------- set ID Label invisible
insert into EntityTypePresentationProperty(ViewPropertyPresentationPropertyTypeId, PresentationThemeId,ValueOptionId,ViewTypeId,EntityTypeAttributeId)
SELECT        [ConfigurationPropertyPresentation].ViewPropertyPresentationPropertyTypeId, [ConfigurationPropertyPresentation].PresentationThemeId, [ConfigurationPropertyPresentation].ValueOptionId, 
                         [ConfigurationPropertyPresentation].ViewTypeId, t.Id
FROM            [ConfigurationPropertyPresentation] CROSS JOIN
                         (SELECT        EntityTypeAttributes.Id
FROM            EntityTypeAttributes INNER JOIN
                         Attributes ON EntityTypeAttributes.AttributeId = Attributes.Id
WHERE        (Attributes.Name = 'Id')) t
where [ConfigurationPropertyPresentation].Id = 3

------- set ID Label invisible
insert into EntityTypePresentationProperty(ViewPropertyPresentationPropertyTypeId, PresentationThemeId,ValueOptionId,ViewTypeId,EntityTypeAttributeId)
SELECT        [ConfigurationPropertyPresentation].ViewPropertyPresentationPropertyTypeId, [ConfigurationPropertyPresentation].PresentationThemeId, [ConfigurationPropertyPresentation].ValueOptionId, 
                         [ConfigurationPropertyPresentation].ViewTypeId, t.Id
FROM            [ConfigurationPropertyPresentation] CROSS JOIN
                         (SELECT        EntityTypeAttributes.Id
FROM            EntityTypeAttributes INNER JOIN
                         Attributes ON EntityTypeAttributes.AttributeId = Attributes.Id
WHERE        (Attributes.Name = 'Id')) t
where [ConfigurationPropertyPresentation].Id = 4


------- set ID Label invisible
insert into EntityTypePresentationProperty(ViewPropertyPresentationPropertyTypeId, PresentationThemeId,ValueOptionId,ViewTypeId,EntityTypeAttributeId)
SELECT        [ConfigurationPropertyPresentation].ViewPropertyPresentationPropertyTypeId, [ConfigurationPropertyPresentation].PresentationThemeId, [ConfigurationPropertyPresentation].ValueOptionId, 
                         [ConfigurationPropertyPresentation].ViewTypeId, t.Id
FROM            [ConfigurationPropertyPresentation] CROSS JOIN
                         (SELECT        EntityTypeAttributes.Id
FROM            EntityTypeAttributes INNER JOIN
                         Attributes ON EntityTypeAttributes.AttributeId = Attributes.Id
WHERE        (Attributes.Name = 'Id')) t
where [ConfigurationPropertyPresentation].Id = 5

insert into EntityTypeAttributeCache(Id)
SELECT        EntityTypeAttributes.Id
FROM            EntityTypeAttributes INNER JOIN
                         Attributes ON EntityTypeAttributes.AttributeId = Attributes.Id
WHERE        (Attributes.Name like '%Name%')


------- set ID Label DisplayText
insert into EntityTypePresentationProperty(ViewPropertyPresentationPropertyTypeId, PresentationThemeId,ValueOptionId,ViewTypeId,EntityTypeAttributeId)
SELECT        [ConfigurationPropertyPresentation].ViewPropertyPresentationPropertyTypeId, [ConfigurationPropertyPresentation].PresentationThemeId, [ConfigurationPropertyPresentation].ValueOptionId, 
                         [ConfigurationPropertyPresentation].ViewTypeId, t.Id
FROM            [ConfigurationPropertyPresentation] CROSS JOIN
                         (SELECT        EntityTypeAttributes.Id, Attributes.Name
FROM            EntityTypeAttributes INNER JOIN
                         Attributes ON EntityTypeAttributes.AttributeId = Attributes.Id
WHERE        (Attributes.Name LIKE '%Id') AND (Attributes.Name <> 'Id')) t
where [ConfigurationPropertyPresentation].Id = 6


delete from PropertyValue where id in (SELECT      EntityTypePresentationProperty.Id
FROM            EntityTypePresentationProperty INNER JOIN
                         EntityTypeAttributes ON EntityTypePresentationProperty.EntityTypeAttributeId = EntityTypeAttributes.Id INNER JOIN
                         Attributes ON EntityTypeAttributes.AttributeId = Attributes.Id
WHERE        (EntityTypePresentationProperty.ViewPropertyPresentationPropertyTypeId = 10) )

insert into PropertyValue(Id, [Value])
SELECT      EntityTypePresentationProperty.Id,  dbo.SpaceBeforeCap(left(Attributes.Name, LEN(Attributes.Name)-2)) as displaytxt
FROM            EntityTypePresentationProperty INNER JOIN
                         EntityTypeAttributes ON EntityTypePresentationProperty.EntityTypeAttributeId = EntityTypeAttributes.Id INNER JOIN
                         Attributes ON EntityTypeAttributes.AttributeId = Attributes.Id
WHERE        (EntityTypePresentationProperty.ViewPropertyPresentationPropertyTypeId = 10) 


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


delete from PropertyValue where id in (SELECT      EntityTypePresentationProperty.Id
FROM            EntityTypePresentationProperty INNER JOIN
                         EntityTypeAttributes ON EntityTypePresentationProperty.EntityTypeAttributeId = EntityTypeAttributes.Id INNER JOIN
                         Attributes ON EntityTypeAttributes.AttributeId = Attributes.Id
WHERE        (EntityTypePresentationProperty.ViewPropertyPresentationPropertyTypeId = 10) and (Attributes.Name not like '%Id'))

insert into PropertyValue(Id, [Value])
SELECT      EntityTypePresentationProperty.Id,  dbo.SpaceBeforeCap(Attributes.Name) as displaytxt
FROM            EntityTypePresentationProperty INNER JOIN
                         EntityTypeAttributes ON EntityTypePresentationProperty.EntityTypeAttributeId = EntityTypeAttributes.Id INNER JOIN
                         Attributes ON EntityTypeAttributes.AttributeId = Attributes.Id
WHERE        (EntityTypePresentationProperty.ViewPropertyPresentationPropertyTypeId = 10) and (Attributes.Name not like '%Id')