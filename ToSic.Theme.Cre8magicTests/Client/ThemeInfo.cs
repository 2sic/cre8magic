using System.Collections.Generic;
using Oqtane.Models;
using Oqtane.Themes;
using Oqtane.Shared;

namespace ToSic.Theme.Cre8magicTests
{
    public class ThemeInfo : ITheme
    {
        public Oqtane.Models.Theme Theme => new Oqtane.Models.Theme
        {
            Name = "ToSic Cre8magicTests",
            Version = "1.0.0",
            PackageName = "ToSic.Theme.Cre8magicTests",
            ThemeSettingsType = "ToSic.Theme.Cre8magicTests.ThemeSettings, ToSic.Theme.Cre8magicTests.Client.Oqtane",
            ContainerSettingsType = "ToSic.Theme.Cre8magicTests.ContainerSettings, ToSic.Theme.Cre8magicTests.Client.Oqtane",
            Resources = new List<Resource>()
            {
		        // obtained from https://cdnjs.com/libraries
                new Resource { ResourceType = ResourceType.Stylesheet, Url = "https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.3.2/css/bootstrap.min.css", Integrity = "sha512-b2QcS5SsA8tZodcDtGRELiGv5SaKSk1vDHDaQRda0htPYWZ6046lr3kJ5bAAQdpV2mmA/4v0wQF9MyU6/pDIAg==", CrossOrigin = "anonymous" },
                new Resource { ResourceType = ResourceType.Stylesheet, Url = "~/Theme.css" },
                new Resource { ResourceType = ResourceType.Script, Url = "https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.3.2/js/bootstrap.bundle.min.js", Integrity = "sha512-X/YkDZyjTf4wyc2Vy16YGCPHwAY8rZJY+POgokZjQB2mhIRFJCckEGc6YyX9eNsPfn0PzThEuNs+uaomE5CO6A==", CrossOrigin = "anonymous" }
            }
        };

    }
}
