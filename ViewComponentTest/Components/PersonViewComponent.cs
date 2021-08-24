using ViewComponentTest.Models;

namespace ViewComponentTest.Components
{
    public class PersonViewComponent
    {
        public string Invoke(User user)
        {
            return $"Name: {user.Name}  Age: {user.Age}";
        }
    }
}
