using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAPI.Models;
using WebBLL;
using WebHelper;
using WebModel;

namespace WebAPI.Controllers.LYService
{
    public class UploadFilesController : Controller
    {
        FileUploadBal bal = new FileUploadBal();
        // GET: UploadFiles
        public ActionResult Index()
        {
            return View();
        }
        //上传动态----------完成
        //上传文件和信息到服务器并保存到数据库，并获取上传的ID，次ID就就是上传图片的id名字
        /// <summary>
        /// FTP上传为何失败，https://blog.csdn.net/wulex/article/details/79008164
        /// </summary>
        /// <returns></returns>
        public string UploadFilesToServer(IEnumerable<HttpPostedFileBase> files, string userId)
        {
            MsgModel ret = new MsgModel();
            int leng = System.Text.Encoding.Default.GetBytes(bal.GroupIdExsit(userId).ToCharArray()).Length;
            if (leng < 32)
            {
                ret.scu = false;
                ret.msg = "暂无权限，请联系管理员为该用户设置团号！";
            }
            string uploadId = "";//从数据库带回来的记录id
            int bufferLen = 1024;
            byte[] buffer = new byte[bufferLen];
            int contentLen = 0;
            FileStream fs = null;//文件流用于写文件
            Stream uploadStream = null;//流的父类
            string fileName = "";//文件名
            string uploadPath = "";//上传路径
            string Ext = "";
            string url = "http://" + base.HttpContext.Request.Url.Host + ":" + base.HttpContext.Request.Url.Port;
            if (Request.Files.Count > 0)
            {
                foreach (HttpPostedFileBase postFileBase in files)
                {
                    if (postFileBase != null)
                    {
                        string uploadTempId = Guid.NewGuid().ToString("N");//临时的上传Id
                        try
                        {
                            uploadStream = postFileBase.InputStream;
                            Ext = Path.GetExtension(postFileBase.FileName);
                            fileName = uploadTempId + Ext;
                            uploadId = url + "/temp/images/" + uploadTempId + Ext + ";";
                            string baseUrl = Server.MapPath("/");
                            uploadPath = baseUrl + @"\temp\images\";
                            if (!Directory.Exists(uploadPath))  //没有目录就创建
                            {
                                Directory.CreateDirectory(uploadPath);
                            }
                            fs = new FileStream(uploadPath + fileName, FileMode.Create, FileAccess.ReadWrite);
                            while ((contentLen = uploadStream.Read(buffer, 0, bufferLen)) != 0)//上传流
                            {
                                fs.Write(buffer, 0, contentLen);
                                fs.Flush();
                            }
                            fs.Close();
                            FileStream fsPic = new FileStream(uploadPath + fileName, FileMode.Open, FileAccess.ReadWrite);
                            byte[] pic = new byte[fsPic.Length];
                            fsPic.Read(pic, 0, pic.Length);
                            fsPic.Close();
                        }
                        catch (Exception ex)
                        {
                            ret.scu = false;
                            ret.msg = ex.ToString();
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
                ret.scu = true;
                ret.msg = uploadId;
            }
            else
            {
                ret.scu = false;
                ret.msg = "上传参数错误！";
            }
            return GlobalFuc.ModelToJson<MsgModel>(ret);
        }
        //回显图片，限制当前用户可见
        //[HttpPost]
        /// <summary>
        /// 测试地址
        /// http://localhost:11775/UploadFiles/ShowImages?workId=b97d6e11c2954b6bb2cb15e6b250b055
        /// </summary>
        /// <param name="workId"></param>
        /// <returns></returns>

        public string ShowImages(string workId)
        {
            string json = "0";
            DataTable dt = new DataTable();
            ListViewModel lv = new ListViewModel();
            dt.Columns.AddRange(new DataColumn[] {
                    new DataColumn("userName", typeof(string)),
                    new DataColumn("remark", typeof(string)),
                    new DataColumn("uploadId", typeof(string)),
                    new DataColumn("uploadTime", typeof(string)),
                    new DataColumn("display", typeof(Boolean)),
                });
            string url = "http://" + base.HttpContext.Request.Url.Host + ":" + base.HttpContext.Request.Url.Port;
            string strImageUrl = "";
            string[] file_fullname;
            DataTable dt_id = bal.GetUploadId(workId);
            if (dt_id != null && dt_id.Rows.Count > 0)
            {
                string[] str = dt_id.Rows[0]["uploadId"].ToString().Split(new char[] { ',' });//得到uploadId字符串
                for (int i = 0; i < str.Length - 1; i++)
                {
                    file_fullname = str[i].Split(new char[] { '.' });//得到文件全名称
                    strImageUrl = url + "/temp/images/" + file_fullname[0] + "." + file_fullname[1];
                    dt.Rows.Add(new object[] { dt_id.Rows[0]["userName"], dt_id.Rows[0]["remark"], strImageUrl, dt_id.Rows[0]["uploadTime"], Convert.ToBoolean(dt_id.Rows[0]["display"]) });
                }
            }
            lv.uploadId = workId;
            lv.dt = dt;
            if (lv.dt.Rows.Count > 0)
            {
                json = GlobalFuc.ModelToJson<ListViewModel>(lv);
            }
            return json;
        }


        public string SaveData(string userId, string uploadId, string remark, string uploadTitle, int uploadType)
        {
            MsgModel ret = new MsgModel();
            DataTable dtheader = bal.GetUserInfo(userId);
            UploadFilesData uploadFilesData = new UploadFilesData();
            uploadFilesData.UserName = dtheader.Rows[0]["userName"].ToString();
            uploadFilesData.UserHeader = dtheader.Rows[0]["userHeader"].ToString();
            uploadFilesData.Remark = remark;
            uploadFilesData.UploadId = uploadId;
            uploadFilesData.GroupId = dtheader.Rows[0]["groupId"].ToString();
            uploadFilesData.UploadType = Convert.ToInt32(uploadType);//代表动态类型
            uploadFilesData.UploadTime = DateTime.Now;
            uploadFilesData.Display = false;
            uploadFilesData.UploadTitle = uploadTitle;
            int result = bal.UploadImgSave(uploadFilesData);
            if (result > 0)
            {
                ret.scu = true;
                ret.msg = "文件上传成功！";
            }
            else
            {
                ret.scu = false;
                ret.msg = "文件上传失败！";
            }
            return GlobalFuc.ModelToJson<MsgModel>(ret);
        }

    }
}