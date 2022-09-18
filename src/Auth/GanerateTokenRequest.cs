namespace UsersAuthExample.Auth
{
    public class GanerateTokenRequest
    {
        public int UserId { get; set; }

        public int[] Roles { get; set; }
    }
}
