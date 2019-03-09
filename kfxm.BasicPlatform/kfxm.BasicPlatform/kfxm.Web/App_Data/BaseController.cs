using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

    public class BaseController : Controller
    {
        protected HttpRequestBase Request
        {

            get { return base.HttpContext.Request; }
            private set;
        }

        protected HttpResponseBase Response 
        {
            get { return base.HttpContext.Response;}
            private set;
        }

    }
