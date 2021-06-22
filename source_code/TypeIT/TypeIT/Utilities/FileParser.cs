using IronPdf;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TypeIT.Models;

namespace TypeIT.Utilities
{
    internal class FileParser
    {
        /// <summary>
        /// Creating a new document from the users input.
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <param name="pageNum"></param>
        /// <param name="text"></param>
        private void CreateDocument(string fileLocation, int pageNum, string text)
        {
            System.IO.File.WriteAllText(fileLocation + pageNum + ".txt", text.Trim());
        }

        /// <summary>
        /// Opening the document that the user had added.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private string OpenDocument(string location)
        {
            string contents;

            using (var sr = new StreamReader(location))
            {
                contents = sr.ReadToEnd();
            }

            return contents;
        }

        /// <summary>
        /// Getting the file extension of the Users document. 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private string GetFileExtension(string filePath)
        {
            return Path.GetExtension(filePath);
        }

        /// <summary>
        /// Parsing the users document depening on what type of document it is. 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="title"></param>
        public async Task<DocumentModel> ParseFile(string filePath, string title)
        {
            DocumentModel model;
            Task t;

            string dir = @"../../../Documents/" + title + "/";
            string docDir = @"../../../Documents/" + title;

            switch (GetFileExtension(filePath))
            {
                case ".pdf":
                    t = Task.Run(() => ParsePDF(filePath, dir));
                    break;
                case ".txt":
                    t = Task.Run(() => ParseTXT(filePath, dir));
                    break;
                default:
                    throw new Exception("That file type is not supported.");
            }

            model = await t.ContinueWith(antecendent =>
            {
                model = new DocumentModel(docDir);
                return model;
            });

            model.Name = title;

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
        /// <param name="filePath">Direct Path to the file</param>
        /// <param name="storagePath">Direct Path to where the file should be stored</param>
        public bool ParsePDF(string filePath, string storagePath)
        {
            PdfDocument pdf = PdfDocument.FromFile(filePath);

            if (!Directory.Exists(storagePath))
            {
                Directory.CreateDirectory(storagePath);
            }

            for (int i = 0; i < pdf.PageCount; i++)
            {
                string text = pdf.ExtractTextFromPage(i);
                if (text != "")
                {
                    CreateDocument(storagePath, i, RemoveSpecialCharacters(text));
                }
            }

            return true;
        }

        /// <summary>
        /// Parsing TXT files
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="storagePath"></param>
        public bool ParseTXT(string filePath, string storagePath)
        {
            string text = OpenDocument(filePath);

            if (!Directory.Exists(storagePath))
            {
                Directory.CreateDirectory(storagePath);
            }

            CreateDocument(storagePath, 0, RemoveSpecialCharacters(text));

            return true;
        }
    }
}
