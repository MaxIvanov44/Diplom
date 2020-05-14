using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAccounting.Data.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAccounting.Data.Repositories.Implementations
{
    public class PracticRepository : BaseRepository<PracticEntityModel>, IPracticRepository
    {
        public PracticRepository(StudentAccountingContext context) : base(context) { }



        public PracticEntityModel DeletePracitc(Guid guid)
        {
            var practic = GetQuery().FirstOrDefault(p => p.Id == guid);
            Delete(practic);
            return practic;
        }

        public PracticEntityModel GetPractic(Guid id)
        {
            return GetQuery().FirstOrDefault(p => p.Id == id);
        }

        public PracticEntityModel UpdatePractic(PracticEntityModel entity)
        {
            Update(entity);
            return entity;
        }
    }
}
