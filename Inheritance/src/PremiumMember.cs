namespace LibraraySystem
{
    public class PremiumMember : Member
    {
        public PremiumMember(string id, string fullName, string phoneNumber, double premiumFeeDiscount) : base(id, fullName, phoneNumber,10,premiumFeeDiscount)
        {
        }
        public int ReadingPoints
        {
            get
            {
                int points = 0;
                foreach (Loan loan in Loans)
                {
                    if (loan.Status == LoanStatus.Returned)
                    {
                        points += 5;
                    }
                }
                return points;
            }
        }
    }
}
