using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace BbsSignatures
{
    /// <summary>
    /// Represents a BLS secret key
    /// </summary>
    public class BlsKeyPair
    {
        private BlsDeterministicPublicKey _deterministicPublicKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlsKeyPair"/> class.
        /// </summary>
        internal BlsKeyPair()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlsKeyPair"/> class.
        /// </summary>
        /// <param name="secretKey">The secret key.</param>
        public BlsKeyPair(byte[] secretKey)
        {
            SecretKey = new ReadOnlyCollection<byte>(secretKey);
        }

        /// <summary>
        /// Gets the deterministic public key.
        /// </summary>
        /// <returns></returns>
        public BlsDeterministicPublicKey PublicKey
        {
            get
            {
                if (_deterministicPublicKey != null) return _deterministicPublicKey;

                using var context = new UnmanagedMemoryContext();
                context.Reference(SecretKey.ToArray(), out var secretKey);

                NativeMethods.bls_get_public_key(secretKey, out var publicKey, out var error);
                context.ThrowIfNeeded(error);

                context.Dereference(publicKey, out var pk);
                return _deterministicPublicKey = new BlsDeterministicPublicKey(pk);
            }
        }

        /// <summary>
        /// The key data
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public ReadOnlyCollection<byte> SecretKey { get; internal set; }

        /// <summary>
        /// Generates public key with the specified <paramref name="messageCount"/>
        /// </summary>
        /// <param name="messageCount">The message count.</param>
        /// <returns></returns>
        public BbsPublicKey GeneratePublicKey(uint messageCount)
        {
            using var context = new UnmanagedMemoryContext();

            context.Reference(SecretKey.ToArray(), out var secretKey);

            NativeMethods.bls_secret_key_to_bbs_key(secretKey, messageCount, out var publicKey, out var error);
            context.ThrowIfNeeded(error);

            context.Dereference(publicKey, out var _publicKey);

            return new BbsPublicKey(_publicKey);
        }
    }
}