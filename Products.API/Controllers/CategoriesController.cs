namespace Products.API.Controllers
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Products.Domain;
    using System.Collections.Generic;
    using Products.API.Models;

    //  El Autorize obliga al usuario que este logueado para poder acceder al mismo \\
    //  [Authorize(Roles ="Admin")]
    //  [Authorize(Users = "carlos.e.herrera.j@gmail.com")]
    [Authorize]
    public class CategoriesController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/Categories
        //public IQueryable<Category> GetCategories()
        //{
        //    return db.Categories;
        //}

        public async Task<IHttpActionResult> GetCategory()
        {
            var categories = await db.Categories.ToListAsync();
            var categoriesResponse = new List<CategoryResponse>();

            //  Recorre cada Category y lo asigna al objeto CategoryResponse
            foreach (var category in categories)
            {
                //  Recorre el objeto Product dentro de cada categoria y lo asigna al ProductResponse
                var productsResponse = new List<ProductResponse>();
                foreach (var product in category.Products)
                {
                    productsResponse.Add(new ProductResponse
                    {
                        Description = product.Description,
                        Image = product.Image,
                        IsActive = product.IsActive,
                        LastPurchase = product.LastPurchase,
                        Price = product.Price,
                        ProductId = product.ProductId,
                        Remarks = product.Remarks,
                        Stock = product.Stock,
                    });
                }

                categoriesResponse.Add(new CategoryResponse
                {
                    CategoryId = category.CategoryId,
                    Description = category.Description,
                    Products = productsResponse,
                });
            }
            return Ok(categoriesResponse);
        }

        // GET: api/Categories/5
        [ResponseType(typeof(Category))]
        public async Task<IHttpActionResult> GetCategory(int id)
        {
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // PUT: api/Categories/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCategory(int id, Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != category.CategoryId)
            {
                return BadRequest();
            }

            db.Entry(category).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Categories
        [ResponseType(typeof(Category))]
        public async Task<IHttpActionResult> PostCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Categories.Add(category);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = category.CategoryId }, category);
        }

        // DELETE: api/Categories/5
        [ResponseType(typeof(Category))]
        public async Task<IHttpActionResult> DeleteCategory(int id)
        {
            Category category = await db.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            db.Categories.Remove(category);
            await db.SaveChangesAsync();

            return Ok(category);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoryExists(int id)
        {
            return db.Categories.Count(e => e.CategoryId == id) > 0;
        }
    }
}