using Microsoft.AspNetCore.Mvc;
using PracticalProject.Models;

namespace PracticalProject.Interface_and_repos
{
    public interface IPatientTransactions
    {
        public FileContentResult GeneratePDF(HomeViewModel model);
        public bool AddPatient(HomeViewModel model);
        public HomeViewModel UpdatePatientById(HomeViewModel model);
        public HomeViewModel EditPatientById(string id);
        public HomeViewModel GetPatientById(string id);
        public List<HomeViewModel> GetPatients();
        public bool RemovePatient(string id);

    }
}
