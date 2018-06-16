// ***********************************************************************
// Assembly         : 
// Author           : SigiLu
// Created          : 2015-11-20  5:55 PM
//
// Last Modified By : Kevin
// Last Modified On : 06-15-2018
// ***********************************************************************
// <copyright file="HttpUtils.cs" company="Shanghai Yuyi Mdt InfoTech Ltd.">
//     Copyright ©  2012-2015 Shanghai Yuyi Mdt InfoTech Ltd. All rights reserved.
// </copyright>
// <summary></summary>
// ************************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using MoeLibCore;
using ReflectionMagic;
using CookieHeaderValue = System.Net.Http.Headers.CookieHeaderValue;


namespace MoeLib.Web.Core
{
    /// <summary>
    ///     HttpUtils.
    /// </summary>
    public static class HttpUtils
    {
        /// <summary>
        ///     The HTTP context base key
        /// </summary>
        private const string HTTP_CONTEXT_BASE_KEY = "MS_HttpContext";

        /// <summary>
        /// Creates the proxy HTTP request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>HttpRequestMessage.</returns>
        public static HttpRequestMessage AsHttpRequestMessage(this HttpRequest request)
        {
          
            var requestMessage = new HttpRequestMessage();
            var requestMethod = request.Method;
            if (!HttpMethods.IsGet(requestMethod) &&
                !HttpMethods.IsHead(requestMethod) &&
                !HttpMethods.IsDelete(requestMethod) &&
                !HttpMethods.IsTrace(requestMethod))
            {
                var streamContent = new StreamContent(request.Body);
                requestMessage.Content = streamContent;
            }


            requestMessage.SetAbsoluteUri(request);
            requestMessage.Method = new HttpMethod(request.Method);

            return requestMessage;
        }
        private static HttpRequestMessage SetAbsoluteUri(this HttpRequestMessage msg, HttpRequest req)
            => msg.Set(m => m.RequestUri = new UriBuilder
            {
                Scheme = req.Scheme,
                Host = req.Host.Host,
                Port = req.Host.Port.Value,
                Path = req.PathBase.Add(req.Path),
                Query = req.QueryString.ToString()
            }.Uri);

        public static Uri GetAbsoluteUri(this HttpRequest req)
        {
            return new UriBuilder
            {
                Scheme = req.Scheme,
                Host = req.Host.Host,
                Port = req.Host.Port.Value,
                Path = req.PathBase.Add(req.Path),
                Query = req.QueryString.ToString()
            }.Uri;
        }

