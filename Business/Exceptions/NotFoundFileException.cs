using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Exceptions
{
    public class NotFoundFileException : Exception
    {
        public string PropertyName { get; set; }

        public NotFoundFileException(string propName,string? message) : base(message)
        {
            PropertyName = propName;
        }
    }
}
