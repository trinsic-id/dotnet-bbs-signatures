using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Hyperledger.Ursa.Bbs.Tests")]
[assembly: InternalsVisibleTo("Hyperledger.Ursa.Bbs.Tests.Ios")]
[assembly: InternalsVisibleTo("Hyperledger.Ursa.Bbs.Tests.Android")]

namespace Hyperledger.Ursa.Bbs
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