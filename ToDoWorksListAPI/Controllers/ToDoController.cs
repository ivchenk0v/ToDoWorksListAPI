using Microsoft.AspNetCore.Mvc;
using ToDoWorksListAPI.Models;
using ToDoWorksListAPI.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ToDoWorksListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        // GET: api/<ToDoController>
        [HttpGet]
        public IEnumerable<ToDoItem> Get()
        {
            return _toDoList.GetToDoList();
        }

        // GET api/<ToDoController>/5
        [HttpGet("{id}")]
        public ToDoItem? Get(int id)
        {
            return _toDoList.GetToDoItem(id);
        }

        // POST api/<ToDoController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            _toDoList.AddItemToDoList(value);
        }

        // PUT api/<ToDoController>/5
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

        // DELETE api/<ToDoController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _toDoList.DelItemToDoList(id);
        }

        [HttpGet("/GetLifetime")]
        public string GetLifetime()
        {
            return $"Время регистрации объекта: {_logService.LifeStartTime:HH:mm:ss.ffff}, время обращения к объекту: {DateTime.Now:HH:mm:ss.ffff}, время жизни: {(DateTime.Now - _logService.LifeStartTime).TotalMilliseconds } миллисекунды";
        }
    }
}
