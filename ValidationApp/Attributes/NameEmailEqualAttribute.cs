using System.ComponentModel.DataAnnotations;
using ValidationApp.Models;

namespace ValidationApp.Attributes
{
    public class NameEmailEqualAttribute : ValidationAttribute
    {
        public NameEmailEqualAttribute()
        {
            ErrorMessage = "Имя и Email не должны совпадать!";
        }
        public override bool IsValid(object value)
        {
            Person p = value as Person;

            if (p.Name == p.Email)
            {
                return false;
            }
            return true;
        }
    }
}
