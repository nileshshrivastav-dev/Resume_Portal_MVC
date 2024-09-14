using Resum_Portal.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Razor.Tokenizer.Symbols;
using System.Web.Security;

namespace Resum_Portal.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        private readonly PdfSearchService _pdfSearchService;

        public HomeController()
        {
            _pdfSearchService = new PdfSearchService(new wholeProjectModel()); 
        }
        private wholeProjectModel db = new wholeProjectModel();
        [Authorize(Roles ="Admin")]
        public ActionResult Index()
        {
            return View(db.tbl_Designation.ToList());
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "Email,Name,Mobile,Gender,Designation,Date_of_Joining,Photo,Address,password,Role,Status,CreatedBy")] tbl_User tbl_User, HttpPostedFileBase Photo)
        {
            if (ModelState.IsValid)
            {
                // Check if email already exists
                if (db.tbl_User.Any(u => u.Email == tbl_User.Email))
                {
                    ModelState.AddModelError("Email", "Email address already exists.");
                    ViewBag.msg = "Email address already exists.";
                    return View(tbl_User);
                }

                // Additional validation checks
                if (string.IsNullOrEmpty(tbl_User.Email) || tbl_User.Email.IndexOf("@") <= 0 || !tbl_User.Email.Contains("."))
                {
                    ViewBag.msg = "Invalid Email";
                    return View(tbl_User);
                }
                if (tbl_User.Mobile > 0)
                {
                    if ((tbl_User.Mobile.ToString().Length != 10 || (tbl_User.Mobile.ToString()[0] != '7') && (tbl_User.Mobile.ToString()[0] != '8') && (tbl_User.Mobile.ToString()[0] != '9')))
                    {
                        ViewBag.msg = "Invalid Mobile Number";
                        return View(tbl_User);
                    }
                }
                if (tbl_User.password.Length < 8)
                {
                    ViewBag.msg = "Password must be at least 8 characters long";
                   // return View();
                }

                // Handle file upload
                    if (Photo != null && Photo.ContentLength > 0)
                    {
                        var filename = Path.GetFileName(Photo.FileName);
                        var path = Path.Combine(Server.MapPath("~/Content/profile/"), filename);
                        Photo.SaveAs(path);
                        tbl_User.Photo = filename;
                    }

                    // Add user to database
                    tbl_User.CreatedBy = User.Identity.Name; // Assuming CreatedBy is set to current user
                    db.tbl_User.Add(tbl_User);
                    db.SaveChanges();

                    ViewBag.msg = "Record Added";
                    return RedirectToAction("Users"); 
                
            }
            else
            {
                // Model validation failed
                ViewBag.msg = "Invalid data submitted";
                return View(tbl_User);
            }
        }

        public ActionResult Delete()
        {
            if (Request.QueryString["id"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string decodedEmail = HttpUtility.UrlDecode(Request.QueryString["id"]);
            tbl_User tbl_User = db.tbl_User.Find(decodedEmail);
            if (tbl_User == null)
            {
                return HttpNotFound();
            }
            return View(tbl_User);
        }

        // POST: tbl_User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed()
        {
            string decodedEmail = HttpUtility.UrlDecode(Request.QueryString["id"]);
            tbl_User tbl_User = db.tbl_User.Find(decodedEmail);
            db.tbl_User.Remove(tbl_User);
            db.SaveChanges();
            return RedirectToAction("Users");
        }
        public ActionResult Edit()
        {
            if (Request.QueryString["id"] == null)
            {
                return RedirectToAction("Default404");
            }
            string decodedEmail = HttpUtility.UrlDecode(Request.QueryString["id"]);
            var tbl_User = db.tbl_User.Find(decodedEmail);
            var designation = db.tbl_Designation.ToList();
            var viewModel = new DashBoardModel
            {
                Users = null, // No need to populate this for non-admin
                Resumes = null,
                Designations = designation,
                UserData = tbl_User,
                Resumesdata = null

            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Email,Name,Mobile,Gender,Designation,Date_of_Joining,Photo,Address,password,Role,Status,CreatedBy")] tbl_User tbl_User, HttpPostedFileBase Photo)
        {
            if (tbl_User.Email != null && tbl_User.Name != null && tbl_User.Gender != null && tbl_User.Designation != null && tbl_User.Date_of_Joining != null && tbl_User.Photo != null && tbl_User.Address != null && tbl_User.password != null && tbl_User.CreatedBy != null)
            {
                if (ModelState.IsValid)
                {
                        if ((tbl_User.Email.IndexOf("@") <= 0))
                        {
                            ViewBag.msg = "Invalid Email";
                            return View();
                        }
                        if ((tbl_User.Email[tbl_User.Email.Length - 4] != '.') && (tbl_User.Email[tbl_User.Email.Length - 3] != '.'))
                        {
                            ViewBag.msg = "Invalid Email";
                            return View();
                        }
                        if (tbl_User.Mobile.ToString().Length != 10)
                        {
                            ViewBag.msg = "Check Mobile Number";
                            return View();
                        }
                        if ((tbl_User.Mobile.ToString()[0] != '7') && (tbl_User.Mobile.ToString()[0] != '8') && (tbl_User.Mobile.ToString()[0] != '9'))
                        {
                            ViewBag.msg = "Check Mobile Number";
                            return View();
                        }

                        if (tbl_User.password.Length < 8)
                        {
                            ViewBag.msg = "Password must be 8 Chareacter";
                            return View();
                        }
                        else
                        {
                        var filname = Photo.FileName;
                        Photo.SaveAs(Server.MapPath("/Content/profile/" + filname));

                        tbl_User.Photo = filname;
                        db.Entry(tbl_User).State = EntityState.Modified;
                           db.SaveChanges();
                        ViewBag.msg = "Record Updated";
                        Task.WaitAll(Task.Delay(2000));
                        return RedirectToAction("UserData");

                        }
                    
                }
                else
                {
                    ViewBag.msg = "Something Wrong";
                    return View(tbl_User);
                }

            }
            else
            {
                ViewBag.msg = "All Filled Required";
                return View();
            }


        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Login(string email, string Password)
        {
            if (email != null && Password != null)
            {
                var user = db.tbl_User.FirstOrDefault(u => u.Email == email && u.password == Password);
                if (user != null)
                {
                    if (user.Status != false)
                    {
                        FormsAuthentication.SetAuthCookie(email, false);
                        Session["Name"] = email;
                        Session["Role"] = user.Role;
                        Session["User"] = user.Name;
                        Session["pic"] = user.Photo;
                        if (user.Role.Equals("Admin"))
                        {
                            // ViewBag.error = "Admin Login";
                            // System.Threading.Thread.Sleep(3000);
                            return RedirectToAction("DashBoard");
                        }
                        else
                        {
                            // ViewBag.error = "User Login";
                            // System.Threading.Thread.Sleep(3000);
                            return RedirectToAction("DashBoard");
                        }
                        // return RedirectToAction("Create");
                    }
                    else
                    {
                        ViewBag.error = "You Are Blocked Admin Side";
                        return View();
                    }
                }
                else
                {
                    ViewBag.error = "Username or Password Mismatched";
                    return View();
                }
                // tbl_User tbl_User = db.tbl_User.Find(username);
                // return Content("<script>alert('ok');</script>");
            }
            else
            {
                ViewBag.error = "Filled Username & Password";
                return View();
            }
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("login");
        }
        [Authorize]
        public ActionResult Dashboard()
        {
            if (Session["Role"] != null && Session["Role"].Equals("Admin"))
            {
                var users = db.tbl_User.ToList();  // Retrieve all users for Admin

                var viewModel = new DashBoardModel
                {
                    Users = users, // No need to populate this for non-admin
                    Resumes = null,
                    Designations = null,
                    UserData = null,
                    Resumesdata=null
                };

                return View(viewModel);
            }
            else
            {
                /*
                var query = (from t1 in db.tbl_User
                             join t2 in db.tbl_resume on t1.Email equals t2.Email
                             where t1.Email == User.Identity.Name
                             select new DataModel
                             {
                                 Email = t1.Email,
                                 Name = t1.Name,
                                 Mobile = t1.Mobile,
                                 Gender = t1.Gender,
                                 Designation = t1.Designation,
                                 Date_of_Joining = t1.Date_of_Joining,
                                 Address = t1.Address,
                                 Role = t1.Role,
                                 Status = t1.Status,
                                 Photo = t1.Photo,
                                 CreatedBy = t1.CreatedBy,
                                 id = t2.id,
                                 Resume = t2.Resume
                             }).FirstOrDefault();
                */
                var query = db.tbl_User.FirstOrDefault(u => u.Email == User.Identity.Name);
                var viewModel = new DashBoardModel
                {
                    Users = null, // No need to populate this for non-admin
                    Resumes = null,
                    Designations = null,
                    UserData = query,
                    Resumesdata = null

                };

                return View(viewModel);
            }
        }
        [Authorize(Roles ="Admin")]
        public ActionResult CreateDesignation()
        {
            return View();
        }
        [HttpPost, Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDesignation([Bind(Include = "Designation,Status")] tbl_Designation tbl_designation)
        {
            if (tbl_designation.Designation != null)
            {
                if (ModelState.IsValid)
                {

                    if (db.tbl_Designation.Any(u => u.Designation == tbl_designation.Designation))
                    {
                        ModelState.AddModelError("Designation", "Designation address already exists.");
                        ViewBag.msg = "Designation address already exists.";
                        return View();
                    }
                    db.tbl_Designation.Add(tbl_designation);
                    db.SaveChanges();
                    ViewBag.msg = "Record Added";
                    Task.WaitAll(Task.Delay(2000));
                    // System.Threading.Thread.Sleep(2000);
                    return RedirectToAction("Designation");
                }
                else
                {
                    ViewBag.msg = "Something Missing ! Try Again..";
                    return View();
                }
                
            }
            ViewBag.msg = "All Filled Required";
            return View();

        }
        [Authorize(Roles ="Admin")]
        public ActionResult Designation()
        {
            return View(db.tbl_Designation.ToList());
        }
        [Authorize]
        public ActionResult ResumeEnter()
        {
            var designation = db.tbl_Designation.ToList();
            var viewModel = new DashBoardModel
            {
                Users = null, // No need to populate this for non-admin
                Resumes = null,
                Designations = designation,
                UserData = null,
                Resumesdata = null

            };
            return View(viewModel);
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ResumeEnter([Bind(Include = "Email,Designation,Resume,Title,KeyWords,Status,UploadedBy")] tbl_resume tbl_resume, HttpPostedFileBase Resume)
        //{
        //    if (tbl_resume.Email != null && tbl_resume.Designation != null && tbl_resume.Resume != null&&tbl_resume.Title!=null&&tbl_resume.KeyWords!=null&&tbl_resume.Status!=null)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            if (db.tbl_resume.Any(u => u.Email == tbl_resume.Email))
        //            {
        //                ModelState.AddModelError("Email", "Email address already exists.");
        //                ViewBag.msg = "Email address already exists.";
        //                return RedirectToAction("UserData");
        //            }
        //            else
        //            {
        //                // Get the directory path based on designation
        //                string dirpath = Server.MapPath("/Content") + "/" + tbl_resume.Designation;
        //                if (!Directory.Exists(dirpath))
        //                {
        //                    Directory.CreateDirectory(dirpath);
        //                }

        //                // Get the file name
        //                //var fileName = Path.GetFileName(tbl_resume.Resume);
        //                var filname = Resume.FileName;
        //                // Combine directory path and file name to get the full file path
        //                var filePath = Path.Combine(dirpath, filname);
        //                //save the file 
        //                Resume.SaveAs(filePath);

        //                tbl_resume.Resume = filname;
        //                // Save the resume data to the database
        //                string KeyWords = string.Join(",", tbl_resume.KeyWords);

        //                // Create new Product instance

        //                db.tbl_resume.Add(tbl_resume);
        //                db.SaveChanges();

        //                ViewBag.msg = "Record Added";
        //                Task.WaitAll(Task.Delay(2000));
        //                return RedirectToAction("UserData");
        //            }
        //        }
        //        else
        //        {
        //            ViewBag.msg = "Something Wrong";
        //            return View();
        //        }
        //    }
        //    else
        //    {
        //        ViewBag.msg = "All Fields Required";
        //        return View();
        //    }
        //}
        [Authorize]
        public ActionResult UserData(int page = 1, int pageSize = 10)
        {
            // Get resumes associated with the logged-in user
            var userEmail = User.Identity.Name;
            var resumesQuery = db.tbl_resume.Where(r => r.Email == userEmail);

            // Pagination: Calculate skip and take based on page number and page size
            var totalResumes = resumesQuery.Count();
            var resumes = resumesQuery.OrderBy(r => r.id)
                                     .Skip((page - 1) * pageSize)
                                     .Take(pageSize)
                                     .ToList();

            var designation = db.tbl_Designation.ToList();

            var viewModel = new DashBoardModel
            {
                Users=null,
                UserData=null,
                Resumes=null,
                Designations = designation,
                Resumesdata = resumes,
                CurrentPage = page,
                PageSize = pageSize,
                TotalResumes = totalResumes
                
            };

            return View(viewModel);
        }

        //[Authorize(Roles ="User")]
        //public ActionResult ChangeResume()
        //{
        //    if (Request.QueryString["id"] == null)
        //    {
        //        return RedirectToAction("Default404");
        //    }
        //    int sid = Convert.ToInt32(Request.QueryString["id"]);
        //    tbl_resume tbl_resume = db.tbl_resume.Find(sid);
        //    if (tbl_resume == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tbl_resume);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ResumeChange(DataModel model)
        //{
        //    if (model.Email != null && model.Designation != null && model.Resumedata != null && model.Title != null && model.KeyWords != null && model.Status != null)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            foreach (var item in model.Resumedata)
        //            {
        //                // Get the directory path based on designation
        //                string dirpath = Server.MapPath("/Content") + "/" + model.Designation;
        //                if (!Directory.Exists(dirpath))
        //                {
        //                    Directory.CreateDirectory(dirpath);
        //                }
        //                var filname = item.FileName;
        //                var filePath = Path.Combine(dirpath, filname);
        //                item.SaveAs(filePath);
        //                var tbl_resume = new tbl_resume();
        //                {
        //                    tbl_resume.Email = model.Email;
        //                    tbl_resume.Designation = model.Designation;
        //                    tbl_resume.Resume = filname;
        //                    tbl_resume.Title = model.Title;
        //                    tbl_resume.KeyWords = model.KeyWords;
        //                    tbl_resume.Status = model.Status;
        //                    tbl_resume.UploadedBy = model.UploadedBy;
        //                    tbl_resume.FilePath = filePath;
        //                }


        //                db.Entry(tbl_resume).State = EntityState.Modified;
        //                db.SaveChanges();
        //                // return RedirectToAction("DashBoard");
        //                ViewBag.msg = "Resume Added";
        //                Task.WaitAll(Task.Delay(2000));
        //                return RedirectToAction("UserData");
        //            }
        //            return View();
        //        }
        //        else
        //        {
        //            ViewBag.msg = "Something Wrong";
        //            return View();
        //        }
        //    }
        //    else
        //    {
        //        ViewBag.msg = "Add a resume or Press Back button";
        //        return View();
        //    }
        //}
        [Authorize]
        public ActionResult Users()
        {
            var users = db.tbl_User.ToList();
            return View(users);
        }
        public ActionResult ChangeStatus()
        {
            //string decodedEmail = HttpUtility.UrlDecode(Request.QueryString["id"]);
            var des = Request.QueryString["id"];
            tbl_Designation tbl_Designation = db.tbl_Designation.Find(des);
            if (tbl_Designation.Status.Equals(true))
            {
                tbl_Designation.Status = false;
            }
            else
            {
                tbl_Designation.Status = true;
            }
            db.Entry(tbl_Designation).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Designation");
        }
        public ActionResult ChangeStatusUser()
        {
            //string decodedEmail = HttpUtility.UrlDecode(Request.QueryString["id"]);
            var des = Request.QueryString["id"];
            tbl_User tbl_User = db.tbl_User.Find(des);
            if (tbl_User.Status.Equals(true))
            {
                tbl_User.Status = false;
            }
            else
            {
                tbl_User.Status = true;
            }
            db.Entry(tbl_User).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Users");
        }
        public ActionResult ChangeStatusResume()
        {
            var des = Convert.ToInt32(Request.QueryString["id"]);
            tbl_resume tbl_resume = db.tbl_resume.Find(des);
            if (tbl_resume.Status.Equals(true))
            {
                tbl_resume.Status = false;
            }
            else
            {
                tbl_resume.Status = true;
            }
            db.Entry(tbl_resume).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("UserData");
        }
        [Authorize]
        public ActionResult AllResumeData()
        {
            if (Request.QueryString["SearchString"] != null)
            {
                var searchResults = _pdfSearchService.SearchPdfDocuments(Request.QueryString["SearchString"]);
                var viewModel = new DashBoardModel
                {
                    Users = null, // No need to populate this for non-admin
                    Resumes = null,
                    Designations = null,
                    UserData = null,
                    Resumesdata = searchResults
                };

                return View(viewModel); // Return search results view
                /*
                using (var db = new AllprojectDataModel())
                {
                    string searchString = Request.QueryString["SearchString"]; // Replace with actual search string

                    var searchKeywords = searchString.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

                    var resumesContainingKeywords = db.tbl_resume
                                                     .Where(p => searchKeywords.Any(nk => p.KeyWords.Contains(nk)))
                                                     .ToList();
                    var viewModel = new DashBoardModel
                    {
                        Users = null, // No need to populate this for non-admin
                        Resumes = null,
                        Designations = null,
                        UserData = null,
                        Resumesdata = resumesContainingKeywords
                    };

                    return View(viewModel);
                }*/
            }
            else
            {
                var resume = db.tbl_resume.ToList();
                var viewModel = new DashBoardModel
                {
                    Users = null, // No need to populate this for non-admin
                    Resumes = null,
                    Designations = null,
                    UserData = null,
                    Resumesdata = resume
                };

                return View(viewModel);
            }
        }
        public ActionResult ChangeStatusAllresume()
        {
            var des = Convert.ToInt32(Request.QueryString["id"]);
            tbl_resume tbl_resume = db.tbl_resume.Find(des);
            if (tbl_resume.Status.Equals(true))
            {
                tbl_resume.Status = false;
            }
            else
            {
                tbl_resume.Status = true;
            }
            db.Entry(tbl_resume).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("AllResumeData");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddResume(DataModel model)
        {
            if (model.Email != null && model.Designation != null && model.Resumes != null && model.Title != null && model.KeyWords != null && model.Status != null)
            {
                if (ModelState.IsValid)
                {
                    foreach (var item in model.Resumes)
                    {
                        if (item.FileName.EndsWith(".pdf"))
                        {
                            //Get the directory path based on designation
                            string dirpath = Server.MapPath("/Content") + "/" + model.Designation;
                            if (!Directory.Exists(dirpath))
                            {
                                Directory.CreateDirectory(dirpath);
                            }
                            var filname = item.FileName;
                            var filePath = Path.Combine(dirpath, filname);
                            item.SaveAs(filePath);
                            var tbl_resume = new tbl_resume();
                            {
                                tbl_resume.Email = model.Email;
                                tbl_resume.Designation = model.Designation;
                                tbl_resume.Resume = filname;
                                tbl_resume.Title = model.Title;
                                tbl_resume.KeyWords = model.KeyWords;
                                tbl_resume.Status = model.Status;
                                tbl_resume.UploadedBy = model.UploadedBy;
                                tbl_resume.FilePath = filePath;
                            }     // Create new Product instance

                            db.tbl_resume.Add(tbl_resume);
                            db.SaveChanges();
                        }
                        else
                        {
                            ViewBag.msg = "Only Pdf File Acceptable..";
                           // return View();
                        }
                    }
                    ViewBag.msg = "Record Added";
                    Task.WaitAll(Task.Delay(2000));
                    return RedirectToAction("UserData");
                }
                else
                {
                    ViewBag.msg = "Something Wrong";
                    return View();
                }
            }
            else
            {
                ViewBag.msg = "All Fields Required";
                return View();
            }
        }
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(string Username, string password,string cpassword,string opassword)
        {
            
            if (string.IsNullOrEmpty(password))
            {
                ViewBag.Message = "New password cannot be empty.";
                ModelState.AddModelError("newPassword", "New password cannot be empty.");
                return View();
            }
            if (password == cpassword)
            {
                using (var context = new wholeProjectModel())
                {
                    var user = context.tbl_User.FirstOrDefault(u => u.Email == Username);
                    if (user != null)
                    {
                        if (user.password != opassword)
                        {
                            ViewBag.Message = "Check Old Password";
                            return View();
                        }
                        else
                        {
                            user.password = password;
                            context.SaveChanges();

                            ViewBag.Message = "Password changed successfully";
                            return RedirectToAction("Login");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Username", "User not found.");
                        ViewBag.Message = "Email not found.";
                        return View(); // Return to the view indicating user not found
                    }
                }
            }
            else {
                ViewBag.Message = "Password Not Matched";
                return View();
            }
        }
        //private string HashPassword(string password)
        //{
        //    // Implement a secure hashing algorithm like bcrypt, PBKDF2, etc.
        //    // For example purposes, this is just a simple demonstration
        //    return Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
        //}
        public ActionResult DefaultPage()
        {
            return View();
        }
        public ActionResult Default404()
        {
            return View();
        }
        [Authorize]
        public ActionResult UserProfile()
        {
            var query = db.tbl_User.FirstOrDefault(u => u.Email == User.Identity.Name);
            var viewModel = new DashBoardModel
            {
                Users = null, // No need to populate this for non-admin
                Resumes = null,
                Designations = null,
                UserData = query,
                Resumesdata = null

            };

            return View(viewModel);
        }
        public ActionResult ForgotPassword()
        {
            return View();
                
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(string Email)
        {
            using (var context = new wholeProjectModel())
            {
                var user = context.tbl_User.FirstOrDefault(u => u.Email == Email);
                if (user != null)
                {
                    Session["Email"] = user.Email;
                    Random rnd = new Random();
                    Session["otp"] = (rnd.Next(100000, 999999)).ToString();
                    MailMessage message = new MailMessage("nileshsrivastav9422@gmail.com", Email);
                    message.Subject = "Your Otp from Resume Portal";
                    message.Body = $"Hello {user.Name}<br/> your otp is {Session["otp"]}\n\n <br/>Thank you";
                    message.IsBodyHtml = true;

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential("nileshsrivastav9422@gmail.com", "lemu bbcy pydu cixz");
                    smtp.EnableSsl = true;
                    smtp.Send(message);
                    ViewBag.message = "Otp is Send Your Email..";
                    return RedirectToAction("VerifyOTP");
                }
                else
                {
                    ViewBag.message = "Email is Not Resistered";
                    return View();
                }
            }
               

        }
        public ActionResult VerifyOTP()
        {
            return View();
        }
            [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VerifyOTP(string OTP)
        {
            if (Session["otp"].Equals(OTP))
            {
                ViewBag.message = "OTP Verified";
                return RedirectToAction("forgotform");
            }
            else
            {
                ViewBag.message = "Not Matched";
                return View();
            }
        }
        public ActionResult forgotform()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult forgotform(string password,string cpassword,string username)
        {
            if (password == cpassword)
            {
                using (var context = new wholeProjectModel())
                {
                    var user = context.tbl_User.FirstOrDefault(u => u.Email == username);
                    if (user != null)
                    {
                       
                       
                            user.password = password;
                            context.SaveChanges();

                            ViewBag.Message = "Password changed successfully";
                            return RedirectToAction("Login");
                     
                    }
                    else
                    {
                        ModelState.AddModelError("Username", "User not found.");
                        ViewBag.Message = "Something Mismatch.";
                        return View(); // Return to the view indicating user not found
                    }
                }
            }
            else
            {
                ViewBag.Message = "Password Not Matched";
                return View();
            }
        }
    }
}