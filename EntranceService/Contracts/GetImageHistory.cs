namespace Contracts{
    public interface GetImageHistory {
        public Guid CommandID {get;}
        public DateTime Timestamp {get;}
    }
}