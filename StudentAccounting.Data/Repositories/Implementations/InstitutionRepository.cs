using StudentAccounting.Data.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAccounting.Data.Repositories.Implementations
{
    public class InstitutionRepository : BaseRepository<InstitutionEntityModel>, IInstitutionRepository
    {
        public InstitutionRepository(StudentAccountingContext context) : base(context) { }

        public InstitutionEntityModel CreateInstitution(string name)
        {
            var newInst = new InstitutionEntityModel { Name = name };
            Insert(newInst);
            return newInst;

        }
        public InstitutionEntityModel UpdateInstitution(InstitutionEntityModel model)
        {
            Update(model);
            return model;
        }
        public InstitutionEntityModel DeleteInstitution(Guid id)
        {
            var institution = GetQuery().FirstOrDefault(inst => inst.Id == id);
            Delete(institution);
            return institution;
        }
        public Guid GetIdByName(string name)
        {
            return GetQuery().FirstOrDefault(inst => inst.Name == name).Id;
        }
        public string GetNameById(Guid id)
        {
            return GetQuery().FirstOrDefault(inst => inst.Id == id).Name;
        }
    }
}
