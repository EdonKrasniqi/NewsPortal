using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Exceptions
{
    public class AppBadDataException : Exception
    {
        public Dictionary<string, string[]> Errors { get; } = new Dictionary<string, string[]>();

        public AppBadDataException()
        {
        }

        public AppBadDataException(string message) : base(message)
        {
        }

        public AppBadDataException(string message, string path, params string[] errors) : base(message)
        {
            Errors.Add(path, errors);
        }
    }
}
