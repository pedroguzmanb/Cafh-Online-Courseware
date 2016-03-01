using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lang;

namespace Org.Cafh.Courseware.Models.Courseware
{
    public class LanguageSelectViewModel
    {
        [Display(Name="Language", ResourceType = typeof(Global))]
        public Guid Language { set; get; }

        public string Title { set; get; }
    }
}
