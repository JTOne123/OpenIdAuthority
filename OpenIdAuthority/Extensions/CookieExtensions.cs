﻿// Copyright (c) Ryan Foster. All rights reserved. 
// Licensed under the Apache License, Version 2.0.

using Microsoft.AspNetCore.Http;
using System;

namespace SimpleIAM.OpenIdAuthority
{
    public static class CookieExtensions
    {
        public const string ClientNonceCookieName = OpenIdAuthorityConstants.RecognizedDevices.ClientNonceCookieName;
        public const string DeviceIdCookieName = OpenIdAuthorityConstants.RecognizedDevices.DeviceIdCookieName;

        public static string GetClientNonce(this HttpRequest request)
        {
            return request.Cookies[ClientNonceCookieName];
        }

        public static string GetDeviceId(this HttpRequest request)
        {
            return request.Cookies[DeviceIdCookieName];
        }

        public static void SetClientNonce(this HttpResponse response, string value)
        {
            response.SetSecureCookie(ClientNonceCookieName, value, TimeSpan.FromMinutes(OpenIdAuthorityConstants.OneTimeCode.DefaultValidityMinutes));
        }

        public static void SetDeviceId(this HttpResponse response, string value)
        {
            response.SetSecureCookie(DeviceIdCookieName, value, TimeSpan.FromDays(OpenIdAuthorityConstants.RecognizedDevices.DeviceIdCookieValidityDays));
        }

        public static void SetSecureCookie(this HttpResponse response, string key, string value, TimeSpan maxAge)
        {
            var options = new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                IsEssential = true,
                SameSite = SameSiteMode.None, // we allow cookies to be sent to us with CORS request
                MaxAge = maxAge, // modern browsers, no need to account for clock skew
                Expires = DateTime.UtcNow.Add(maxAge) // older browsers
            };
            response.Cookies.Append(key, value, options);
        }
    }
}
