using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using newspublish.mode.Request;
using newspublish.mode.Response;
using newspublish.service;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace newspublishweb.Areas.admin.Controllers
{
    [Area("admin")]
    public class BannerController : Controller
    {
        private BannerService _bannerService;
        private IHostingEnvironment _host;
        public BannerController(BannerService bannerService, IHostingEnvironment host)
        {
            _bannerService = bannerService;
            _host = host;
        }
        // GET: Banner
        public ActionResult Index()
        {
            var banner = _bannerService.GetBannelList();
            //DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return View(banner);
        }
        [HttpPost]
        public async Task<JsonResult>AddBanner(AddBanner banner,IFormCollection collection)
        {
            var file = collection.Files;
            if (file.Count > 0)
            {
                //上传图片
                var webrootpath = _host.WebRootPath;
                string relativeDirPath = "\\BannerPic";
                string absolutepath = webrootpath + relativeDirPath;

                string [] filetypes = new string[] { ".gif", ".jpg", ".png" };

                string extension = Path.GetExtension(file[0].FileName);
                if (filetypes.Contains(extension.ToLower()))
                {
                    //
                    if (!Directory.Exists(absolutepath))
                    {
                        Directory.CreateDirectory(absolutepath);
                    }

                    string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    var filepath = absolutepath + "\\" + fileName;
                    using (var stream = new FileStream(filepath, FileMode.Create))
                    {
                        await file[0].CopyToAsync(stream);
                    }

                    banner.Image = "/BannerPic/"+ fileName;
                    return Json(_bannerService.AddBannerEntity(banner));
                }
                else
                    return Json(new ResponseModel { Code = -200, Result = "图片格式有误！" });
            }
            else
                return Json(new ResponseModel { Code = -200, Result = "请上传图片文件！" });
        }

        [HttpPost]
        public JsonResult DelBanner(int id)
        {
            if (id <=0)
                return Json(new ResponseModel { Code = -200, Result = "参数id不正确！" });

            return Json(_bannerService.DelBannerEntity(id));
        }

        public ActionResult BannerAdd()
        {

            return View();
        }


    }
}