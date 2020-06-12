using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BbsSignatures
{
    public static class Extensions
    {
        /// <summary>
        /// Ases the bytes.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public static byte[] AsBytes(this string message) => Encoding.UTF8.GetBytes(message);
    }
}