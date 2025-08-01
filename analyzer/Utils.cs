using System.Diagnostics;

namespace Roslynator.Analyzer
{
    public static class Utils
    {
        public static string? GetFirstFileLine(string filePath)
        {
            if (File.Exists(filePath))
            {
                return File.ReadLines(filePath).First();
            }

            return null;
        }

        public static bool RunProcess(string fileName, string arguments)
        {
            using Process process = new();
            process.StartInfo.FileName = fileName;
            process.StartInfo.Arguments = arguments;
            process.Start();
            process.WaitForExit();

            return process.ExitCode == 0;
        }

        public static bool AreFilesDifferent(string path1, string path2)
        {
            string[] lines1 = File.ReadAllLines(path1);
            string[] lines2 = File.ReadAllLines(path2);

            if (lines1.Length != lines2.Length)
            {
                return true;
            }

            for (int i = 0; i < lines1.Length; i++)
            {
                if (lines1[i] != lines2[i])
                {
                    return true;
                }
            }

            return false;
        }
    }
}