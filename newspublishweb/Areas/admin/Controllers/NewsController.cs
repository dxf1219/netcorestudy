using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using newspublish.mode;
using newspublish.mode.Request;
using newspublish.mode.Response;
using newspublish.service;

namespace newspublishweb.Areas.admin.Controllers
{
    [Area("admin")]
    public class NewsController : Controller
    {

        private NewsService _newsService;
        private IHostingEnvironment _host;
        public NewsController(NewsService newsService,  IHostingEnvironment host)
        {
            this._newsService = newsService;
            _host = host;
        }
        public IActionResult Index()
        {
            var newsclssifys = _newsService.GetNewsClassifyList();
            return View(newsclssifys);
        }

        public IActionResult NewsAdd()
        {
            var newsclssifys = _newsService.GetNewsClassifyList();
            return View(newsclssifys);
        }

        [HttpGet]
        /// pageindex 页码 pagesize 每页显示条目数  classifyid 新闻类别的id newstitle 新闻标题
        public JsonResult GetNews(int pageindex ,int pagesize,int classifyid,string newstitle)
        {
            List<Expression<Func<News, bool>>> expressions = new List<Expression<Func<News, bool>>>();
            if (classifyid >0)
            {
                expressions.Add(c => c.NewsClassifyId == classifyid);
            }
            if (!string.IsNullOrEmpty(newstitle))
            {
                expressions.Add(c=>c.Title.Contains(newstitle));
            }
            int total = 0;
            var news = _newsService.NewsPageQuery(pagesize, pageindex, out total, expressions);
            return Json(new{ total = total,data=news.data });

        }

        #region 新闻类别管理
        public IActionResult NewsClassify()
        {
            var results=  _newsService.GetNewsClassifyList();
            return View(results);
        }

        public IActionResult NewsClassifyAdd()
        {
           
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> AddNews(AddNews addnews, IFormCollection collection)
        {
            if( (string.IsNullOrEmpty(addnews.Title)) || (string.IsNullOrEmpty(addnews.Contents)) )
            {
                return  Json(new ResponseModel { Code = -200, Result = "标题和内容不能为空" });
            }
            var file = collection.Files;
            if (file.Count > 0)
            {
                //上传图片
                var webrootpath = _host.WebRootPath;
                string relativeDirPath = "\\NewsPic";
                string absolutepath = webrootpath + relativeDirPath;

                string[] filetypes = new string[] { ".gif", ".jpg", ".png","jpeg" };

                string extension = Path.GetExtension(file[0].FileName);
                if (filetypes.Contains(extension.ToLower()))
                {
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

                    addnews.Image = "/NewsPic/" + fileName;
                }
            }
            else
                addnews.Image = "";
            addnews.PublishDate = DateTime.Now;
            return Json(_newsService.AddNews(addnews));
        }

        [HttpPost]
        public JsonResult DelNews(int id)
        {
            if (id < 0)
            {
                return Json(new ResponseModel { Code = -200, Result = "新闻id不能为空!" });
            }
            else
                return Json(_newsService.DelOneNews(id));
        }
        [HttpPost]
        public JsonResult AddNewsClassify(AddNewsClassify addNewsClassify)
        {
            if(string.IsNullOrEmpty(addNewsClassify.Name))
            {
                return Json(new ResponseModel {Code=-200,Result="新闻类别不为空!"});
            }
            else
            {
                return Json(_newsService.AddNewsClassifyEntity(addNewsClassify));
            }
        }

        public IActionResult NewsClassifyEdit(int id)
        {
            return View(_newsService.GetOneNewsclassify(id));
        }

        [HttpPost]
        public JsonResult EditNewsClassify(EditNewsClassify editNewsClassify)
        {
            if (string.IsNullOrEmpty(editNewsClassify.Name))
            {
                return Json(new ResponseModel { Code = -200, Result = "编辑时信息不能为空!" });
            }
            else
            {
                return Json(_newsService.UpdateNewsclassify(editNewsClassify));
            }
        }
        #endregion
    }
}