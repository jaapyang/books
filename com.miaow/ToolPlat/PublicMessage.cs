using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolPlat
{
    public static class PublicMessage
    {
        public static event EventHandler MessageChanged;

        private static string _message;

        public static string Message
        {
            get => _message;
            set
            {
                _message = value;
                MessageChanged?.Invoke(null, null);
            }
        }
    }
}
