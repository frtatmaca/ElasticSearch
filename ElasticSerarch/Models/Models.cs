using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nest;

namespace ElasticSerarch
{
    public class Post
    {
        public int UserID { get; set; }
        public DateTime PostDate { get; set; }
        public string PostTest { get; set; }
    }  
}