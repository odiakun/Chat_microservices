namespace Contracts{
    public interface GetHistory {
        public Guid CommandID {get;}
        public DateTime Timestamp {get;}
    }
}