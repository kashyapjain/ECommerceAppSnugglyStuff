//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SnugglyStuffMVC.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class FireBaseToken
    {
        public int ID { get; set; }
        public Nullable<int> UserID { get; set; }
        public string Token { get; set; }
    
        public virtual tblUser tblUser { get; set; }
    }
}