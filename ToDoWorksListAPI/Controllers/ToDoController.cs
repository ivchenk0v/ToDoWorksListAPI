using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoWorksListAPI.Models;
using ToDoWorksListAPI.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ToDoWorksListAPI.Controllers
{
    /// <summary>
    /// Работа со список задач
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class ToDoController : ControllerBase
    {
        private readonly ILogger<ToDoController> _logger;
        private readonly IToDoList _toDoList;
        private readonly ILogService _logService;

        public ToDoController(ILogger<ToDoController> logger, IToDoList toDoList, ILogService logService)
        {
            _logger = logger;
            _toDoList = toDoList;
            _logService = logService;
        }

        /// <summary>
        /// Получить весь список задач
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<ToDoItem> Get()
        {
            string email = string.Empty;
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null) email = identity.FindFirst(ClaimTypes.Email).Value;
            return _toDoList.GetToDoList(email);
        }

        /// <summary>
        /// Получить задачу
        /// </summary>
        /// <param name="id">Номер задачи</param>
        /// <returns>Объект ToDoItem</returns>
        [HttpGet("{id}")]
        public ToDoItem? Get(int id)
        {
            return _toDoList.GetToDoItem(id);
        }

        /// <summary>
        /// Добавление новой задачи
        /// </summary>
        /// <param name="value">Навание новой задачи</param>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            string email = string.Empty;
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null) email = identity.FindFirst(ClaimTypes.Email).Value;
            _toDoList.AddItemToDoList(value, email);
        }

        /// <summary>
        /// Обновление задачи
        /// </summary>
        /// <param name="id">Id задачи</param>
        /// <param name="toDoItem">Объект задачи ToDoItem</param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] ToDoItem toDoItem)
        {
            _toDoList.UpdToDoItem(id, toDoItem);
        }
        //[HttpPut]
        //public void Put([FromBody]ToDoItem toDoItem)
        //{
        //    _toDoList.UpdToDoItem(toDoItem);
        //}

        /// <summary>
        /// Удаление задачи
        /// </summary>
        /// <param name="id">Id задачи</param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _toDoList.DelItemToDoList(id);
        }

        /// <summary>
        /// Получение времени жизни зарегистрированного объекта
        /// </summary>
        /// <returns></returns>
        [HttpGet("/GetLifetime")]
        public string GetLifetime()
        {
            return $"Время регистрации объекта: {_logService.LifeStartTime:HH:mm:ss.ffff}, время обращения к объекту: {DateTime.Now:HH:mm:ss.ffff}, время жизни: {(DateTime.Now - _logService.LifeStartTime).TotalMilliseconds } миллисекунды";
        }
    }
}
