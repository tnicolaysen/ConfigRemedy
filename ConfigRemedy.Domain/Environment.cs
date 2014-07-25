using System;
using ConfigRemedy.Core.Properties;

namespace ConfigRemedy.Domain
{
    [UsedImplicitly, MeansImplicitUse]
    public class Environment : IComparable<Environment>
    {
        [NotNull]
        public string Id { get; set; }

        [NotNull]
        public string ShortName { get; set; }

        //public short Order { get; set; }
        public string LongName { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }


        public int CompareTo(Environment other)
        {
            // TODO: Order needs to be added to allow custom sorting. Use short name for how.
            if (other == null) return 1;

            //return Order.CompareTo(other.Order);
            return String.Compare(ShortName, other.ShortName, StringComparison.InvariantCultureIgnoreCase);
        }

        // TODO: Add validation that id and shortname is present
    }
}
