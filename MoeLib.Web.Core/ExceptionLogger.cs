using System;
using System.Diagnostics;
using System.Text;
using System.Web;
using System.Web.Http.ExceptionHandling;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using MoeLibCore;

namespace MoeLib.Web.Core
{
    /// <summary>
    ///     ExceptionLogger.
    /// </summary>
    public class ExceptionLogger : System.Web.Http.ExceptionHandling.ExceptionLogger
    {
        /// <summary>
        ///     When overridden in a derived class, logs the exception synchronously.
        /// </summary>
        /// <param name="context">The exception logger context.</param>
        public override void Log(ExceptionLoggerContext context)
        {
            try
            {
                // Retrieve the current HttpContext instance for this request.
                HttpContext httpContext = HttpUtils.GetHttpContext(context.Request);

                if (httpContext == null)
                {
                    return;
                }

                string request = httpContext.Request.Dump();

                StringBuilder messageBuilder = new StringBuilder();

                messageBuilder.AppendFormat("{0} {1} {2} ", httpContext.Request.Method, httpContext.Request.GetEncodedUrl(), httpContext.Response.StatusCode);
                messageBuilder.Append(Environment.NewLine);
                messageBuilder.Append(request);
                messageBuilder.Append(Environment.NewLine);
                messageBuilder.Append(context.Exception.GetExceptionString());

                Trace.TraceError(messageBuilder.ToString());
            }
            catch (Exception e)
            {
                Trace.TraceError("ExceptionLoggerInternalError" + e.Message);
            }
        }
    }
}