namespace BbsSignatures
{
    public enum SignatureProofStatus
    {
        /* The proof verified */
        Success = 200,
        /* The proof failed because the signature proof of knowledge failed */
        BadSignature = 400,
        /* The proof failed because a hidden message was invalid when the proof was created */
        BadHiddenMessage = 401,
        /* The proof failed because a revealed message was invalid */
        BadRevealedMessage = 402,
    }
}
