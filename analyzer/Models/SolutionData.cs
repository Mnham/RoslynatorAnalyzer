using Microsoft.Build.Construction;

namespace Roslynator.Analyzer.Models
{
    public sealed class SolutionData
    {
        public required string Sln { get; init; }
        public required string Directory { get; init; }
        public required string? PreviousDirectory { get; init; }
        public required SolutionFile File { get; init; }
        public required bool HasProtos { get; init; }
    }
}