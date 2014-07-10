using System;

namespace ConfigRemedy.Domain
{
    public class Environment : IComparable<Environment>
    {
        public string Id { get; set; }
        public short Order { get; set; }
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }

        public int CompareTo(Environment other)
        {
            if (other == null) return 1;
            return Order.CompareTo(other.Order);
        }
    }
}