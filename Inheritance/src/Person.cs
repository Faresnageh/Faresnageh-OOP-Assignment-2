namespace LibraraySystem
{
    public class Person
    {
        protected Person(string id, string fullName, string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                throw new ArgumentNullException(nameof(fullName));
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException(nameof(id));
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentNullException(nameof(phoneNumber));
            Id = id;
            FullName = fullName;
            PhoneNumber = phoneNumber;
        }

        public string Id { get; }
        public string FullName { get; }
        public string PhoneNumber { get; }
    }
}
