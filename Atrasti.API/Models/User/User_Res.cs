namespace Atrasti.API.Models.User
{
    public class User_Res
    {
        public User_Res(string firstName, string lastName, string company, string logo)
        {
            FirstName = firstName;
            LastName = lastName;
            Company = company;
            Logo = logo;
        }

        public string FirstName { get;  }
        public string LastName { get;  }
        public string Company { get; }
        public string Logo { get; }
    }
}