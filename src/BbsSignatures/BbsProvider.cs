using BbsSignatures.Bls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BbsSignatures
{
    public class BbsProvider
    {
        /// <summary>
        /// Signs the messages
        /// </summary>
        /// <param name="signerKey">The signer key.</param>
        /// <param name="publicKey">The public key.</param>
        /// <param name="messages">The messages.</param>
        /// <returns></returns>
        public static async Task<byte[]> SignAsync(BlsSecretKey myKey, string[] messages)
        {
            var publicKey = myKey.GeneratePublicKey((uint)messages.Length);

            var handle = NativeMethods.bbs_sign_context_init(out var error);
            await error.ThrowAndYield();

            foreach (var message in messages)
            {
                NativeMethods.bbs_sign_context_add_message_string(handle, message, out error);
                await error.ThrowAndYield();
            }

            NativeMethods.bbs_sign_context_set_public_key(handle, publicKey.Key, out error);
            await error.ThrowAndYield();

            NativeMethods.bbs_sign_context_set_secret_key(handle, myKey.Key, out error);
            await error.ThrowAndYield();

            NativeMethods.bbs_sign_context_finish(handle, out var signature, out error);
            await error.ThrowAndYield();

            var actual = signature.Dereference();
            return actual;
        }

        /// <summary>
        /// Blinds the commitment asynchronous.
        /// </summary>
        /// <param name="publicKey">The public key.</param>
        /// <param name="nonce">The nonce.</param>
        /// <param name="messages">The messages.</param>
        /// <returns></returns>
        public static async Task<BlindCommitment> BlindCommitmentAsync(BlsDeterministicPublicKey theirKey, string nonce, string[] messages)
        {
            var publicKey = theirKey.GeneratePublicKey((uint)messages.Length);

            var handle = NativeMethods.bbs_blind_commitment_context_init(out var error);
            await error.ThrowAndYield();

            for (int i = 0; i < messages.Length; i++)
            {
                NativeMethods.bbs_blind_commitment_context_add_message_string(handle, (uint)i, messages[i], out error);
                await error.ThrowAndYield();
            }

            NativeMethods.bbs_blind_commitment_context_set_nonce_string(handle, nonce, out error);
            await error.ThrowAndYield();

            NativeMethods.bbs_blind_commitment_context_set_public_key(handle, publicKey.Key, out error);
            await error.ThrowAndYield();

            NativeMethods.bbs_blind_commitment_context_finish(handle, out var outContext, out var blindingFactor, out error);
            await error.ThrowAndYield();

            return new BlindCommitment
            {
                Context = outContext.Dereference(),
                BlindingFactor = blindingFactor.Dereference()
            };
        }

        /// <summary>
        /// Blinds the sign asynchronous.
        /// </summary>
        /// <param name="myKey">My key.</param>
        /// <param name="context">The context.</param>
        /// <param name="nonce">The nonce.</param>
        /// <param name="messages">The messages.</param>
        /// <returns></returns>
        public static async Task<byte[]> BlindSignAsync(BlsSecretKey myKey, byte[] context, string nonce, string[] messages)
        {
            var publicKey = myKey.GeneratePublicKey((uint)messages.Length);

            var handle = NativeMethods.bbs_blind_sign_context_init(out var error);
            await error.ThrowAndYield();

            for (int i = 0; i < messages.Length; i++)
            {
                NativeMethods.bbs_blind_sign_context_add_message_string(handle, (uint)i, messages[i], out error);
                await error.ThrowAndYield();
            }

            NativeMethods.bbs_blind_sign_context_set_public_key(handle, publicKey.Key, out error);
            await error.ThrowAndYield();

            NativeMethods.bbs_blind_sign_context_set_secret_key(handle, myKey.Key, out error);
            await error.ThrowAndYield();

            NativeMethods.bbs_blind_sign_context_set_commitment(handle, context, out error);
            await error.ThrowAndYield();

            NativeMethods.bbs_blind_sign_context_finish(handle, out var blindedSignature, out error);
            await error.ThrowAndYield();

            return blindedSignature.Dereference();
        }

        public static async Task<byte[]> CreateProofAsync(BlsSecretKey myKey, BlsDeterministicPublicKey theirKey, string nonce, string[] messages)
        {
            var publicKey = theirKey.GeneratePublicKey((uint)messages.Length);

            var handle = NativeMethods.bbs_create_proof_context_init(out var error);
            await error.ThrowAndYield();

            var commitment = await BlindCommitmentAsync(theirKey, nonce, messages);

            foreach (var message in messages)
            {
                NativeMethods.bbs_create_proof_context_add_proof_message_string(handle, message, ProofMessageType.Revealed, commitment.BlindingFactor, out error);
                await error.ThrowAndYield();
            }
            
            NativeMethods.bbs_create_proof_context_set_nonce_string(handle, nonce, out error);
            await error.ThrowAndYield();

            NativeMethods.bbs_create_proof_context_set_public_key(handle, publicKey.Key, out error);
            await error.ThrowAndYield();

            var signature = await SignAsync(myKey, messages);

            NativeMethods.bbs_create_proof_context_set_signature(handle, signature, out error);
            await error.ThrowAndYield();

            NativeMethods.bbs_create_proof_context_finish(handle, out var proof, out error);
            await error.ThrowAndYield();

            return proof.Dereference();
        }
    }
}
