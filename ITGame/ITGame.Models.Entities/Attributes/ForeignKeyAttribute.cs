using System;

namespace ITGame.Models.Entities.Attributes
{
    public class ForeignKeyAttribute : Attribute
    {
        public ForeignKeyAttribute(Type entityType)
            : this(entityType, string.Empty)
        {
        }

        public ForeignKeyAttribute(Type entityType, string propertyName)
        {
            EntityType = entityType;
            PropertyName = propertyName;
        }

        public Type EntityType { get; set; }
        public string PropertyName { get; set; }
        public bool IsCollection { get; set; }
    }
}