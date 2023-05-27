namespace EntranceService.Models{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
    }

    public class UserDTO{
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }

        public UserDTO(){ }
        public UserDTO(User user) =>
        (Id, Username, Email, Gender) = (user.Id, user.Username, user.Email, user.Gender);
    }

    public class PostUserDTO{
        public string Username { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }

        public PostUserDTO(){ }
        public PostUserDTO(User user) =>
        (Username, Email, Gender) = (user.Username, user.Email, user.Gender);
    }
}