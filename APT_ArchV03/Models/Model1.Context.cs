﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Db_APT_ArchEntities : DbContext
    {
        public Db_APT_ArchEntities()
            : base("name=Db_APT_ArchEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Caw> Caws { get; set; }
        public virtual DbSet<CawJob> CawJobs { get; set; }
        public virtual DbSet<NavClient> NavClients { get; set; }
        public virtual DbSet<NavJob> NavJobs { get; set; }
        public virtual DbSet<CawFile> CawFiles { get; set; }
        public virtual DbSet<NavResource> NavResources { get; set; }
        public virtual DbSet<CityCode> CityCodes { get; set; }
        public virtual DbSet<VNavJob> VNavJobs { get; set; }
        public virtual DbSet<VNavResource> VNavResources { get; set; }
        public virtual DbSet<VNavClient> VNavClients { get; set; }
    }
}
