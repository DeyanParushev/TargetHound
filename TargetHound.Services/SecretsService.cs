namespace TargetHound.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.Extensions.Configuration;

    using TargetHound.Services.Interfaces;

    public class SecretsService : ISecretsService
    {
        private IConfiguration configuration;

        public SecretsService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
      
        public string GetSecret(params string[] values)
        {
            var secretStringBuilder = new StringBuilder();

            for (int i = 0; i < values.Length; i++)
            {
                if(values.Length == 1 || i == values.Length - 1)
                {
                    secretStringBuilder.Append(values[i]);
                    continue;
                }

                secretStringBuilder.Append(values[i]);
                secretStringBuilder.Append(":");
            }

            var secret = secretStringBuilder.ToString().Trim();

            var secretValue = this.configuration[secret];
            return secretValue;
        }
    }
}
