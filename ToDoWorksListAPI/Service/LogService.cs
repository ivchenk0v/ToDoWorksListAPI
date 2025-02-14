namespace ToDoWorksListAPI.Service
{
    public class LogService: ILogService
    {
        public DateTime LifeStartTime { get; set; }
        public LogService()
        {
            LifeStartTime = DateTime.Now;
        }
    }
}
