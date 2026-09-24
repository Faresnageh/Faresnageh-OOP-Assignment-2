namespace SrpLab;

public sealed class SupportTicket
{
    public string Id { get; }
    public string Subject { get; private set; }
    public string Body { get; private set; }
    public DateTimeOffset OpenedAt { get; }
    public string Priority { get; private set; } = "P3";
    private readonly PriorityCalculator _priorityCalculator = new();

    public SupportTicket(string id, string subject, string body, DateTimeOffset openedAt)
    {
        Id = id; Subject = subject; Body = body; OpenedAt = openedAt; RecalculatePriority();
    }

    public void AppendCustomerMessage(string text)
    {
        Body += "\n---\n" + text; RecalculatePriority();
    }

    private void RecalculatePriority()
    {
        Priority = _priorityCalculator.CalculatePriority(Subject, Body);
    }
}

public sealed class PriorityCalculator
{
    public string CalculatePriority(string subject, string body)
    {
        var blob = (subject + " " + body).ToLowerInvariant();
        if (blob.Contains("down") || blob.Contains("outage") || blob.Contains("cannot login")) return "P1";
        if (blob.Contains("urgent") || blob.Contains("asap") || blob.Contains("blocked")) return "P2";
        return "P3";
    }
}

public sealed class SlaCalculator
{
    public DateTimeOffset SlaDeadline(SupportTicket ticket)
    {
        var hours = ticket.Priority switch { "P1" => 4, "P2" => 24, _ => 72 };
        return ticket.OpenedAt.AddHours(hours);
    }

    public bool IsBreached(SupportTicket ticket, DateTimeOffset now)
    {
        return now > SlaDeadline(ticket);
    }
}

public sealed class PublicReplyBuilder
{
    public string DraftPublicReply(SupportTicket ticket, string agentName, SlaCalculator slaCalculator)
    {
        var apology = ticket.Priority == "P1" ? "We are treating this as a critical incident." : "Thanks for reaching out.";
        return $"Hi,\n{apology}\nTicket {ticket.Id} is with {agentName}. Next update before {slaCalculator.SlaDeadline(ticket):u}.\n";
    }
}

public sealed class InternalEscalationBuilder
{
    public string InternalEscalationBlurb(SupportTicket ticket, SlaCalculator slaCalculator)
    {
        return $"ESCALATE {ticket.Id} priority={ticket.Priority} breachAt={slaCalculator.SlaDeadline(ticket):u} keywords-scanned=yes";
    }
}