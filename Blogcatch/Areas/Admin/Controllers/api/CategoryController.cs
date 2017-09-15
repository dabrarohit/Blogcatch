using AutoMapper;
using Blogcatch.Areas.Admin.Models;
using Blogcatch.Areas.Admin.Models.dto;
using Blogcatch.AutoMapper;
using Blogcatch.Models;
using System.Linq;
using System.Web.Http;
using System.Web.Routing;

namespace Blogcatch.Areas.Admin.Controllers.api
{
    [RoutePrefix("Admin/Api/Category")]
    public class CategoryController : ApiController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CategoryController()
        {
            _context = new ApplicationDbContext();
            _mapper = WebApiMapper.GetBlogMapper();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }


        // POST : admin/api/blog/AddCategory
        [Route("AddCategory")]
        [HttpPost]
        public IHttpActionResult AddCategory([FromBody]CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var category = new Category(categoryDto.Name);

            if (_context.Categories.Any(c => c.Name == category.Name))
            {
                return Ok("titleTaken");
            }

            _context.Categories.Add(category);
            _context.SaveChanges();
            return Ok(category.Id);
        }

        // POST : admin/api/blog/ReorderCategories
        [Route("ReorderCategories")]
        [HttpPost]
        public IHttpActionResult ReorderCategories([FromBody] int[] id)
        {
            int sortingOrder = 0;
            foreach (var catId in id)
            {
                var category = _context.Categories.Find(catId);
                if (category == null)
                {
                    return NotFound();
                }
                category.Sorting = sortingOrder;
                _context.SaveChanges();
                sortingOrder++;
            }
            return Ok();
        }

        // DELETE : admin/api/blog/DeleteCategory/id
        [Route("DeleteCategory/{id}")]
        [HttpDelete]
        public IHttpActionResult DeleteCategory([FromUri] int id)
        {
            if (id == 1)
                return BadRequest();

            var category = _context.Categories.Find(id);

            if (category == null)
                return NotFound();

            _context.Categories.Remove(category);
            _context.SaveChanges();
            return Ok(id);
        }

    }
}
