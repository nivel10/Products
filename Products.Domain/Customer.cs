namespace Products.Domain
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Display(Name = "Customer Type")]
        public int CustomerType { get; set; }

        [Required(ErrorMessage = "The field {0} is required.")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        [Display(Name = "Firts Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The field {0} is required.")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The field {0} is required.")]
        [MaxLength(50, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        [Index("Customer_Email_Index", IsUnique = true)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [MaxLength(20, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [MaxLength(100, ErrorMessage = "The field {0} only can contain {1} characters lenght.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "The field {0} is required.")]
        [StringLength(20, ErrorMessage = "The field {0} must be have betwen {2} and {1} characters lenght.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        //  [NotMapped] = Se emplea para no guardar en la base de datos, pero si se recibe el campo por el api, ya que se va a encriptado
        //  Saca el campo de la base de datos, pero si es requerido en el modelo Este campo se va a guardar en la base de datos 
        [NotMapped]
        public string Password { get; set; }
    }
}
