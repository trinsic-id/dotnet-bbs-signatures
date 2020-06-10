using BbsSignatures.Bls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace BbsSignatures
{
    public class BbsProvider
    {
        public static int SignatureSize => NativeMethods.bbs_signature_size();

        public static int BlindSignatureSize => NativeMethods.bbs_blind_signature_size();

        /// <summary>
        /// Signs the messages
        /// </summary>
        /// <param name="secretKey">My key.</param>
        /// <param name="messages">The messages.</param>
        /// <returns></returns>
        public static async Task<byte[]> SignAsync(BlsSecretKey secretKey, BbsPublicKey publicKey, string[] messages)
        {
            var handle = NativeMethods.bbs_sign_context_init(out var error);
            await error.ThrowAndYield();

            foreach (var message in messages)
            {
                NativeMethods.bbs_sign_context_add_message_bytes(handle, message.AsBytes(), out error);
                await error.ThrowAndYield();
            }

            NativeMethods.bbs_sign_context_set_public_key(handle, publicKey.Key, out error);
            await error.ThrowAndYield();

            NativeMethods.bbs_sign_context_set_secret_key(handle, secretKey.Key, out error);
            await error.ThrowAndYield();

            NativeMethods.bbs_sign_context_finish(handle, out var signature, out error);
            await error.ThrowAndYield();

            var actual = signature.Dereference();
            return actual;
        }

        /// <summary>
        /// Unblinds the signature asynchronous.
        /// </summary>
        /// <param name="blindedSignature">The blinded signature.</param>
        /// <param name="blindingFactor">The blinding factor.</param>
        /// <returns></returns>
        public static async Task<byte[]> UnblindSignatureAsync(byte[] blindedSignature, byte[] blindingFactor)
        {
            NativeMethods.bbs_unblind_signature(blindedSignature, blindingFactor, out var unblindSignature, out var error);
            await error.ThrowAndYield();

            return unblindSignature.Dereference();
        }

        /// <summary>
        /// Verifies the asynchronous.
        /// </summary>
        /// <param name="publicKey">The public key.</param>
        /// <param name="messages">The messages.</param>
        /// <param name="signature">The signature.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static async Task<bool> VerifyAsync(BbsPublicKey publicKey, string[] messages, byte[] signature)
        {
            var handle = NativeMethods.bbs_verify_context_init(out var error);
            await error.ThrowAndYield();

            NativeMethods.bbs_verify_context_set_public_key(handle, publicKey.Key, out error);
            await error.ThrowAndYield();

            NativeMethods.bbs_verify_context_set_signature(handle, signature, out error);
            await error.ThrowAndYield();

            foreach (var message in messages)
            {
                NativeMethods.bbs_verify_context_add_message_string(handle, message, out error);
                await error.ThrowAndYield();
            }

            var result = NativeMethods.bbs_verify_context_finish(handle, out error);
            await error.ThrowAndYield();

            return result == 1;
        }

        /// <summary>
        /// Verifies the proof asynchronous.
        /// </summary>
        /// <param name="publicKey">The public key.</param>
        /// <param name="proof">The proof.</param>
        /// <param name="revealedMessages">The indexed messages.</param>
        /// <param name="nonce">The nonce.</param>
        /// <returns></returns>
        public static async Task<SignatureProofStatus> VerifyProofAsync(BbsPublicKey publicKey, byte[] proof, IndexedMessage[] revealedMessages, string nonce)
        {
            var handle = NativeMethods.bbs_verify_proof_context_init(out var error);
            await error.ThrowAndYield();

            NativeMethods.bbs_verify_proof_context_set_public_key(handle, publicKey.Key, out error);
            await error.ThrowAndYield();

            NativeMethods.bbs_verify_proof_context_set_nonce_string(handle, nonce, out error);
            await error.ThrowAndYield();

            NativeMethods.bbs_verify_proof_context_set_proof(handle, proof, out error);
            await error.ThrowAndYield();

            foreach (var item in revealedMessages)
            {
                NativeMethods.bbs_verify_proof_context_add_message_string(handle, item.Message, out error);
                await error.ThrowAndYield();

                NativeMethods.bbs_verify_proof_context_add_revealed_index(handle, item.Index, out error);
                await error.ThrowAndYield();
            }

            var result = NativeMethods.bbs_verify_proof_context_finish(handle, out error);
            await error.ThrowAndYield();

            return (SignatureProofStatus)result;
        }

        /// <summary>
        /// Verifies the blind commitment asynchronous.
        /// </summary>
        /// <param name="proof">The proof.</param>
        /// <param name="blindedIndices">The blinded indices.</param>
        /// <param name="publicKey">The public key.</param>
        /// <param name="nonce">The nonce.</param>
        /// <returns></returns>
        public static async Task<SignatureProofStatus> VerifyBlindedCommitmentAsync(byte[] proof, uint[] blindedIndices, BbsPublicKey publicKey, string nonce)
        {
            var handle = NativeMethods.bbs_verify_blind_commitment_context_init(out var error);
            await error.ThrowAndYield();

            NativeMethods.bbs_verify_blind_commitment_context_set_nonce_string(handle, nonce, out error);
            await error.ThrowAndYield();

            NativeMethods.bbs_verify_blind_commitment_context_set_proof(handle, proof, out error);
            await error.ThrowAndYield();

            NativeMethods.bbs_verify_blind_commitment_context_set_public_key(handle, publicKey.Key, out error);
            await error.ThrowAndYield();

            foreach (var item in blindedIndices)
            {
                NativeMethods.bbs_verify_blind_commitment_context_add_blinded(handle, item, out error);
                await error.ThrowAndYield();
            }

            var result = NativeMethods.bbs_verify_blind_commitment_context_finish(handle, out error);
            await error.ThrowAndYield();

            return (SignatureProofStatus)result;
        }

        /// <summary>
        /// Blinds the commitment asynchronous.
        /// </summary>
        /// <param name="publicKey">The public key.</param>
        /// <param name="nonce">The nonce.</param>
        /// <param name="messages">The messages.</param>
        /// <returns></returns>
        public static async Task<BlindCommitment> CreateBlindCommitmentAsync(BbsPublicKey publicKey, string nonce, IndexedMessage[] blindedMessages)
        {
            var handle = NativeMethods.bbs_blind_commitment_context_init(out var error);
            await error.ThrowAndYield();

            foreach (var item in blindedMessages)
            {
                NativeMethods.bbs_blind_commitment_context_add_message_string(handle, item.Index, item.Message, out error);
                await error.ThrowAndYield();
            }

            NativeMethods.bbs_blind_commitment_context_set_nonce_string(handle, nonce, out error);
            await error.ThrowAndYield();

            NativeMethods.bbs_blind_commitment_context_set_public_key(handle, publicKey.Key, out error);
            await error.ThrowAndYield();

            NativeMethods.bbs_blind_commitment_context_finish(handle, out var commitment, out var outContext, out var blindingFactor, out error);
            await error.ThrowAndYield();

            return new BlindCommitment
            {
                Commitment = new ReadOnlyCollection<byte>(commitment.Dereference()),
                BlindSignContext = new ReadOnlyCollection<byte>(outContext.Dereference()),
                BlindingFactor = new ReadOnlyCollection<byte>(blindingFactor.Dereference())
            };
        }

        /// <summary>
        /// Blinds the sign asynchronous.
        /// </summary>
        /// <param name="myKey">My key.</param>
        /// <param name="commitment">The commitment.</param>
        /// <param name="messages">The messages.</param>
        /// <returns></returns>
        public static async Task<byte[]> BlindSignAsync(BlsSecretKey myKey, BbsPublicKey publicKey, byte[] commitment, IndexedMessage[] messages)
        {
            var handle = NativeMethods.bbs_blind_sign_context_init(out var error);
            await error.ThrowAndYield();

            foreach (var item in messages)
            {
                NativeMethods.bbs_blind_sign_context_add_message_string(handle, item.Index, item.Message, out error);
                await error.ThrowAndYield();
            }

            NativeMethods.bbs_blind_sign_context_set_public_key(handle, publicKey.Key, out error);
            await error.ThrowAndYield();

            NativeMethods.bbs_blind_sign_context_set_secret_key(handle, myKey.Key, out error);
            await error.ThrowAndYield();

            NativeMethods.bbs_blind_sign_context_set_commitment(handle, commitment, out error);
            await error.ThrowAndYield();

            NativeMethods.bbs_blind_sign_context_finish(handle, out var blindedSignature, out error);
            await error.ThrowAndYield();

            return blindedSignature.Dereference();
        }

        /// <summary>
        /// Creates the proof asynchronous.
        /// </summary>
        /// <param name="myKey">My key.</param>
        /// <param name="nonce">The nonce.</param>
        /// <param name="messages">The messages.</param>
        /// <returns></returns>
        public static async Task<byte[]> CreateProofAsync(BbsPublicKey publicKey, ProofMessage[] proofMessages, byte[] blindingFactor, byte[] unblindedSignature, string nonce)
        {
            var handle = NativeMethods.bbs_create_proof_context_init(out var error);
            await error.ThrowAndYield();

            foreach (var message in proofMessages)
            {
                NativeMethods.bbs_create_proof_context_add_proof_message_string(handle, message.Message, message.ProofType, blindingFactor, out error);
                await error.ThrowAndYield();
            }
            
            NativeMethods.bbs_create_proof_context_set_nonce_string(handle, nonce, out error);
            await error.ThrowAndYield();

            NativeMethods.bbs_create_proof_context_set_public_key(handle, publicKey.Key, out error);
            await error.ThrowAndYield();

            NativeMethods.bbs_create_proof_context_set_signature(handle, unblindedSignature, out error);
            await error.ThrowAndYield();

            NativeMethods.bbs_create_proof_context_finish(handle, out var proof, out error);
            await error.ThrowAndYield();

            return proof.Dereference();
        }
    }
}
