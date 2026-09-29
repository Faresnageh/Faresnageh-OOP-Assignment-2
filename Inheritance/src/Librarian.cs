namespace LibraraySystem
{
    public class Librarian : Staff
    {
        public Librarian(string id, string fullName, string phoneNumber, DateOnly hireDate, decimal monthlySalary) : base(id, fullName, phoneNumber, hireDate, monthlySalary,0m)
        {
        }
        public void RegisterReturn(Loan loan, DateOnly returnDate)
        {
            if (loan == null)
                throw new ArgumentNullException(nameof(loan));

            loan.MarkAsReturned(returnDate);
        }

        public void RegisterLost(Loan loan)
        {
            if (loan == null)
                throw new ArgumentNullException(nameof(loan));

            loan.MarkAsLost();
        }
    }
}
