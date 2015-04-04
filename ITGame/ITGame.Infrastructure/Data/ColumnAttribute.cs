using System;

namespace ITGame.Infrastructure.Data
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ColumnAttribute : Attribute
    {
    }
}
