﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace NuGetGallery.Areas.Admin.ViewModels
{
    public class SupportRequestAdminsViewModel
    {
        public SupportRequestAdminsViewModel()
        {
            Admins = new List<Models.Admin>();
        }

        public List<Models.Admin> Admins { get; set; }
    }
}