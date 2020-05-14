using Microsoft.AspNetCore.Mvc;
using StudentAccounting.Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StudentAccounting.Logic.Services
{
    public interface IPracticService
    {

        PracticModel GetPractic(Guid id);
        PracticModel UpdatePractic(PracticModel practic);
        PracticModel DeletePractic(Guid id);
    }
}
