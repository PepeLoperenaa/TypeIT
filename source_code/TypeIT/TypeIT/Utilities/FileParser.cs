using IronPdf;
using Syncfusion.DocIO.DLS;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TypeIT.Utilities
{
    class FileParser
    {
        /// <summary>
        /// Creating a new document from the users input.
        /// </summary>
        /// <param name="FileLocation"></param>
        /// <param name="PageNum"></param>
        /// <param name="Text"></param>
        private void CreateDocument(string FileLocation, int PageNum, string Text)
        {
            System.IO.File.WriteAllText(FileLocation + PageNum + ".txt", Text);
        }

        /// <summary>
        /// Opening the document that the user had added.
        /// </summary>
        /// <param name="Location"></param>
        /// <returns></returns>
        private string OpenDocument(string Location)
        {
            string contents;

            using (var sr = new StreamReader(Location))
            {
                contents = sr.ReadToEnd();
            }

            return contents;
        }

        /// <summary>
        /// Getting the file extension of the Users document. 
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        private string GetFileExtension(string FilePath)
        {
            return Path.GetExtension(FilePath);
        }

        /// <summary>
        /// Parsing the users document depening on what type of document it is. 
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="Title"></param>
        public void ParseFile(string FilePath, string Title)
        {
            string dir = @"../../../Documents/" + Title + "/";

            switch (GetFileExtension(FilePath))
            {
                case ".pdf":
                    new Task(() => ParsePDF(FilePath, dir)).Start();
                    break;
                case ".txt":
                    new Task(() => ParseTXT(FilePath, dir)).Start();
                    break;
                case ".docx":
                    new Task(() => ParseDOCX(FilePath, dir)).Start();
                    break;
                default:
                    throw new Exception("That file type is not supported.");
            }
        }

        /// <summary>
        /// Removing the special characters from the text that has being parsed. 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string RemoveSpecialCharacters(string str)
        {
            // replace characters with counter-parts on keyboard
            str = str.Replace('\u2018', '\'').Replace('\u2019', '\'').Replace('\u201c', '\"').Replace('\u201d', '\"').
                Replace("\u2026", "...").Replace("\r\n", " ").Replace('—', '-').Replace(@"\s+", " ");
            
            // replace multiple spaces with single ones
            str = Regex.Replace(str, @"\s+", " ");

            // remove characters which aren't on the keyboard (qwerty)
            str = Regex.Replace(str, @"[^a-zA-Z0-9`!@#$%^&*()_+|\-=\\{}\[\]:"";<>?,./\r\n\'' ]+", "", RegexOptions.Compiled);

            return str;
        }

        /// <summary>
        /// parsing PDF files.
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="StoragePath"></param>
        public void ParsePDF(string FilePath, string StoragePath)
        {
            PdfDocument pdf = PdfDocument.FromFile(FilePath);

            if (!Directory.Exists(StoragePath))
            {
                Directory.CreateDirectory(StoragePath);
            }

            for (int i = 0; i < pdf.PageCount; i++)
            {
                string Text = pdf.ExtractTextFromPage(i);
                if (Text != "")
                {
                    CreateDocument(StoragePath, i, RemoveSpecialCharacters(Text));
                }
            }
        }

        /// <summary>
        /// Parsing TXT files
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="StoragePath"></param>
        public void ParseTXT(string FilePath, string StoragePath)
        {
            string Text = OpenDocument(FilePath);

            if (!Directory.Exists(StoragePath))
            {
                Directory.CreateDirectory(StoragePath);
            }

            CreateDocument(StoragePath, 0, RemoveSpecialCharacters(Text));
        }

        /// <summary>
        /// Parsing Word document files
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="StoragePath"></param>
        public void ParseDOCX(string FilePath, string StoragePath)
        {
            string text;

            if (!Directory.Exists(StoragePath))
            {
                Directory.CreateDirectory(StoragePath);
            }

            // opens the Word template document
            using WordDocument document = new WordDocument(FilePath); // template.docx does not exist here we should change this.
                                                                      // gets the string that contains whole document content as text
            text = document.GetText();

            // cleanse the file of the unnecessary charactest
            RemoveSpecialCharacters(text);

            // create a new text file and write specified string in it
            File.WriteAllText("Result.txt", text);
        }
    }
}
