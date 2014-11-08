using System;

namespace ITGame.Models
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
