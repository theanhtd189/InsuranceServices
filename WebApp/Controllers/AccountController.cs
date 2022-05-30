using Microsoft.AspNetCore.Mvc;
using WebApp.Models;


namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        private InsuranceOnlineContext db = new InsuranceOnlineContext();
        public IActionResult Index()
        {
            if (TempData["Msg"]!=null)
            {
                ViewBag.Msg = TempData["Msg"];
            }

            var uid = HttpContext.Session.GetInt32("user_id");
            var u = db.Customers.FirstOrDefault(x => x.Id == uid);

            return View("Detail",u);
        }


        [HttpPost]
        public ActionResult Edit(string username, string email, string address, string phone, DateTime birthday, bool gender, string? password)
        {
            if (!string.IsNullOrEmpty(email))
            {
                var uid = HttpContext.Session.GetInt32("user_id");
                var u = db.Customers.FirstOrDefault(x => x.Id == uid);
                
                if(u.Username!=username)
                    u.Username= username;
                if(u.Email!=email)
                    u.Email= email;
                if(u.Address!=address)
                    u.Address= address;
                if(u.Gender!=gender)
                    u.Gender=gender;
                if(u.Birthday!=birthday)
                    u.Birthday= birthday;
                if (!string.IsNullOrEmpty(password))
                {
                    if(u.Password!=password)
                        u.Password= password;
                }

                var stt = db.SaveChanges() > 0;
                if (stt)
                {
                    TempData["Msg"] = "Saved change successfully!";
                }
                else
                {
                    TempData["Msg"] = "Save change failed!";
                }
            }
            else
            {
                TempData["Msg"] = "Please entered valid value!";
                
            }
            return RedirectToAction("Index");
        }

        [HttpGet,Route("Login")]
        public ActionResult Login(string? callback)
        {
            var uid = HttpContext.Session.GetInt32("user_id");
            if (uid!=null)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            if (TempData["Msg"] != null)
            {
                ViewBag.Msg = TempData["Msg"].ToString();
            }
            if (callback != null && !string.IsNullOrEmpty(callback))
            {
                ViewBag.Callback = callback;
            }
                return View("~/Views/Home/Login.cshtml");
        }

        [HttpPost,Route("Login")]
        public ActionResult Login(string email, string password,string? callback)
        {         
            if(!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password)){
                var o = db.Customers.FirstOrDefault(x=>x.Email==email && x.Password==password);
                if (o != null)
                {
                    if (o.Status==false)
                    {
                        ViewBag.Error = "Your account has been deactive! Please contact admin!";
                    }
                    else
                    {
                        HttpContext.Session.SetInt32("user_id", o.Id);

                        if (callback != null && !string.IsNullOrEmpty(callback))
                        {
                            return Redirect(callback);
                        }
                        else
                            return RedirectToAction("", "Account");
                    }
                    
                }
                else
                {
                    ViewBag.Error = "Email or Password is incorrect!";
                }    
            }
            else
            {
                ViewBag.Error = "Email or password is invalid";
            }    
            return View("~/Views/Home/Login.cshtml");
        }

        [HttpPost, Route("SignUp")]
        public JsonResult SingUp(string fullname,string address, string phone, string email, DateTime birthday, bool p_gender, string password, string? callback)
        {
            string msg = "";
            int stt_code = 400;
            int return_id = 0;

            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
            {
                var check_email = db.Customers.FirstOrDefault(x=>x.Email==email);
                if(check_email != null)
                {
                    msg = "This email already has used, please choose another one!";
                    stt_code = 202;
                }
                else
                {
                    Customer e = new Customer
                    {
                        Username = fullname,
                        Email = email,
                        Phone = phone,
                        Password = password,
                        Address = address,
                        Birthday = birthday,
                        Gender = p_gender,
                        CreatedAt = DateTime.Now,
                        Status = true
                    };
                    db.Customers.Add(e);
                    var stt = db.SaveChanges() > 0;
                    if (stt)
                    {
                        msg = "Created a new account successfully!";
                        stt_code = 200;
                        HttpContext.Session.SetInt32("user_id", e.Id);
                        return_id = e.Id;
                    }
                    else
                    {
                        msg = "Created failed";
                        stt_code = 204;
                    }
                }
            }
            else
            {
                if(string.IsNullOrEmpty(email))
                    msg = "Email is invalid";
                else
                if (string.IsNullOrEmpty(password))
                    msg = "Password is invalid";
            }

            var r = new
            {
                stt_code = stt_code,
                message = msg,
                id = return_id
            };

            return Json(r);
        }

        [HttpGet,Route("LogOut")]
        public ActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("","Home");
        }
    }
}
