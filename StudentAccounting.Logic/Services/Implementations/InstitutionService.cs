using StudentAccounting.Data.Repositories;
using StudentAccounting.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StudentAccounting.Logic.Services.Implementations
{
    public class InstitutionService : IInstitutionService
    {
        private readonly IInstitutionRepository _institutionRepository;
        public InstitutionService(IInstitutionRepository institutionRepository)
        {
            _institutionRepository = institutionRepository;
        }

        public InstitutionModel CreateInstitution(string name)
        {
            return _institutionRepository.CreateInstitution(name);
        }
        public InstitutionModel UpdateInstitution(InstitutionModel model)
        {
            var result = _institutionRepository.UpdateInstitution(model);
            return result;
        }
        public InstitutionModel DeleteInstitution(Guid id)
        {
            var result = _institutionRepository.DeleteInstitution(id);
            return result;
        }
        public Guid GetIdByName(string name)
        {
            return _institutionRepository.GetIdByName(name);
        }
        public string GetNameById(Guid id)
        {
            return _institutionRepository.GetNameById(id);
        }

    }
}
