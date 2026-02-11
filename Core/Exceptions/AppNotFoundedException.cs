using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Core.Exceptions
{
    public class AppNotFoundException : Exception
    {
        public object? Value { get; }

        public AppNotFoundException() : base(Constants.ErrorMessages.ResourceNotFounded)
        {

        }

        public AppNotFoundException(object value)
        {
            Value = value;
        }

        public AppNotFoundException(string message) : base(message)
        {
        }

        public AppNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AppNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
