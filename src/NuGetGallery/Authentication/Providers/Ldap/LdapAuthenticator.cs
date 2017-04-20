// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Web.Mvc;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using NuGetGallery.Authentication.Providers.Cookie;
using NuGetGallery.Configuration;
using Owin;

namespace NuGetGallery.Authentication.Providers.Ldap
{
    public class LdapUserAuthenticator : Authenticator<LdapAuthenticatorConfiguration>
    {
        public override AuthenticatorUI GetUI()
        {
            return new AuthenticatorUI(
                Strings.LdapUser_SignInMessage,
                Strings.LdapUser_AccountNoun,
                Strings.LdapUser_Caption, 
                Strings.LdapUser_Description)
            {
                IconCssClass = "icon-windows"
            };
        }
    }
}