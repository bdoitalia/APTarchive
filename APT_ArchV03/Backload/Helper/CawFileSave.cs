using APT_ArchV03.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APT_ArchV03.Backload.Helper
{
    /*public class CawFileModel
    {        
        public string FileName { get; set; }
        public string objectContext { get; set; }
        public int? uploadContext { get; set; }
        public virtual ICollection<CawFile> CawFiles { get; set; }

        public CawFileModel(int uploadContext) {

        }
    }*/

    public class CawFileSave
    {
        private Db_APT_ArchEntities db;
        public CawFileSave()
        {
            db = new Db_APT_ArchEntities();
        }

        public void WriteFile(string filename, string partnername, int cawid ) {
            Caw caw = new Caw();
            caw = db.Caws.Find(cawid);
            CawFile cawFile = new CawFile();
            cawFile.cawfiles_fn = partnername + "\\" + filename;
            caw.CawFiles.Add(cawFile);
            db.SaveChanges();

        }

        public void DeleteFile(string filename, int cawid) {
            Caw caw = new Caw();
            caw = db.Caws.Find(cawid);
            CawFile cawFile = new CawFile();
            cawFile = caw.CawFiles.First(x=>x.cawfiles_fn.Contains(filename));
            db.CawFiles.Remove(cawFile);
            db.SaveChanges();
        }

    }
}