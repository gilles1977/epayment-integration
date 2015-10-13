using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Utils
{
    public class LoggingModule : IHttpModule
    {
        private string requestInfo;

        public void Dispose()
        {
            
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += (src, args) =>
            {
                var ctx = HttpContext.Current;
                requestInfo = ctx.Request.Browser.Browser;
            };

            context.EndRequest += (src, args) =>
            {
                var ctx = HttpContext.Current;
                ctx.Response.Write(string.Format("<div>Browser used to request: {0}</div>", requestInfo));
            };
        }
    }
}
