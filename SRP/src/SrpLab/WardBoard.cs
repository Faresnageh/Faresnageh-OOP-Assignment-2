namespace SrpLab;

/// <summary>
/// Hospital ward board: tracks beds, computes acuity scores, drafts nurse handoff notes,
/// and decides which pager code to fire. Looks like "one ward concern" — it is not.
/// </summary>
public sealed class WardBoard
{
    private readonly Dictionary<int, string> _bedPatient = new();
    public List<int> GetBeds()
    {
        return _bedPatient.Keys.ToList();
    }
    public bool BedPatient(int bed)
    {
        if (_bedPatient.TryGetValue(bed, out var patient))
        {
            return true;
        }
        return false;
    }
    public string GetPatient(int bed)
    {
        return _bedPatient[bed];
    }
    private readonly Dictionary<int, int> _vitalsScore = new();
    public int GetVitalsScore(int bed)
    {
        return _vitalsScore[bed];
    }
    private readonly Pager_Log _log = new();
    public IReadOnlyList<string> DrainPagerLog()
    {
        return _log.DrainPagerLog();
    }
    public void AssignBed(int bed, string patientId, int heartRate, int spo2)
    {
        AcuityScorer acuityScorer = new AcuityScorer();

        if (bed <= 0)
            throw new ArgumentOutOfRangeException(nameof(bed));

        if (string.IsNullOrWhiteSpace(patientId))
            throw new ArgumentException("patient required");

        _bedPatient[bed] = patientId.Trim().ToUpperInvariant();
        _vitalsScore[bed] = acuityScorer.ScoreAcuity(heartRate, spo2);


        _log.CheckAndAddAlert(bed, _vitalsScore[bed]);
    }
}

public sealed class AcuityScorer
{
    public int ScoreAcuity(int heartRate, int spo2)
    {
        // Clinical scoring mixed with arbitrary pager thresholds (policy will churn separately).
        var score = 0;

        if (heartRate > 120 || heartRate < 45)
            score += 4;
        else if (heartRate > 100)
            score += 2;

        if (spo2 < 90)
            score += 5;
        else if (spo2 < 94)
            score += 2;

        return Math.Min(score, 10);
    }
}

public sealed class HabdoffNote
{
    public string BuildHandoffNote(WardBoard wardBoard,int bed)
    {
        if (!wardBoard.BedPatient(bed))
            return $"Bed {bed}: empty";

        var acuity = wardBoard.GetVitalsScore(bed);

        var tone = acuity >= 8
            ? "ESCALATE"
            : acuity >= 4
                ? "WATCH"
                : "STABLE";

        // Presentation / narrative format will change without clinical rules changing.
        return $"[HANDOFF {DateTime.UtcNow:yyyy-MM-dd}] Bed {bed} · {wardBoard.GetPatient(bed)} · acuity={acuity} · {tone}";
    }
}

public sealed class Pager_Log
{
    private readonly List<string> _pagerLog = new();
    public IReadOnlyList<string> DrainPagerLog()
    {
        var copy = _pagerLog.ToList();
        _pagerLog.Clear();
        return copy;
    }
    public void CheckAndAddAlert(int bed, int acuity)
    {
        if (acuity >= 8)
            _pagerLog.Add($"CODE-YELLOW bed={bed} at {DateTime.UtcNow:HH:mm}");
    }
}

public sealed class Export_Census_Csv
{
    public string ExportCensusCsv(WardBoard wardBoard)
    {
        // Persistence/export shape mixed into the same type as acuity + paging.
        var lines = new List<string>
        {
            "bed,patient,acuity"
        };

        foreach (var bed in wardBoard.GetBeds().OrderBy(x => x))
        {
            lines.Add($"{bed},{wardBoard.GetPatient(bed)},{wardBoard.GetVitalsScore(bed)}");
        }

        return string.Join('\n', lines);
    }
}