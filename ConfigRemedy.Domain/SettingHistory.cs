using System;

namespace ConfigRemedy.Domain
{
    public struct SettingHistory
    {
        public string Who { get; set; }
        public ChangeType What { get; set; }
        public DateTime When { get; set; }
        public string Where{ get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}