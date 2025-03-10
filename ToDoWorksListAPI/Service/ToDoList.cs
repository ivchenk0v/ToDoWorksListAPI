using ToDoWorksListAPI.Models;

namespace ToDoWorksListAPI.Service
{
    public class ToDoList : IToDoList
    {
        private List<ToDoItem> ToDoItemsList;
        public ToDoList()
        {
            ToDoItemsList = [
                new() { Id = 1, Name = "Создать веб-приложение", IsDone = true, Email = "admin@gmail.com", Deadline = DateTime.Now.AddDays(-3) },
                new() { Id = 2, Name = "Создать сервис списка дел", IsDone = true, Email = "admin@gmail.com", Deadline = DateTime.Now.AddDays(-3)  },
                new() { Id = 3, Name = "Создание моделей", IsDone = true, Email = "admin@gmail.com", Deadline = DateTime.Now.AddDays(-3)  },
                new() { Id = 4, Name = "Использовать при создании списка дел четыре метода", Email = "admin@gmail.com", Deadline = DateTime.Now.AddDays(4)  },
                new() { Id = 5, Name = "Создать контроллер ToDoController, добавить методы", Email = "admin@gmail.com", Deadline = DateTime.Now.AddDays(-3)  },
                new() { Id = 6, Name = "Создать IdentityServer", IsDone = true, Email = "user@gmail.com", Deadline = DateTime.Now.AddDays(-3) },
                new() { Id = 7, Name = "Обновить созданный IdentityServer", IsDone = true, Email = "user@gmail.com", Deadline = DateTime.Now.AddDays(-3)  },
                new() { Id = 8, Name = "Добавить аутентификацию в приложение", IsDone = true, Email = "user@gmail.com", Deadline = DateTime.Now.AddDays(-3)  },
                new() { Id = 9, Name = "Добавить аутентификацию для контроллера ToDo", Email = "user@gmail.com", Deadline = DateTime.Now.AddDays(-1)  },
                new() { Id = 10, Name = "Обновить модель элемента списка дел", Email = "user@gmail.com", Deadline = DateTime.Now.AddDays(-2)  },
                new() { Id = 11, Name = "Обновить сервис списка дел", Email = "user@gmail.com", Deadline = DateTime.Now.AddDays(-3)  },
                new() { Id = 12, Name = "Обновить контроллер ToDo", Email = "user@gmail.com", Deadline = DateTime.Now.AddDays(3)  }
            ];
        }
        public List<ToDoItem> GetToDoList(string email = "") => ToDoItemsList.Where(x => x.Email == email || string.IsNullOrEmpty(email)).ToList();
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
        public void AddItemToDoList(string name, string email) => ToDoItemsList.Add(new() { Id = ToDoItemsList.Max(x => x.Id) + 1, Name = name, Email = email, Deadline = DateTime.Now.AddDays(3) });
        public void DelItemToDoList(int id) => ToDoItemsList.RemoveAll(x => x.Id == id);
    }
}
