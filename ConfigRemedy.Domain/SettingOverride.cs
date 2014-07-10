using ConfigRemedy.Domain.Annotations;

namespace ConfigRemedy.Domain
{
    [UsedImplicitly, MeansImplicitUse]
    public class SettingOverride
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}