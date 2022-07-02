using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityBot.Exceptions 
{
    public class InputException : Exception
    {
        private string Error;
        public InputException (string message)
           : base (message)
        {
            Error = message;
        }
    }
}
