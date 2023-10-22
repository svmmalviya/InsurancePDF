using System.ComponentModel.DataAnnotations;

namespace PracticalProject.Models
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            this.Items = new List<PaymentTransaction>();
        }

        [Display(Name = "Id")]
        public Guid Id { get; set; }


        [Display(Name = "Coverage Amount")]
        [Required(ErrorMessage ="Coverage Amount is required.")]
        public double CoverageAmount { get; set; }

        [Display(Name = "Policy Number")]
        [Required(ErrorMessage = "Policy Number is required.")]
        public string PolicyNumber { get; set; }

        [Required(ErrorMessage = "Patient Name is required")]
        [Display(Name = "Patient Name")]
        public string PartyName { get; set; }
        [Required(ErrorMessage = "Date Of Birth is required.")]
        [Display(Name = "DOB")]
        public DateTime DOB { get; set; }
        public List<PaymentTransaction> Items { get; set; }
    }

    public class PaymentTransaction 
    {
        public Guid Id { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public DateTime PaymentDate { get; set; }
        public double amount { get; set; }
    }
}
