using System;
using System.Linq;
using System.Text;

namespace BbsSignatures
{
    public class BbsProvider
    {
        public static int SignatureSize => Native.bbs_signature_size();

        public static int BlindSignatureSize => Native.bbs_blind_signature_size();

        /// <summary>
        /// Signs a set of messages with a BBS key pair and produces a BBS signature
        /// </summary>
        /// <param name="signRequest">Request for the sign operation</param>
        /// <returns>The raw signature value</returns>
        /// <exception cref="BbsException">
        /// Secret key not found
        /// or
        /// Messages cannot be null
        /// </exception>
        public static byte[] Sign(SignRequest signRequest)
        {
            if (signRequest?.KeyPair?.SecretKey is null) throw new BbsException("Secret key not found");
            if (signRequest?.Messages is null) throw new BbsException("Messages cannot be null");

            var bbsKeyPair = signRequest.KeyPair.GeyBbsKeyPair((uint)signRequest.Messages.Length);

            using var context = new UnmanagedMemoryContext();

            var handle = Native.bbs_sign_context_init(out var error);
            context.ThrowIfNeeded(error);

            foreach (var message in signRequest.Messages)
            {
                Native.bbs_sign_context_add_message_string(handle, message, out error);
                context.ThrowIfNeeded(error);
            }

            Native.bbs_sign_context_set_public_key(handle, context.ToBuffer(bbsKeyPair.PublicKey), out error);
            context.ThrowIfNeeded(error);

            Native.bbs_sign_context_set_secret_key(handle, context.ToBuffer(signRequest.KeyPair.SecretKey!), out error);
            context.ThrowIfNeeded(error);

            Native.bbs_sign_context_finish(handle, out var signature, out error);
            context.ThrowIfNeeded(error);

            return context.ToByteArray(signature);
        }

        /// <summary>
        /// Verifies a BBS+ signature for a set of messages with a BBS public key
        /// </summary>
        /// <param name="verifyRequest">Request for the signature verification operation</param>
        /// <returns>
        /// A result indicating if the signature was verified
        /// </returns>
        public static bool Verify(VerifyRequest verifyRequest)
        {
            using var context = new UnmanagedMemoryContext();

            var handle = Native.bbs_verify_context_init(out var error);
            context.ThrowIfNeeded(error);

            Native.bbs_verify_context_set_public_key(handle, context.ToBuffer(verifyRequest.KeyPair.PublicKey), out error);
            context.ThrowIfNeeded(error);

            Native.bbs_verify_context_set_signature(handle, context.ToBuffer(verifyRequest.Signature), out error);
            context.ThrowIfNeeded(error);

            foreach (var message in verifyRequest.Messages)
            {
                Native.bbs_verify_context_add_message_string(handle, message, out error);
                context.ThrowIfNeeded(error);
            }

            var result = Native.bbs_verify_context_finish(handle, out error);
            context.ThrowIfNeeded(error);

            return result == 1;
        }

        /// <summary>
        /// Signs a set of messages featuring both known and blinded messages to the signer and produces a BBS+ signature
        /// </summary>
        /// <param name="keyPair">The signing key containing the secret BLS key.</param>
        /// <param name="commitment">The commitment.</param>
        /// <param name="messages">The messages.</param>
        /// <returns></returns>
        public static byte[] BlindSign(BlindSignRequest request)
        {
            using var context = new UnmanagedMemoryContext();

            var handle = Native.bbs_blind_sign_context_init(out var error);
            context.ThrowIfNeeded(error);

            foreach (var item in request.Messages)
            {
                Native.bbs_blind_sign_context_add_message_string(handle, item.Index, item.Message, out error);
                context.ThrowIfNeeded(error);
            }

            Native.bbs_blind_sign_context_set_public_key(handle, context.ToBuffer(request.PublicKey), out error);
            context.ThrowIfNeeded(error);

            Native.bbs_blind_sign_context_set_secret_key(handle, context.ToBuffer(request.SecretKey.SecretKey.ToArray()), out error);
            context.ThrowIfNeeded(error);

            Native.bbs_blind_sign_context_set_commitment(handle, context.ToBuffer(request.Commitment), out error);
            context.ThrowIfNeeded(error);

            Native.bbs_blind_sign_context_finish(handle, out var blindedSignature, out error);
            context.ThrowIfNeeded(error);

            return context.ToByteArray(blindedSignature);
        }

        /// <summary>
        /// Unblinds the signature asynchronous.
        /// </summary>
        /// <param name="request">Unbling signature request</param>
        /// <returns></returns>
        public static byte[] UnblindSignature(UnblindSignatureRequest request)
        {
            using var context = new UnmanagedMemoryContext();

            Native.bbs_unblind_signature(context.ToBuffer(request.BlindedSignature), context.ToBuffer(request.BlindingFactor), out var unblindedSignature, out var error);
            context.ThrowIfNeeded(error);

            return context.ToByteArray(unblindedSignature);
        }

