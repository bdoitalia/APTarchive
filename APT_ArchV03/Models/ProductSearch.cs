using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APT_ArchV03.Models
{
    public class ProductSearchModel
    {
        public int? Year { get; set; }
        public string Client { get; set; }
        public string Job { get; set; }
        public string Partner { get; set; }
        public string Manager { get; set; }
        public string Office { get; set; }
        public int? Status { get; set; }
        //public enum StatusValue
        //{
        //    Opened = 1,
        //    Reporting = 2,
        //    Closed = 3
        //}
        //StatusValue Status { get; set; }
    }

    public class ProductBusinessLogic
    {
        private Db_APT_ArchEntities Context;
        public ProductBusinessLogic()
        {
            Context = new Db_APT_ArchEntities();
        }

        public IQueryable<Caw> GetProducts(ProductSearchModel searchModel)
        {
            var result = Context.Caws.AsQueryable();
            if (searchModel != null)
            {
                if (searchModel.Year.HasValue)
                    result = result.Where(x => x.caw_stdate.Value.Year == searchModel.Year);
                if (!string.IsNullOrEmpty(searchModel.Client))
                    result = result.Where(x => x.caw_client.Contains(searchModel.Client));
                if (!string.IsNullOrEmpty(searchModel.Job))
                    result = result.Where(x => x.CawJobs.Select( y => y.cawjob_jc ).Contains(searchModel.Job));
                if (!string.IsNullOrEmpty(searchModel.Partner))
                    result = result.Where(x => x.caw_partner.Contains(searchModel.Partner));
                if (!string.IsNullOrEmpty(searchModel.Manager))
                    result = result.Where(x => x.caw_manager.Contains(searchModel.Manager));
                if (!string.IsNullOrEmpty(searchModel.Office))
                    result = result.Where(x => x.caw_office.Contains(searchModel.Office));
                if ( searchModel.Status >=1 && searchModel.Status <= 3)
                    result = result.Where(x => x.caw_status == searchModel.Status);
            }
            return result;
        }
    }
}