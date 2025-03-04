using System.ComponentModel.DataAnnotations;

namespace ToDoWorksListAPI.Models
{
    public class LoginUserModel
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        [Required(ErrorMessage = "Поле UserName является обязательным")]
        public string UserName { get; set; }
        /// <summary>
        /// Пароль пользователя
        /// </summary>
        [Required(ErrorMessage = "Поле Password является обязательным")]
        public string Password { get; set; }
    }
}
