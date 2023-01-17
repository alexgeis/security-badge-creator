namespace CatWorx.BadgeMaker
{
    class Employee
    {
        public string FirstName;
        public string LastName;
        public int Id;
        public string PhotoUrl;

        // 1. properties must be declared in class before they can be used
        // 2. constructor method must be public
        // 3. use PascalCase for public variables, camelCase for private variables.
        public Employee(string firstName, string lastName, int id, string photoUrl)
        {
            FirstName = firstName;
            LastName = lastName;
            Id = id;
            PhotoUrl = photoUrl;
        }

        public string GetFullName()
        {
            return FirstName + " " + LastName;
        }
    }
}
