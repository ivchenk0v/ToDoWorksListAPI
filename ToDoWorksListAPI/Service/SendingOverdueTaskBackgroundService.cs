using System.Threading.Tasks;

namespace ToDoWorksListAPI.Service
{
    public class SendingOverdueTaskBackgroundService(IToDoList toDoList) : BackgroundService
    {
        private readonly IToDoList _toDoList = toDoList;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var tasksOverdue = _toDoList.GetToDoList().Where(x => !x.IsDone && x.Deadline < DateTime.Now);
                foreach (var task in tasksOverdue)
                {
                    //TODO: необходима реализация отправки на почту, а пока просто выведем в консоль
                    Console.WriteLine($"Просрочена задача [{task.Name}] для пользователя [{task.Email}]");
                }
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
            await Task.CompletedTask;
        }
    }
}
