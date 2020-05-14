using Microsoft.AspNetCore.Mvc;
using StudentAccounting.Data.EntityModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StudentAccounting.Data.Repositories
{
    public interface IPracticRepository
    {
        PracticEntityModel GetPractic(Guid id);
        PracticEntityModel UpdatePractic(PracticEntityModel entity);
        PracticEntityModel DeletePracitc(Guid guid);
    }
}
