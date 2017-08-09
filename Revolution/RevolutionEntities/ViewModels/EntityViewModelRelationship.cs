using System;

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
}