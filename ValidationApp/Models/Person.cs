using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ValidationApp.Attributes;

namespace ValidationApp.Models
{
    [NameEmailEqual]
    public class Person
    {
        [Display(Name = "Имя и фамилия")]
        [PersonName(new string[] { "Tom", "Sam", "Alice", "irina.butyugova@sharp-dev.net" }, ErrorMessage = "Недопустимое имя")]
        [Required(ErrorMessage = "Не указано имя")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 50 символов")]
        public string Name { get; set; }

        [Display(Name = "Электронная почта")]
        [Required]
        [EmailAddress(ErrorMessage = "Некорректный адрес")]
        [Remote(action: "CheckEmail", controller: "Home", ErrorMessage = "Email уже используется")]
        public string Email { get; set; }

        [Display(Name = "Домашняя страница")]
        [UIHint("Url")]
        public string HomePage { get; set; }

        [Display(Name = "Пароль")]
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]

        [Display(Name = "Подтверждение пароля")]
        public string PasswordConfirm { get; set; }

        [Display(Name = "Возраст")]
        [Range(1, 110, ErrorMessage = "Недопустимый возраст")]
        public int Age { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }
    }
}
