namespace ToDoWorksListAPI.Models
{
    public class ToDoItem
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public bool IsDone { get; set; } = false;
    }
}
