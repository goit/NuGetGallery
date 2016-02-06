using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuGetGallery.ViewModels;

namespace NuGetGallery.Services
{
    public interface ILdapService
    {
        LdapUserModel ValidateUsernameAndPassword(string username, string password);

        bool ValidateCredentials(ICollection<Credential> credentials, string password, out Credential matched);
    }
}
