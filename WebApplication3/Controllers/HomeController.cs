using PagedList;
using System.Linq;
using System.Web.Mvc;
using WebApplication3.Models;
using WebApplication3.ViewModels;

namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 2;

            using (var db = new CustomerEntities())
            {
                //var cust = db.Database.SqlQuery<Customer>("Select * from customer").ToList();
                //var customersList = db.Customers.OrderBy(x => x.Id).Select(x => new CustomerViewModel
                //{
                //    ID = x.Id,
                //    Name = x.Name,
                //    Address = x.Address,
                //    Phone = x.Phone
                //}).ToPagedList(pageNumber, pageSize);

                var customersList = db.Customers.OrderBy(x => x.Id).ToPagedList(pageNumber, pageSize);

                var model = new GridViewModel
                {
                    Customers = customersList
                };

                return View(model);
            }
        }

        public JsonResult AddCustomer(CustomerViewModel customer)
        {
            using(var db = new CustomerEntities())
            {
                var cust = new Customer()
                {
                    Name = customer.Name,
                    Address = customer.Address,
                    Phone = customer.Phone
                };

                db.Customers.Add(cust);
                if (db.SaveChanges() > 0)
                    return Json(new { status = "success", result = cust });
                else
                    return Json(new { status = "failed", reason = "could save to db" });
            }
        }

        public JsonResult DeleteCustomer(int id)
        {
            using (var db = new CustomerEntities())
            {
                var cus = db.Customers.Find(id);
                if (cus != null)
                {
                    db.Customers.Remove(cus);
                    if (db.SaveChanges() > 0)
                        return Json(new { status = "success" });
                    else
                        return Json(new { status = "failed" });
                }
                else
                {
                    return Json(new { status = "failed", reason = "not exists" });
                }
            }
        }

        public JsonResult SaveCustomer(CustomerViewModel customer)
        {
            using (var db = new CustomerEntities())
            {
                var cus = db.Customers.Find(customer.ID);
                if (cus != null)
                {
                    cus.Name = customer.Name;
                    cus.Address = customer.Address;
                    cus.Phone = customer.Phone;

                    if (db.SaveChanges() > 0)
                        return Json(new { status = "success", response = cus });
                    else
                        return Json(new { status = "failed", reason = "could not save" });
                }
                else
                {
                    return Json(new { status = "failed", reason = "not found" });
                }
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}