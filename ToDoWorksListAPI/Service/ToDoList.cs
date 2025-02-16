using ToDoWorksListAPI.Models;

namespace ToDoWorksListAPI.Service
{
    public class ToDoList : IToDoList
    {
        private List<ToDoItem> ToDoItemsList;
        public ToDoList()
        {
            ToDoItemsList = [
                new() { Id = 1, Name = "Создать веб-приложение", IsDone = true },
                new() { Id = 2, Name = "Создать сервис списка дел", IsDone = true },
                new() { Id = 3, Name = "Создание моделей", IsDone = true },
                new() { Id = 4, Name = "Использовать при создании списка дел четыре метода" },
                new() { Id = 5, Name = "Создать контроллер ToDoController, добавить методы" }
            ];
        }
        public List<ToDoItem> GetToDoList() => ToDoItemsList;
        public ToDoItem? GetToDoItem(int id) => ToDoItemsList.FirstOrDefault(x => x.Id == id);
        public void UpdToDoItem(int id, ToDoItem toDoItem)
        {
            var item = ToDoItemsList.FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                item.Name = toDoItem.Name;
                item.IsDone = toDoItem.IsDone;
            }
        }
        public void AddItemToDoList(string name) => ToDoItemsList.Add(new() { Id = ToDoItemsList.Max(x => x.Id) + 1, Name = name });
        public void DelItemToDoList(int id) => ToDoItemsList.RemoveAll(x => x.Id == id);
    }
}
