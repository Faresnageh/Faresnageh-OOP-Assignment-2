namespace SrpLab;

public sealed class LoanDesk
{
    public decimal RequestedAmount { get; }
    public int CreditScore { get; }
    public int EmploymentMonths { get; }
    public bool HasCollateral { get; }

    public LoanDesk(decimal requestedAmount, int creditScore, int employmentMonths, bool hasCollateral)
    {
        RequestedAmount = requestedAmount; CreditScore = creditScore; EmploymentMonths = employmentMonths; HasCollateral = hasCollateral;
    }
}

public sealed class RiskCalculator
{
    public decimal RiskScore(LoanDesk loan)
    {
        decimal score = 100m;
        score -= Math.Max(0, 700 - loan.CreditScore) * 0.15m;
        if (loan.EmploymentMonths < 6) score -= 20m;
        if (loan.RequestedAmount > 50_000m && !loan.HasCollateral) score -= 25m;
        if (loan.RequestedAmount > 150_000m) score -= 10m;
        return Math.Clamp(score, 0m, 100m);
    }
}

public sealed class EligibilityChecker
{
    public bool IsEligible(LoanDesk loan, RiskCalculator riskCalculator)
    {
        return riskCalculator.RiskScore(loan) >= 55m && loan.CreditScore >= 580;
    }
}

public sealed class RequiredDocumentsBuilder
{
    public IReadOnlyList<string> RequiredDocuments(LoanDesk loan, RiskCalculator riskCalculator, EligibilityChecker eligibilityChecker)
    {
        var docs = new List<string> { "National ID", "Proof of income (3 months)" };
        if (loan.RequestedAmount > 40_000m) docs.Add("Bank statements (6 months)");
        if (loan.HasCollateral) docs.Add("Collateral ownership deed");
        if (loan.EmploymentMonths < 12) docs.Add("Employer letter");
        if (!eligibilityChecker.IsEligible(loan, riskCalculator)) docs.Add("Manual underwriter referral form");
        return docs;
    }
}

public sealed class DecisionLetterBuilder
{
    public string DecisionLetter(string applicantName, LoanDesk loan, RiskCalculator riskCalculator, EligibilityChecker eligibilityChecker, RequiredDocumentsBuilder documentsBuilder)
    {
        if (eligibilityChecker.IsEligible(loan, riskCalculator))
            return $"Dear {applicantName},\nYour request for {loan.RequestedAmount:C} is pre-approved (risk {riskCalculator.RiskScore(loan):0}).\nPlease upload: {string.Join("; ", documentsBuilder.RequiredDocuments(loan, riskCalculator, eligibilityChecker))}.\n";
        return $"Dear {applicantName},\nWe are unable to approve {loan.RequestedAmount:C} at this time.\nReference risk={riskCalculator.RiskScore(loan):0}. You may reapply after improving documentation.\n";
    }
}

public sealed class UnderwriterCsvExporter
{
    public string UnderwriterCsvRow(string applicationId, LoanDesk loan, RiskCalculator riskCalculator, EligibilityChecker eligibilityChecker)
    {
        return $"{applicationId},{loan.CreditScore},{loan.EmploymentMonths},{(loan.HasCollateral ? 1 : 0)},{riskCalculator.RiskScore(loan):0.00},{(eligibilityChecker.IsEligible(loan, riskCalculator) ? "Y" : "N")}";
    }
}