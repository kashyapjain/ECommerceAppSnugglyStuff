using SnugglyStuffMVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace SnugglyStuffMVC.Controllers
{
    public class WebApiController : Controller
    {
       [System.Web.Mvc.HttpPost]
        public HttpResponseMessage PlaceOrder([FromBody] Order order)
        {
            var response = new HttpRequestMessage();

            try
            {
                var Data = GetOrdersDataList();
                var MaxID = Data[0].ID;
                var ImageName = "Order_" + MaxID.ToString() + ".jpg";

                var bytes = Convert.FromBase64String(order.OrderedImage);
                var path = Path.Combine(Server.MapPath("~/Images/Orders/"), ImageName);

                System.IO.File.WriteAllBytes(path, bytes);

                using (SnugglyStuffEntities entities = new SnugglyStuffEntities())
                {
                    order.OrderedImage = ImageName;

                    entities.Orders.Add(order);
                    entities.SaveChanges();
                }

                return response.CreateResponse(System.Net.HttpStatusCode.Created);
            }
            catch (Exception e)
            {
                return response.CreateResponse(System.Net.HttpStatusCode.BadRequest, e.ToString());
            }
        }

        public List<Order> GetOrdersDataList()
        {
            var OrdersList = new SnugglyStuffEntities().Orders.SqlQuery("SELECT " +
                                          "convert(int,IDENT_CURRENT ('Orders')) as ID, " +
                                          "convert(int,IDENT_CURRENT ('Orders')) as ItemNo, " +
                                          "convert(int,IDENT_CURRENT ('Orders')) as UserID, " +
                                          "convert(datetime,IDENT_CURRENT ('Orders')) as DateAndTime, " +
                                          "convert(int,IDENT_CURRENT ('Orders')) as Quantity, " +
                                          "convert(varchar(MAX),IDENT_CURRENT('Orders')) as Status," +
                                          "convert(varchar(MAX),IDENT_CURRENT('Orders')) as OrderedImage")
                                          .ToList<Order>();

            return OrdersList;
        }
    }
}