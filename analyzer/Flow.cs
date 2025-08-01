using Roslynator.Analyzer.Diagnostics;
using Roslynator.Analyzer.Enums;
using Roslynator.Analyzer.Helpers;
using Roslynator.Analyzer.Models;

namespace Roslynator.Analyzer
{
    public static class Flow
    {
        public static async Task Run(string repositoryPath)
        {
            Utils.RunProcess("dotnet", "nuget --version");
            Print(MessageLevel.None, "Roslynator Command Line");
            Utils.RunProcess("roslynator", "--version");
            Print(MessageLevel.None, "Protolint Command Line");
            Utils.RunProcess("protolint", "version");

            var repository = Repository.Parse(repositoryPath);
            if (await CheckErrors(repository))
            {
                Environment.ExitCode = 1;
            }
            else
            {
                foreach (SolutionData solution in repository.Solutions)
                {
                    RestoreNuget(solution);
                    CheckSln(solution.Sln);

                    if (solution.HasProtos)
                    {
                        CheckProtos(solution.Directory);
                    }
                }
            }

            Console.WriteLine();
            if (Environment.ExitCode == 1)
            {
                Print(MessageLevel.Error, "Проверка завершена c ошибками");
            }
            else
            {
                Print(MessageLevel.Command, "Проверка завершена");
            }
        }

        private static async Task<bool> CheckErrors(Repository repository)
        {
            List<Message> messages = [];

            messages.AddRange(new RepositoryAnalyzer().Analyze(repository));

            foreach (SolutionData solution in repository.Solutions)
            {
                messages.AddRange(new SolutionAnalyzer().Analyze(solution));
                messages.AddRange(new ComponentAnalyzer().Analyze(solution.Directory));
            }

            if (messages.Any(x => x.Level == MessageLevel.Error))
            {
                Print(messages);

                return true;
            }

            Task<IReadOnlyCollection<Message>>[] tasks = repository.CsprojPaths
                .Select(csproj => new ProjectAnalyzer().Analyze(csproj))
                .ToArray();

            await Task.WhenAll(tasks);
            messages.AddRange(tasks.SelectMany(x => x.Result));

            Print(messages);

            return messages.Any(x => x.Level == MessageLevel.Error);
        }

        private static void RestoreNuget(SolutionData solution)
        {
            Print(MessageLevel.Section, $"dotnet restore {solution.Sln}");

            if (!Utils.RunProcess("dotnet", $"restore \"{solution.Sln}\""))
            {
                Print(MessageLevel.Error, "NuGet пакеты не восстановлены");
                Environment.ExitCode = 1;
            }
        }

        private static void CheckSln(string sln)
        {
            Print(MessageLevel.Section, "roslynator analyze");

            if (!Utils.RunProcess("roslynator", $"analyze \"{sln}\" --ignore-compiler-diagnostics --severity-level error"))
            {
                Environment.ExitCode = 1;
            }
        }

        private static void CheckProtos(string directory)
        {
            Print(MessageLevel.Section, $"protolint lint {directory}");

            if (!Utils.RunProcess("protolint", $"lint -config_path={LocalFileHelper.ProtolintPath} \"{directory}\""))
            {
                Environment.ExitCode = 1;
            }
        }

        private static void Print(IEnumerable<Message> messages)
        {
            foreach (Message message in messages)
            {
                Print(message.Level, message.Text);
            }
        }

        private static void Print(MessageLevel level, string? message)
        {
            // https://learn.microsoft.com/en-us/azure/devops/pipelines/scripts/logging-commands?view=azure-devops&tabs=bash#formatting-commands
            (string azureFormattingCommand, Console.ForegroundColor) = level switch
            {
                MessageLevel.None => ("", Console.ForegroundColor),
                MessageLevel.Warning => ("##[warning]", ConsoleColor.DarkYellow),
                MessageLevel.Error => ("##[error]", ConsoleColor.Red),
                MessageLevel.Section => ("##[section]", ConsoleColor.Green),
                MessageLevel.Debug => ("##[debug]", ConsoleColor.Magenta),
                MessageLevel.Command => ("##[command]", ConsoleColor.DarkCyan),
            };

            if (level == MessageLevel.Section)
            {
                Console.WriteLine();
            }

            Console.WriteLine(azureFormattingCommand + message);
            Console.ResetColor();
        }
    }
}