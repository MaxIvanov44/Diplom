using StudentAccounting.Data.EntityModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StudentAccounting.Data.Repositories
{
    public interface IInstitutionRepository : IBaseRepository<InstitutionEntityModel>
    {
        InstitutionEntityModel CreateInstitution(string name);
        InstitutionEntityModel UpdateInstitution(InstitutionEntityModel model);
        InstitutionEntityModel DeleteInstitution(Guid id);
        Guid GetIdByName(string name);
        string GetNameById(Guid id);
    }
}
