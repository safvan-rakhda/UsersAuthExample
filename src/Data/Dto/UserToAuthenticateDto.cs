namespace UsersAuthExample.Data.Dto
{
    public class UserToAuthenticateDto
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Secret { get; set; }
    }
}
