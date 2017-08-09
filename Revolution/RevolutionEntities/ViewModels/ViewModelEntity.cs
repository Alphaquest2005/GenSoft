using System;
using GenSoft.Interfaces;

namespace RevolutionEntities.ViewModels
{
    public class ViewModelEntity
    {
        public ViewModelEntity() { }

        public ViewModelEntity(string entityType, string viewProperty, string property)
        {
            EntityType = entityType;
            ViewProperty = viewProperty;
            Property = property;
        }

        public string EntityType { get; set; }
        public string ViewProperty { get; set; }
        public string Property { get; set; }

    }
}