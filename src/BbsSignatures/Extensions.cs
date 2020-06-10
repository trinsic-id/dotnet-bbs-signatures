using System.Runtime.CompilerServices;
using System.Text;
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

        /// <summary>
        /// Ases the bytes.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public static byte[] AsBytes(this string message) => Encoding.UTF8.GetBytes(message);
    }
}