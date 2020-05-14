using Microsoft.AspNetCore.Mvc;
using StudentAccounting.Common.Enums;
using StudentAccounting.Common.Models;
using StudentAccounting.Data.EntityModels;
using StudentAccounting.Data.Repositories;
using StudentAccounting.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StudentAccounting.Logic.Services.Implementations
{
    public class PracticService : IPracticService
    {
        private readonly IPracticRepository _practicRepository;
        public PracticService(IPracticRepository practicRepository)
        {
            _practicRepository = practicRepository;
        }


        public PracticModel DeletePractic(Guid id)
        {
            return _practicRepository.DeletePracitc(id);
        }

        public PracticModel GetPractic(Guid id)
        {
            return _practicRepository.GetPractic(id);
        }

        public PracticModel UpdatePractic(PracticModel practic)
        {
            //practic.FillingDate = DateTime.Now;
            practic.ItogCriteriaMark = PracticModel.GetItogMark(practic);
            var result = _practicRepository.UpdatePractic(practic);
            return result;
        }

    }
}
