using System.Collections.Generic;

namespace SqlSchemaReplacer.Utils
{
    public static class EnvironmentSchema
    {
        public static readonly Dictionary<string, string> Map =
            new()
            {
                { "SIT",  "HOSTTEST"  },
                { "UAT",  "KRX_OWNER"  },
                { "PROD", "HOSTVPBS" }
            };
    }
}
