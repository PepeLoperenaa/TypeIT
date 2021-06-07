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
            switch (GetFileExtension(FilePath))
            {
                case ".pdf":
                    ParsePDF(FilePath, Title);
                    break;
                case ".txt":
                    ParseTXT(FilePath, Title);
                    break;
                case ".docx":
                    ParseDOCX(FilePath, Title);
                    break;
                default:
                    throw new Exception("That file type is not supported.");
            }
        }
        private static string RemoveSpecialCharacters(string str)
        {
            str = str.Replace('\u2018', '\'').Replace('\u2019', '\'').Replace('\u201c', '\"').Replace('\u201d', '\"').Replace("\u2026", "...").Replace("\r\n", " ").Replace('—', '-').Replace(@"\s+", " ");
            str = Regex.Replace(str, @"\s+", " ");
            str = Regex.Replace(str, @"[^a-zA-Z0-9`!@#$%^&*()_+|\-=\\{}\[\]:"";'<>?,./\r\n\'' ]+", "", RegexOptions.Compiled);
            return str;
        }

        public void ParsePDF(string FilePath, string Title)
        {
            PdfDocument pdf = PdfDocument.FromFile(FilePath);

            string dir = "../../Documents/" + Title + "/";

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            for (int i = 0; i < pdf.PageCount; i++)
            {
                string Text = pdf.ExtractTextFromPage(i);
                if (Text != "")
                {
                    CreateDocument(dir, i, RemoveSpecialCharacters(Text));
                }
            }
        }

        public void ParseTXT(string FilePath, string Title)
        {
            string Text = OpenDocument(FilePath);

            string dir = "../../Documents/" + Title + "/";

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            CreateDocument(dir, 0, RemoveSpecialCharacters(Text));
        }

        public void ParseDOCX(string FilePath, string Title)
        {
            string Text = OpenDocument(FilePath);

            string dir = "../../Documents/" + Title + "/";

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            //Opens the Word template document
            using (WordDocument document = new WordDocument(FilePath)) //template.docx does not exist here we should change this.
            {
                //Gets the string that contains whole document content as text
                string text = document.GetText();
                //Create a new text file and write specified string in it
                File.WriteAllText("Result.txt", text);
            }
            System.Diagnostics.Process.Start("Result.txt"); // this is a test to see if we get the txt file.
        }
    }
}
