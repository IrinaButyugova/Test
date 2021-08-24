using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ValidationApp.Attributes
{
    public class PersonNameAttribute : ValidationAttribute
    {
        //массив для хранения допустимых имен
        string[] _names;

        public PersonNameAttribute(string[] names)
        {
            _names = names;
        }
        public override bool IsValid(object value)
        {
            if (value != null && _names.Contains(value.ToString()))
                return true;

            return false;
        }
    }
}
