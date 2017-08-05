using System;

namespace RevolutionEntities.ViewModels
{
    public class ViewModelEntity
    {
        public ViewModelEntity() { }

        public ViewModelEntity(Type entityType, string viewProperty, string property)
        {
            EntityType = entityType;
            ViewProperty = viewProperty;
            Property = property;
        }

        public Type EntityType { get; set; }
        public string ViewProperty { get; set; }
        public string Property { get; set; }

    }
}