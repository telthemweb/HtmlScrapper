using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace HtmlCloner
{
    class Program
    {
        private const string InputFileName = "problem.html"; 
        private const string OutputFileName = "problem1.txt";
        private const string Charset = "utf-8";
        private static Regex regexWhitespace = new Regex("\n\\s+");
        static void Main(string[] args)
        {
            if (!File.Exists(InputFileName)){
                Console.WriteLine(
                    "File " + InputFileName+" not found"
                    );
                return;
            }
            StreamReader reader = null;
            StreamWriter writer = null;
            try
            {
                Encoding ecoding = Encoding.GetEncoding(Charset);
                reader = new StreamReader(InputFileName, ecoding);
                writer = new StreamWriter(OutputFileName, false);

                RemoveHtmlTags(reader, writer);

            }
            catch (IOException)
            {
                Console.WriteLine("Cannot read file " + InputFileName + ".");
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                if (writer != null) { writer.Close(); }
            }
        }

        private static void RemoveHtmlTags(StreamReader reader, StreamWriter writer)
        {
            int openedTags = 0;
            StringBuilder buffer = new StringBuilder();
            while (true)
            {
                int nextChar = reader.Read();
                if(nextChar == -1)
                {
                    // End of file reached     
                    PrintBuffer(writer, buffer);
                    break;
                }
                char ch = (char)nextChar;
                if (ch == '<')
                {
                    if(openedTags == 0)
                    {
                        PrintBuffer(writer, buffer);
                        buffer.Length = 0;
                    }
                    openedTags++;
                }
                else if (ch == '>')
                {
                    openedTags--;
                }
                else
                {
                    // We aren't in tags (not "<" or ">")
                    if (openedTags == 0)
                    {
                        buffer.Append(ch);
                    }
                }
            }
        }

        private static void PrintBuffer(StreamWriter writer, StringBuilder buffer)
        {
            string str = buffer.ToString();
            string trimmed = str.Trim();
            string textOnly = regexWhitespace.Replace(trimmed, "\n");
            if (!string.IsNullOrEmpty(textOnly))
            {
                writer.WriteLine(textOnly);
            }
        }
    }
}
