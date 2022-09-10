namespace UsersAuthExample.Services.ServiceRequest
{
    public class CreateUserServiceRequest
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Role { get ; set ; }
    }
}
