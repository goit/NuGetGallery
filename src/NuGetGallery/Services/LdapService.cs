using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.Protocols;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using NuGetGallery.Authentication;
using NuGetGallery.Authentication.Providers;
using NuGetGallery.Authentication.Providers.Ldap;
using NuGetGallery.Configuration;
using NuGetGallery.ViewModels;

namespace NuGetGallery.Services
{
    public class LdapService : ILdapService
    {
        private static readonly string CredentialType_LdapUser = CredentialTypes.ExternalPrefix + AuthenticationTypes.LdapUser;

        public LdapService(ConfigurationService configuration)
        {
            var ldapConfig = new LdapAuthenticatorConfiguration();
            configuration.ResolveConfigObject(ldapConfig, "Auth.LdapUser.");

            this.Configuration = ldapConfig;
        }

        public LdapAuthenticatorConfiguration Configuration { get; private set; }

        public LdapUserModel ValidateUsernameAndPassword(string username, string password)
        {
            var ldapServer = Configuration.Server;
            var baseDn = Configuration.BaseDn;

            try
            {
                LdapConnection connection = new LdapConnection(ldapServer);
                connection.SessionOptions.SecureSocketLayer = true;
                connection.SessionOptions.VerifyServerCertificate = (ldapConnection, certificate) => true;
                connection.AuthType = AuthType.Negotiate;

                NetworkCredential credential = new NetworkCredential(username, password);
                connection.Credential = credential;
                connection.Bind();

                string filter = string.Format(CultureInfo.InvariantCulture, "(&(objectClass=user)(objectCategory=user) (sAMAccountName={0}))", LdapEncode(username));
                var attributes = new[] { "sAMAccountName", "displayName", "mail" };
                SearchRequest searchRequest = new SearchRequest(baseDn, filter, SearchScope.Subtree, attributes);

                var searchResponse = (SearchResponse)connection.SendRequest(searchRequest);

                if (searchResponse?.ResultCode == ResultCode.Success)
                {
                    var entry = searchResponse.Entries[0];
                    var model = new LdapUserModel
                    {
                        Identity = GetStringValue(entry, "sAMAccountName"),
                        Email = GetStringValue(entry, "mail"),
                        Username = GetStringValue(entry, "sAMAccountName"),
                    };

                    return model;
                }
            }
            catch (Exception)
            {
                return null;
            }

            return null;
        }

        public bool ValidateCredentials(ICollection<Credential> credentials, string password, out Credential matched)
        {
            var ldapCred = credentials.FirstOrDefault(c => c.Type == CredentialType_LdapUser);
            matched = ldapCred;
            if (ldapCred != null)
            {
                try
                {
                    LdapConnection connection = new LdapConnection(this.Configuration.Server);
                    connection.SessionOptions.SecureSocketLayer = true;
                    connection.SessionOptions.VerifyServerCertificate = (ldapConnection, certificate) =>
                    {
                        return true;
                    };
                    connection.AuthType = AuthType.Negotiate;

                    NetworkCredential credential = new NetworkCredential(ldapCred.Value, password);
                    connection.Credential = credential;
                    connection.Bind();

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return false;
        }

        public static string GetStringValue(SearchResultEntry entry, string attributeName)
        {
            var attr = entry.Attributes[attributeName];
            var values = (string[])attr.GetValues(typeof(string));
            if (values.Length == 1)
            {
                return values[0];
            }

            return null;
        }

        public static string LdapEncode(string queryValue)
        {
            var buffer = new StringBuilder(queryValue.Length);

            foreach (var c in queryValue)
            {
                switch (c)
                {
                    case '*':
                        buffer.Append(@"\2a");
                        break;
                    case '(':
                        buffer.Append(@"\28");
                        break;
                    case ')':
                        buffer.Append(@"\29");
                        break;
                    case '\\':
                        buffer.Append(@"\5c");
                        break;
                    case '/':
                        buffer.Append(@"\2f");
                        break;
                    case '\0':
                        buffer.Append(@"\00");
                        break;
                    default:
                        buffer.Append(c);
                        break;
                }
            }

            return buffer.ToString();
        }
    }
}