//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TDLA
{
    using System;
    using System.Collections.Generic;
    
    public partial class ToDoLists
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ToDoLists()
        {
            this.ToDoListShares = new HashSet<ToDoListShares>();
        }
    
        public int ID { get; set; }
        public string Title { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public int Creator { get; set; }
        public System.DateTime LastChange { get; set; }
        public int ChangedBy { get; set; }
    
        public virtual Users Users { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ToDoListShares> ToDoListShares { get; set; }
    }
}
