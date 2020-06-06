using BbsSignatures.Bls;
using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace BbsSignatures
{
    public class BlsKey
    {
        public byte[] PublicKey { get; private set; }
        public byte[] SecretKey { get; private set; }

        /// <summary>
        /// Creates new <see cref="BlsKey"/> using a random seed.
        /// </summary>
        /// <returns></returns>
        public static BlsKey Create()
        {
            return NativeMethods.bls_generate_key(ByteArray.None, out var pk, out var sk, out var error) switch
            {
                0 => new BlsKey
                {
                    PublicKey = pk.ToByteArray(),
                    SecretKey = sk.ToByteArray()
                },
                _ => throw error.ToException()
            };
        }

        /// <summary>
        /// Creates new <see cref="BlsKey"/> using a input seed as string
        /// </summary>
        /// <param name="seed">The seed.</param>
        /// <returns></returns>
        public static BlsKey Create(string seed) => Create(Encoding.UTF8.GetBytes(seed));

        /// <summary>
        /// Creates new <see cref="BlsKey"/> using a input seed as byte array.
        /// </summary>
        /// <param name="seed">The seed.</param>
        /// <returns></returns>
        public static BlsKey Create(byte[] seed)
        {
            return NativeMethods.bls_generate_key(ByteArray.Create(seed), out var pk, out var sk, out var error) switch
            {
                0 => new BlsKey
                {
                    PublicKey = pk.ToByteArray(),
                    SecretKey = sk.ToByteArray()
                },
                _ => throw error.ToException()
            };
        }
    }
}