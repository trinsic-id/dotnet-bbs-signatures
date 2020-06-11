using BbsSignatures.Bls;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace BbsSignatures
{
    public class BlsSecretKey
    {
        BlsDeterministicPublicKey _dPublicKey;

        /// <summary>
        /// Gets the deterministic public key.
        /// </summary>
        /// <returns></returns>
        public BlsDeterministicPublicKey GetDeterministicPublicKey()
        {
            using var context = new UnmanagedMemoryContext();

            if (_dPublicKey != null) return _dPublicKey;

            var secretKey = context.Reference(Key.ToArray());
            unsafe
            {
                NativeMethods.bls_get_public_key(&secretKey, out var publicKey, out var error);
                error.ThrowIfNeeded();

                context.Dereference(publicKey, out var pk);
                return _dPublicKey = new BlsDeterministicPublicKey
                {
                    Key = new ReadOnlyCollection<byte>(pk)
                };
            }
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public ReadOnlyCollection<byte> Key { get; private set; }

        /// <summary>
        /// Creates new <see cref="BlsSecretKey"/> using a random seed.
        /// </summary>
        /// <returns></returns>
        public static BlsSecretKey Generate() => Generate(Array.Empty<byte>());

        /// <summary>
        /// Creates new <see cref="BlsSecretKey"/> using a input seed as string
        /// </summary>
        /// <param name="seed">The seed.</param>
        /// <returns></returns>
        public static BlsSecretKey Generate(string seed) => Generate(Encoding.UTF8.GetBytes(seed));

        /// <summary>
        /// Creates new <see cref="BlsSecretKey"/> using a input seed as byte array.
        /// </summary>
        /// <param name="seed">The seed.</param>
        /// <returns></returns>
        public static BlsSecretKey Generate(byte[] seed)
        {
            unsafe
            {
                using var context = new UnmanagedMemoryContext();
                var seedRef = context.Reference(seed);
                var result = NativeMethods.bls_generate_key(&seedRef, out var pk, out var sk, out var error);

                context.Dereference(pk, out var publicKey);
                context.Dereference(sk, out var secretKey);

                return result switch
                {
                    0 => new BlsSecretKey
                    {
                        Key = new ReadOnlyCollection<byte>(secretKey)
                    },
                    _ => throw error.Dereference()
                };
            }
        }

        public BbsPublicKey GeneratePublicKey(uint messageCount)
        {
            unsafe
            {
                using var context = new UnmanagedMemoryContext();

                var secretKey = context.Reference(Key.ToArray());

                NativeMethods.bls_secret_key_to_bbs_key(&secretKey, messageCount, out var publicKey, out var error);
                error.ThrowIfNeeded();

                context.Dereference(publicKey, out var _publicKey);

                return new BbsPublicKey
                {
                    Key = new ReadOnlyCollection<byte>(_publicKey)
                };
            }
        }
    }
}