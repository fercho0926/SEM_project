using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SEM_project.Models.Entities;

namespace SEM_project.Models
{
    public class Balance_ReferenceByuser
    {
        public Balance Balance { get; set; }
        public ReferedByUser ReferedByUser { get; set; }
    }
}