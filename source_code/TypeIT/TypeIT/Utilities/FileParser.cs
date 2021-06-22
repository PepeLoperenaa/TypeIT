using IronPdf;
using Syncfusion.DocIO.DLS;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TypeIT.Models;

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
            System.IO.File.WriteAllText(FileLocation + PageNum + ".txt", Text.Trim());
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
        public async Task<DocumentModel> ParseFile(string FilePath, string Title)
        {
            DocumentModel model;
            Task t;

            string dir = @"../../../Documents/" + Title + "/";
            string docDir = @"../../../Documents/" + Title;

            switch (GetFileExtension(FilePath))
            {
                case ".pdf":
                    t = Task.Run(() => ParsePDF(FilePath, dir));
                    break;
                case ".txt":
                    t = Task.Run(() => ParseTXT(FilePath, dir));
                    break;
                default:
                    throw new Exception("That file type is not supported.");
            }

            model = await t.ContinueWith(antecendent => 
            { 
                model = new DocumentModel(docDir); 
                return model; 
            });

            model.Name = Title;

            return model;
        }

        /// <summary>
        /// Removing the special characters from the text that has being parsed. 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string RemoveSpecialCharacters(string str)
        {
            // replace characters with counter-parts on keyboard
            // Replacing them individually like this, since there are no inbuilt / online methods for autmatically replacing special characters for their asii counterparts
            str = str.Replace('\u2018', '\'').Replace('\u2019', '\'').Replace('\u201c', '\"').Replace('\u201d', '\"').
                Replace("\u2026", "...").Replace("\r\n", " ").Replace('—', '-').Replace(@"\s+", " ");
            
            // replace multiple spaces with single ones
            str = Regex.Replace(str, @"\s+", " ");

            // remove characters which aren't on the keyboard (qwerty)
            str = Regex.Replace(str, @"[^a-zA-Z0-9`!@#$%^&*()_+|\-=\\{}\[\]:"";<>?,./\r\n\'' ]+", "", RegexOptions.Compiled);

            str = Regex.Replace(str, @"\s+", " ");

            return str;
        }

        /// <summary>
        /// parsing PDF files.
        /// </summary>
        /// <param name="FilePath">Direct Path to the file</param>
        /// <param name="StoragePath">Direct Path to where the file should be stored</param>
        public bool ParsePDF(string FilePath, string StoragePath)
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

            return true;
        }

        /// <summary>
        /// Parsing TXT files
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="StoragePath"></param>
        public bool ParseTXT(string FilePath, string StoragePath)
        {
            string Text = OpenDocument(FilePath);

            if (!Directory.Exists(StoragePath))
            {
                Directory.CreateDirectory(StoragePath);
            }

            CreateDocument(StoragePath, 0, RemoveSpecialCharacters(Text));

            return true;
        }
    }
}
