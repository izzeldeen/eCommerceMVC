using eCommerceMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCommerceMVC.Areas.Dashboard.ViewModels
{
    public class ManagerViewModel : PageViewModel
    {
        public int EntryOneProductsCount { get; set; }
        public int EntryTwoProductsCount { get; set; }

        public string EntryOneId { get; set; }
        public string EntryTwoId { get; set; }

    }
}