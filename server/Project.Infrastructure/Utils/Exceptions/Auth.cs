using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Infrastructure.Utils.Exceptions
{
    public class ExpiredAccessTokenException : UnauthorizedAccessException
    {
    }
    public class FailAuthenticationAttemptException : UnauthorizedAccessException
    {
        public FailAuthenticationAttemptException(string message = "Failed to authenticate") : base(message) { }
    }
}
