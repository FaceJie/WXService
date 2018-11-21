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
                CountryVisaInfoNeed modelView = GlobalFuc.JsonToModel<CountryVisaInfoNeed>(sData);
                CountryVisaInfoNeed model = new CountryVisaInfoNeed();
                model.needId = Guid.NewGuid().ToString("N");
                string ServerUrl = "https://" + base.HttpContext.Request.Url.Host;
                string needTableImgUrl = ServerUrl + "/temp/needTableImgUrl/" + model.needId + ".png";//申请表图片
                string needDemandImgUrl = ServerUrl + "/temp/needDemandImgUrl/" + model.needId + ".jpg";//需求表图片
                string needTableUrl = ServerUrl + "/temp/needTableUrl/" + model.needId + ".doc";//申请表
                string needDemandUrl = ServerUrl + "/temp/needDemandUrl/" + model.needId + ".doc";//需求表
                model.needTableName= "";
                model.needTableUrl = needTableUrl;
                model.needDemandName ="";
                model.needDemandUrl = needDemandUrl;
                model.needTableImgUrl = needTableImgUrl;
                model.needDemandImgUrl = needDemandImgUrl;
                model.needCountry = modelView.needCountry;
                model.needEnglishCountry = modelView.needEnglishCountry;
                if (bal.Save(model))
                {
                    var result = new
                    {
                        needId = model.needId
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            return Json("erorr", JsonRequestBehavior.AllowGet);
        }
        public JsonResult UploadFilesToServer()
        {
            string picID = "";
            string tempPIC_ID = Request.Form["needId"];
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
                    try
                    {
                        HttpPostedFileBase postFileBase = Request.Files["file"];
                        fileReallyName = postFileBase.FileName.Split('.')[0];
                        uploadStream = postFileBase.InputStream;
                        string Ext = Path.GetExtension(postFileBase.FileName);
                        string baseUrl = Server.MapPath("/");
                        switch (Ext)
                        {
                            case ".png":
                                uploadPath = baseUrl + @"\temp\needTableImgUrl\";
                                fileName = tempPIC_ID + ".png";
                                break;
                            case ".jpg":
                                uploadPath = baseUrl + @"\temp\needDemandImgUrl\";
                                fileName = tempPIC_ID + Ext;
                                
                                break;
                            case ".doc":
                                uploadPath = baseUrl + @"\temp\needTableUrl\";
                                fileName = tempPIC_ID + ".doc";
                                //needTableName
                                bal.SaveName(fileReallyName, tempPIC_ID, "needTableName");
                                break;
                            case ".docx":
                                uploadPath = baseUrl + @"\temp\needDemandUrl\";
                                fileName = tempPIC_ID + ".doc";
                                //needDemandName
                                bal.SaveName(fileReallyName, tempPIC_ID, "needDemandName");
                                break;
                            default:
                                picID = "error";
                                break;
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
    public class FilesUpViewModel
    {
        public string needId { get; set; }
        public string needName { get; set; }
        public string needType { get; set; }
        public string needUrl { get; set; }
        public string needImgUrl { get; set; }
        public string needCountyr { get; set; }
        public string needEnglishCountry { get; set; }
    }
}