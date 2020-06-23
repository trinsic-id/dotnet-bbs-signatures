using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BbsSignatures
{
    /// <summary>
    /// A BBS+ key pair
    /// </summary>
    public class BbsKeyPair
    {
        public BbsKeyPair(byte[]? secretKey, byte[] publicKey, uint messageCount)
        {
            if (secretKey != null)
            {
                SecretKey = new ReadOnlyCollection<byte>(secretKey);
            }
            PublicKey = new ReadOnlyCollection<byte>(publicKey);
            MessageCount = messageCount;
        }

        /// <summary>
        /// Raw public key value for the key pair
        /// </summary>
        /// <returns></returns>
        public ReadOnlyCollection<byte> PublicKey { get; internal set; }

        /// <summary>
        /// Raw secret/private key value for the key pair
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public ReadOnlyCollection<byte>? SecretKey { get; internal set; }

        /// <summary>
        /// Number of messages that can be signed
        /// </summary>
        /// <value>
        /// The message count.
        /// </value>
        public uint MessageCount { get; internal set; }
    }
}