namespace Contracts {
        public interface DeleteUser{
                public Guid CommandID {get;}
                public DateTime Timestamp {get;}
                public String Username {get;}
        }
}