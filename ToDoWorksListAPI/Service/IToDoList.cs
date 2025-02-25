﻿using ToDoWorksListAPI.Models;

namespace ToDoWorksListAPI.Service
{
    public interface IToDoList
    {
        public List<ToDoItem> GetToDoList();
        public ToDoItem? GetToDoItem(int id);
        public void UpdToDoItem(int id, ToDoItem toDoItem);
        public void AddItemToDoList(string name);
        public void DelItemToDoList(int id);
    }
}
