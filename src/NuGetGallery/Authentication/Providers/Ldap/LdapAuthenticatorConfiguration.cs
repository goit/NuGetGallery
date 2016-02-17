// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Configuration;
using System.Globalization;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.MicrosoftAccount;
using NuGetGallery.Authentication.Providers.MicrosoftAccount;
using NuGetGallery.Configuration;
using Owin;

namespace NuGetGallery.Authentication.Providers.Ldap
{
    public class LdapAuthenticatorConfiguration : AuthenticatorConfiguration
    {
        public string Server { get; set; }

        public string BaseDn { get; set; }

        public LdapAuthenticatorConfiguration()
        {
            AuthenticationType = AuthenticationTypes.LdapUser;
        }
    }
}