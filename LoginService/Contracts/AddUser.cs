using LoginService.Models;

namespace Contracts{
    public interface AddUser {
        public Guid CommandID {get;}
        public DateTime Timestamp {get;}
        public UserDTO userDTO {get;}
    }
}