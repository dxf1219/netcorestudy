using newspublish.mode.Entity;
using newspublish.mode.Request;
using newspublish.mode.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace newspublish.service
{
    public class BannerService
    {
        private Db _db;
        public BannerService(Db db)
        {
            this._db = db;
        }
        public ResponseModel AddBannerEntity(AddBanner addBanner)
        {
            var ba = new Banner { AddTime = DateTime.Now, Image = addBanner.Image, Url = addBanner.Url, Remark = addBanner.Remark };
            _db.Banner.Add(ba);
            int i = _db.SaveChanges();
            if (i>0)
            {
                return new ResponseModel { Code = 200, Result = "Banner 添加成功!" };
            }
            else
            {
                return new ResponseModel { Code = -200, Result = "Banner 添加失败!" };
            }
        }

        public ResponseModel GetBannelList()
        {
            var banners = _db.Banner.ToList().OrderByDescending(c=>c.AddTime);
            var rmodel = new ResponseModel();
            rmodel.Code = 200;
            rmodel.Result = "Banner list 获取成功";
            rmodel.data = new List<Banner>();
            foreach (var bitem in banners)
            {
                rmodel.data.Add(bitem);
            }
            return rmodel;
        }


        public ResponseModel DelBannerEntity(int baid)
        {
            var bannelinfo = _db.Banner.Find(baid);
            if (bannelinfo ==null)
            {
                return new ResponseModel { Code = -200, Result = "Banner 不存在!"+baid };
            }

            _db.Banner.Remove(bannelinfo);

            int i = _db.SaveChanges();
            if (i > 0)
            {
                return new ResponseModel { Code = 200, Result = "Banner 删除成功!" +baid};
            }
            else
            {
                return new ResponseModel { Code = -200, Result = "Banner 删除失败!" + baid };
            }
        }
    }
}
