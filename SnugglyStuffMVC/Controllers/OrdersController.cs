using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SnugglyStuffMVC.Models;
using SnugglyStuffMVC.Service;

namespace SnugglyStuffMVC.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private SnugglyStuffEntities db = new SnugglyStuffEntities();

        // GET: Orders
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.Item).Include(o => o.Item1).Include(o => o.tblUser).Include(o => o.tblUser1);
            return View(orders.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemNo = new SelectList(db.Items, "ID", "Description", order.ItemNo);
            ViewBag.ItemNo = new SelectList(db.Items, "ID", "Description", order.ItemNo);
            ViewBag.UserID = new SelectList(db.tblUsers, "ID", "Email", order.UserID);
            ViewBag.UserID = new SelectList(db.tblUsers, "ID", "Email", order.UserID);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ItemNo,UserID,DateAndTime,Quantity,Status,OrderedImage")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();

                var path = Server.MapPath("~/private_key.json");

                var res = NotificationService.SendNotification(order.UserID.ToString(), "Message",
                    "Order has been " + order.Status, path);

                return RedirectToAction("Index");
            }
            ViewBag.ItemNo = new SelectList(db.Items, "ID", "Description", order.ItemNo);
            ViewBag.ItemNo = new SelectList(db.Items, "ID", "Description", order.ItemNo);
            ViewBag.UserID = new SelectList(db.tblUsers, "ID", "Email", order.UserID);
            ViewBag.UserID = new SelectList(db.tblUsers, "ID", "Email", order.UserID);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
