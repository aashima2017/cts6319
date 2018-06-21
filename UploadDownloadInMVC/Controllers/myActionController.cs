using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace UploadDownloadInMVC.Controllers
{
    public class myActionController : Controller
    {
        


        public ActionResult Index()
        {

            //register user
            //Register();
            //to log actions of user
            var logger = NLog.LogManager.GetCurrentClassLogger();
            
            foreach (string upload in Request.Files)
            {
                if (Request.Files[upload].FileName != "")
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + "/App_Data/uploads/";
                    string filename = Path.GetFileName(Request.Files[upload].FileName);
                    Request.Files[upload].SaveAs(Path.Combine(path, filename));
                    ViewBag.Message = "Upload success";
                    logger.Info("file uploaded : " +filename);
                }
            }
            
            return View("Upload");
        }
        public ActionResult Downloads()
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            var dir = new System.IO.DirectoryInfo(Server.MapPath("~/App_Data/uploads/"));
            System.IO.FileInfo[] fileNames = dir.GetFiles("*.*"); List<string> items = new List<string>();
            foreach (var file in fileNames)
            {
                logger.Info("Getting file : " + file);
                items.Add(file.Name);
            }
            return View(items);
        }

        public FileResult Download(string ImageName)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            var FileVirtualPath = "~/App_Data/uploads/" + ImageName;
            ViewBag.Message = "Download success";
            logger.Info("Downloading file : " + ImageName);
            return File(FileVirtualPath, "application/force-download", Path.GetFileName(FileVirtualPath));
        }

        [HttpPost]
        public string Delete(string ImageName)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();
            string path = AppDomain.CurrentDomain.BaseDirectory + "/App_Data/uploads/" + ImageName;
            var FileVirtualPath = "~/App_Data/uploads/" + ImageName;
            FileInfo file = new FileInfo(path);
            if(file.Exists)
                file.Delete();
            logger.Info("Deleted file : " + ImageName);
            ViewBag.Message = "Delete success";
            return "ok"; 
        }
    }
}
