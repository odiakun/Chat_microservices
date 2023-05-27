namespace Contracts {
        public interface GetUser{
                public Guid CommandID {get;}
                public DateTime Timestamp {get;}
                public String Username {get;}
        }
}