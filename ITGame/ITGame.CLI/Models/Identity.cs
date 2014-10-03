using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ITGame.CLI.Models
{
    public interface Identity
    {
        Guid Id
        {
            get;
            set;
        }

        string Name
        {
            get;
            set;
        }
    }
}
