using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using SnugglyStuffWebApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Web;

namespace SnugglyStuffWebApp.Service
{
    public class NotificationService
    {
        public static string SendNotification(string userID,string title,string body,string path)
        {
            try
            {
                int userid = Convert.ToInt32(userID);
                var TokensList = new List<FireBaseToken>();
                //Get User Token From FirebaseTokens Table Using UserID
                using (SnugglyStuffEntities entities = new SnugglyStuffEntities())
                {
                    TokensList = entities.FireBaseTokens.SqlQuery("select * from FireBaseTokens " +
                                                                  "where UserID ="+userID).ToList();
                }
                var MaxID = TokensList.Max(i => i.ID);
                var UserToken = TokensList.Find(i => i.ID == MaxID).Token;

                path = path + "/private_key.json";

                var res = new HttpClient().GetAsync(path).Result;
                var bytes = res.Content.ReadAsByteArrayAsync().Result;

                Stream stream = new MemoryStream(bytes);


                if (FirebaseApp.DefaultInstance == null)
                {
                    FirebaseApp.Create(new AppOptions()
                    {
                        //"D:\\FreeLance\\SnugglyStuffApp\\SnugglyStuff\\SnugglyStuffWebApp\\private_key.json"
                        Credential = GoogleCredential.FromStream(stream)
                    });
                }

                // This registration token comes from the client FCM SDKs.
                var registrationToken = UserToken;

                // See documentation on defining a message payload.
                var message = new Message()
                {
                    Token = registrationToken,
                    Notification = new Notification()
                    {
                        Title = title,
                        Body = body
                    }
                };

                // Send a message to the device corresponding to the provided
                // registration token.

                string response = FirebaseMessaging.DefaultInstance.SendAsync(message).Result;

                return response;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}