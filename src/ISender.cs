namespace DesignPatternChallenge
{
    public interface ISender
    {
        string Recipient { get; set; }
        void Send(string message, string subject = "");
    }
}
