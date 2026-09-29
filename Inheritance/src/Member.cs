namespace LibraraySystem
{
    public class Member : Person
    {
        private List<Loan> loans = new List<Loan>();

        public IReadOnlyList<Loan> Loans
        {
            get
            {
                return loans;
            }
        }
        public int ActiveLoanCount
        {
            get
            {
                int count = 0;

                foreach (Loan loan in loans)
                {
                    if (loan.Status == LoanStatus.Borrowed)
                    {
                        count++;
                    }
                }

                return count;
            }
        }
        protected Member(string id, string fullName, string phoneNumber, int maxActiveLoans, double feeDiscount) : base(id, fullName, phoneNumber)
        {
            if (maxActiveLoans <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxActiveLoans));

            MaxActiveLoans = maxActiveLoans;
            if (feeDiscount < 0 || feeDiscount > 100)
                throw new ArgumentOutOfRangeException(nameof(feeDiscount));
            FeeDiscount = feeDiscount;
        }
        public int MaxActiveLoans { get; }
        public double FeeDiscount { get; }
        public Loan BorrowItem(string loanId, DateOnly borrowDate, LibraryItem libraryItem)
        {
            if (ActiveLoanCount >= MaxActiveLoans)
                throw new InvalidOperationException("Member has reached the maximum number of active loans");
            Loan loan = new Loan(loanId, borrowDate, this, libraryItem);
            loans.Add(loan);
            return loan;
        }
    }
}