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
using SnugglyStuffMVC.ViewModel;

namespace SnugglyStuffMVC.Controllers
{
    public class ItemsController : Controller
    {
        private SnugglyStuffEntities db = new SnugglyStuffEntities();

        // GET: Items
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Items.ToList());
        }

        // GET: Items/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: Items/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Items/Create

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Item item, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                var test = GetItemsList();

                int MaxId = test[0].ID + 1;

                string ImageName = MaxId + ".jpg";

                string path = Path.Combine(Server.MapPath("~/Images/"), ImageName);

                if (file != null)
                {
                    file.SaveAs(path);
                }

                item.ImageSrc = ImageName;

                db.Items.Add(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(item);
        }

        public List<Item> GetItemsList()
        {
            var ItemsList = db.Items.SqlQuery("SELECT " +
                                          "convert(int,IDENT_CURRENT ('Items')) as ID, " +
                                          "convert(int,IDENT_CURRENT ('Items')) as CategoryID,"+
                                          "convert(varchar(MAX),IDENT_CURRENT('Items')) as Description," +
                                          "convert(int,IDENT_CURRENT('Items')) as Price," +
                                          "convert(int,IDENT_CURRENT('Items')) as Quantity," +
                                          "convert(varchar(MAX),IDENT_CURRENT('Items')) as ImageSrc")
                                          .ToList<Item>();

            return ItemsList;
        }

        // GET: Items/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            return View(item);
        }

        // POST: Items/Edit/5

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Item item, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                string ImageName = item.ID.ToString() + ".jpg";

                item.ImageSrc = ImageName;

                string path = Path.Combine(Server.MapPath("~/Images/"), ImageName);

                if (file != null)
                {
                    file.SaveAs(path);
                }

                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(item);
        }

        // GET: Items/Delete/5

        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5

        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Items.Find(id);

            try
            {
                var _Orders = db.Orders.Where(o => o.ItemNo==item.ID);

                if (_Orders.Any())
                {
                    foreach (var order in _Orders)
                    {
                        db.Orders.Remove(order);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            db.Items.Remove(item);
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
