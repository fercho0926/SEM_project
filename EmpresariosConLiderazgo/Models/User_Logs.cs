﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpresariosConLiderazgo.Models
{
    public class Logs : BaseClass
    {
        public DateTime Date { get; set; }
        public string? User { get; set; }
        public string? Action { get; set; }
        public string? Notes { get; set; }
    }
}
