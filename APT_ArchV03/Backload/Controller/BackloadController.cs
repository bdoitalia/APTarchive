using Backload.Contracts.Context;
using Backload.Contracts.FileHandler;
using Backload.Context.DataProvider;
using Backload.Helper;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using APT_ArchV03.Models;
using APT_ArchV03.Backload.Helper;
using System;

namespace Backload.Controllers
{

    /// <summary>
    /// The integrated controller to handle file requests. 
    /// You can remove this code, if you have a custom controller or handler.
    /// </summary>
    public partial class BackloadController : Controller
    {
        
        /// <summary>
        /// The Backload file handler. 
        /// To access it in an Javascript ajax request use: <code>var url = "/{Application}/Backload/FileHandler/";</code>.
        /// </summary>
        [AcceptVerbs(HttpVerbs.Get|HttpVerbs.Post|HttpVerbs.Put|HttpVerbs.Delete|HttpVerbs.Options)]
        public async Task<ActionResult> FileHandler()
        {
            try
            {

                // Create and initialize the handler
                IFileHandler handler = Backload.FileHandler.Create();
                handler.Init(HttpContext.Request);

                // Call the execution pipeline and get the result
                IBackloadResult result = await handler.Execute();


                var provider = new BackloadDataProvider(HttpContext.Request);
                
                if (HttpContext.Request.HttpMethod == "POST")
                {
                    //var filename = provider.Files[0].FileName;
                    //var ext = System.IO.Path.GetExtension(filename);
                    //var pureName = System.IO.Path.GetFileNameWithoutExtension(filename);

                    foreach (var file in handler.FileStatus.Files)
                    {

                        if (handler.Context.Chunked)
                        {
                            if (file.StorageInfo.ChunkInfo.ChunksComplete)
                            {

                                //start file data saving procedure
                                var FileSave = new CawFileSave();
                                FileSave.WriteFile(file.FileName, file.ObjectContext, Convert.ToInt32(file.UploadContext));

                            }
                        }
                        else
                        {

                            //Start File data saving procedure

                        }

                    }

                                        
                }
                if (HttpContext.Request.HttpMethod == "DELETE")
                {
                    var FileRemove = new CawFileSave();
                    FileRemove.DeleteFile(handler.FileStatus.Files[0].FileName, Convert.ToInt32(handler.FileStatus.Files[0].UploadContext));
                }
                //handler.Init(provider);

                

                // Helper to create an ActionResult object from the IBackloadResult instance
                return ResultCreator.Create(result);

            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }
    }
}
