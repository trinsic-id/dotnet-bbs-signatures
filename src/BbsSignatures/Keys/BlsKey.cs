using System;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace BbsSignatures
{
    /// <summary>
    /// Represents a BLS secret key
    /// </summary>
    public class BlsKey
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlsKey" /> class.
        /// </summary>
        /// <param name="secretKey">The secret key.</param>
        /// <param name="deterministicPublicKey">The deterministic public key.</param>
        internal BlsKey(byte[] secretKey, byte[] deterministicPublicKey)
        {
            SecretKey = new ReadOnlyCollection<byte>(secretKey);
            PublicKey = new ReadOnlyCollection<byte>(deterministicPublicKey);
        }

        public int PublicKeySize => Native.bls_public_key_size();
        public int SecretKeySize => Native.bls_secret_key_size();

        public BlsKey(byte[] keyData)
        {
            if (keyData.Length == SecretKeySize)
            {
                SecretKey = new ReadOnlyCollection<byte>(keyData);

                using var context = new UnmanagedMemoryContext();

                Native.bls_get_public_key(context.ToBuffer(keyData), out var publicKey, out var error);
                context.ThrowIfNeeded(error);

                PublicKey = context.ToReadOnlyCollection(publicKey);
            }
            else if (keyData.Length == PublicKeySize)
            {
                PublicKey = new ReadOnlyCollection<byte>(keyData);
            }
            else
            {
                throw new BbsException("Invalid key size");
            }
        }

        /// <summary>
        /// Gets the deterministic public key.
        /// </summary>
        /// <returns></returns>
        public ReadOnlyCollection<byte> PublicKey { get; internal set; }

        /// <summary>
        /// The key data
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public ReadOnlyCollection<byte>? SecretKey { get; internal set; }

        /// <summary>
        /// Generates public key with the specified <paramref name="messageCount"/>
        /// </summary>
        /// <param name="messageCount">The message count.</param>
        /// <returns></returns>
        public BbsKey GenerateBbsKey(uint messageCount)
        {
            using var context = new UnmanagedMemoryContext();

            if (SecretKey is null)
            {
                Native.bls_public_key_to_bbs_key(context.ToBuffer(PublicKey), messageCount, out var publicKey, out var error);
                context.ThrowIfNeeded(error);

                return new BbsKey(context.ToByteArray(publicKey), messageCount);
            }
            else
            {
                Native.bls_secret_key_to_bbs_key(context.ToBuffer(SecretKey), messageCount, out var publicKey, out var error);
                context.ThrowIfNeeded(error);

                return new BbsKey(context.ToByteArray(publicKey), messageCount);
            }
        }
    }
}