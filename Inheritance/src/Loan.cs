namespace LibraraySystem
{
    public class Loan
    {
        public Loan(string id, DateOnly borrowDate, Member member, LibraryItem libraryItem)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("ID cannot be null or empty", nameof(id));
            if (member == null)
                throw new ArgumentNullException(nameof(member), "Member cannot be null");
            if (libraryItem == null)
                throw new ArgumentNullException(nameof(libraryItem), "Library item cannot be null");
            if (libraryItem.IsWithdraw)
                throw new InvalidOperationException("This item is withdrawn.");

            if (libraryItem.IsOnLoan)
                throw new InvalidOperationException("This item is already on loan.");

            Id = id;
            BorrowDate = borrowDate;
            this.member = member;
            this.libraryItem = libraryItem;
            Status = LoanStatus.Borrowed;
            libraryItem.IsOnLoan = true;
        }
        public LoanStatus Status { get; private set; }
        public string Id { get; }
        public DateOnly BorrowDate { get; }
        public Member member { get; }
        public LibraryItem libraryItem { get; }
        public DateOnly? ReturnDate { get; private set; }
        public DateOnly DueDate
        {
            get
            {
                return BorrowDate.AddDays(libraryItem.KeepDays);
            }
        }
        public void MarkAsReturned(DateOnly returnDate)
        {
            if (Status != LoanStatus.Borrowed)
                throw new InvalidOperationException("Only a borrowed loan can be returned");

            if (returnDate < BorrowDate)
                throw new ArgumentException("Return date cannot be earlier than borrow date", nameof(returnDate));

            ReturnDate = returnDate;
            Status = LoanStatus.Returned;
            libraryItem.IsOnLoan = false;
        }
        public void MarkAsLost()
        {
            if (Status != LoanStatus.Borrowed)
                throw new InvalidOperationException("Only a borrowed loan can be marked as lost");

            Status = LoanStatus.Lost;
        }
        public decimal LateFee
        {
            get
            {
                if (ReturnDate == null || ReturnDate <= DueDate)
                    return 0m;

                int lateDays = ReturnDate.Value.DayNumber - DueDate.DayNumber;

                decimal feeBeforeDiscount = lateDays * libraryItem.DailyLateFees;

                decimal discountAmount =
                    feeBeforeDiscount * ((decimal)member.FeeDiscount / 100m);

                return feeBeforeDiscount - discountAmount;
            }
        }
    }
}
