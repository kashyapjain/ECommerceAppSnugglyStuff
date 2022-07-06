using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SnugglyStuffMVC.Models;

namespace SnugglyStuffMVC.Controllers
{
    [Authorize]
    public class SlidersController : Controller
    {
        private SnugglyStuffEntities db = new SnugglyStuffEntities();

        // GET: Sliders
        public ActionResult Index()
        {
            var sliders = db.Sliders.Include(s => s.Category);
            return View(sliders.ToList());
        }

        // GET: Sliders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Sliders.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // GET: Sliders/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Image");
            return View();
        }

        // POST: Sliders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Slider slider, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                var Data = GetSilderDataList();

                int MaxId = Data[0].ID + 1;

                string ImageName = "Slider_" + MaxId + ".jpg";

                string path = Path.Combine(Server.MapPath("~/Images/Sliders/"), ImageName);

                if (file != null)
                {
                    file.SaveAs(path);
                }

                slider.Image = ImageName;

                db.Sliders.Add(slider);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Image", slider.CategoryID);
            return View(slider);
        }

        public List<Slider> GetSilderDataList()
        {
            var SlidersList = db.Sliders.SqlQuery("SELECT " +
                                          "convert(int,IDENT_CURRENT ('Sliders')) as ID, " +
                                          "convert(varchar(MAX),IDENT_CURRENT('Sliders')) as Image," +
                                          "convert(int,IDENT_CURRENT('Sliders')) as CategoryID")
                                          .ToList<Slider>();

            return SlidersList;
        }

        // GET: Sliders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Sliders.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Image", slider.CategoryID);
            return View(slider);
        }

        // POST: Sliders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Slider slider, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                var ImageName = "Slider_" + slider.ID + ".jpg";

                string path = Path.Combine(Server.MapPath("~/Images/Sliders/"), ImageName);

                slider.Image = ImageName;

                if (file != null)
                {
                    file.SaveAs(path);
                }

                db.Entry(slider).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Image", slider.CategoryID);
            return View(slider);
        }

        // GET: Sliders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Sliders.Find(id);
            if (slider == null)
            {
                return HttpNotFound();
            }
            return View(slider);
        }

        // POST: Sliders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Slider slider = db.Sliders.Find(id);
            db.Sliders.Remove(slider);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
