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