using System;
using System.Linq;

namespace ReadingIsGood.Utils.Jwt
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class JwtCheckBaseAttribute : Attribute
    {
        private readonly string[] _pathSegments;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathSegments"></param>
        protected JwtCheckBaseAttribute(params string[] pathSegments)
        {
            this._pathSegments = pathSegments;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string[] PathSegments => this._pathSegments.Select(x => !x.StartsWith("/") ? $"/{x}" : x).ToArray();
    }
}