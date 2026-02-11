using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Core.Exceptions
{
    public class AppConflictException : Exception
    {
        public object? Value { get; set; }

        public AppConflictException(string message) : base(message)
        {
        }

        public AppConflictException(string message, object value) : base(message)
        {
            Value = value;
        }

        public AppConflictException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AppConflictException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
