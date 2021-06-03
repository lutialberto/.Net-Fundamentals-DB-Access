using System;

namespace ConectandoBD.Utils
{
    public static class ExceptionPrinter
    {
        public static void Print(Exception e)
        {
            Console.WriteLine($"Message: { e.Message }");
        }
    }
}
