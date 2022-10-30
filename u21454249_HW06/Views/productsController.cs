using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using u21454249_HW06.Models;
using Newtonsoft.Json;
using PagedList;
using PagedList.Mvc;

namespace u21454249_HW06.Views
{
    public class productsController : Controller
    {
        private BikeStoresEntities db = new BikeStoresEntities();

        // GET: products
        public ActionResult Index(int ? page)
        {
            int pageSize = 5;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var products = db.products.Include(p => p.brand).Include(p => p.category);
            return View(products.ToList().ToPagedList(pageIndex,pageSize));
        }

        // GET: products/Details/5
        public ActionResult Details(int? id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //product product = db.products.Find(id);
            //if (product == null)
            //{
            //    return HttpNotFound();
            //}
            return PartialView();
        }

        // GET: products/Create
        public ActionResult Create()
        {
            return PartialView();
        }
        public string CreateProd(int? id)
        {
            /*if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }*/
            db.Configuration.ProxyCreationEnabled = false;
            //product product = db.products.Find(id);
            object product = db.products.Where(x => x.product_id == id).Select(p => new { id = p.product_id, name = p.product_name, brand = p.brand.brand_name, catergory = p.category.category_name, model = p.model_year, price = p.list_price }).FirstOrDefault();
            /* if (product == null)
             {
                 return HttpNotFound();
             }*/
            //ViewBag.brand_id = new SelectList(db.brands, "brand_id", "brand_name", product.brand_id);
            //ViewBag.category_id = new SelectList(db.categories, "category_id", "category_name", product.category_id);
            //return View(product);
            return JsonConvert.SerializeObject(product);
        }

        // POST: products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public string Create(int product_id, string product_name, int brand_id, int category_id, short model_year, decimal list_price)
        {
            
                db.products.Add(new product { product_id =product_id });
            db.products.Add(new product { product_name=product_name }); 
            db.products.Add(new product { brand_id=brand_id });
            db.products.Add(new product { category_id=category_id });
            db.products.Add(new product { model_year=model_year });
            db.products.Add(new product { list_price=list_price });
  
                db.SaveChanges();

            return JsonConvert.SerializeObject(new { message = "New product added" });
        }

        // GET: products/Edit/5
        public ActionResult Edit()
        {
            return PartialView();
        }
        public string EditProd(int? id)
        {
            /*if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }*/
            db.Configuration.ProxyCreationEnabled = false;
            //product product = db.products.Find(id);
            object product = db.products.Where(x => x.product_id == id).Select(p => new { id = p.product_id, name = p.product_name, brand = p.brand.brand_name, catergory = p.category.category_name, model = p.model_year, price = p.list_price }).FirstOrDefault();
            /* if (product == null)
             {
                 return HttpNotFound();
             }*/
            //ViewBag.brand_id = new SelectList(db.brands, "brand_id", "brand_name", product.brand_id);
            //ViewBag.category_id = new SelectList(db.categories, "category_id", "category_name", product.category_id);
            //return View(product);
            return JsonConvert.SerializeObject(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Edits(int product_id, string product_name, int brand_id, int category_id, short model_year, decimal list_price)
        {
            product product = new product
            {
                product_id = product_id,
                product_name = product_name,
                brand_id = brand_id,
                category_id = category_id,
                model_year = model_year,
                list_price = list_price

            };
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.brand_id = new SelectList(db.brands, "brand_id", "brand_name", product.brand_id);
            ViewBag.category_id = new SelectList(db.categories, "category_id", "category_name", product.category_id);
            return View(product);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return PartialView();
        }

        // POST: products/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public string DeleteConfirmed(int id)
        {
            product product = db.products.Find(id);
            db.products.Remove(product);
            db.SaveChanges();
            return JsonConvert.SerializeObject(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        //public string AddPlaylist(string name)
        //{
        //    db.products.Add(new prod { Name = name });

        //    db.SaveChanges();

        //    return JsonConvert.SerializeObject(new { message = "New playlist added!" });
        //}
    }
}
