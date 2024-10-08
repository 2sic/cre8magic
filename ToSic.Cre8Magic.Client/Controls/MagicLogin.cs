﻿using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Oqtane;

namespace ToSic.Cre8magic.Client.Controls
{
    public abstract class MagicLogin: Oqtane.Themes.Controls.LoginBase, IMagicControlWithSettings
    {
        [Inject] private IStringLocalizer<SharedResources> Localizer { get; set; }

        [CascadingParameter] public MagicSettings Settings { get; set; }

        protected bool IsLoggedIn => _isLoggedIn ??= PageState.User is { IsAuthenticated: true };
        private bool? _isLoggedIn;

        protected string LocalizedLabel => Localizer[IsLoggedIn ? "Logout" : "Login"];

        protected async Task ChangeLogin()
        {
            if (IsLoggedIn)
                await LogoutUser();
            else
                LoginUser();
        }

        public string? Classes(string target) => Settings?.ThemeDesigner.Classes(target);

        public string? Value(string target) => Settings?.ThemeDesigner.Value(target);

        public string? Id(string name) => Settings?.ThemeDesigner.Id(name);
    }
}
