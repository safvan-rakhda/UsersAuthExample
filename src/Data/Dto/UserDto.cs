using System;

namespace UsersAuthExample.Data.Dto
{
    public class UserDto
    {
        private int userId;
        private int role;
        private DateTime createdDate;

        public int UserId { get => userId; set => userId = value; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int Role { get => role; set => role = value; }

        public DateTime CreatedDate { get => createdDate; set => createdDate = value; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }
    }
}
