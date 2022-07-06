using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace SnugglyStuffWebApp.Security
{
    public class BasicAuthentication : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {

            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.
                    Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
            }

            else
            {
                string authenticationToken = 
                    actionContext.Request.Headers.Authorization.Parameter;

                string decodedToken = 
                    Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));

                if (TokenAuthentication.Check(decodedToken))
                {
                    Thread.CurrentPrincipal = 
                        new GenericPrincipal(new GenericIdentity(decodedToken), null);

                }
                else
                {
                    actionContext.Response = actionContext.
                    Request.CreateResponse(System.Net.HttpStatusCode.Unauthorized);
                }
            }
        }
    }
}