using BbsSignatures.Bls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BbsSignatures.Tests
{
    public class BbsCreateProofTests
    {
        //[Fact(DisplayName = "Create proof for a message")]
        //public void CreateProofSingleMessage()
        //{
        //    // Secret key
        //    NativeMethods.bls_generate_key(ByteBuffer.None, out var _, out var secretKey, out var error);
        //    Assert.Equal(0, error.Code);
        //    var sk = secretKey.Dereference();

        //    // Bbs public key
        //    NativeMethods.bls_secret_key_to_bbs_key(sk, 1, out var publicKey, out error);
        //    Assert.Equal(0, error.Code);
        //    var pk = publicKey.Dereference();

        //    var handle = NativeMethods.bbs_create_proof_context_init(out error);
        //    Assert.Equal(0, error.Code);

        //    NativeMethods.bbs_create_proof_context_add_proof_message_string(handle, "test", ProofMessageType.Revealed, GetFactor(pk, "test"), out error);
        //    Assert.Equal(0, error.Code);

        //    NativeMethods.bbs_create_proof_context_set_nonce_string(handle, "123", out error);
        //    Assert.Equal(0, error.Code);
        //    NativeMethods.bbs_create_proof_context_set_public_key(handle, pk, out error);
        //    Assert.Equal(0, error.Code);
        //    NativeMethods.bbs_create_proof_context_set_signature(handle, GetSignature(sk), out error);
        //    Assert.Equal(0, error.Code);

        //    NativeMethods.bbs_create_proof_context_finish(handle, out var proof, out error);
        //    Assert.Equal(0, error.Code);

        //    Assert.NotNull(proof.Dereference());
        //}

        //public byte[] GetSignature(byte[] sk)
        //{
        //    NativeMethods.bls_secret_key_to_bbs_key(sk, 1, out var bbsPublicKey, out var error);

        //    var pk = bbsPublicKey.Dereference();

        //    var handle = NativeMethods.bbs_sign_context_init(out error);

        //    NativeMethods.bbs_sign_context_add_message_string(handle, "test", out error);

        //    NativeMethods.bbs_sign_context_set_public_key(handle, pk, out error);

        //    NativeMethods.bbs_sign_context_set_secret_key(handle, sk, out error);

        //    NativeMethods.bbs_sign_context_finish(handle, out var signature, out error);

        //    return signature.Dereference();
        //}

        private byte[] GetFactor(byte[] pk, params string[] message)
        {
            var handle = NativeMethods.bbs_blind_commitment_context_init(out var error);

            for (int i = 0; i < message.Length; i++)
            {
                NativeMethods.bbs_blind_commitment_context_add_message_string(handle, (uint)i, message[i], out error);
            }

            NativeMethods.bbs_blind_commitment_context_set_nonce_string(handle, "123", out error);
            NativeMethods.bbs_blind_commitment_context_set_public_key(handle, pk, out error);

            NativeMethods.bbs_blind_commitment_context_finish(handle, out var _, out var _, out var blindingFactor, out error);

            return blindingFactor.Dereference();
        }

        [Fact(DisplayName = "Create proof for a message using API")]
        public async Task CreateProofSingleMessageUsingApi()
        {
            var myKey = BlsSecretKey.Generate();

            var nonce = "123";
            var messages = new[] { "message_1", "message_2" };

            //var proof = await BbsProvider.CreateProofAsync(myKey, nonce, messages);

            //Assert.NotNull(proof);
        }
    }
}