        /// <summary>
        /// Create a blinded commitment of messages for use in producing a blinded BBS+ signature
        /// </summary>
        /// <param name="request">Request for producing the blinded commitment</param>
        /// <returns></returns>
        public static BlindedCommitment CreateBlindedCommitment(CreateBlindedCommitmentRequest request)
        {
            using var context = new UnmanagedMemoryContext();

            var handle = Native.bbs_blind_commitment_context_init(out var error);
            context.ThrowIfNeeded(error);

            foreach (var item in request.Messages)
            {
                Native.bbs_blind_commitment_context_add_message_string(handle, item.Index, item.Message, out error);
                context.ThrowIfNeeded(error);
            }

            Native.bbs_blind_commitment_context_set_nonce_string(handle, request.Nonce, out error);
            context.ThrowIfNeeded(error);

            Native.bbs_blind_commitment_context_set_public_key(handle, context.ToBuffer(request.PublicKey), out error);
            context.ThrowIfNeeded(error);

            Native.bbs_blind_commitment_context_finish(handle, out var commitment, out var outContext, out var blindingFactor, out error);
            context.ThrowIfNeeded(error);

            return new BlindedCommitment(context.ToByteArray(outContext), context.ToByteArray(blindingFactor), context.ToByteArray(commitment));
        }

        /// <summary>
        /// Verifies a blinded commitment of messages
        /// </summary>
        /// <param name="request">Request for the commitment verification</param>
        /// <returns></returns>
        public static SignatureProofStatus VerifyBlindedCommitment(VerifyBlindedCommitmentRequest request)
        {
            using var context = new UnmanagedMemoryContext();

            var handle = Native.bbs_verify_blind_commitment_context_init(out var error);
            context.ThrowIfNeeded(error);

            Native.bbs_verify_blind_commitment_context_set_nonce_string(handle, request.Nonce, out error);
            context.ThrowIfNeeded(error);

            Native.bbs_verify_blind_commitment_context_set_proof(handle, context.ToBuffer(request.Proof), out error);
            context.ThrowIfNeeded(error);

            Native.bbs_verify_blind_commitment_context_set_public_key(handle, context.ToBuffer(request.PublicKey), out error);
            context.ThrowIfNeeded(error);

            foreach (var index in request.BlindedIndices)
            {
                Native.bbs_verify_blind_commitment_context_add_blinded(handle, index, out error);
                context.ThrowIfNeeded(error);
            }

            var result = Native.bbs_verify_blind_commitment_context_finish(handle, out error);
            context.ThrowIfNeeded(error);

            return (SignatureProofStatus)result;
        }

        /// <summary>
        /// Creates the proof asynchronous.
        /// </summary>
        /// <param name="myKey">My key.</param>
        /// <param name="nonce">The nonce.</param>
        /// <param name="messages">The messages.</param>
        /// <returns></returns>
        public static byte[] CreateProof(CreateProofRequest proofRequest)
        {
            using var context = new UnmanagedMemoryContext();

            var handle = Native.bbs_create_proof_context_init(out var error);
            context.ThrowIfNeeded(error);

            foreach (var message in proofRequest.Messages)
            {
                Native.bbs_create_proof_context_add_proof_message_string(handle, message.Message, message.ProofType, context.ToBuffer(proofRequest.BlindingFactor ?? Array.Empty<byte>()), out error);
                context.ThrowIfNeeded(error);
            }

            Native.bbs_create_proof_context_set_nonce_string(handle, proofRequest.Nonce, out error);
            context.ThrowIfNeeded(error);

            Native.bbs_create_proof_context_set_public_key(handle, context.ToBuffer(proofRequest.PublicKey), out error);

             context.ThrowIfNeeded(error);

            Native.bbs_create_proof_context_set_signature(handle, context.ToBuffer(proofRequest.Signature), out error);
            context.ThrowIfNeeded(error);

            Native.bbs_create_proof_context_finish(handle, out var proof, out error);
            context.ThrowIfNeeded(error);

            return context.ToByteArray(proof);
        }

        /// <summary>
        /// Verifies a proof
        /// </summary>
        /// <param name="publicKey">The public key.</param>
        /// <param name="proof">The proof.</param>
        /// <param name="revealedMessages">The indexed messages.</param>
        /// <param name="nonce">The nonce.</param>
        /// <returns></returns>
        public static SignatureProofStatus VerifyProof(VerifyProofRequest request)
        {
            using var context = new UnmanagedMemoryContext();

            var handle = Native.bbs_verify_proof_context_init(out var error);
            context.ThrowIfNeeded(error);

            Native.bbs_verify_proof_context_set_public_key(handle, context.ToBuffer(request.PublicKey), out error);
            context.ThrowIfNeeded(error);

            Native.bbs_verify_proof_context_set_nonce_string(handle, request.Nonce, out error);
            context.ThrowIfNeeded(error);

            Native.bbs_verify_proof_context_set_proof(handle, context.ToBuffer(request.Proof), out error);
            context.ThrowIfNeeded(error);

            foreach (var item in request.Messages)
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