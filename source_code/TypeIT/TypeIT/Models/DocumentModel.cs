using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeIT.Models
{
    public class DocumentModel
    {
        public string DocumentName { get; set; }
        public int TotalPageNumber { get; set; }
        public int UserPageNumber { get; set; }
        public double DocumentAccuracy { get; set; }
    }
}
