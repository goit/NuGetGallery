using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NuGetGallery.Authentication;
using NuGetGallery.ViewModels;

namespace NuGetGallery.Services
{
    public class LdapService : ILdapService
    {
        private static readonly string CredentialType_LdapUser = CredentialTypes.ExternalPrefix + AuthenticationTypes.LdapUser;

        public LdapUserModel ValidateUsernameAndPassword(string username, string password)
        {
            if (username == "x" && password == "xx")
            {
                return new LdapUserModel
                {
                    Username = username,
                    Email = username +"@test.dev",
                    Identity = "DOMAIN\\" + username
                };
            }

            return null;
        }

        public bool ValidateCredentials(ICollection<Credential> credentials, string password, out Credential matched)
        {
            var ldapCred = credentials.FirstOrDefault(c => c.Type == CredentialType_LdapUser);
            matched = ldapCred;
            if (ldapCred != null)
            {
                return ValidateUsernameAndPassword(ldapCred.Value, password) != null;
            }

            return false;
        }
    }
}