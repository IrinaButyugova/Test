using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFDataApp.Models
{
    public class SortViewModel
    {
        public SortState NameSort { get; set; }
        public SortState AgeSort { get; set; }
        public SortState CompanySort { get; set; }
        public SortState Current { get; set; }
        public bool Up { get; set; }

        public SortViewModel(SortState sortOrder)
        {
            NameSort = SortState.NameAsc;
            AgeSort = SortState.AgeAsc;
            CompanySort = SortState.CompanyAsc;
            Up = true;

            if (sortOrder == SortState.AgeDesc || sortOrder == SortState.NameDesc
                || sortOrder == SortState.CompanyDesc)
            {
                Up = false;
            }

            Current = sortOrder;
            switch (sortOrder)
            {
                case SortState.NameDesc:
                    NameSort = SortState.NameAsc;
                    break;
                case SortState.AgeAsc:
                    AgeSort = SortState.AgeDesc;
                    break;
                case SortState.AgeDesc:
                    AgeSort = SortState.AgeAsc;
                    break;
                case SortState.CompanyAsc:
                    CompanySort = SortState.CompanyDesc;
                    break;
                case SortState.CompanyDesc:
                    CompanySort = SortState.CompanyAsc;
                    break;
                default:
                    NameSort = SortState.NameDesc;
                    break;
            }
        }
    }
}
