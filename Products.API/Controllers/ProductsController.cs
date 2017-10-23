namespace Products.API.Controllers
{
    using Products.API.Helpers;
    using Products.API.Models;
    using Products.Domain;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;

    //  El Autorize obliga al usuario que este logueado para poder acceder al mismo \\
    //  [Authorize(Roles ="Admin")]
    //  [Authorize(Users = "carlos.e.herrera.j@gmail.com")]
    [Authorize]
    public class ProductsController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/Products
        public IQueryable<Product> GetProducts()
        {
            return db.Products;
        }

        // GET: api/Products/5
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> GetProduct(int id)
        {
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        //// PUT: api/Products/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutProduct(int id, Product product)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != product.ProductId)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(product).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!ProductExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        // PUT: api/Products/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProduct(int id, ProductRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //  Valida que ImageArray sedifrente de nulo y tenga almenos un byte
            if (request.ImageArray != null && request.ImageArray.Length > 0)
            {
                //  Transforma el ImageArray en MemoryStream
                var stream = new MemoryStream(request.ImageArray);
                //  Genra un string con un Guid
                var guid = Guid.NewGuid().ToString();
                //  Crea el nombre del archivo concatenando la extencion .jpg
                var file = string.Format("{0}.jpg", guid);
                //  Crea la ruta del FolderPath
                var folder = "~/Content/Images";
                //  Crea la ruta FullPath
                var fullPath = string.Format("{0}/{1}", folder, file);
                //  Invoca el metodo del FileHelper (Recibe un MemoryStream y retorna un Array byte[]
                var response = FilesHelper.UploadPhoto(stream, folder, file);

                //  Valida si el metodo anterior falló
                if (response)
                {
                    request.Image = fullPath;
                }
            }

            if (id != request.ProductId)
            {
                return BadRequest();
            }

            db.Entry(request).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Products
        //[ResponseType(typeof(Product))]
        //public async Task<IHttpActionResult> PostProduct(Product product)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Products.Add(product);
        //    await db.SaveChangesAsync();

        //    return CreatedAtRoute("DefaultApi", new { id = product.ProductId }, product);
        //}
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> PostProduct(ProductRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //  Valida que ImageArray sedifrente de nulo y tenga almenos un byte
            if (request.ImageArray != null && request.ImageArray.Length > 0)
            {
                //  Transforma el ImageArray en MemoryStream
                var stream = new MemoryStream(request.ImageArray);
                //  Genra un string con un Guid
                var guid = Guid.NewGuid().ToString();
                //  Crea el nombre del archivo concatenando la extencion .jpg
                var file = string.Format("{0}.jpg", guid);
                //  Crea la ruta del FolderPath
                var folder = "~/Content/Images";
                //  Crea la ruta FullPath
                var fullPath = string.Format("{0}/{1}", folder, file);
                //  Invoca el metodo del FileHelper (Recibe un MemoryStream y retorna un Array byte[]
                var response = FilesHelper.UploadPhoto(stream, folder, file);

                //  Valida si el metodo anterior falló
                if (response)
                {
                    request.Image = fullPath;
                }
            }

            var product = ToProduct(request);
            db.Products.Add(product);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null &&
                    ex.InnerException.InnerException != null &&
                    ex.InnerException.InnerException.Message.Contains("Index"))
                {
                    return BadRequest("There are a record with the same description.");
                }
                else
                {
                    return BadRequest(ex.Message);
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = product.ProductId }, product);
        }

        // DELETE: api/Products/5
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> DeleteProduct(int id)
        {
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            await db.SaveChangesAsync();

            return Ok(product);
        }

        #region Methods

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.ProductId == id) > 0;
        }

        private Product ToProduct(ProductRequest request)
        {
            return new Product
            {
                Category = request.Category,
                CategoryId = request.CategoryId,
                Description = request.Description,
                Image = request.Image,
                IsActive = request.IsActive,
                LastPurchase = request.LastPurchase,
                Price = request.Price,
                ProductId = request.ProductId,
                Remarks = request.Remarks,
                Stock = request.Stock,
            };
        } 

        #endregion Methods
    }
}