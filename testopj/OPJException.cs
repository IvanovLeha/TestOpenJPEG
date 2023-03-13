using System;

namespace testopj
{
    public class OPJException : Exception
    {
        public OPJException()
        {
        }
        
        public OPJException(string message)
            : base(message)
        {
        }
        
        public OPJException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
