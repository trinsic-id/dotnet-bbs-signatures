using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace BbsSignatures.Bls
{
    internal class NativeMethods
    {
        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bls_secret_key_size();

        [DllImport(Constants.BbsSignaturesLibrary, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int bls_public_key_size();
    }
}
