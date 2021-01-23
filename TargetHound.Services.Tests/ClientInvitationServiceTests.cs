namespace TargetHound.Services.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;

    using TargetHound.Data;
    using TargetHound.DataModels;

    public class ClientInvitationServiceTests
    {
        [TestCase("RandomEmail@email.com")]
        [TestCase("RandomEmail2@email.com")]
        public async Task AcceptInvitationShouldThrowExceptioWhenClientDontExist(string email)
        {
            //// Arrange
            var clientId = Guid.NewGuid().ToString();
            var invitationEmail = email;
            var context = this.SetupContext();
            var clientInvitationService = new ClientInvitationsService(context);

            //// Assert
            Assert.That(
                async () => await clientInvitationService.AcceptInvitationAsync(clientId, invitationEmail),
                Throws.Exception);
        }

        [TestCase("RandomEmail@email.com")]
        [TestCase("RandomEmail2@email.com")]
        public async Task AcceptInvitationShouldThrowExceptioWhenClientIsDeleted(string email)
        {
            //// Arrange
            var client = new Client
            {
                Id = Guid.NewGuid().ToString(),
                Name = "SomeName",
                IsDeleted = true,
            };
            var invitationEmail = email;
            var context = this.SetupContext();
            context.Clients.Add(client);
            await context.SaveChangesAsync();

            var clientInvitationService = new ClientInvitationsService(context);

            //// Assert
            Assert.That(
                async () => await clientInvitationService.AcceptInvitationAsync(client.Id, invitationEmail),
                Throws.Exception);
        }

        [TestCase]
        public async Task AcceptInvitationShouldThrowExceptionForWrongClient()
        {
            //// Arange
            var client = new Client
            {
                Id = Guid.NewGuid().ToString(),
                Name = "SomeName",
                IsDeleted = false,
            };
            var email = "correct@email.com";

            var invitationWithWrongClient = new ClientInvitation
            {
                Id = Guid.NewGuid().ToString(),
                ClientId = Guid.NewGuid().ToString(),
                IsAccepted = false,
                Email = email,
            };

            var context = this.SetupContext();
            context.Clients.Add(client);
            context.ClientInvitations.Add(invitationWithWrongClient);
            await context.SaveChangesAsync();
            var clientInvitattionService = new ClientInvitationsService(context);

            //// Act
            var exception = new ArgumentException();
            try
            {
                await clientInvitattionService.AcceptInvitationAsync(client.Id, email);
            }
            catch (ArgumentException ex)
            {
                exception = ex;
            }

            //// Assert
            Assert.That(
                async () => await clientInvitattionService.AcceptInvitationAsync(client.Id, email),
                Throws.ArgumentException);
            Assert.AreEqual("Invalid invitation.", exception.Message);
        }

        [TestCase]
        public async Task AcceptInvitationShouldThrowExceptionForWrongEmail()
        {
            //// Arange
            var client = new Client
            {
                Id = Guid.NewGuid().ToString(),
                Name = "SomeName",
                IsDeleted = false,
            };
            var email = "wrong@email.com";

            var invitationWithWrongEmail = new ClientInvitation
            {
                Id = Guid.NewGuid().ToString(),
                ClientId = client.Id,
                IsAccepted = false,
                Email = "correct@email.com",
            };

            var context = this.SetupContext();
            context.Clients.Add(client);
            context.ClientInvitations.Add(invitationWithWrongEmail);
            await context.SaveChangesAsync();
            var clientInvitattionService = new ClientInvitationsService(context);

            //// Act
            var exception = new ArgumentException();
            try
            {
                await clientInvitattionService.AcceptInvitationAsync(client.Id, email);
            }
            catch (ArgumentException ex)
            {
                exception = ex;
            }

            //// Assert
            Assert.That(
                async () => await clientInvitattionService.AcceptInvitationAsync(client.Id, email),
                Throws.ArgumentException);
            Assert.AreEqual("Invalid invitation.", exception.Message);
        }

        [TestCase]
        public async Task AcceptInvitationShouldThrowExceptionForDeletedInvitation()
        {
            //// Arange
            var client = new Client
            {
                Id = Guid.NewGuid().ToString(),
                Name = "SomeName",
                IsDeleted = false,
            };
            var email = "correct@email.com";

            var invitationIsAccepted = new ClientInvitation
            {
                Id = Guid.NewGuid().ToString(),
                ClientId = client.Id,
                IsAccepted = true,
                Email = email,
            };

            var context = this.SetupContext();
            context.Clients.Add(client);
            context.ClientInvitations.Add(invitationIsAccepted);
            await context.SaveChangesAsync();
            var clientInvitattionService = new ClientInvitationsService(context);

            //// Act
            var exception = new ArgumentException();
            try
            {
                await clientInvitattionService.AcceptInvitationAsync(client.Id, email);
            }
            catch (ArgumentException ex)
            {
                exception = ex;
            }

            //// Assert
            Assert.That(
                async () => await clientInvitattionService.AcceptInvitationAsync(client.Id, email),
                Throws.ArgumentException);
            Assert.AreEqual("Invalid invitation.", exception.Message);
        }

        [TestCase]
        public async Task AcceptInvitationShouldWorkProperly()
        {
            //// Arange
            var client = new Client
            {
                Id = Guid.NewGuid().ToString(),
                Name = "SomeName",
                IsDeleted = false,
            };
            var email = "correct@email.com";

            var newClientInvitation = new ClientInvitation
            {
                Id = Guid.NewGuid().ToString(),
                ClientId = client.Id,
                IsAccepted = false,
                Email = email,
            };

            var context = this.SetupContext();
            context.Clients.Add(client);
            context.ClientInvitations.Add(newClientInvitation);
            await context.SaveChangesAsync();
            var clientInvitattionService = new ClientInvitationsService(context);

            //// Act
            await clientInvitattionService.AcceptInvitationAsync(client.Id, email);
            var invitation = context.ClientInvitations.FirstOrDefault(x => x.ClientId == client.Id && x.Email == email && x.IsAccepted == true);
            
            //// Assert
            Assert.AreEqual(invitation.Id, newClientInvitation.Id);
        }

        private TargetHoundContext SetupContext()
        {
            var options = new DbContextOptionsBuilder<TargetHoundContext>()
               .UseInMemoryDatabase(databaseName: "FakeConnectionString")
               .Options;
            var context = new TargetHoundContext(options);

            return context;
        }
    }
}
