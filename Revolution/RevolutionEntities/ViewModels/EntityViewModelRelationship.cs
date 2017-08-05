using System;

namespace RevolutionEntities.ViewModels
{
    public class EntityViewModelRelationship
    {
        public Type ParentType { get; set; }
        public string ViewParentProperty { get; set; }
        public string ParentProperty { get; set; }
        public string ChildProperty { get; set; }
        public Type ChildType { get; set; }
        public string ViewChildProperty { get; set; }
    }
}