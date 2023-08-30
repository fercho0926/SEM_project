using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEM_project.Services
{
    public interface IDocumentService
    {
        byte[] GeneratePdfFromString();
    }
}
