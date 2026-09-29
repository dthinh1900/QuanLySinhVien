using DinhDucThinh_2022603165.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DinhDucThinh_2022603165.Controllers
{
    public class SinhVienController : ApiController
    {
        QLSVEntities db = new QLSVEntities();
        //tên DATABASE + Entities
        [HttpGet]
        public List<SinhVien> getall()
        {
            return db.SinhViens.ToList();
        }

        //Post
        public IHttpActionResult Post(SinhVien x)
        {
            if (getall().Any(y => y.MaSV == x.MaSV))
            {
                return BadRequest();
            }
            db.SinhViens.Add(x);
            db.SaveChanges();
            return Ok();
        }
        //Put
        public IHttpActionResult Put(SinhVien x)
        {
            if (x.DiemTB < 0 || x.DiemTB > 10)
            {
                return BadRequest();
            }
            SinhVien sv = db.SinhViens.FirstOrDefault(z => z.MaSV == x.MaSV);
            if (sv == null)
            {
                return NotFound();
            }
            sv.TenSV = x.TenSV;
            sv.Lop = x.Lop;
            sv.DiemTB = x.DiemTB;


            db.SaveChanges();
            return Ok();
        }
        //trên Delete
        public IHttpActionResult Delete(string iD)
        {
            SinhVien sv = db.SinhViens.FirstOrDefault(z => z.MaSV == iD);
            if (sv == null)
            {
                return NotFound();
            }
            db.SinhViens.Remove(sv);
            db.SaveChanges();
            return Ok();
        }
    }
}
