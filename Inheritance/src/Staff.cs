namespace LibraraySystem
{
    public class Staff : Person
    {
        protected Staff(string id, string fullName, string phoneNumber, DateOnly hireDate, decimal monthlySalary, decimal responsibilityAllowance) : base(id, fullName, phoneNumber)
        {
            HireDate = hireDate;
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(monthlySalary);
            MonthlySalary = monthlySalary;
            ResponsibilityAllowance = responsibilityAllowance;
        }
        public DateOnly HireDate { get; }
        public decimal MonthlySalary { get; private set; }
        public decimal ResponsibilityAllowance { get; }
        public decimal MonthlyPay
        {
            get
            {
                return MonthlySalary + ResponsibilityAllowance;
            }
        }
        public void GiveRaise(decimal raisePercentage)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(raisePercentage);
            MonthlySalary += ((raisePercentage / 100) * MonthlySalary);
        }
    }
}
