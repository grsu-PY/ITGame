using System;

namespace ITGame.Infrastructure.Data
{
    public interface Identity
    {
        Guid Id { get; set; }

        string Name { get; set; }
    }
}
