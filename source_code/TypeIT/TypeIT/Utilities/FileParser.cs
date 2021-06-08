using IronPdf;
using Syncfusion.DocIO.DLS;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace TypeIT.Utilities
{
    class FileParser
    {
        private void CreateDocument(string FileLocation, int PageNum, string Text)
        {
            System.IO.File.WriteAllText(FileLocation + PageNum + ".txt", Text);
        }

        private string OpenDocument(string Location)
        {
            string contents;

            using (var sr = new StreamReader(Location))
            {
                contents = sr.ReadToEnd();
            }

            return contents;
        }

        private string GetFileExtension(string FilePath)
        {
            return Path.GetExtension(FilePath);
        }

        public void ParseFile(string FilePath, string Title)
        {
            string dir = "../../Documents/" + Title + "/";

            switch (GetFileExtension(FilePath))
            {
                case ".pdf":
                    ParsePDF(FilePath, dir);
                    break;
                case ".txt":
                    ParseTXT(FilePath, dir);
                    break;
                case ".docx":
                    ParseDOCX(FilePath, dir);
                    break;
                default:
                    throw new Exception("That file type is not supported.");
            }
        }
        private static string RemoveSpecialCharacters(string str)
        {
            // replace characters with counter-parts on keyboard
            str = str.Replace('\u2018', '\'').Replace('\u2019', '\'').Replace('\u201c', '\"').Replace('\u201d', '\"').Replace("\u2026", "...").Replace("\r\n", " ").Replace('—', '-').Replace(@"\s+", " ");
            
            // replace multiple spaces with single ones
            str = Regex.Replace(str, @"\s+", " ");

            // remove characters which aren't on the keyboard (qwerty)
            str = Regex.Replace(str, @"[^a-zA-Z0-9`!@#$%^&*()_+|\-=\\{}\[\]:"";<>?,./\r\n\'' ]+", "", RegexOptions.Compiled);

            return str;
        }

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

        public void ParseTXT(string FilePath, string StoragePath)
        {
            string Text = OpenDocument(FilePath);

            if (!Directory.Exists(StoragePath))
            {
                Directory.CreateDirectory(StoragePath);
            }

            CreateDocument(StoragePath, 0, RemoveSpecialCharacters(Text));
        }

        public void ParseDOCX(string FilePath, string StoragePath)
        {
            string Text = OpenDocument(FilePath);

            if (!Directory.Exists(StoragePath))
            {
                Directory.CreateDirectory(StoragePath);
            }

            // opens the Word template document
            using (WordDocument document = new WordDocument(FilePath)) // template.docx does not exist here we should change this.
            {
                // gets the string that contains whole document content as text
                string text = document.GetText();

                // cleanse the file of its sins
                RemoveSpecialCharacters(text);

                // create a new text file and write specified string in it
                File.WriteAllText("Result.txt", text);
            }
            System.Diagnostics.Process.Start("Result.txt"); // this is a test to see if we get the txt file.
        }
    }
}
