namespace LibraraySystem
{
    public class HeadLibrarian : Staff
    {
        public HeadLibrarian(string id, string fullName, string phoneNumber, DateOnly hireDate, decimal monthlySalary) : base(id, fullName, phoneNumber, hireDate, monthlySalary, 400)
        {
        }
        public void ChangeItemLateFee(LibraryItem libraryItem, decimal newBaseLateFee)
        {
            if (libraryItem == null)
                throw new ArgumentNullException(nameof(libraryItem));

            libraryItem.UpdateBaseLateFee(newBaseLateFee);
        }

        public void WithdrawItem(LibraryItem libraryItem)
        {
            if (libraryItem == null)
                throw new ArgumentNullException(nameof(libraryItem));

            libraryItem.Withdraw();
        }

        public void RestoreItem(LibraryItem libraryItem)
        {
            if (libraryItem == null)
                throw new ArgumentNullException(nameof(libraryItem));

            libraryItem.Restore();
        }
    }
}
