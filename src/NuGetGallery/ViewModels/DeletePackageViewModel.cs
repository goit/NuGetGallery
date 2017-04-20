// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NuGet.Versioning;

namespace NuGetGallery
{
    public class DeletePackageViewModel : DisplayPackageViewModel
    {
        public DeletePackageViewModel(Package package, ReportPackageReason[] reportOtherPackageReasons)
            : base(package, package.PackageRegistration.Packages.OrderByDescending(p => new NuGetVersion(p.Version)))
        {
            DeletePackagesRequest = new DeletePackagesRequest
            {
                Packages = new List<string> { string.Format(CultureInfo.InvariantCulture, "{0}|{1}", package.PackageRegistration.Id, package.Version) },
                ReasonChoices = reportOtherPackageReasons
            };
        }

        public DeletePackagesRequest DeletePackagesRequest { get; set; }
    }
}