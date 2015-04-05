using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITGame.Models.Entities
{
    public interface IViewModelItem
    {
        bool IsSelectedModelItem { get; set; }
    }
}
