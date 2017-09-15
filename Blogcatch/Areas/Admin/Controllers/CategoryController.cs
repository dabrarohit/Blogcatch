using Blogcatch.Areas.Admin.Models.AjaxUtilities;
using Blogcatch.Models;
using Blogcatch.ViewModel.Admin;
using System.Linq;
using System.Web.Mvc;

namespace Blogcatch.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: Admin/Blog/Categories
        public ActionResult Categories()
        {
            var categoryVMList = _context.Categories.ToArray().Select(x => new CategoryViewModel(x)).OrderBy(x => x.Sorting).ToList();
            return View(categoryVMList);
        }

        // POST: Admin/Blog/Categories/RenameCategory
        [HttpPost]
        public JsonResult RenameCategory(int id, string newCatName)
        {
            if (_context.Categories.Any(x => x.Name == newCatName))
                return Json(JsonResponseFactory.ErrorResponse("That category name is already taken!"));

            var category = _context.Categories.SingleOrDefault(x => x.Id == id);

            if (category == null)
                return Json(JsonResponseFactory.ErrorResponse("Server error: category can not be found."));

            category.Name = newCatName;
            category.Slug = newCatName.Replace(" ", "-").ToLower();

            _context.SaveChanges();


            return Json(JsonResponseFactory.SuccessResponse());
        }
    }
}