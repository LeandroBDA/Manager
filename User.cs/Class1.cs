using System.ComponentModel.DataAnnotations;

namespace Manager.Domain.Entities {

    public class User : Base {

        public string Name { get; private set; }
       
        public string Email { get; private set; }
       
        public string Password { get; private set; }
        
        protected User(){}
        
        public User(string name, string email, string password)
        {
            Name = name;
            Email = email;
            Password = password;
        }

        public void ChangeName(string name)
        {
            Name = name;
            Validate();
        }
    }
}
