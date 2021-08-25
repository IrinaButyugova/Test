using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EFDataApp.Models
{
    public class IndexViewModel
    {
        public IEnumerable<User> Users { get; set; }
        public SortViewModel SortViewModel { get; set; }
        public FilterViewModel FilterViewModel { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
