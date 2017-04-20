using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using System.Web;
using NuGetGallery.Authentication;
using NuGetGallery.Authentication.Providers.Ldap;
using NuGetGallery.ViewModels;

namespace NuGetGallery.Services
{
    public class NullLdapService : ILdapService
    {
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Type is immutable")]
        public static readonly NullLdapService Instance = new NullLdapService();

        private NullLdapService() { }

        public LdapUserModel ValidateUsernameAndPassword(string username, string password)
        {
            return null;
        }

        public bool ValidateCredentials(ICollection<Credential> credentials, string password, out Credential matched)
        {
            matched = null;
            return false;
        }
    }
}