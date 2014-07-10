using System;
using ConfigRemedy.Domain.Annotations;

namespace ConfigRemedy.Domain
{
    [UsedImplicitly, MeansImplicitUse]
    public class Environment : IComparable<Environment>
    {
        [NotNull]
        public string Id { get; set; }

        [NotNull]
        public string ShortName { get; set; }

        public short Order { get; set; }
        public string LongName { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }


        public int CompareTo(Environment other)
        {
            if (other == null) return 1;
            return Order.CompareTo(other.Order);
        }

        // TODO: Add validation that id and shortname is present
    }
}
