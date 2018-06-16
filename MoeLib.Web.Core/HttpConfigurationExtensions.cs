using System.Web.Http;
using System.Web.Http.Filters;

namespace MoeLib.Web.Core
{
    /// <summary>
    ///     HttpConfigurationExtensions.
    /// </summary>
    public static class HttpConfigurationExtensions
    {
        /// <summary>
        ///     Maps the HTTP batch route.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <returns>HttpConfiguration.</returns>
        public static HttpConfiguration MapHttpBatchRoute(this HttpConfiguration config)
        {
            config.Routes.MapHttpBatchRoute("WebApiBatch", "$batch", new BatchHandler(GlobalConfiguration.DefaultServer));
            return config;
        }

        /// <summary>
        ///     Uses the ordered filter.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <returns>HttpConfiguration.</returns>
        public static HttpConfiguration UseOrderedFilter(this HttpConfiguration config)
        {
            config.Services.Replace(typeof(IFilterProvider), new ConfigurationFilterProvider());
            config.Services.Add(typeof(IFilterProvider), new OrderedFilterProvider());
            return config;
        }
    }
}