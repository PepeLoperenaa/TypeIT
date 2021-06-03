using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeIT.Objects
{
    class Document
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public List<Page> Pages { get; set; }

        public Document(string location)
        {
            Location = location;
            Pages = new List<Page>();

            foreach (string file in Directory.GetFiles(location, ""))
            {
                Page page = new Page(file);
                Pages.Add(page);
            }

            Pages = Pages.OrderBy(o => o.Number).ToList();
        }

        public Page GetPageByPageNumber(int pageNumber)
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
