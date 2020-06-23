using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BbsSignatures
{
    public class BbsProvider
    {
        public static int SignatureSize => Native.bbs_signature_size();

        public static int BlindSignatureSize => Native.bbs_blind_signature_size();

        /// <summary>
        /// Signs the messages
        /// </summary>
        /// <param name="secretKey">My key.</param>
        /// <param name="messages">The messages.</param>
        /// <returns></returns>
        public static byte[] Sign(BbsSignRequest signRequest)
        {
            if (signRequest.KeyPair is null) throw new BbsException("Public key not found");

            using var context = new UnmanagedMemoryContext();

            var handle = Native.bbs_sign_context_init(out var error);
            context.ThrowIfNeeded(error);

            foreach (var message in signRequest.Messages)
            {
                Native.bbs_sign_context_add_message_string(handle, message, out error);
                context.ThrowIfNeeded(error);
            }

            Native.bbs_sign_context_set_public_key(handle, context.ToBuffer(signRequest.KeyPair), out error);
            context.ThrowIfNeeded(error);

            Native.bbs_sign_context_set_secret_key(handle, context.ToBuffer(blsKey.SecretKey), out error);
            context.ThrowIfNeeded(error);

            Native.bbs_sign_context_finish(handle, out var signature, out error);
            context.ThrowIfNeeded(error);

            return context.ToByteArray(signature);
        }

        /// <summary>
        /// Unblinds the signature asynchronous.
        /// </summary>
        /// <param name="blindedSignature">The blinded signature.</param>
        /// <param name="blindingFactor">The blinding factor.</param>
        /// <returns></returns>
        public static byte[] UnblindSignature(byte[] blindedSignature, byte[] blindingFactor)
        {
            using var context = new UnmanagedMemoryContext();

            Native.bbs_unblind_signature(context.ToBuffer(blindedSignature), context.ToBuffer(blindingFactor), out var unblindSignature, out var error);
            context.ThrowIfNeeded(error);

            return context.ToByteArray(unblindSignature);
        }

        /// <summary>
        /// Verifies the asynchronous.
        /// </summary>
        /// <param name="publicKey">The public key.</param>
        /// <param name="messages">The messages.</param>
        /// <param name="signature">The signature.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static bool Verify(BbsKeyPair publicKey, string[] messages, byte[] signature)
        {
            using var context = new UnmanagedMemoryContext();

            var handle = Native.bbs_verify_context_init(out var error);
            context.ThrowIfNeeded(error);

            Native.bbs_verify_context_set_public_key(handle, context.ToBuffer(publicKey), out error);
            context.ThrowIfNeeded(error);

            Native.bbs_verify_context_set_signature(handle, context.ToBuffer(signature), out error);
            context.ThrowIfNeeded(error);

            foreach (var message in messages)
            {
                Native.bbs_verify_context_add_message_string(handle, message, out error);
                context.ThrowIfNeeded(error);
            }

            var result = Native.bbs_verify_context_finish(handle, out error);
            context.ThrowIfNeeded(error);

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
        public static SignatureProofStatus VerifyProof(BbsKeyPair publicKey, byte[] proof, IndexedMessage[] revealedMessages, string nonce)
        {
            using var context = new UnmanagedMemoryContext();

            var handle = Native.bbs_verify_proof_context_init(out var error);
            context.ThrowIfNeeded(error);

            Native.bbs_verify_proof_context_set_public_key(handle, context.ToBuffer(publicKey), out error);
            context.ThrowIfNeeded(error);

            Native.bbs_verify_proof_context_set_nonce_string(handle, nonce, out error);
            context.ThrowIfNeeded(error);

            Native.bbs_verify_proof_context_set_proof(handle, context.ToBuffer(proof), out error);
            context.ThrowIfNeeded(error);

            foreach (var item in revealedMessages)
            {
                Native.bbs_verify_proof_context_add_message_string(handle, item.Message, out error);
                context.ThrowIfNeeded(error);

                Native.bbs_verify_proof_context_add_revealed_index(handle, item.Index, out error);
                context.ThrowIfNeeded(error);
            }

            var result = Native.bbs_verify_proof_context_finish(handle, out error);
            context.ThrowIfNeeded(error);

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
        public static SignatureProofStatus VerifyBlindedCommitment(byte[] proof, uint[] blindedIndices, BbsKeyPair publicKey, string nonce)
        {
            using var context = new UnmanagedMemoryContext();

            var handle = Native.bbs_verify_blind_commitment_context_init(out var error);
            context.ThrowIfNeeded(error);

            Native.bbs_verify_blind_commitment_context_set_nonce_string(handle, nonce, out error);
            context.ThrowIfNeeded(error);

            Native.bbs_verify_blind_commitment_context_set_proof(handle, context.ToBuffer(proof), out error);
            context.ThrowIfNeeded(error);

            Native.bbs_verify_blind_commitment_context_set_public_key(handle, context.ToBuffer(publicKey), out error);
            context.ThrowIfNeeded(error);

            foreach (var item in blindedIndices)
            {
                Native.bbs_verify_blind_commitment_context_add_blinded(handle, item, out error);
                context.ThrowIfNeeded(error);
            }

            var result = Native.bbs_verify_blind_commitment_context_finish(handle, out error);
            context.ThrowIfNeeded(error);

            return (SignatureProofStatus)result;
        }

        /// <summary>
        /// Blinds the commitment asynchronous.
        /// </summary>
        /// <param name="publicKey">The public key.</param>
        /// <param name="nonce">The nonce.</param>
        /// <param name="messages">The messages.</param>
        /// <returns></returns>
        public static BlindedCommitment CreateBlindedCommitment(BbsKeyPair publicKey, string nonce, IndexedMessage[] blindedMessages)
        {
            using var context = new UnmanagedMemoryContext();

            var handle = Native.bbs_blind_commitment_context_init(out var error);
            context.ThrowIfNeeded(error);

            foreach (var item in blindedMessages)
            {
                Native.bbs_blind_commitment_context_add_message_string(handle, item.Index, item.Message, out error);
                context.ThrowIfNeeded(error);
            }

            Native.bbs_blind_commitment_context_set_nonce_string(handle, nonce, out error);
            context.ThrowIfNeeded(error);

            Native.bbs_blind_commitment_context_set_public_key(handle, context.ToBuffer(publicKey), out error);
            context.ThrowIfNeeded(error);

            Native.bbs_blind_commitment_context_finish(handle, out var commitment, out var outContext, out var blindingFactor, out error);
            context.ThrowIfNeeded(error);

            return new BlindedCommitment(context.ToByteArray(outContext), context.ToByteArray(blindingFactor), context.ToByteArray(commitment));
        }

        /// <summary>
        /// Blinds the sign asynchronous.
        /// </summary>
        /// <param name="keyPair">The signing key containing the secret BLS key.</param>
        /// <param name="commitment">The commitment.</param>
        /// <param name="messages">The messages.</param>
        /// <returns></returns>
        public static byte[] BlindSign(BlsKeyPair keyPair, BbsKeyPair publicKey, byte[] commitment, IndexedMessage[] messages)
        {
            if (keyPair.SecretKey is null) throw new BbsException("Secret key cannot be null");

            using var context = new UnmanagedMemoryContext();

            var handle = Native.bbs_blind_sign_context_init(out var error);
            context.ThrowIfNeeded(error);

            foreach (var item in messages)
            {
                Native.bbs_blind_sign_context_add_message_string(handle, item.Index, item.Message, out error);
                context.ThrowIfNeeded(error);
            }

            Native.bbs_blind_sign_context_set_public_key(handle, context.ToBuffer(publicKey), out error);
            context.ThrowIfNeeded(error);

            Native.bbs_blind_sign_context_set_secret_key(handle, context.ToBuffer(keyPair.SecretKey), out error);
            context.ThrowIfNeeded(error);

            Native.bbs_blind_sign_context_set_commitment(handle, context.ToBuffer(commitment), out error);
            context.ThrowIfNeeded(error);

            Native.bbs_blind_sign_context_finish(handle, out var blindedSignature, out error);
            context.ThrowIfNeeded(error);

            return context.ToByteArray(blindedSignature);
        }

        /// <summary>
        /// Creates the proof asynchronous.
        /// </summary>
        /// <param name="myKey">My key.</param>
        /// <param name="nonce">The nonce.</param>
        /// <param name="messages">The messages.</param>
        /// <returns></returns>
        public static byte[] CreateProof(BbsKeyPair publicKey, ProofMessage[] proofMessages, byte[] blindingFactor, byte[] signature, string nonce)
        {
            using var context = new UnmanagedMemoryContext();

            var handle = Native.bbs_create_proof_context_init(out var error);
            context.ThrowIfNeeded(error);

            foreach (var message in proofMessages)
            {
                Native.bbs_create_proof_context_add_proof_message_string(handle, message.Message, message.ProofType, context.ToBuffer(blindingFactor ?? Array.Empty<byte>()), out error);
                context.ThrowIfNeeded(error);
            }

            Native.bbs_create_proof_context_set_nonce_string(handle, nonce, out error);
            context.ThrowIfNeeded(error);

            Native.bbs_create_proof_context_set_public_key(handle, context.ToBuffer(publicKey), out error);

             context.ThrowIfNeeded(error);

            Native.bbs_create_proof_context_set_signature(handle, context.ToBuffer(signature), out error);
            context.ThrowIfNeeded(error);

            Native.bbs_create_proof_context_finish(handle, out var proof, out error);
            context.ThrowIfNeeded(error);

            return context.ToByteArray(proof);
        }


        /// <summary>
        /// Generates new <see cref="BlsKeyPair"/> using a random seed.
        /// </summary>
        /// <returns></returns>
        public static BlsKeyPair GenerateBlsKey() => GenerateBlsKey(Array.Empty<byte>());

        /// <summary>
        /// Generates new <see cref="BlsKeyPair" /> using a input seed as string
        /// </summary>
        /// <param name="seed">The seed.</param>
        /// <returns></returns>
        public static BlsKeyPair GenerateBlsKey(string seed) => GenerateBlsKey(Encoding.UTF8.GetBytes(seed ?? throw new Exception("Seed cannot be null")));

        /// <summary>
        /// Creates new <see cref="BlsKeyPair"/> using a input seed as byte array.
        /// </summary>
        /// <param name="seed">The seed.</param>
        /// <returns></returns>
        public static BlsKeyPair GenerateBlsKey(byte[] seed)
        {
            using var context = new UnmanagedMemoryContext();

            var result = Native.bls_generate_key(context.ToBuffer(seed), out var publicKey, out var secretKey, out var error);
            context.ThrowIfNeeded(error);

            return new BlsKeyPair(context.ToByteArray(secretKey), context.ToByteArray(publicKey));
        }
    }
}