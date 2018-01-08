using MVCFileUploadSample.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCFileUploadSample.Controllers
{
    public class UploadController : Controller
    {
        ProjectDB mydb = new ProjectDB();

        // GET: Upload
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            var data = mydb.UploadTables.Find(id);

            System.IO.File.Delete(HttpContext.Server.MapPath(data.Url)); // dosyadan sil

            mydb.UploadTables.Remove(data);
            mydb.SaveChanges(); // db den sil

            return Json("OK");
        }

        //resim okuma
        public ActionResult GetImages()
        {
            var data = mydb.UploadTables.ToList();

            return View(data);
        }

        //HttpPostedFileBase tipinde upload edilen dosyalar gelir, video,resim vs dosya

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase myfile)
        {
            var filename = Guid.NewGuid().ToString().Replace("-","").Substring(0,10) + Path.GetExtension(myfile.FileName);

            //HttpContext.Server.MapPath => proje dizini

            myfile.SaveAs(HttpContext.Server.MapPath("~/img/" + filename));


            using (ProjectDB db = new ProjectDB())
            {
                db.UploadTables.Add(new UploadTable { Url = "/img/" + filename });
                db.SaveChanges();
            }

            return View();
        }

        [HttpPost]
        public ActionResult Multiple(IEnumerable<HttpPostedFileBase> myfiles)
        {
            return View();
        }
    }
}