using APT_ArchV03.Models;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;

namespace APT_ArchV03.Helpers
{
    public class ADUser {

        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName {
            get
            {
                return FirstName + " " + LastName;
            }
        }
        public string Email { get; set; }
        public string Office { get; set; }

        public void PopulateADUser(ADUser user)
        {
            try
            {                
                string DomainPath = "LDAP://OU=BDOItalia,DC=bdo,DC=priv";
                DirectoryEntry searchRoot = new DirectoryEntry(DomainPath);
                DirectorySearcher search = new DirectorySearcher(searchRoot);

                var attributeName = "samaccountname";
                var searchString = "*" + user.UserName + "*";

                search.Filter = string.Format("(&(objectcategory=user)({0}={1}))", attributeName, searchString);

                search.PropertiesToLoad.Add("samaccountname");
                search.PropertiesToLoad.Add("givenName");
                search.PropertiesToLoad.Add("sn");
                search.PropertiesToLoad.Add("mail");
                search.PropertiesToLoad.Add("l");

                SearchResult result;

                SearchResultCollection resultCol = search.FindAll();

                if (resultCol != null)
                {
                    for (int counter = 0; counter < resultCol.Count; counter++)
                    {
                        string UserNameEmailString = string.Empty;
                        result = resultCol[counter];

                        if (result.Properties.Contains("samaccountname"))
                        {
                            if (result.Properties["givenName"].Count > 0)
                            {
                                user.FirstName = result.Properties["givenName"][0].ToString();
                            }
                            if (result.Properties["sn"].Count > 0)
                            {
                                user.LastName = result.Properties["sn"][0].ToString();
                            }
                            if (result.Properties["mail"].Count > 0)
                            {
                                user.Email = result.Properties["mail"][0].ToString();
                            }
                            if (result.Properties["l"].Count > 0)
                            {
                                user.Office = result.Properties["l"][0].ToString();
                            }
                        }

                    }

                }

            }
            catch (System.Exception ex)
            {
                string message = ex.Message;
               
                throw;
            }
        }
    }    
}