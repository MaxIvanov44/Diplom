using StudentAccounting.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StudentAccounting.Logic.Services
{
    public interface IInstitutionService
    {

        InstitutionModel CreateInstitution(string name);
        InstitutionModel UpdateInstitution(InstitutionModel model);
        InstitutionModel DeleteInstitution(Guid id);
        Guid GetIdByName(string name);
        string GetNameById(Guid id);

    }
}
