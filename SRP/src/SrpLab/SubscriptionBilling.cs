namespace SrpLab;

public sealed class SubscriptionBilling
{
    public string CustomerId { get; }
    public decimal MonthlyPrice { get; }
    public DateOnly PeriodStart { get; }
    public DateOnly PeriodEnd { get; }
    public int FailedPayments { get; private set; }

    public SubscriptionBilling(string customerId, decimal monthlyPrice, DateOnly periodStart, DateOnly periodEnd)
    {
        CustomerId = customerId;
        MonthlyPrice = monthlyPrice;
        PeriodStart = periodStart;
        PeriodEnd = periodEnd;
    }

    public void RegisterFailedPayment()
    {
        FailedPayments++;
    }
}

public sealed class BillingProrate
{
    public decimal Prorate(DateOnly activeFrom, SubscriptionBilling sB)
    {
        if (activeFrom <= sB.PeriodStart) return sB.MonthlyPrice;
        if (activeFrom >= sB.PeriodEnd) return 0m;
        var totalDays = sB.PeriodEnd.DayNumber - sB.PeriodStart.DayNumber;
        if (totalDays <= 0) return sB.MonthlyPrice;
        var used = sB.PeriodEnd.DayNumber - activeFrom.DayNumber;
        return Math.Round(sB.MonthlyPrice * used / totalDays, 2);
    }
}

public sealed class BillingNextInvoiceNumber
{
    private static int _invoiceSeq = 1000;

    public string NextInvoiceNumber(SubscriptionBilling sB)
    {
        var n = ++_invoiceSeq;
        return $"INV-{sB.PeriodStart:yyyyMM}-{n:D5}";
    }
}

public sealed class BillingDunningEmail
{
    public string DunningEmail(string customerName, DateOnly asOf, BillingProrate bP, SubscriptionBilling sB, BillingNextInvoiceNumber nI)
    {
        var amount = bP.Prorate(sB.PeriodStart, sB);
        var invoice = nI.NextInvoiceNumber(sB);
        var severity = sB.FailedPayments switch
        {
            <= 1 => "friendly reminder",
            2 => "second notice",
            _ => "final notice before suspension"
        };
        return $"Subject: {severity} {invoice}\nHi {customerName},\nBalance {amount:C} as of {asOf:o} ({sB.FailedPayments} failures).\n";
    }
}

public sealed class BillingLedgerJournalLine
{
    public string LedgerJournalLine(DateOnly activeFrom, SubscriptionBilling sB, BillingProrate bP, BillingNextInvoiceNumber nI)
    {
        return $"{sB.CustomerId},{nI.NextInvoiceNumber(sB)},{bP.Prorate(activeFrom, sB):0.00},AR-SUB";
    }
}