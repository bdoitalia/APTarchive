//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace APT_ArchV03.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class NavClient
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NavClient()
        {
            this.NavJobs = new HashSet<NavJob>();
        }
    
        public int ID { get; set; }
        public string Client_Name { get; set; }
        public string Client_ID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NavJob> NavJobs { get; set; }
    }
}
