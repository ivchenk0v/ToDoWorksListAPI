namespace ToDoWorksListAPI.Service
{
    public interface IHangfireBackgroundJobs
    {
        void FireAndForgetJob();
        void ReccuringJob();
        void DelayedJob();
        void ContinuationJob();
    }
}
