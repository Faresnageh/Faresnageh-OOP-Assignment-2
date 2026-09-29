namespace LibraraySystem
{
    public class LibraryItem
    {
        protected LibraryItem(string catalogNumber, string title, decimal baseLateFee, int keepDays, decimal lateFeeMultiplier)
        {
            if(string.IsNullOrWhiteSpace(catalogNumber))
                throw new ArgumentException("Catalog number cannot be null or empty", nameof(catalogNumber));
            if(string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be null or empty", nameof(title));
            if(baseLateFee <= 0)
                throw new ArgumentException("Base late fee cannot be zero or negative", nameof(baseLateFee));
            if(lateFeeMultiplier <= 0)
                throw new ArgumentException("Late fee multiplier cannot be zero or negative", nameof(lateFeeMultiplier));
            CatalogNumber = catalogNumber;
            Title = title;
            BaseLateFee = baseLateFee;
            LateFeeMultiplier = lateFeeMultiplier;
            KeepDays = keepDays;
        }
        public int KeepDays { get; }
        public string CatalogNumber { get; }
        public string Title { get; }
        public decimal BaseLateFee { get; private set; }
        private decimal LateFeeMultiplier { get; }
        public bool IsOnLoan { get; internal set; }
        public decimal DailyLateFees
        {
            get
            {
                return BaseLateFee * LateFeeMultiplier;
            }
        }
        public void UpdateBaseLateFee(decimal newBaseLateFee)
        {
            if (newBaseLateFee <= 0)
                throw new ArgumentException("New base late fee cannot be zero or negative", nameof(newBaseLateFee));
            BaseLateFee = newBaseLateFee;
        }
        public bool IsWithdraw { get; private set; }
        public void Withdraw()
        {
            IsWithdraw = true;
        }
        public void Restore()
        {
            IsWithdraw = false;
        }
    }
}
