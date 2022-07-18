    using SnugglyStuffWebApp.Models;
using SnugglyStuffWebApp.Security;
using SnugglyStuffWebApp.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace SnugglyStuffWebApp.Controllers
{
    public class DataController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Reorder(string id)
        {
            try
            {
                var res = "";
                using (SnugglyStuffEntities entities = new SnugglyStuffEntities())
                {
                    var order = entities.Orders.Find(Convert.ToInt32(id));

                    Order ModifiedOrder = new Order
                    {
                        Status = "Recived",
                        DateAndTime = DateTime.Now,
                        ItemNo = order.ItemNo,
                        OrderedImage = order.OrderedImage,
                        Quantity = order.Quantity,
                        UserID = order.UserID, 
                    };

                    entities.Orders.Add(ModifiedOrder);
                    entities.SaveChanges();

                    var path = Request.RequestUri.GetLeftPart(UriPartial.Authority) +
                               Request.GetRequestContext().VirtualPathRoot;

                    res =NotificationService.SendNotification(ModifiedOrder.UserID.ToString(),"Reordered",
                        "Order Recived. Delivery Within 7 Days", path);
                }
                return Request.CreateResponse(HttpStatusCode.Created , res);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.ToString());
            }
        }
        public IEnumerable<Order> GetOrders(int id)
        {
            return new SnugglyStuffEntities().Orders.Where(o => o.UserID == id).ToList();
        }
        public List<Item> GetItems()
        {
            var data = new SnugglyStuffEntities().Items.ToList();

            return data;
        }
        public IEnumerable<Item> GetItems(int ID)
        {
            var data = new SnugglyStuffEntities().Items.Where(i => i.CategoryID==ID);

            return data;
        }
        public IEnumerable<Category> GetCategories()
        {
            var data = new SnugglyStuffEntities().Categories.ToList();

            return data;
        }
        public IEnumerable<Slider> GetSliders()
        {
            try
            {
                var data = new SnugglyStuffEntities().Sliders.ToList();

                return data;
            }
            catch(Exception ex)
            {
                return new List<Slider> { new Slider { Image = ex.ToString() } };
            }
        }

        [HttpPost]
        public HttpResponseMessage Register([FromBody] tblUser user)
        {
            try
            {
                using (SnugglyStuffEntities entities = new SnugglyStuffEntities())
                {
                    var NewUser = entities.tblUsers.Add(user);
                    entities.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.Created, NewUser.ID);
                }
            }
            catch(Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,e.ToString());
            }
        }

        [HttpGet]
        [BasicAuthentication]
        public tblUser GetUserDetails()
        {
            int id = Convert.ToInt32(Thread.CurrentPrincipal.Identity.Name);

            using(SnugglyStuffEntities entities = new SnugglyStuffEntities())
            {
                var user = entities.tblUsers.Find(id);

                return user;
            }
        }

        [HttpPost]
        public HttpResponseMessage Login([FromBody] FirebaseTokenModel ftm)
        {
            var user = new tblUser
            {
                Email = ftm.Email,
                Password = ftm.Password
            };
            using (SnugglyStuffEntities entities = new SnugglyStuffEntities())
            {
                if(entities.tblUsers.Any(u => u.Email.Equals(user.Email) &&
                                              u.Password.Equals(user.Password)))
                {
                    var id = entities.tblUsers
                        .Where<tblUser>(u => u.Email.Equals(user.Email))
                        .FirstOrDefault().ID;

                    entities.tblTokens.Add(new tblToken
                    { 
                        Token = id.ToString() 
                    });

                    var fbt = new FireBaseToken
                    {
                        UserID = id,
                        Token = ftm.Token,
                    };
                    entities.FireBaseTokens.Add(fbt);
                    entities.SaveChanges();

                    return Request.CreateResponse(HttpStatusCode.OK,id);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
        }
    }
}
