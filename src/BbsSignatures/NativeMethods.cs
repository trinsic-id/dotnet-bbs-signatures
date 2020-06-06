using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace BbsSignatures.Bls
{
    internal class NativeMethods
    {
        #region BLS

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bls_secret_key_size();

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bls_public_key_size();

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bls_generate_key(ByteArray seed, out ByteBuffer public_key, out ByteBuffer secret_key, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bls_get_public_key(ByteArray secret_key, out ByteBuffer public_key, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bls_secret_key_to_bbs_key(ByteArray secret_key, uint message_count, out ByteBuffer public_key, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bls_public_key_to_bbs_key(ByteArray d_public_key, uint message_count, out ByteBuffer public_key, out ExternError err);

        #endregion

        #region BBS Blind Commitment

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_blind_signature_size();

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern ulong bbs_blind_commitment_context_init(out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_blind_commitment_context_finish(ulong handle, out ByteBuffer out_context, out ByteBuffer blinding_factor, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_blind_commitment_context_add_message_string(ulong handle, uint index, string message, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_blind_commitment_context_add_message_bytes(ulong handle, uint index, ByteArray message, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_blind_commitment_context_add_message_prehash(ulong handle, uint index, ByteArray message, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_blind_commitment_context_set_public_key(ulong handle, ByteArray value, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_blind_commitment_context_set_nonce_string(ulong handle, string value, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_blind_commitment_context_set_nonce_bytes(ulong handle, ByteArray value, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_blind_commitment_context_set_nonce_prehashed(ulong handle, ByteArray value, out ExternError err);

        #endregion

        #region BBS Blind Sign

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern ulong bbs_blind_sign_context_init(out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_blind_sign_context_finish(ulong handle, out ByteBuffer blinded_signature, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_blind_sign_context_add_message_string(ulong handle, uint index, string message, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_blind_sign_context_add_message_bytes(ulong handle, uint index, ByteArray message, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_blind_sign_context_add_message_prehashed(ulong handle, uint index, ByteArray message, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_blind_sign_context_set_public_key(ulong handle, ByteArray value, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_blind_sign_context_set_secret_key(ulong handle, ByteArray value, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_blind_sign_context_set_commitment(ulong handle, ByteArray value, out ExternError err);

        #endregion

        #region BBS Create Proof

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern ulong bbs_create_proof_context_init(out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_create_proof_context_finish(ulong handle, out ByteBuffer proof, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_create_proof_context_add_proof_message_string(ulong handle, string message, ProofMessageType xtype, ByteArray blinding_factor, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_create_proof_context_add_proof_message_bytes(ulong handle, ByteArray message, ProofMessageType xtype, ByteArray blinding_factor, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_create_proof_context_add_proof_message_prehashed(ulong handle, ByteArray message, ProofMessageType xtype, ByteArray blinding_factor, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_create_proof_context_set_signature(ulong handle, ByteArray signature, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_create_proof_context_set_public_key(ulong handle, ByteArray public_key, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_create_proof_context_set_nonce_string(ulong handle, string nonce, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_create_proof_context_set_nonce_bytes(ulong handle, ByteArray nonce, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_create_proof_context_set_nonce_prehashed(ulong handle, ByteArray nonce, out ExternError err);

        #endregion

        #region BBS Sign

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_signature_size();

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern ulong bbs_sign_context_init(out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_sign_context_finish(ulong handle, out ByteBuffer signature, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_sign_context_add_message_string(ulong handle, string message, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_sign_context_add_message_bytes(ulong handle, ByteArray message, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_sign_context_add_message_prehashed(ulong handle, ByteArray message, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_sign_context_set_public_key(ulong handle, ByteArray public_key, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_sign_context_set_secret_key(ulong handle, ByteArray secret_key, out ExternError err);

        #endregion

        #region BBS Verify Proof

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern ulong bbs_verify_proof_context_init(out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_verify_proof_context_finish(ulong handle, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_verify_proof_context_add_revealed_index(ulong handle, uint index, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_verify_proof_context_add_message_string(ulong handle, string message, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_verify_proof_context_add_message_bytes(ulong handle, ByteArray message, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_verify_proof_context_add_message_prehashed(ulong handle, ByteArray message, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_verify_proof_context_set_proof(ulong handle, ByteArray proof, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_verify_proof_context_set_public_key(ulong handle, ByteArray public_key, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_verify_proof_context_set_nonce_string(ulong handle, string nonce, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_verify_proof_context_set_nonce_bytes(ulong handle, ByteArray nonce, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_verify_proof_context_set_nonce_prehashed(ulong handle, ByteArray nonce, out ExternError err);

        #endregion

        #region BBS Verify Blind Blind Commitment

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern ulong bbs_verify_blind_commitment_context_init(out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_verify_blind_commitment_context_add_blinded(ulong handle, uint index, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_verify_blind_commitment_context_set_commitment(ulong handle, ByteArray commitment, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_verify_blind_commitment_context_set_challenge_hash(ulong handle, ByteArray challenge_hash, out ExternError err);
        
        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_verify_blind_commitment_context_set_public_key(ulong handle, ByteArray public_key, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_verify_blind_commitment_context_set_nonce_string(ulong handle, string nonce, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_verify_blind_commitment_context_set_nonce_bytes(ulong handle, ByteArray nonce, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_verify_blind_commitment_context_set_nonce_prehashed(ulong handle, ByteArray nonce, out ExternError err);

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bbs_verify_blind_commitment_context_set_proof(ulong handle, ByteArray proof, out ExternError err);

        #endregion
    }
}
