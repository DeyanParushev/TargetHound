using System;

namespace TargetHound.Services.Messages
{
    public static class MessageTemplates
    {
        public static string ClientInvitation(
            string senderName, 
            string clientName, 
            string linkToJoin)
        {
            return $"Hello {clientName}!! {senderName} has invited you to join his company on the " +
                $"Target Hound platform. " +
                $"If you want to join him please click the link bellow." +
                $"If you don`t know this person and don`t want to join our platform " +
                $"please ignore this email." + Environment.NewLine + Environment.NewLine + linkToJoin;
        }

        public static string ProjectInvitation(
            string senderName, 
            string receiverName, 
            string projectName, 
            string linkToJoin)
        {
            return $"Hello {receiverName}!! {senderName} has invited you to join his project: {projectName} " +
                $"on the Target Hound platform. " +
                $"If you want to join him please click the link bellow." +
                $"If you don`t know this person and don`t want to join our " +
                $"platform please ignore this email." + Environment.NewLine + Environment.NewLine + linkToJoin;
        }

        public static string InvitationSubject(string userName)
        {
            return $"Invitation from {userName}";
        }
    }
}
