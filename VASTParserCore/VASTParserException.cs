using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace VASTParserCore
{
    public sealed class VASTParserException : Exception
    {
        public VASTParserException()
        {
        }

        public VASTParserException(string? message) : base(message)
        {
        }

        public VASTParserException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
