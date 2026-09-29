namespace PatternsLab.Problems.Builder;

public sealed class CourseRegistration
{
    public string StudentEmail { get; }
    public string CourseCode { get; }
    public string AccessMode { get; }
    public string? GroupCode { get; }
    public string? DiscountCode { get; }
    public bool SendWhatsApp { get; }
    public bool SendEmailWelcome { get; }
    public string? MentorNote { get; }
    public DateOnly? PreferredStart { get; }

    public CourseRegistration(
        string studentEmail,
        string courseCode,
        string accessMode,
        string? groupCode,
        string? discountCode,
        bool sendWhatsApp,
        bool sendEmailWelcome,
        string? mentorNote,
        DateOnly? preferredStart)
    {
        if (string.IsNullOrWhiteSpace(studentEmail)) throw new ArgumentException("email required");
        if (string.IsNullOrWhiteSpace(courseCode)) throw new ArgumentException("course required");

        if (accessMode == "LiveGroup" && string.IsNullOrWhiteSpace(groupCode))
            throw new InvalidOperationException("LiveGroup requires GroupCode");
        if (accessMode == "VideosOnly" && !string.IsNullOrWhiteSpace(groupCode))
            throw new InvalidOperationException("VideosOnly cannot have GroupCode");

        StudentEmail = studentEmail;
        CourseCode = courseCode;
        AccessMode = accessMode;
        GroupCode = groupCode;
        DiscountCode = discountCode;
        SendWhatsApp = sendWhatsApp;
        SendEmailWelcome = sendEmailWelcome;
        MentorNote = mentorNote;
        PreferredStart = preferredStart;
    }
    public override string ToString()
        => $"{StudentEmail} → {CourseCode} [{AccessMode}] group={GroupCode ?? "-"} discount={DiscountCode ?? "-"} wa={SendWhatsApp} mail={SendEmailWelcome}";
}
public sealed class CourseRegistrationBuilder
{
    public CourseRegistrationBuilder(string studentEmail, string courseCode, string accessMode)
    {
        if (string.IsNullOrWhiteSpace(studentEmail)) throw new ArgumentException("email required");
        if (string.IsNullOrWhiteSpace(courseCode)) throw new ArgumentException("course required");
        if (string.IsNullOrWhiteSpace(accessMode)) throw new ArgumentException("accessMode required");
        _studentEmail = studentEmail;
        _courseCode = courseCode;
        _accessMode = accessMode;
    }
    private string _studentEmail;
    private string _courseCode;
    private string _accessMode;
    private string? _groupCode;
    public CourseRegistrationBuilder WithGroupCode(string? groupCode)
    {
        _groupCode = groupCode;
        return this;
    }
    private string? _discountCode;
    public CourseRegistrationBuilder WithDiscountCode(string? discountCode)
    {
        _discountCode = discountCode;
        return this;
    }
    private bool _sendWhatsApp;
    public CourseRegistrationBuilder WithSendWhatsApp(bool sendWhatsApp)
    {
        _sendWhatsApp = sendWhatsApp;
        return this;
    }
    private bool _sendEmailWelcome;
    public CourseRegistrationBuilder WithSendEmailWelcome(bool sendEmailWelcome)
    {
        _sendEmailWelcome = sendEmailWelcome;
        return this;
    }
    private string? _mentorNote;
    public CourseRegistrationBuilder WithMentorNote(string? mentorNote)
    {
        _mentorNote = mentorNote;
        return this;
    }
    private DateOnly? _preferredStart;
    public CourseRegistrationBuilder WithPreferredStart(DateOnly? preferredStart)
    {
        _preferredStart = preferredStart;
        return this;
    }
    public CourseRegistration Build()
    {
        if (_accessMode == "LiveGroup" && string.IsNullOrWhiteSpace(_groupCode))
            throw new InvalidOperationException("LiveGroup requires GroupCode");
        if (_accessMode == "VideosOnly" && !string.IsNullOrWhiteSpace(_groupCode))
            throw new InvalidOperationException("VideosOnly cannot have GroupCode");
        return new CourseRegistration(_studentEmail, _courseCode, _accessMode, _groupCode, _discountCode, _sendWhatsApp, _sendEmailWelcome, _mentorNote, _preferredStart);
    }
}

public static class RegistrationCallSites
{
    public static CourseRegistration CreateLiveStudentUgly()
    {
        return new CourseRegistrationBuilder("sara@mail.com", "SEF-101", "LiveGroup").WithGroupCode("G1")
            .WithDiscountCode("EARLY10").WithSendWhatsApp(true).WithSendEmailWelcome(true)
            .WithMentorNote("Needs evening slot").WithPreferredStart(new DateOnly(2026, 10, 1)).Build();
    }

    public static CourseRegistration CreateVideosOnlyUgly()
    {
        return new CourseRegistrationBuilder("ali@mail.com", "SEF-101", "VideosOnly").WithSendEmailWelcome(true).Build();
    }
}
