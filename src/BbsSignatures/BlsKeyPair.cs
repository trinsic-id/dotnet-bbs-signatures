using BbsSignatures.Bls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace BbsSignatures
{
    public class BlsKeyPair
    {
        public byte[] DeterministicPublicKey { get; private set; }
        public byte[] SecretKey { get; private set; }

        /// <summary>
        /// Creates new <see cref="BlsKeyPair"/> using a random seed.
        /// </summary>
        /// <returns></returns>
        public static BlsKeyPair Generate()
        {
            return NativeMethods.bls_generate_key(ByteBuffer.None, out var pk, out var sk, out var error) switch
            {
                0 => new BlsKeyPair
                {
                    DeterministicPublicKey = pk.Dereference(),
                    SecretKey = sk.Dereference()
                },
                _ => throw error.Dereference()
            };
        }

        /// <summary>
        /// Creates new <see cref="BlsKeyPair"/> using a input seed as string
        /// </summary>
        /// <param name="seed">The seed.</param>
        /// <returns></returns>
        public static BlsKeyPair Generate(string seed) => Generate(Encoding.UTF8.GetBytes(seed));

        /// <summary>
        /// Creates new <see cref="BlsKeyPair"/> using a input seed as byte array.
        /// </summary>
        /// <param name="seed">The seed.</param>
        /// <returns></returns>
        public static BlsKeyPair Generate(byte[] seed)
        {
            return NativeMethods.bls_generate_key(seed, out var pk, out var sk, out var error) switch
            {
                0 => new BlsKeyPair
                {
                    DeterministicPublicKey = pk.Dereference(),
                    SecretKey = sk.Dereference()
                },
                _ => throw error.Dereference()
            };
        }

        public BbsPublicKey GenerateBbsKey(uint messageCount)
        {
            if (SecretKey != null)
            {
                NativeMethods.bls_secret_key_to_bbs_key(SecretKey, messageCount, out var publicKey, out var error);
                error.ThrowOnError();

                return new BbsPublicKey
                {
                    PublicKey = publicKey.Dereference()
                };
            }
            else
            {

                NativeMethods.bls_public_key_to_bbs_key(DeterministicPublicKey, messageCount, out var publicKey, out var error);
                error.ThrowOnError();

                return new BbsPublicKey
                {
                    PublicKey = publicKey.Dereference(),
                    MessageCount = messageCount
                };
            }
        }

        public BbsPublicKey GenerateBbsKeyFromPublicKey(uint messageCount)
        {
            NativeMethods.bls_public_key_to_bbs_key(DeterministicPublicKey, messageCount, out var publicKey, out var error);
            error.ThrowOnError();

            return new BbsPublicKey
            {
                PublicKey = publicKey.Dereference(),
                MessageCount = messageCount
            };
        }
    }
}