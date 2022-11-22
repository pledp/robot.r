using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pLdevTest
{
    static class HandleMethod
    {
        public static void RunMethod(string method)
        {
            List<ExpressionElements> methods;
            methods = new List<ExpressionElements>();

            switch (method)
            {
                case "print":
                    Debug.WriteLine("print");
                    break;
            }
        }
    }
}
