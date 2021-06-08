using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeIT.Models
{
    public class DocumentModel
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public List<PageModel> Pages { get; set; }
        public int UserPageNumber { get; set; }
        public double Accuracy { get; set; }

        public DocumentModel(string location)
        {
            Location = location;
            Pages = new List<PageModel>();

            foreach (string file in Directory.GetFiles(location, ""))
            {
                PageModel page = new PageModel(file);
                Pages.Add(page);
            }

            Pages = Pages.OrderBy(o => o.Number).ToList();
        }

        public DocumentModel()
        {
            
        }

        public PageModel GetPageByPageNumber(int pageNumber)
        {
            try
            {
                return Pages[pageNumber];
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int GetNumberOfPages()
        {
            return Pages.Count;
        }
    }
}
