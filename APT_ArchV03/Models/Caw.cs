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
    
    public partial class Caw
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Caw()
        {
            this.CawJobs = new HashSet<CawJob>();
            this.CawFiles = new HashSet<CawFile>();
        }
    
        public int caw_id { get; set; }
        public string caw_name { get; set; }
        public string caw_client { get; set; }
        public string caw_partner { get; set; }
        public string caw_manager { get; set; }
        public string caw_office { get; set; }
        public Nullable<System.DateTime> caw_stdate { get; set; }
        public Nullable<System.DateTime> caw_reldate { get; set; }
        public Nullable<System.DateTime> caw_dldate { get; set; }
        public Nullable<System.DateTime> caw_archdate { get; set; }
        public string caw_notes { get; set; }
        public Nullable<int> caw_status { get; set; }
        public Nullable<System.DateTime> caw_crdate { get; set; }
        public string caw_usrcreator { get; set; }
        public string caw_client_code { get; set; }
        public string caw_partner_code { get; set; }
        public string caw_manager_code { get; set; }
        public string caw_usrcreator_code { get; set; }
        public Nullable<int> caw_delplan { get; set; }
        public string caw_type { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CawJob> CawJobs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CawFile> CawFiles { get; set; }
    }
}
