namespace TargetHound.Services.Interfaces
{
    public interface ISecretsService
    {
        public string GetSecret(params string[] values);
    }
}
