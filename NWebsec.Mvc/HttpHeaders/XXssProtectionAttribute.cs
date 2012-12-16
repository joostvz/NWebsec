﻿// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using System.Web.Mvc;
using NWebsec.HttpHeaders;
using NWebsec.Modules.Configuration;

namespace NWebsec.Mvc.HttpHeaders
{
    /// <summary>
    /// Specifies whether the X-Xss-Protection security header should be set in the HTTP response.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class XXssProtectionAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Gets or sets whether the X-Xss-Protection security header should be set in the HTTP response.
        /// Possible values are: Disabled, FilterDisabled, FilterEnabled. The default is FilterEnabled.
        /// </summary>
        public HttpHeadersConstants.XXssProtection Policy { get; set; }

        /// <summary>
        /// Gets or sets whether to enable the IE XSS filter block mode. This setting only takes effect when the Policy is set to FilterEnabled.
        /// The default is true.
        /// </summary>
        public bool BlockMode { get; set; }

        public XXssProtectionAttribute()
        {
            Policy = HttpHeadersConstants.XXssProtection.FilterEnabled;
            BlockMode = true;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            new HttpHeaderHelper(filterContext.HttpContext).SetXXssProtectionOverride(new XXssProtectionConfigurationElement { Policy = Policy, BlockMode = BlockMode });
            base.OnActionExecuting(filterContext);
        }
    }
}