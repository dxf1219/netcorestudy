using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using newspublish.mode.Entity;
using newspublish.service;

namespace newspublishweb.Areas.admin.Controllers
{
    [Area("admin")]
    public class CommentController : Controller
    {
        private CommentService _commentService;
      
        public CommentController(CommentService commentService)
        {
            this._commentService = commentService;
           
        }
        public IActionResult Comment()
        {
            var commentlist = _commentService.GetNewsCommentList();
            return View(commentlist);
        }
    }
}