﻿namespace ToSic.Cre8Magic.Client.Tokens;

internal interface ITokenReplace
{
    string NameId { get; }
    string? Parse(string? value);
}