using SnugglyStuffWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SnugglyStuffWebApp.Security
{
    public class TokenAuthentication
    {
        public static bool Check(string token)
        {
            using(SnugglyStuffEntities entities = new SnugglyStuffEntities())
            {
                if(entities.tblTokens.Any(i => i.Token.Equals(token)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}