namespace LibraraySystem
{
    public class Shelver : Staff
    {
        public Shelver(string id, string fullName, string phoneNumber, DateOnly hireDate, decimal monthlySalary, string section) : base(id, fullName, phoneNumber, hireDate, monthlySalary,0)
        {
            if(string.IsNullOrWhiteSpace(section))
                throw new ArgumentException("Section cannot be null or empty", nameof(section));
            Section = section;
        }
        public string Section { get; private set; }
        public void Reassign(string newSection)
        {
            if (string.IsNullOrWhiteSpace(newSection))
                throw new ArgumentException("New section cannot be null or empty", nameof(newSection));
            Section = newSection;
        }
    }
}