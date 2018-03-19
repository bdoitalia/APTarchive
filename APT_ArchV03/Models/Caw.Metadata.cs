using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace APT_ArchV03.Models
{
    [MetadataType(typeof(CawMetaData))]
    public partial class Caw
    {
    }

    public class CawMetaData
    {
        [Required]
        [Display(Name = "Name")]
        public object caw_name;

        [Display(Name = "Client")]
        public object caw_client;

        [Display(Name = "Partner")]
        public object caw_partner;

        [Display(Name = "Manager")]
        public object caw_manager;

        [Display(Name = "Office")]
        public object caw_office;

        //[Required]
        [Display(Name = "Statement date")]
        [DataType(DataType.DateTime), Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public object caw_stdate;

        [Display(Name = "Report date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public object caw_reldate;

        [Display(Name = "Delivery date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public object caw_dldate;

        [Display(Name = "Archiving date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public object caw_archdate;

        

        [Display(Name = "Notes")]
        public object caw_notes;

        [Display(Name = "Status")]
        public object caw_status;

        [Display(Name = "Created by")]
        public object caw_usrcreator;

        [Display(Name = "Jobs")]
        public object CawJobs;

        [Display(Name = "Due days")]
        public object caw_delplan;

        [Display(Name = "Caw type")]
        public object caw_type;
    }
}