using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace WebAPI.Controllers.Api
{
    public class DownAndPreviewController : ApiController
    {
        /// <summary>
        /// 先下载文件
        /// </summary>
        /// 
        public HttpResponseMessage GetFileFromWebApi(string fileName)
        {
            try
            {
                var FilePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/temp" + fileName);
                var stream = new FileStream(FilePath, FileMode.Open);
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(stream);
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                return response;
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }
        }
    }
}
