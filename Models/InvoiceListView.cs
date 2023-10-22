using System.ComponentModel.DataAnnotations;

namespace PracticalProject.Models
{
   
    public class InvoiceListView
    {
        public InvoiceListView()
        {
            this.Invoices = new List<HomeViewModel>();
        }
        public List<HomeViewModel> Invoices { get; set; }
    }

}
