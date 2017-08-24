using System;
using SystemInterfaces;
using GenSoft.Interfaces;

namespace RevolutionEntities.ViewModels
{
    public class ViewModelEntity
    {
        public ViewModelEntity() { }

        public ViewModelEntity(IDynamicEntityType entityType, string viewProperty, string property)
        {
            EntityType = entityType;
            ViewProperty = viewProperty;
            Property = property;
        }

        public IDynamicEntityType EntityType { get; set; }
        public string ViewProperty { get; set; }
        public string Property { get; set; }

    }
}