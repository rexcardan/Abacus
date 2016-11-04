using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Abacus.Helper
{
    public static class FileHelper
    {
        public static byte[] ReadBytes(string file)
        {
            try
            {
                using (var fsSource = new FileStream(file,
                    FileMode.Open, FileAccess.Read))
                {
                    // Read the source file into a byte array.
                    var bytes = new byte[fsSource.Length];
                    var numBytesToRead = (int) fsSource.Length;
                    int numBytesRead = 0;
                    while (numBytesToRead > 0)
                    {
                        // Read may return anything from 0 to numBytesToRead.
                        int n = fsSource.Read(bytes, numBytesRead, numBytesToRead);

                        // Break when the end of the file is reached.
                        if (n == 0)
                            break;

                        numBytesRead += n;
                        numBytesToRead -= n;
                    }
                    return bytes;
                }
            }
            catch (FileNotFoundException ioEx)
            {
                Console.WriteLine(ioEx.Message);
            }
            return null;
        }

        public static string[] ReadLines(string file)
        {
            var lines = new List<string>();
            string line;
            // Read the file and display it line by line.
            var sr = new StreamReader(file);
            while ((line = sr.ReadLine()) != null)
            {
                lines.Add(line);
            }
            sr.Close();
            return lines.ToArray();
        }

        public static string[] ReadLines(byte[] bytes)
        {
            var lines = new List<string>();
            string line;
            // Read the file and display it line by line.
            var sr = new StreamReader(new MemoryStream(bytes), Encoding.ASCII);
            while ((line = sr.ReadLine()) != null)
            {
                lines.Add(line);
            }
            sr.Close();
            return lines.ToArray();
        }

        public static void WriteLines(string[] lines, string file)
        {
            TextWriter tw = new StreamWriter(file);
            foreach (string line in lines)
            {
                tw.WriteLine(line);
            }
            tw.Close();
        }
    }
}