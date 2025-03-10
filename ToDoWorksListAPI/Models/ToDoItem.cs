namespace ToDoWorksListAPI.Models
{
    /// <summary>
    /// Задача
    /// </summary>
    public class ToDoItem
    {
        /// <summary>
        /// Id задачи
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Название
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// Почта пользователя
        /// </summary>
        public required string Email { get; set; }
        /// <summary>
        /// Признак завершенности задачи
        /// </summary>
        public bool IsDone { get; set; } = false;
        /// <summary>
        /// Признак завершенности задачи
        /// </summary>
        public required DateTime Deadline { get; set; }
    }
}
