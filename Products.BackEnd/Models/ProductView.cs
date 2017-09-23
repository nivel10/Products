namespace Products.BackEnd.Models
{
    using Products.Domain;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web;

    //  Evita que esta clase se vaya a base de datos, ya que solo se emplea para el campo ImageFile \\
    [NotMapped]
    public class ProductView : Product
    {
        [Display(Name ="Image")]
        public HttpPostedFileBase ImageFile { get; set; }
    }
}