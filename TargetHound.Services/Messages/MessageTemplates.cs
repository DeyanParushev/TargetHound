namespace TargetHound.Services.Messages
{
    public static class MessageTemplates
    {
        public static string ClientInvitation(string senderName, string clientName)
        {
            return $"Hello {clientName}!! {senderName} has invited you to join his company on the Target Hound platform. " +
                $"If you want to join him please click the link bellow." +
                $"If you don`t know this person and don`t want to join our platform please ignore this email.";
        }

        public static string ProjectInvitation(string senderName, string clientName, string projectName)
        {
            return $"Hello {clientName}!! {senderName} has invited you to join his project: {projectName} on the Target Hound platform. " +
                $"If you want to join him please click the link bellow." +
                $"If you don`t know this person and don`t want to join our platform please ignore this email.";
        }
    }
}
