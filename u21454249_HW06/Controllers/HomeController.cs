using u21454249_HW06.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using System.Collections;
using System.Data.Entity;

namespace u21454249_HW06.Controllers
{
    public class HomeController : Controller
    {
        private readonly BikeStoresEntities datab = new BikeStoresEntities();
        public ActionResult Index()
        {
            
            return View();
        }

        public string GetProducts(int? page)
        {
            
           object Productitems = datab.products.Select(p => new { id = p.product_id, name = p.product_name, brand = p.brand.brand_name, catergory = p.category.category_name, model = p.model_year, price = p.list_price }).ToList();
           return JsonConvert.SerializeObject(Productitems);
        }

        public string GetCategorynames()
        {
            object Categories = datab.categories.Select(u => new { id = u.category_id, name = u.category_name }).ToList();
            return JsonConvert.SerializeObject(Categories);
        }
        public string Search(string text)
        {
            datab.Configuration.ProxyCreationEnabled = false;
            object Productitem = datab.products.Where(o => o.product_name.Contains(text)).Select(p => new { id = p.product_id, name = p.product_name, brand = p.brand.brand_name, catergory = p.category.category_name, model = p.model_year, price = p.list_price }).ToList();
            return JsonConvert.SerializeObject(Productitem);
        }
        public string GetBrandData()
        {
            datab.Configuration.ProxyCreationEnabled = false;

            List<brand> data = datab.brands.ToList();

            return JsonConvert.SerializeObject(data);
        }
        public string ProductDetails(int id)
        {
            //db.Configuration.ProxyCreationEnabled = false;
            object productDetial = datab.stocks.Where(y => y.product_id == id).Include(v => v.product).Select(p => new {
                productname = p.product.product_name,
                year = p.product.model_year,
                price = p.product.list_price,
                brand = p.product.brand.brand_name,
                catergory = p.product.category.category_name,
                stores = datab.stocks.Where(s => s.product_id == id).Select(n => new { storename = n.store.store_name, quantity = n.quantity })

            }).FirstOrDefault();


            return JsonConvert.SerializeObject(productDetial);

        }
        public ActionResult Orders(int ?page)
        {
            int pageSize = 5;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            IPagedList<Orders> ord = null;
            List<Orders> Proctuctitem = datab.order_items.Select(p => new Orders { Orderid = p.order_id, Category = p.product.category.category_name, Product = datab.products.Where(x => x.product_id == p.product_id).FirstOrDefault(), Quantity = p.quantity, Price = p.list_price, Total = (p.list_price * p.quantity), OrderDate = datab.orders.Where(o => o.order_id == p.order_id).FirstOrDefault().order_date }).ToList();
            ord = Proctuctitem.ToPagedList(pageIndex, pageSize);
            return View(Proctuctitem);
        }
        public ActionResult SearchOrders(DateTime date)
        {
            string day = date.ToShortDateString();
            //var totalorders  = bikes.Where(o => o.orderdate.Month = 1).um( )-
            // object orders = db.orders.Select(p => new {prods = db.order_items.Where(oi=> oi.order_id == p.order_id.).ToList(), }).ToList();
            List<Orders> productDatas = datab.order_items.Where(y => y.order.order_date <= date).Select(p => new Orders { Orderid = p.order_id, Category = p.product.category.category_name, Product = datab.products.Where(x => x.product_id == p.product_id).FirstOrDefault(), Quantity = p.quantity, Price = p.list_price, Total = (p.list_price * p.quantity), OrderDate = datab.orders.Where(o => o.order_id == p.order_id).FirstOrDefault().order_date }).ToList();
            return View("Orders", productDatas);
        }
        public ActionResult Report()
        {

            return View();
        }
        public string GetReports()
        {
            datab.Configuration.ProxyCreationEnabled = false;
            object bikes = datab.orders.Select(o => new
            {
                month = o.order_date.Month,
                bike = datab.order_items.Where(x => x.order_id == o.order_id && x.product.category.category_id == 6).ToList(),
            }).ToList();

            return JsonConvert.SerializeObject(bikes);
        }
    }
}