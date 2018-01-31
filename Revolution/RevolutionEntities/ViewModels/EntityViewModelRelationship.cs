using System.Collections.Generic;
using SystemInterfaces;
using GenSoft.Entities;

namespace RevolutionEntities.ViewModels
{
    public class EntityViewModelRelationship
    {
        public string ParentType { get; set; }
        public string ViewParentProperty { get; set; }
        public string ParentProperty { get; set; }
        public string ChildProperty { get; set; }
        public string ChildType { get; set; }
        public string ViewChildProperty { get; set; }
    }

    public class EntityTypeViewModel
    {
        public ISystemProcess SystemProcess { get; set; }
        public string EntityTypeName { get; set; }
        public string Symbol { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public List<EntityViewModelRelationship> EntityViewModelRelationships { get; set; }
        public List<EntityTypeViewModelCommand> EntityTypeViewModelCommands { get; set; }
        public string ViewModelTypeName { get; set; }
        public EntityRelationshipOrdinality RelationshipOrdinality { get; set; }
    }
}