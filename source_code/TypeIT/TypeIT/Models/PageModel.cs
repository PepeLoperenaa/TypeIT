using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeIT.Models
{
    public class PageModel
    {
        public string FileLocation { get; set; }
        public string Text { get; set; }
        public int Number { get; set; }

        public PageModel(string location)
        {
            FileLocation = location;
            Text = File.ReadAllText(location);
            try
            {
                Number = Int32.Parse(Path.GetFileNameWithoutExtension(location));

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
