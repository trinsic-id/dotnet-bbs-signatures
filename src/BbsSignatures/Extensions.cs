namespace BbsSignatures
{
    public static class Extensions
    {
        internal static void ThrowOnError(this ExternError error)
        {
            if (error.Code != 0)
            {
                throw error.Dereference();
            }
        }
    }
}