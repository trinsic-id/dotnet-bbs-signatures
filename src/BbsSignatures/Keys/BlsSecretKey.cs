using BbsSignatures.Bls;
using System.Collections.ObjectModel;
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
            if (_dPublicKey != null) return _dPublicKey;

            NativeMethods.bls_get_public_key(Key, out var publicKey, out var error);
            error.ThrowOnError();

            return _dPublicKey = new BlsDeterministicPublicKey
            {
                Key = new ReadOnlyCollection<byte>(publicKey.Dereference())
            };
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
        public static BlsSecretKey Generate()
        {
            return NativeMethods.bls_generate_key(ByteBuffer.None, out var pk, out var sk, out var error) switch
            {
                0 => new BlsSecretKey
                {
                    Key = new ReadOnlyCollection<byte>(sk.Dereference())
                },
                _ => throw error.Dereference()
            };
        }

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
            return NativeMethods.bls_generate_key(seed, out var pk, out var sk, out var error) switch
            {
                0 => new BlsSecretKey
                {
                    Key = new ReadOnlyCollection<byte>(sk.Dereference())
                },
                _ => throw error.Dereference()
            };
        }

        public BbsPublicKey GeneratePublicKey(uint messageCount)
        {
            NativeMethods.bls_secret_key_to_bbs_key(Key, messageCount, out var publicKey, out var error);
            error.ThrowOnError();

            return new BbsPublicKey
            {
                Key = new ReadOnlyCollection<byte>(publicKey.Dereference())
            };
        }
    }
}