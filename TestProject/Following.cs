using System;

namespace TestProject
{
    public static class Following
    {
        public static Action Code(Action a)
        {
            return a;
        }
    }
}