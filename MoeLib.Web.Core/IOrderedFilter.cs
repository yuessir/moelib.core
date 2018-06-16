using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace MoeLib.Web.Core
{
    /// <summary>
    ///     Interface IOrderedFilter
    /// </summary>
    public interface IOrderedFilter : IFilter
    {
        /// <summary>
        ///     Gets or sets the order.
        /// </summary>
        /// <value>The order.</value>
        int Order { get; set; }
    }

    /// <summary>
    ///     Class OrderedActionFilterAttribute.
    /// </summary>
    public class OrderedActionFilterAttribute : ActionFilterAttribute, IOrderedFilter
    {
        #region IOrderedFilter Members

        /// <summary>
        ///     Gets or sets the order.
        /// </summary>
        /// <value>The order.</value>
        public int Order { get; set; }

        #endregion IOrderedFilter Members
    }

    /// <summary>
    ///     OrderedAuthenticationFilterAttribute.
    /// </summary>
    public abstract class OrderedAuthenticationFilterAttribute : FilterAttribute, IAuthenticationFilter, IOrderedFilter
    {
        #region IAuthenticationFilter Members

        /// <summary>
        ///     Authenticates the request.
        /// </summary>
        /// <param name="context">The authentication context.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>A Task that will perform authentication.</returns>
        public abstract Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken);

        /// <summary>
        ///     Challenges the asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Task.</returns>
        public abstract Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken);

        #endregion IAuthenticationFilter Members

        #region IOrderedFilter Members

        /// <summary>
        ///     Gets or sets the order.
        /// </summary>
        /// <value>The order.</value>
        public int Order { get; set; }

        #endregion IOrderedFilter Members
    }

    /// <summary>
    ///     Class OrderedAuthorizationFilterAttribute.
    /// </summary>
    public class OrderedAuthorizationFilterAttribute : AuthorizationFilterAttribute, IOrderedFilter
    {
        #region IOrderedFilter Members

        /// <summary>
        ///     Gets or sets the order.
        /// </summary>
        /// <value>The order.</value>
        public int Order { get; set; }

        #endregion IOrderedFilter Members
    }

    /// <summary>
    ///     Class OrderedExceptionFilterAttribute.
    /// </summary>
    public class OrderedExceptionFilterAttribute : ExceptionFilterAttribute, IOrderedFilter
    {
        #region IOrderedFilter Members

        /// <summary>
        ///     Gets or sets the order.
        /// </summary>
        /// <value>The order.</value>
        public int Order { get; set; }

        #endregion IOrderedFilter Members
    }
}