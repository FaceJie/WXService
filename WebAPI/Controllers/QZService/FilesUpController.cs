using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBLL;
using WebHelper;
using WebModel;

namespace WebAPI.Controllers.QZService
{
    public class FilesUpController : Controller
    {
        FilesUpBal bal = new FilesUpBal();
        // GET: FilesUp
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult SavaDataToSql()
        {
            string sData = Request.Params.Get("data");
            if (!string.IsNullOrEmpty(sData))
            {
                FilesUpViewModel modelView = GlobalFuc.JsonToModel<FilesUpViewModel>(sData);
                modelView.needId = Guid.NewGuid().ToString("N");
                if (bal.Save(modelView))
                {
                    var result = new
                    {
                        needId = modelView.needId
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            return Json("erorr", JsonRequestBehavior.AllowGet);
        }
        public JsonResult UploadFilesToServer()
        {
            string picID = "";
            //上传参数
            string needId = Request.Form["needId"];
            string ServerUrl = "https://" + base.HttpContext.Request.Url.Host+":"+ base.HttpContext.Request.Url.Port;
            FilesUpViewModel filesUpViewModel;
            //流文件
            int bufferLen = 1024;
            byte[] buffer = new byte[bufferLen];
            int contentLen = 0;
            FileStream fs = null;
            Stream uploadStream = null;
            string fileName = "";
            string uploadPath = "";
            string fileReallyName = "";
            if (Request.Files.Count > 0)
            {
                if (Request.Files["file"] != null)
                {
                    filesUpViewModel=new FilesUpViewModel();
                    filesUpViewModel.needId = needId;
                    try
                    {
                        HttpPostedFileBase postFileBase = Request.Files["file"];
                        fileReallyName = postFileBase.FileName.Split('.')[0];
                        uploadStream = postFileBase.InputStream;
                        string Ext = Path.GetExtension(postFileBase.FileName);
                        string baseUrl = Server.MapPath("/");
                        if (Ext== ".png"|| Ext == ".jpg")
                        {
                            uploadPath = baseUrl + @"\temp\images\";
                            fileName = needId + Ext;

                           
                            filesUpViewModel.fileImg= ServerUrl + "/temp/images/" + fileName;
                        }
                        else if (Ext == ".docx" || Ext == ".doc"|| Ext == ".pdf")
                        {
                            uploadPath = baseUrl + @"\temp\files\";
                            fileName = fileReallyName + Ext;

                            filesUpViewModel.fileType = Ext.TrimStart('.');
                            filesUpViewModel.fileName = fileReallyName;
                            filesUpViewModel.fileUrl = ServerUrl + "/temp/files/" + fileName;
                        }
                        else
                        {
                            picID = "error";
                        }
                      
                        if (!Directory.Exists(uploadPath))
                        {
                            Directory.CreateDirectory(uploadPath);
                        }
                        fs = new FileStream(uploadPath + fileName, FileMode.Create, FileAccess.ReadWrite);
                        while ((contentLen = uploadStream.Read(buffer, 0, bufferLen)) != 0)
                        {
                            fs.Write(buffer, 0, contentLen);
                            fs.Flush();
                        }
                        fs.Close();
                        FileStream fsPic = new FileStream(uploadPath + fileName, FileMode.Open, FileAccess.ReadWrite);
                        byte[] pic = new byte[fsPic.Length];
                        fsPic.Read(pic, 0, pic.Length);
                        //保存
                        if (filesUpViewModel!=null)
                        {
                            bal.SaveName(filesUpViewModel);
                        }
                        fsPic.Close();
                        picID = "scu";
                    }
                    catch (Exception ex)
                    {
                        picID = "error";
                    }
                    finally
                    {
                        if (null != fs)
                        {
                            fs.Close();
                        }
                        if (null != uploadStream)
                        {
                            uploadStream.Close();
                        }

                    }

                }
            }
            return Json(picID,JsonRequestBehavior.AllowGet);
        }


        public string GetVisaNeedInfo()
        {
            string json = "";
            DataTable dt = bal.GetVisaNeedInfo();
            if (dt!=null&&dt.Rows.Count>0)
            {
                json =DatatableToJson.Dtb2Json(dt);
            }
            return json;
        }

        public string GetAllVisaNeed()
        {
            string json = "";
            DataTable dt = bal.GetVisaNeedInfo();
            if (dt != null && dt.Rows.Count > 0)
            {
                json = DatatableToJson.Dtb2Json(dt);
            }
            return json;
        }


        public string GetCountry()
        {
            string json = "";
            DataTable dt = bal.GetCountry();
            if (dt != null && dt.Rows.Count > 0)
            {
                json = DatatableToJson.Dtb2Json(dt);
            }
            return json;
        }
    }
    
}