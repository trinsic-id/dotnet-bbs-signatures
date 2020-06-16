using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("BbsSignatures.Tests")]

namespace BbsSignatures
{
    internal class Constants
    {
#if __IOS__
        internal const string BbsSignaturesLibrary = "*";
#else
        internal const string BbsSignaturesLibrary = "bbs";
#endif
    }
}