//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace T4Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class EntityProperty : BaseEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EntityProperty()
        {
            this.DataProperties = new HashSet<DataProperty>();
            this.PresentationProperties = new HashSet<PresentationProperty>();
            this.TestValues = new HashSet<TestValue>();
            this.EntityViewProperties = new HashSet<EntityViewProperty>();
            this.ParentRelationships = new HashSet<EntityRelationship>();
            this.ChildRelationships = new HashSet<EntityRelationship>();
        }
    
        public int EntityId { get; set; }
        public string PropertyName { get; set; }
    
        public virtual Entity Entity { get; set; }
        public virtual PresentationProperty PresentationProperty { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DataProperty> DataProperties { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PresentationProperty> PresentationProperties { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TestValue> TestValues { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EntityViewProperty> EntityViewProperties { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EntityRelationship> ParentRelationships { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EntityRelationship> ChildRelationships { get; set; }
    }
}
