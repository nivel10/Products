namespace Products.BackEnd.Controllers
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using System.Net;
    using System.Web.Mvc;
    using Products.BackEnd.Models;
    using Products.Domain;
    using Products.BackEnd.Helpers;
    using System;

    //  El Autorize obliga al usuario que este logueado para poder acceder al mismo \\
    //  [Authorize(Roles ="Admin")]
    //  [Authorize(Users = "carlos.e.herrera.j@gmail.com")]
    [Authorize]

    public class ProductsController : Controller
    {
        private DataContextLocal db = new DataContextLocal();

        // GET: Products
        public async Task<ActionResult> Index()
        {
            var products = db.Products.Include(p => p.Category);
            return View(await products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Description");
            return View();
        }

        // POST: Products/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductView view)
        {
            if (ModelState.IsValid)
            {
                var pic = string.Empty;
                var folder = "~/Content/Images";

                if (view.ImageFile != null)
                {
                    pic = FilesHelper.UploadPhoto(view.ImageFile, folder);
                    pic = string.Format("{0}/{1}", folder, pic);
                }

                var product = ToProduct(view);
                product.Image = pic;
                db.Products.Add(product);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Description", view.CategoryId);
            return View(view);
        }

        // GET: Products/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Description", product.CategoryId);
            var view = ToView(product);
            return View(view);
        }

        // POST: Products/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProductView view)
        {
            if (ModelState.IsValid)
            {
                var pic = view.Image;
                var folder = "~/Content/Images";

                if (view.ImageFile != null)
                {
                    pic = FilesHelper.UploadPhoto(view.ImageFile, folder);
                    pic = string.Format("{0}/{1}", folder, pic);
                }

                var product = ToProduct(view);
                product.Image = pic;
                db.Entry(product).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "Description", view.CategoryId);
            return View(view);
        }

        // GET: Products/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Product product = await db.Products.FindAsync(id);
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Metodo que convierte el objeto ProductView wn Product
        /// </summary>
        /// <param name="view">Objeto de tipo ProductView</param>
        /// <returns>Objeto Product</returns>
        private Product ToProduct(ProductView view)
        {
            return new Product
            {
                Category = view.Category,
                CategoryId = view.CategoryId,
                Description = view.Description,
                Image = view.Image,
                IsActive = view.IsActive,
                ProductId = view.ProductId,
                Price = view.Price,
                LastPurchase = view.LastPurchase,
                Remarks = view.Remarks,
                Stock = view.Stock,
            };
        }


        /// <summary>
        /// Metodo que convierte el objeto Product a un ProductView
        /// </summary>
        /// <param name="product">Objeto Product</param>
        /// <returns>Objeto ProductView</returns>
        private ProductView ToView(Product product)
        {
            return new ProductView
            {
                Category = product.Category,
                CategoryId = product.CategoryId,
                Description = product.Description,
                Image = product.Image,
                IsActive = product.IsActive,
                ProductId = product.ProductId,
                Price = product.Price,
                LastPurchase = product.LastPurchase,
                Remarks = product.Remarks,
                Stock = product.Stock,
            };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
