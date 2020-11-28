namespace TargetHound.DataModels
{
    public class ClientInvitation
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string ClientId { get; set; }

        public Client Client { get; set; }

        public bool IsDeleted { get; set; }
    }
}
