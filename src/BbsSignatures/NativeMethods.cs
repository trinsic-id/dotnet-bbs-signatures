using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace BbsSignatures.Bls
{
    internal class NativeMethods
    {
        #region Bls

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
    }
}
