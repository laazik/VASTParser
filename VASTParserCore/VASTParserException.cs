///
/// Copyright 2024 Marek Laasik, Rockit Holding OÜ
/// Licensed under EUPL. Please see the following link for details
/// https://joinup.ec.europa.eu/collection/eupl/eupl-text-eupl-12
///

namespace Rockit.VASTParserCore
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
