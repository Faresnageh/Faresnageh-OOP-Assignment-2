namespace SrpLab;

/// <summary>
/// Grade book: aggregation, letter-band policy, and transcript export.
/// </summary>
public sealed class GradeBook
{
    private readonly Dictionary<string, List<decimal>> _scores = new(StringComparer.OrdinalIgnoreCase);
    public IReadOnlyList<decimal> GetScores(string studentId)
    {
        if (_scores.TryGetValue(studentId, out var scores)) return scores.ToList();
        return Array.Empty<decimal>();
    }
    public IReadOnlyList<string> GetStudentIds()
    {
        return _scores.Keys.OrderBy(x => x).ToList();
    }

    public void Record(string studentId, decimal score)
    {
        if (score is < 0 or > 100) throw new ArgumentOutOfRangeException(nameof(score));
        if (!_scores.TryGetValue(studentId, out var list))
        {
            list = new List<decimal>();
            _scores[studentId] = list;
        }
        list.Add(score);
    }
}
public sealed class CalAverage
{
    public decimal Average(string studentId, GradeBook gradeBook)
    {
        var scores = gradeBook.GetScores(studentId);
        if (scores.Count == 0) return 0m;
        return Math.Round(scores.Average(), 2);
    }
}
public sealed class CalcLetter
{
    public string Letter(string studentId, GradeBook gradeBook, CalAverage average)
    {
        // Academic policy bands change with faculty senate — not with CSV layout.
        var avg = average.Average(studentId, gradeBook);
        if (avg >= 90) return "A";
        if (avg >= 80) return "B";
        if (avg >= 70) return "C";
        if (avg >= 60) return "D";
        return "F";
    }
}
public sealed class CalTranscriptPlain
{
    public string TranscriptPlain(string studentId, string fullName, CalAverage average, GradeBook gradeBook, CalcLetter letter, CalHonorRollPolicy honorRoll)
    {
        // Registrar document format ≠ grading policy.
        return $"TRANSCRIPT\nStudent: {fullName} ({studentId})\nAverage: {average.Average(studentId, gradeBook)}\nLetter: {letter.Letter(studentId, gradeBook, average)}\nHonor: {honorRoll.MeetsHonorRoll(studentId, average, gradeBook, letter)}\n";
    }
}
public sealed class CalExportCsv
{
    public string ExportCsv(CalAverage average, CalcLetter letter, GradeBook gradeBook, CalHonorRollPolicy honorRoll)
    {
        var rows = new List<string> { "studentId,average,letter,honor" };

        foreach (var id in gradeBook.GetStudentIds())
            rows.Add($"{id},{average.Average(id, gradeBook)},{letter.Letter(id, gradeBook, average)},{(honorRoll.MeetsHonorRoll(id, average, gradeBook, letter) ? 1 : 0)}");

        return string.Join('\n', rows);
    }
}
public sealed class CalHonorRollPolicy
{
    public bool MeetsHonorRoll(string studentId, CalAverage average, GradeBook gradeBook, CalcLetter letter)
    {
        // Extra academic rule set embedded beside averaging.
        return average.Average(studentId, gradeBook) >= 85 && letter.Letter(studentId, gradeBook, average) is "A" or "B";
    }
}