using CommandLine;

namespace Roslynator.Analyzer
{
    static class Program
    {
        public sealed class Options
        {
            [Option("repository")]
            public string? RepositoryPath { get; set; }
        }

        static async Task Main(string[] args)
        {
            try
            {
                Options options = Parser.Default.ParseArguments<Options>(args).Value;
                if (options.RepositoryPath is not null)
                {
                    await Flow.Run(options.RepositoryPath);
                }
            }
            catch (Exception ex)
            {
                Environment.ExitCode = 0;
                Console.WriteLine(ex);
            }
        }
    }
}