// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Web.Mvc;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using NuGetGallery.Configuration;
using Owin;

namespace NuGetGallery.Authentication.Providers.Ldap
{
    public class LdapUserAuthenticator : Authenticator
    {
        protected override void AttachToOwinApp(ConfigurationService config, IAppBuilder app)
        {
        }

        protected internal override AuthenticatorConfiguration CreateConfigObject()
        {
            return new AuthenticatorConfiguration()
            {
                AuthenticationType = AuthenticationTypes.LdapUser,
                Enabled = true
            };
        }

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