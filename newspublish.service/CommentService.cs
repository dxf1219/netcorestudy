using Microsoft.EntityFrameworkCore;
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
    public class CommentService
    {
        private Db _db;
        private NewsService _newsService;

        public CommentService(Db db,NewsService newsService)
        {
            this._db = db;
            this._newsService = newsService;
        }

        /// <summary>
        /// 增加评论
        /// </summary>
        /// <param name="addComment"></param>
        /// <returns></returns>
        public ResponseModel AddComment(AddComment addComment)
        {
            var news = _newsService.GetOneNews(addComment.NewsId);
            if (news!=null)
            {
                var newscomment = new Newscomment
                {
                    NewsId = addComment.NewsId,
                    Contents = addComment.Contents,
                    AddTime=DateTime.Now,
                };

                _db.Newscomment.Add(newscomment);
                int i = _db.SaveChanges();
                if (i > 0)
                {
                    return new ResponseModel { Code = 200,
                        Result = "News评论添加成功!",
                        data = new NewsCommentModel
                        {
                            Contents = addComment.Contents,
                            NewsId = addComment.NewsId,
                            AddTime = newscomment.AddTime,
                            Floor="#"+news.data.NewsCommentCount+1,
                            NewsTitle=news.data.Title
                        }
                    };
                }
                else
                {
                    return new ResponseModel { Code = -200, Result = "News评论添加失败!" };
                }
            }
            else
            {
              return  new ResponseModel { Code = -200, Result = "查询不到该条新闻" };
            }
        }

        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResponseModel DelComment(int id)
        {
            var comment = _db.Newscomment.FirstOrDefault(c=>c.Id==id);
            if (comment == null)
            {
                return new ResponseModel { Code = -200, Result = "未找到该id对应的新闻评论" + id };
            }

            _db.Newscomment.Remove(comment);
            int i = _db.SaveChanges();
            if (i > 0)
            {
                return new ResponseModel { Code = 200, Result = "News评论删除成功!" };
            }
            else
            {
                return new ResponseModel { Code = -200, Result = "News评论删除失败!" };
            }
        }
        /// <summary>
        /// 获取评论列表
        /// </summary>
        /// <returns></returns>
        public ResponseModel GetNewsCommentList()
        {
            var newscommentList = _db.Newscomment.Include("News").OrderBy(c => c.AddTime).ToList();

            var resmodel = new ResponseModel();
            resmodel.Code = 200;
            resmodel.Result = "GetNewsCommentList 成功!";
            resmodel.data = new List<NewsCommentModel>();
            foreach (var ncitem in newscommentList)
            {
                NewsCommentModel ncm = new NewsCommentModel();
                ncm.Id = ncitem.Id;
                ncm.NewsId = ncitem.NewsId;
                ncm.NewsTitle = ncitem.News.Title;
                ncm.AddTime = ncitem.AddTime;
                ncm.Remark = ncitem.Remark;
                resmodel.data.Add(ncm);
            }
            return resmodel;
        }
    }
}
