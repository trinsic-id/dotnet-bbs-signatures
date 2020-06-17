using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("BbsSignatures.Tests")]
[assembly: InternalsVisibleTo("BbsSignatures.Tests.Ios")]

namespace BbsSignatures
{
    internal class Constants
    {
#if __IOS__
        internal const string BbsSignaturesLibrary = "__Internal";
#else
        internal const string BbsSignaturesLibrary = "bbs";
#endif
    }
}