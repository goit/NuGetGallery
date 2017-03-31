﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using AuthenticationTypes = NuGetGallery.Authentication.AuthenticationTypes;
using AuthorizationContext = System.Web.Mvc.AuthorizationContext;

namespace NuGetGallery.Filters
{
    public sealed class ApiScopeRequiredAttribute 
        : AuthorizeAttribute
    {
        public List<string> Scopes { get; set; }
        
        public ApiScopeRequiredAttribute(params string[] scopes)
        {
            Scopes = scopes.ToList();
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var identity = httpContext.User.Identity as ClaimsIdentity;
            if (identity != null && identity.IsAuthenticated)
            {
                return identity.HasScopeThatAllowsActionForSubject(
                    subject: null, 
                    requestedActions: Scopes.ToArray());
            }

            return base.AuthorizeCore(httpContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var owinContext = filterContext.HttpContext.GetOwinContext();
            owinContext.Authentication.Challenge(AuthenticationTypes.ApiKey);
            owinContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            filterContext.Result = new HttpStatusCodeWithBodyResult(HttpStatusCode.Forbidden, Strings.ApiKeyNotAuthorized);
        }
    }
}