        private static HttpRequestMessage Set(this HttpRequestMessage msg, Action<HttpRequestMessage> config, bool applyIf = true)
        {
            if (applyIf)
            {
                config.Invoke(msg);
            }

            return msg;
        }
        /// <summary>
        ///     Clones an <see cref="HttpWebRequest" /> in order to send it again.
        /// </summary>
        /// <param name="message">The message to set headers on.</param>
        /// <param name="request">The request with headers to clone.</param>
        public static void CopyHeadersFrom(this HttpRequestMessage message, HttpRequest request)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            // Copy the request headers
            foreach (var header in request.Headers)
            {
                if (!message.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray()) && message.Content != null)
                {
                    message.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
                }
            }
        }

        /// <summary>
        ///     Clones an <see cref="HttpWebRequest" /> in order to send it again.
        /// </summary>
        /// <param name="message">The message to set headers on.</param>
        /// <param name="request">The request with headers to clone.</param>
        public static void CopyHeadersFrom(this HttpRequestMessage message, HttpRequestMessage request)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            foreach (KeyValuePair<string, IEnumerable<string>> header in request.Headers.AsEnumerable())
            {
                if (!message.Headers.TryAddWithoutValidation(header.Key, header.Value))
                {
                    message.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }
        }

        /// <summary>
        ///     Dumps the specified request include headers.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="includeHeaders">if set to <c>true</c> [include headers].</param>
        /// <returns>System.String.</returns>
        public static string Dump(this HttpRequest httpRequest, bool includeHeaders = true)
        {
            MemoryStream memoryStream = new MemoryStream();

            try
            {
                TextWriter writer = new StreamWriter(memoryStream);

                writer.Write(httpRequest.Method);
                writer.Write(httpRequest.GetAbsoluteUri().AbsoluteUri);

                // headers

                if (includeHeaders)
                {
                    if (httpRequest.AsDynamic()._wr != null)
                    {
                        // real request -- add protocol
                        writer.Write(" " + httpRequest.AsDynamic()._wr.GetHttpVersion() + "\r\n");

                        // headers
                        writer.Write(httpRequest.AsDynamic().CombineAllHeaders(true));
                    }
                    else
                    {
                        // manufactured request
                        writer.Write("\r\n");
                    }
                }

                writer.Write("\r\n");
                writer.Flush();

                // entity body

                dynamic httpInputStream = httpRequest.AsDynamic().Body;
                httpInputStream.WriteTo(memoryStream);

                StreamReader reader = new StreamReader(memoryStream);
                return reader.ReadToEnd();
            }
            finally
            {
                memoryStream.Close();
            }
        }

        /// <summary>
        ///     Dumps the specified request include headers.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <param name="includeHeaders">if set to <c>true</c> [include headers].</param>
        /// <returns>System.String.</returns>
        public static string Dump(this HttpRequestMessage httpRequest, bool includeHeaders = true)
        {
            MemoryStream memoryStream = new MemoryStream();

            try
            {
                TextWriter writer = new StreamWriter(memoryStream);

                writer.Write(httpRequest.Method + "\r\n");
                writer.Write(httpRequest.RequestUri.AbsoluteUri + "\r\n");
                writer.Write(httpRequest.Version + "\r\n");

                // headers

                if (includeHeaders)
                {
                    foreach (KeyValuePair<string, IEnumerable<string>> header in httpRequest.Headers)
                    {
                        writer.Write(header.Key + ": " + header.Value.Join(","));
                    }
                }

                writer.Write("\r\n");
                writer.Flush();

                // entity body

                Task<string> contentTask = httpRequest.Content.ReadAsStringAsync();
                contentTask.Wait();
                writer.Write(contentTask.Result);

                StreamReader reader = new StreamReader(memoryStream);
                return reader.ReadToEnd();
            }
            finally
            {
                memoryStream.Close();
            }
        }

        /// <summary>
        ///     Returns an individual cookie from the cookies collection.
        /// </summary>
        /// <param name="request">The instance of <see cref="HttpRequestMessage" />.</param>
        /// <param name="cookieName">The name of the cookie.</param>
        /// <returns>The cookie value. Return null if the cookie does not exist.</returns>
        /// <exception cref="System.ArgumentNullException">If the request is null, throw the ArgumentNullException.</exception>
        /// <exception cref="System.ArgumentNullException">If the cookieName is null, throw the ArgumentNullException.</exception>
        public static string GetCookie(HttpRequestMessage request, string cookieName)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            if (cookieName == null)
            {
                throw new ArgumentNullException(nameof(cookieName));
            }
           CookieHeaderValue cookie = request.Headers.GetCookies(cookieName).FirstOrDefault();
            return cookie?[cookieName].Value;
        }

        /// <summary>
        ///     Returns an individual HTTP Header value that joins all the header value with ' '.
        /// </summary>
        /// <param name="request">The instance of <see cref="HttpRequestMessage" />.</param>
        /// <param name="key">The key of the header.</param>
        /// <returns>The HTTP Header value that joins all the header value with ' '. Return null if the header does not exist.</returns>
        /// <exception cref="System.ArgumentNullException">If the request is null, throw the ArgumentNullException.</exception>
        /// <exception cref="System.ArgumentNullException">If the key is null, throw the ArgumentNullException.</exception>
        public static string GetHeader(HttpRequestMessage request, string key)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            IEnumerable<string> keys;
            if (!request.Headers.TryGetValues(key, out keys))
                return null;

            return keys.Join(" ");
        }

        /// <summary>
        ///     Gets the HTTP httpContext.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>HttpContext.</returns>
        public static HttpContext GetHttpContext(HttpRequestMessage request)
        {
            HttpContext contextBase = GetHttpContextBase(request);

            return contextBase == null ? null : contextBase;
        }

 

        /// <summary>
        ///     Returns an individual querystring value.
        /// </summary>
        /// <param name="request">The instance of <see cref="HttpRequestMessage" />.</param>
        /// <param name="key">The key.</param>
        /// <returns>The querystring value. Return null if the querystring does not exist.</returns>
        /// <exception cref="System.ArgumentNullException">If the request is null, throw the ArgumentNullException.</exception>
        /// <exception cref="System.ArgumentNullException">If the key is null, throw the ArgumentNullException.</exception>
        public static string GetQueryString(this HttpRequestMessage request, string key)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            // IEnumerable<KeyValuePair<string,string>> - right!
            IEnumerable<KeyValuePair<string, string>> queryStrings = request.GetQueryNameValuePairs();
            if (queryStrings == null)
                return null;

            KeyValuePair<string, string> match = queryStrings.FirstOrDefault(kv => string.Compare(kv.Key, key, StringComparison.OrdinalIgnoreCase) == 0);
            return string.IsNullOrEmpty(match.Value) ? null : match.Value;
        }

        /// <summary>
        ///     Returns a dictionary of QueryStrings that's easier to work with
        ///     than GetQueryNameValuePairs KevValuePairs collection.
        ///     If you need to pull a few single values use GetQueryString instead.
        /// </summary>
        /// <param name="request">The instance of <see cref="HttpRequestMessage" />.</param>
        /// <returns>The QueryStrings dictionary.</returns>
        /// <exception cref="System.ArgumentNullException">If the request is null, throw the ArgumentNullException.</exception>
        public static Dictionary<string, string> GetQueryStrings(HttpRequestMessage request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            return request.GetQueryNameValuePairs()
                .ToDictionary(kv => kv.Key, kv => kv.Value, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        ///     Returns the user agent string value.
        /// </summary>
        /// <param name="request">The instance of <see cref="HttpRequestMessage" />.</param>
        /// <returns>The user agent string value.</returns>
        /// <exception cref="System.ArgumentNullException">If the request is null, throw the ArgumentNullException.</exception>
        public static string GetUserAgent(HttpRequestMessage request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            return GetUserAgent(request.ToHttpContext());
        }

        /// <summary>
        ///     Returns the user agent string value.
        /// </summary>
        /// <param name="httpContext">The instance of <see cref="HttpContext" />.</param>
        /// <returns>The user agent string value.</returns>
        public static string GetUserAgent(HttpContext context)
        {
            if (context == null)
            {
                return string.Empty;
            }
            return context.Request.Headers[HeaderNames.UserAgent].ToString();
        }

        /// <summary>
        ///     Returns the user host(ip) string value.
        /// </summary>
        /// <param name="request">The instance of <see cref="HttpRequestMessage" />.</param>
        /// <exception cref="System.ArgumentNullException">If the request is null, throw the ArgumentNullException.</exception>
        public static string GetUserHostAddress(HttpRequestMessage request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }
            return GetUserHostAddress(request.ToHttpContext());
        }

        /// <summary>
        ///     Returns the user host(ip) string value.
        /// </summary>
        /// <param name="httpContext">The instance of <see cref="HttpContext" />.</param>
        public static string GetUserHostAddress(HttpContext httpContext)
        {
            return httpContext == null ? "" : httpContext.Connection.RemoteIpAddress.ToString();
        }

        /// <summary>
        ///     Determines whether the specified HTTP httpContext is from dev.
        /// </summary>
        /// <param name="httpContext">The HTTP httpContext.</param>
        /// <param name="ipStartWith">The ip start with.</param>
        /// <returns><c>true</c> if the specified HTTP httpContext is dev; otherwise, <c>false</c>.</returns>
        public static bool IsFrom(HttpContext httpContext, string ipStartWith)
        {
            string ip = GetUserHostAddress(httpContext);
            return !string.IsNullOrEmpty(ip) && ip.StartsWith(ipStartWith, StringComparison.Ordinal);
        }

        /// <summary>
        ///     Determines whether the specified HTTP httpContext is from dev.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="ipStartWith">The ip start with.</param>
        /// <returns><c>true</c> if the specified HTTP httpContext is dev; otherwise, <c>false</c>.</returns>
        public static bool IsFrom(HttpRequestMessage request, string ipStartWith)
        {
            return IsFrom(request.ToHttpContext(), ipStartWith);
        }

        /// <summary>
        ///     Determines whether the specified request is from ios.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>true</c> if the specified request is ios; otherwise, <c>false</c>.</returns>
        public static bool IsFromIos(HttpRequestMessage request)
        {
            return IsFromIos(request.ToHttpContext());
        }

        /// <summary>
        ///     Determines whether the specified request is from ios.
        /// </summary>
        /// <param name="httpContext">The HTTP httpContext.</param>
        /// <returns><c>true</c> if the specified request is ios; otherwise, <c>false</c>.</returns>
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public static bool IsFromIos(HttpContext httpContext)
        {
            string userAgent = GetUserAgent(httpContext);
            return userAgent != null && (userAgent.ToUpperInvariant().Contains("IPHONE") || userAgent.ToUpperInvariant().Contains("IPAD") || userAgent.ToUpperInvariant().Contains("IPOD"));
        }

        /// <summary>
        ///     Determines whether the specified HTTP httpContext is from localhost.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>true</c> if the specified HTTP httpContext is localhost; otherwise, <c>false</c>.</returns>
        public static bool IsFromLocalhost(HttpRequestMessage request)
        {
            return IsFromLocalhost(request.ToHttpContext());
        }

        /// <summary>
        ///     Determines whether the specified HTTP httpContext is from localhost.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns><c>true</c> if the specified HTTP httpContext is localhost; otherwise, <c>false</c>.</returns>
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public static bool IsFromLocalhost(HttpContext httpContext)
        {
            return httpContext.Request.IsLocal();
        }
        public static bool IsLocal(this HttpRequest request)
        {
            if (request.Host.Host.Equals("localhost", StringComparison.OrdinalIgnoreCase))
                return true;

            var connection = request.HttpContext.Connection;

            if (IsSet(connection.RemoteIpAddress))
            {
                return IsSet(connection.LocalIpAddress)
                    ? connection.RemoteIpAddress.Equals(connection.LocalIpAddress)
                    : IPAddress.IsLoopback(connection.RemoteIpAddress);
            }

            return true;
        }
        private const string NullIpAddress = "::1";
        private static bool IsSet(IPAddress address)
        {
            return address != null && address.ToString() != NullIpAddress;
        }
        /// <summary>
        ///     Determines whether the specified request is from mobile device.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns><c>true</c> if the specified request is from mobile device; otherwise, <c>false</c>.</returns>
        //public static bool IsFromMobileBrowser(HttpContext httpContext)
        //{
        //    return httpContext != null && httpContext.Request.Browser.IsMobileDevice;
        //}

        /// <summary>
        ///     Determines whether the specified request is from mobile device.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>true</c> if the specified request is from mobile device; otherwise, <c>false</c>.</returns>
        //public static bool IsFromMobileBrowser(HttpRequestMessage request)
        //{
        //    return IsFromMobileDevice(request.ToHttpContext());
        //}

        /// <summary>
        ///     Determines whether the specified request is from mobile device.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>true</c> if the specified request is from mobile device; otherwise, <c>false</c>.</returns>
        //public static bool IsFromMobileDevice(HttpRequestMessage request)
        //{
        //    return IsFromMobileDevice(request.ToHttpContext());
        //}

        /// <summary>
        ///     Determines whether the specified request is from mobile device.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns><c>true</c> if the specified request is from mobile device; otherwise, <c>false</c>.</returns>
        //public static bool IsFromMobileDevice(HttpContext httpContext)
        //{
        //    if (httpContext == null)
        //    {
        //        return false;
        //    }

        //    string userAgent = GetUserAgent(httpContext);
        //    if (userAgent.IsNullOrEmpty())
        //    {
        //        return false;
        //    }

        //    userAgent = userAgent.ToUpperInvariant();

        //    return userAgent.Contains("IPHONE") || userAgent.Contains("IOS") || userAgent.Contains("IPAD")
        //           || userAgent.Contains("ANDROID") || httpContext.Request.Browser.IsMobileDevice;
        //}

        /// <summary>
        ///     Redirects to.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="uri">The URI.</param>
        /// <returns>HttpResponseMessage.</returns>
        public static HttpResponseMessage RedirectTo(HttpRequestMessage request, string uri)
        {
            return RedirectTo(request, new Uri(uri));
        }

      

        /// <summary>
        ///     Gets the HTTP httpContext.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>HttpContext.</returns>
        public static HttpContext ToHttpContext(this HttpRequestMessage request)
        {
            return GetHttpContext(request);
        }

        /// <summary>
        ///     Gets the HTTP httpContext base.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>HttpContextBase.</returns>
        public static HttpContext ToHttpContextBase(HttpRequestMessage request)
        {
            return GetHttpContextBase(request);
        }

        /// <summary>
        ///     Gets the HTTP httpContext base.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>HttpContextBase.</returns>
        private static HttpContext GetHttpContextBase(HttpRequestMessage request)
        {
            if (request == null)
            {
                return null;
            }

            object value;

            if (!request.Properties.TryGetValue(HTTP_CONTEXT_BASE_KEY, out value))
            {
                return null;
            }

            return value as HttpContext;
        }

        /// <summary>
        ///     Redirects to.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="uri">The URI.</param>
        /// <returns>HttpResponseMessage.</returns>
        private static HttpResponseMessage RedirectTo(HttpRequestMessage request, Uri uri)
        {
            HttpResponseMessage response = request.CreateResponse(HttpStatusCode.Moved);
            response.Headers.Location = uri;
            return response;
        }
    }
}