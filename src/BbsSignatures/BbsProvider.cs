using BbsSignatures.Bls;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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
        public static Task<byte[]> SignAsync(BlsSecretKey secretKey, BbsPublicKey publicKey, string[] messages)
        {
            using var context = new UnmanagedMemoryContext();
            unsafe
            {
                var handle = NativeMethods.bbs_sign_context_init(out var error);
                error.ThrowIfNeeded();

                foreach (var message in messages)
                {
                    NativeMethods.bbs_sign_context_add_message_string(handle, message, out error);
                    error.ThrowIfNeeded();
                }

                context.Reference(publicKey.Key.ToArray(), out var publicKey_);
                NativeMethods.bbs_sign_context_set_public_key(handle, &publicKey_, out error);
                error.ThrowIfNeeded();

                context.Reference(secretKey.Key.ToArray(), out var secretKey_);
                NativeMethods.bbs_sign_context_set_secret_key(handle, &secretKey_, out error);
                error.ThrowIfNeeded();

                NativeMethods.bbs_sign_context_finish(handle, out var signature, out error);
                error.ThrowIfNeeded();

                context.Dereference(signature, out var signature_);
                return Task.FromResult(signature_);
            }
        }

        /// <summary>
        /// Unblinds the signature asynchronous.
        /// </summary>
        /// <param name="blindedSignature">The blinded signature.</param>
        /// <param name="blindingFactor">The blinding factor.</param>
        /// <returns></returns>
        public static Task<byte[]> UnblindSignatureAsync(byte[] blindedSignature, byte[] blindingFactor)
        {
            using var context = new UnmanagedMemoryContext();

            unsafe
            {
                context.Reference(blindedSignature, out var blindedSignature_);
                context.Reference(blindingFactor, out var blindingFactor_);

                NativeMethods.bbs_unblind_signature(&blindedSignature_, &blindingFactor_, out var unblindSignature, out var error);
                error.ThrowIfNeeded();

                context.Dereference(unblindSignature, out var unblindSignature_);

                return Task.FromResult(unblindSignature_);
            }
        }

        /// <summary>
        /// Verifies the asynchronous.
        /// </summary>
        /// <param name="publicKey">The public key.</param>
        /// <param name="messages">The messages.</param>
        /// <param name="signature">The signature.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static Task<bool> VerifyAsync(BbsPublicKey publicKey, string[] messages, byte[] signature)
        {
            using var context = new UnmanagedMemoryContext();
            unsafe
            {
                var handle = NativeMethods.bbs_verify_context_init(out var error);
                error.ThrowIfNeeded();

                context.Reference(publicKey.Key.ToArray(), out var publicKey_);
                NativeMethods.bbs_verify_context_set_public_key(handle, &publicKey_, out error);
                error.ThrowIfNeeded();

                context.Reference(signature, out var signature_);
                NativeMethods.bbs_verify_context_set_signature(handle, &signature_, out error);
                error.ThrowIfNeeded();

                foreach (var message in messages)
                {
                    NativeMethods.bbs_verify_context_add_message_string(handle, message, out error);
                    error.ThrowIfNeeded();
                }

                var result = NativeMethods.bbs_verify_context_finish(handle, out error);
                error.ThrowIfNeeded();

                return Task.FromResult(result == 1);
            }
        }

        /// <summary>
        /// Verifies the proof asynchronous.
        /// </summary>
        /// <param name="publicKey">The public key.</param>
        /// <param name="proof">The proof.</param>
        /// <param name="revealedMessages">The indexed messages.</param>
        /// <param name="nonce">The nonce.</param>
        /// <returns></returns>
        public static Task<SignatureProofStatus> VerifyProofAsync(BbsPublicKey publicKey, byte[] proof, IndexedMessage[] revealedMessages, string nonce)
        {
            using var context = new UnmanagedMemoryContext();
            unsafe
            {
                var handle = NativeMethods.bbs_verify_proof_context_init(out var error);
                error.ThrowIfNeeded();

                context.Reference(publicKey.Key.ToArray(), out var publicKey_);
                NativeMethods.bbs_verify_proof_context_set_public_key(handle, &publicKey_, out error);
                error.ThrowIfNeeded();

                NativeMethods.bbs_verify_proof_context_set_nonce_string(handle, nonce, out error);
                error.ThrowIfNeeded();

                context.Reference(proof, out var proof_);
                NativeMethods.bbs_verify_proof_context_set_proof(handle, &proof_, out error);
                error.ThrowIfNeeded();

                foreach (var item in revealedMessages)
                {
                    NativeMethods.bbs_verify_proof_context_add_message_string(handle, item.Message, out error);
                    error.ThrowIfNeeded();

                    NativeMethods.bbs_verify_proof_context_add_revealed_index(handle, item.Index, out error);
                    error.ThrowIfNeeded();
                }

                var result = NativeMethods.bbs_verify_proof_context_finish(handle, out error);
                error.ThrowIfNeeded();

                return Task.FromResult((SignatureProofStatus)result);
            }
        }

        /// <summary>
        /// Verifies the blind commitment asynchronous.
        /// </summary>
        /// <param name="proof">The proof.</param>
        /// <param name="blindedIndices">The blinded indices.</param>
        /// <param name="publicKey">The public key.</param>
        /// <param name="nonce">The nonce.</param>
        /// <returns></returns>
        public static Task<SignatureProofStatus> VerifyBlindedCommitmentAsync(byte[] proof, uint[] blindedIndices, BbsPublicKey publicKey, string nonce)
        {
            using var context = new UnmanagedMemoryContext();
            unsafe
            {
                var handle = NativeMethods.bbs_verify_blind_commitment_context_init(out var error);
                error.ThrowIfNeeded();

                NativeMethods.bbs_verify_blind_commitment_context_set_nonce_string(handle, nonce, out error);
                error.ThrowIfNeeded();

                context.Reference(proof, out var proof_);
                NativeMethods.bbs_verify_blind_commitment_context_set_proof(handle, &proof_, out error);
                error.ThrowIfNeeded();

                context.Reference(publicKey.Key.ToArray(), out var publicKey_);
                NativeMethods.bbs_verify_blind_commitment_context_set_public_key(handle, &publicKey_, out error);
                error.ThrowIfNeeded();

                foreach (var item in blindedIndices)
                {
                    NativeMethods.bbs_verify_blind_commitment_context_add_blinded(handle, item, out error);
                    error.ThrowIfNeeded();
                }

                var result = NativeMethods.bbs_verify_blind_commitment_context_finish(handle, out error);
                error.ThrowIfNeeded();

                return Task.FromResult((SignatureProofStatus)result);
            }
        }

        /// <summary>
        /// Blinds the commitment asynchronous.
        /// </summary>
        /// <param name="publicKey">The public key.</param>
        /// <param name="nonce">The nonce.</param>
        /// <param name="messages">The messages.</param>
        /// <returns></returns>
        public static Task<BlindCommitment> CreateBlindCommitmentAsync(BbsPublicKey publicKey, string nonce, IndexedMessage[] blindedMessages)
        {
            using var context = new UnmanagedMemoryContext();

            unsafe
            {
                var handle = NativeMethods.bbs_blind_commitment_context_init(out var error);
                error.ThrowIfNeeded();

                foreach (var item in blindedMessages)
                {
                    NativeMethods.bbs_blind_commitment_context_add_message_string(handle, item.Index, item.Message, out error);
                    error.ThrowIfNeeded();
                }

                NativeMethods.bbs_blind_commitment_context_set_nonce_string(handle, nonce, out error);
                error.ThrowIfNeeded();

                context.Reference(publicKey.Key.ToArray(), out var publicKey_);
                NativeMethods.bbs_blind_commitment_context_set_public_key(handle, &publicKey_, out error);
                error.ThrowIfNeeded();

                NativeMethods.bbs_blind_commitment_context_finish(handle, out var commitment, out var outContext, out var blindingFactor, out error);
                error.ThrowIfNeeded();

                context.Dereference(commitment, out var _commitment);
                context.Dereference(outContext, out var _outContext);
                context.Dereference(blindingFactor, out var _blindingFactor);

                return Task.FromResult(new BlindCommitment
                {
                    Commitment = new ReadOnlyCollection<byte>(_commitment),
                    BlindSignContext = new ReadOnlyCollection<byte>(_outContext),
                    BlindingFactor = new ReadOnlyCollection<byte>(_blindingFactor)
                });
            }
        }

        /// <summary>
        /// Blinds the sign asynchronous.
        /// </summary>
        /// <param name="myKey">My key.</param>
        /// <param name="commitment">The commitment.</param>
        /// <param name="messages">The messages.</param>
        /// <returns></returns>
        public static Task<byte[]> BlindSignAsync(BlsSecretKey myKey, BbsPublicKey publicKey, byte[] commitment, IndexedMessage[] messages)
        {
            using var context = new UnmanagedMemoryContext();

            unsafe
            {
                var handle = NativeMethods.bbs_blind_sign_context_init(out var error);
                error.ThrowIfNeeded();

                foreach (var item in messages)
                {
                    NativeMethods.bbs_blind_sign_context_add_message_string(handle, item.Index, item.Message, out error);
                    error.ThrowIfNeeded();
                }

                context.Reference(publicKey.Key.ToArray(), out var publicKey_);
                NativeMethods.bbs_blind_sign_context_set_public_key(handle, &publicKey_, out error);
                error.ThrowIfNeeded();

                context.Reference(myKey.Key.ToArray(), out var secretKey_);
                NativeMethods.bbs_blind_sign_context_set_secret_key(handle, &secretKey_, out error);
                error.ThrowIfNeeded();

                context.Reference(commitment, out var commitment_);
                NativeMethods.bbs_blind_sign_context_set_commitment(handle, &commitment_, out error);
                error.ThrowIfNeeded();

                NativeMethods.bbs_blind_sign_context_finish(handle, out var blindedSignature, out error);
                error.ThrowIfNeeded();

                context.Dereference(blindedSignature, out var blindedSignature_);
                return Task.FromResult(blindedSignature_);
            }
        }

        /// <summary>
        /// Creates the proof asynchronous.
        /// </summary>
        /// <param name="myKey">My key.</param>
        /// <param name="nonce">The nonce.</param>
        /// <param name="messages">The messages.</param>
        /// <returns></returns>
        public static Task<byte[]> CreateProofAsync(BbsPublicKey publicKey, ProofMessage[] proofMessages, byte[] blindingFactor, byte[] unblindedSignature, string nonce)
        {
            using var context = new UnmanagedMemoryContext();
            unsafe
            {
                var handle = NativeMethods.bbs_create_proof_context_init(out var error);
                error.ThrowIfNeeded();

                context.Reference(blindingFactor, out var blindingFactor_);
                foreach (var message in proofMessages)
                {
                    NativeMethods.bbs_create_proof_context_add_proof_message_string(handle, message.Message, message.ProofType, &blindingFactor_, out error);
                    error.ThrowIfNeeded();
                }

                NativeMethods.bbs_create_proof_context_set_nonce_string(handle, nonce, out error);
                error.ThrowIfNeeded();

                context.Reference(publicKey.Key.ToArray(), out var publicKey_);
                NativeMethods.bbs_create_proof_context_set_public_key(handle, &publicKey_, out error);
                error.ThrowIfNeeded();

                context.Reference(unblindedSignature, out var unblindedSignature_);
                NativeMethods.bbs_create_proof_context_set_signature(handle, &unblindedSignature_, out error);
                error.ThrowIfNeeded();

                NativeMethods.bbs_create_proof_context_finish(handle, out var proof, out error);
                error.ThrowIfNeeded();

                context.Dereference(proof, out var proof_);

                return Task.FromResult(proof_);
            }
        }
    }
}
