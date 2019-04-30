using Microsoft.EntityFrameworkCore;
using newspublish.mode;
using newspublish.mode.Entity;
using newspublish.mode.Request;
using newspublish.mode.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace newspublish.service
{
    
    public class NewsService
    {
        private Db _db;
        public NewsService(Db db)
        {
            this._db = db;
        }

        /// <summary>
        /// 添加新闻类别服务
        /// </summary>
        /// <param name="addNewsClassify"></param>
        /// <returns></returns>
        public ResponseModel AddNewsClassifyEntity(AddNewsClassify addNewsClassify)
        {
            var exit = _db.Newsclassify.FirstOrDefault(c=>c.Name== addNewsClassify.Name);
            if (exit!=null)
            {
               return new ResponseModel { Code = -200, Result = "该类别已经存在！" + addNewsClassify.Name };
            }
            var newsClassify = new Newsclassify { Name=addNewsClassify.Name,Sort=addNewsClassify.Sort,Remark=addNewsClassify.Remark };
            _db.Newsclassify.Add(newsClassify);
            int i = _db.SaveChanges();
            if (i > 0)
            {
                return new ResponseModel { Code = 200, Result = "Newsclassify 添加成功!" };
            }
            else
            {
                return new ResponseModel { Code = -200, Result = "Newsclassify 添加失败!" };
            }
        }

        public ResponseModel GetOneNewsclassify(int id)
        {
            Newsclassify newsclassify = _db.Newsclassify.Find(id);
            var rmodel = new ResponseModel();
            if (newsclassify!=null)
            {
                rmodel.Code = 200;
                rmodel.Result = "Newsclassify  获取成功";
                rmodel.data = newsclassify;
            }
            else
            {
                rmodel.Code = -200;
                rmodel.Result = "未能获取Newsclassify id:"+id;
            }
           
            return rmodel;
        }

        private Newsclassify GetOneNewsclassify(Expression<Func<Newsclassify,bool>> expression)
        {
            return _db.Newsclassify.FirstOrDefault(expression);
        }

        public ResponseModel UpdateNewsclassify(EditNewsClassify editNewsClassify)
        {
            var newsclassify = GetOneNewsclassify(c => c.Id == editNewsClassify.Id);
            if (newsclassify == null)
            {
                return new ResponseModel { Code = -200, Result = "该类别不存在！" +editNewsClassify.Id };
            }

            newsclassify.Name = editNewsClassify.Name;
            newsclassify.Remark = editNewsClassify.Remark;
            newsclassify.Sort = editNewsClassify.Sort;

            _db.Newsclassify.Update(newsclassify);

            int i = _db.SaveChanges();
            if (i > 0)
            {
                return new ResponseModel { Code = 200, Result = "Newsclassify 修改成功!" +editNewsClassify.Id};
            }
            else
            {
                return new ResponseModel { Code = -200, Result = "Newsclassify 修改失败!" + editNewsClassify.Id };
            }

        }

        public ResponseModel GetNewsClassifyList()
        {
            var newsclassifyList = _db.Newsclassify.OrderByDescending(c => c.Sort).ToList();
            var resmodel = new ResponseModel();
            resmodel.Code = 200;
            resmodel.Result = "GetNewsClassifyList 成功!";
            resmodel.data = new List<Newsclassify>();
            foreach (var ncitem in newsclassifyList)
            {
                resmodel.data.Add(ncitem);
            }
            return resmodel;
        }

        /// <summary>
        /// 增加新闻
        /// </summary>
        /// <param name="addNews"></param>
        /// <returns></returns>
        public ResponseModel AddNews(AddNews addNews)
        {
            var newsclassify = GetOneNewsclassify(c => c.Id == addNews.NewsClassifyId);
            if (newsclassify == null)
            {
                return new ResponseModel { Code = -200, Result = "该新闻类别不存在！" + addNews.NewsClassifyId };
            }

            var news = new News { NewsClassifyId = addNews.NewsClassifyId,
                                  Title = addNews.Title,
                                  Contents = addNews.Contents,
                                  Image = addNews.Image,PublishDate=addNews.PublishDate,Remark=addNews.Remark};
            _db.News.Add(news);
            int i = _db.SaveChanges();
            if (i > 0)
            {
                return new ResponseModel { Code = 200, Result = "News添加成功!" };
            }
            else
            {
                return new ResponseModel { Code = -200, Result = "News添加失败!" };
            }
        }

        /// <summary>
        /// 获取单条新闻
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResponseModel GetOneNews(int id )
        {
            News news = _db.News.Include("Newsclassify").Include("Newscomments").FirstOrDefault(c=>c.Id==id);
            var rmodel = new ResponseModel();
            if (news != null)
            {
                rmodel.Code = 200;
                rmodel.Result = "news  获取成功";
                rmodel.data = new NewsModel {
                    NewsId = news.Id,
                    Title = news.Title,
                    Image = news.Image,
                    Contents = news.Contents,
                    PublishDate = news.PublishDate,
                    Remark = news.Remark,
                    NewsClassifyId = news.NewsClassifyId,
                    NewsClassifyName = news.Newsclassify.Name,
                    NewsCommentCount = news.Newscomments.Count

                };
            }
            else
            {
                rmodel.Code = -200;
                rmodel.Result = "未能获取News id:" + id;
            }

            return rmodel;
        }

        public ResponseModel DelOneNews(int id)
        {
            News news = _db.News.FirstOrDefault(c => c.Id == id);
            if (news==null)
            {
                return new ResponseModel { Code = -200, Result = "未找到该id对应的新闻" +id };
            }

            _db.News.Remove(news);
            int i = _db.SaveChanges();
            if (i > 0)
            {
                return new ResponseModel { Code = 200, Result = "News删除成功!" };
            }
            else
            {
                return new ResponseModel { Code = -200, Result = "News删除失败!" };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageSize">每页显示数量</param>
        /// <param name="pageIndex">页码 从1 开始</param>
        /// <param name="total">总数量</param>
        /// <param name="expression">表达式</param>
        /// <returns></returns>
        public ResponseModel NewsPageQuery(int pageSize,int pageIndex,out int total ,List<Expression<Func<News,bool>>>expression)
        {
            var list = _db.News.Include("Newsclassify").Include("Newscomments");
            foreach (var item in expression)
            {
                list = list.Where(item);
            }

            total = list.Count();
            var pagedata = list.OrderByDescending(c => c.PublishDate).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();

            var rmodel = new ResponseModel();
            rmodel.Code = 200;
            rmodel.Result = "news 分页数据获取成功";
            List<NewsModel> newslist = new List<NewsModel>();
            foreach(var news in pagedata)
            {
                NewsModel nmodel = new NewsModel();
                nmodel.NewsId = news.Id;
                nmodel.NewsId = news.Id;
                nmodel.Title = news.Title;
                nmodel.Image = news.Image;
                nmodel.Contents = news.Contents;
                if (news.Contents.Length >50)
                   nmodel.Contents = news.Contents.Substring(0,50)+"...";

                nmodel.PublishDate = news.PublishDate;
                nmodel.Remark = news.Remark;
                nmodel.NewsClassifyId = news.NewsClassifyId;
                nmodel.NewsClassifyName = news.Newsclassify.Name;
                nmodel.NewsCommentCount = news.Newscomments.Count;
                newslist.Add(nmodel);
            }
            rmodel.data = newslist;
            return rmodel;
       

        }
        /// <summary>
        /// 获取新闻列表
        /// </summary>
        /// <param name="where"></param>
        /// <param name="topcount">数量</param>
        /// <returns></returns>
        public ResponseModel GetNewsList(Expression<Func<News, bool>> where,int topcount)
        {
            var list = _db.News.Include("Newsclassify").Include("Newscomment").Where(where).OrderByDescending(c=>c.PublishDate).Take(topcount);

            var rmodel = new ResponseModel();
            rmodel.Code = 200;
            rmodel.Result = "news 数据列表获取成功";
            rmodel.data = new List<NewsModel>();
            foreach (var news in list)
            {
                NewsModel nmodel = new NewsModel();
                nmodel.NewsId = news.Id;
                nmodel.NewsId = news.Id;
                nmodel.Title = news.Title;
                nmodel.Image = news.Image;
                if (news.Contents.Length >80)
                {
                    nmodel.Contents = news.Contents.Substring(0,80);
                }
                else
                   nmodel.Contents = news.Contents;

                nmodel.PublishDate = news.PublishDate;
                nmodel.Remark = news.Remark;
                nmodel.NewsClassifyId = news.NewsClassifyId;
                nmodel.NewsClassifyName = news.Newsclassify.Name;
                nmodel.NewsCommentCount = news.Newscomments.Count;
                rmodel.data.add(nmodel);
            }

            return rmodel;
        }


        /// <summary>
        /// 获取最新新闻评论的列表
        /// </summary>
        /// <param name="topcount"></param>
        /// <returns></returns>
        public ResponseModel GetCommentNewsList(int topcount)
        {
            var newsIds = _db.Newscomment.OrderByDescending(c => c.AddTime).GroupBy(c => c.NewsId).Select(c => c.Key).Take(topcount);

            var list = _db.News.Include("Newsclassify").Include("Newscomments").Where(c=>newsIds.Contains(c.Id)).OrderByDescending(c => c.PublishDate);

            var rmodel = new ResponseModel();
            rmodel.Code = 200;
            rmodel.Result = "最新评论news 数据列表获取成功";
            rmodel.data = new List<NewsModel>();
            foreach (var news in list)
            {
                NewsModel nmodel = new NewsModel();
                nmodel.NewsId = news.Id;
                nmodel.NewsId = news.Id;
                nmodel.Title = news.Title;
                nmodel.Image = news.Image;
                if (news.Contents.Length > 80)
                {
                    nmodel.Contents = news.Contents.Substring(0, 80);
                }
                else
                    nmodel.Contents = news.Contents;

                nmodel.PublishDate = news.PublishDate;
                nmodel.Remark = news.Remark;
                nmodel.NewsClassifyId = news.NewsClassifyId;
                nmodel.NewsClassifyName = news.Newsclassify.Name;
                nmodel.NewsCommentCount = news.Newscomments.Count;
                rmodel.data.add(nmodel);
            }
            return rmodel;
        }

        public ResponseModel GetSearchOneNews(Expression<Func<News, bool>> where)
        {
            var news = _db.News.Where(where).FirstOrDefault();
            if (news==null)
            {
                return new ResponseModel { Code = -200, Result = "搜索新闻失败!" };
            }

            var rmodel = new ResponseModel();
            rmodel.Code = 200;
            rmodel.Result = "news 搜索数据获取成功";
            rmodel.data = new List<NewsModel>();
           
                NewsModel nmodel = new NewsModel();
                nmodel.NewsId = news.Id;
                nmodel.NewsId = news.Id;
                nmodel.Title = news.Title;
                nmodel.Image = news.Image;
              
                nmodel.Contents = news.Contents;

                nmodel.PublishDate = news.PublishDate;
                nmodel.Remark = news.Remark;
                nmodel.NewsClassifyId = news.NewsClassifyId;
                nmodel.NewsClassifyName = news.Newsclassify.Name;
                nmodel.NewsCommentCount = news.Newscomments.Count;
                rmodel.data.add(nmodel);
            

            return rmodel;
        }


        public ResponseModel GetNewsCount(Expression<Func<News, bool>> where)
        {
            int count = _db.News.Where(where).Count();

            return new ResponseModel { Code = 200, Result = "搜索新闻数量成功!",data=count };
        }


        public ResponseModel GetNewsRecommand(int newsid)
        {
            var inews = _db.News.FirstOrDefault(c => c.Id == newsid);
            if (inews == null)
               return new ResponseModel { Code = -200, Result = "搜索新闻推荐该id不存在!"+newsid};

            var newslist = _db.News.Include("Newscomment").Where(c => c.NewsClassifyId == inews.NewsClassifyId && c.Id != inews.Id)
                .OrderByDescending(c => c.PublishDate).OrderByDescending(c => c.Newscomments.Count).Take(10);

            var rmodel = new ResponseModel();
            rmodel.Code = 200;
            rmodel.Result = "最新推荐news 数据列表获取成功";
            rmodel.data = new List<NewsModel>();

            foreach (var news in newslist)
            {
                NewsModel nmodel = new NewsModel();
                nmodel.NewsId = news.Id;
                nmodel.NewsId = news.Id;
                nmodel.Title = news.Title;
                nmodel.Image = news.Image;
                if (news.Contents.Length > 80)
                {
                    nmodel.Contents = news.Contents.Substring(0, 80);
                }
                else
                    nmodel.Contents = news.Contents;

                nmodel.PublishDate = news.PublishDate;
                nmodel.Remark = news.Remark;
                nmodel.NewsClassifyId = news.NewsClassifyId;
                nmodel.NewsClassifyName = news.Newsclassify.Name;
                nmodel.NewsCommentCount = news.Newscomments.Count;
                rmodel.data.add(nmodel);
            }

            return rmodel;

        }
    }
}
