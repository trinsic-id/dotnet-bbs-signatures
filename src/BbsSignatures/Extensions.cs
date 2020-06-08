using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace BbsSignatures
{
    public static class Extensions
    {
        internal static void ThrowOnError(this ExternError error)
        {
            if (error.Code != 0)
            {
                throw error.Dereference();
            }
        }

        internal static YieldAwaitable ThrowAndYield(this ExternError error)
        {
            if (error.Code != 0)
            {
                throw error.Dereference();
            }
            return Task.Yield();
        }
    }
}