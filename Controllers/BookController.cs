using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BooklistMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BooklistMVC.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _db;
        
        [BindProperty]
        public Book Book { get; set; }
        public BookController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            Book = new Book();
            //Create
            if (id==null)
            {
                return View(Book);
            }
            //Update
            Book = _db.Books.FirstOrDefault(u=>u.ID==id);
            if(Book==null)
            {
                return NotFound();
            }
            return View(Book);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Book Book)
        {
           if(ModelState.IsValid)
            {
                if(Book.ID==0)
                {
                    _db.Books.Add(Book);
                }
                else
                {
                    _db.Books.Update(Book);
                }
                _db.SaveChanges();
                return RedirectToAction("index");
              
            }
            return View(Book);
        }
        #region API Calls
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _db.Books.ToListAsync() });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var BookformDB = await _db.Books.FirstOrDefaultAsync(u => u.ID == id);
            if (BookformDB == null)
            {
                return Json(new { success = false, message = "error while Deleting" });
            }
            _db.Books.Remove(BookformDB);
            await _db.SaveChangesAsync();
            return Json(new { sucess = true, message = "Deleted Successfully" });
        }
        #endregion
    }
}
