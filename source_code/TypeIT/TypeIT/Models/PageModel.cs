using System;
using System.IO;

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
                Number = int.Parse(Path.GetFileNameWithoutExtension(location));

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
