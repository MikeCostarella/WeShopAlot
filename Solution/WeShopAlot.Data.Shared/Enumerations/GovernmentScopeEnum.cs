using System.ComponentModel;

namespace WeShopAlot.Data.Shared.Enumerations
{
    public enum GovernmentScopeEnum
    {
        [Description("Federal")]
        Federal = 1,

        [Description("State")]
        State = 2,

        [Description("County")]
        County = 3,

        [Description("Township")]
        Township = 4,

        [Description("Municipal")]
        Municipal = 5
    }
}
