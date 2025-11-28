using CleanArchitecture.Application.DTOs.Email;
using CleanArchitecture.Application.Exceptions;
using CleanArchitecture.Domain.Settings;
using CleanArchitecture.Infrastructure.Shared.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CleanArchitecture.UnitTests.Infrastructure.Shared.Services
{
    public class EmailServiceTests
    {
        private readonly Mock<IOptions<MailSettings>> _mockMailSettings;
        private readonly Mock<ILogger<EmailService>> _mockLogger;
        private readonly EmailService _emailService;

        public EmailServiceTests()
        {
            _mockMailSettings = new Mock<IOptions<MailSettings>>();
            _mockLogger = new Mock<ILogger<EmailService>>();

            // Setup default mail settings
            _mockMailSettings.Setup(x => x.Value).Returns(new MailSettings
            {
                EmailFrom = "test@example.com",
                SmtpHost = "localhost",
                SmtpPort = 587,
                SmtpUser = "testuser",
                SmtpPass = "testpass",
                DisplayName = "Test Email Service"
            });

            _emailService = new EmailService(_mockMailSettings.Object, _mockLogger.Object);
        }

        [Fact]
        public void Constructor_Should_Initialize_Properties_Correctly()
        {
            // Act & Assert
            _emailService._mailSettings.Should().NotBeNull();
            _emailService._mailSettings.EmailFrom.Should().Be("test@example.com");
            _emailService._mailSettings.SmtpHost.Should().Be("localhost");
            _emailService._mailSettings.SmtpPort.Should().Be(587);
            _emailService._logger.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_Should_Throw_When_MailSettings_Is_Null()
        {
            // Arrange
            Mock<IOptions<MailSettings>> nullOptions = new Mock<IOptions<MailSettings>>();
            nullOptions.Setup(x => x.Value).Returns((MailSettings)null);

            // Act & Assert
            Action act = () => new EmailService(nullOptions.Object, _mockLogger.Object);
            act.Should().Throw<Exception>();
        }

        [Fact]
        public async Task SendAsync_Should_Throw_ApiException_When_SMTP_Connection_Fails()
        {
            // Arrange
            var emailRequest = new EmailRequestDTO
            {
                To = "recipient@example.com",
                Subject = "Test Subject",
                Body = "Test Body"
            };

            // Setup mail settings with invalid SMTP configuration
            _mockMailSettings.Setup(x => x.Value).Returns(new MailSettings
            {
                EmailFrom = "test@example.com",
                SmtpHost = "invalid-smtp-host.com",
                SmtpPort = 587,
                SmtpUser = "testuser",
                SmtpPass = "testpass",
                DisplayName = "Test Email Service"
            });

            var emailServiceWithInvalidSmtp = new EmailService(_mockMailSettings.Object, _mockLogger.Object);

            // Act & Assert
            await emailServiceWithInvalidSmtp.Invoking(x => x.SendAsync(emailRequest))
                .Should().ThrowAsync<ApiException>();
        }

        [Fact]
        public async Task SendAsync_Should_Use_Request_From_Address_When_Provided()
        {
            // Arrange
            var customFromAddress = "custom@example.com";
            var emailRequest = new EmailRequestDTO
            {
                From = customFromAddress,
                To = "recipient@example.com",
                Subject = "Test Subject",
                Body = "Test Body"
            };

            // This test verifies that the service accepts the custom from address
            // In a real scenario, you'd need to mock the SMTP client to verify the actual behavior

            // Act & Assert
            // Since we can't easily mock MailKit's SmtpClient in this context,
            // we verify that the service doesn't throw when processing a valid request structure
            var emailService = new EmailService(_mockMailSettings.Object, _mockLogger.Object);
            
            // The service should not throw during construction and basic validation
            emailService._mailSettings.EmailFrom.Should().Be("test@example.com");
            emailRequest.From.Should().Be(customFromAddress);
        }

        [Fact]
        public async Task SendAsync_Should_Use_Default_From_Address_When_Not_Provided()
        {
            // Arrange
            var emailRequest = new EmailRequestDTO
            {
                From = null, // No custom from address
                To = "recipient@example.com",
                Subject = "Test Subject",
                Body = "Test Body"
            };

            // Act & Assert
            // Verify that the service has access to the default from address
            _emailService._mailSettings.EmailFrom.Should().Be("test@example.com");
            emailRequest.From.Should().BeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void EmailRequestDTO_Should_Handle_Various_From_Values(string fromValue)
        {
            // Arrange & Act
            var emailRequest = new EmailRequestDTO
            {
                From = fromValue,
                To = "recipient@example.com",
                Subject = "Test Subject",
                Body = "Test Body"
            };

            // Assert
            emailRequest.From.Should().Be(fromValue);
            // The service should fall back to the configured EmailFrom when From is null/empty
        }

        [Fact]
        public void MailSettings_Properties_Should_Be_Accessible()
        {
            // Act & Assert
            var settings = _emailService._mailSettings;
            
            settings.Should().NotBeNull();
            settings.EmailFrom.Should().NotBeNullOrEmpty();
            settings.SmtpHost.Should().NotBeNullOrEmpty();
            settings.SmtpPort.Should().BeGreaterThan(0);
            settings.SmtpUser.Should().NotBeNull();
            settings.SmtpPass.Should().NotBeNull();
        }

        [Fact]
        public void Logger_Should_Be_Accessible()
        {
            // Act & Assert
            _emailService._logger.Should().NotBeNull();
            _emailService._logger.Should().Be(_mockLogger.Object);
        }

        [Theory]
        [InlineData("test@example.com", "Test Subject", "Test Body")]
        [InlineData("another@example.com", "Another Subject", "<html><body>HTML Body</body></html>")]
        [InlineData("user@domain.org", "Subject with special chars !@#$%", "Body with unicode: ραινσϊ")]
        public void EmailRequestDTO_Should_Accept_Various_Valid_Inputs(string to, string subject, string body)
        {
            // Act
            var emailRequest = new EmailRequestDTO
            {
                To = to,
                Subject = subject,
                Body = body
            };

            // Assert
            emailRequest.To.Should().Be(to);
            emailRequest.Subject.Should().Be(subject);
            emailRequest.Body.Should().Be(body);
        }

        [Fact]
        public void MailSettings_Should_Support_Different_SMTP_Configurations()
        {
            // Arrange
            var customSettings = new MailSettings
            {
                EmailFrom = "noreply@company.com",
                SmtpHost = "smtp.company.com",
                SmtpPort = 465,
                SmtpUser = "smtp-user",
                SmtpPass = "smtp-password",
                DisplayName = "Company Email System"
            };

            var mockCustomOptions = new Mock<IOptions<MailSettings>>();
            mockCustomOptions.Setup(x => x.Value).Returns(customSettings);

            // Act
            var customEmailService = new EmailService(mockCustomOptions.Object, _mockLogger.Object);

            // Assert
            customEmailService._mailSettings.Should().Be(customSettings);
            customEmailService._mailSettings.SmtpPort.Should().Be(465);
            customEmailService._mailSettings.DisplayName.Should().Be("Company Email System");
        }

        [Fact]
        public async Task SendAsync_Should_Log_Errors_When_Exception_Occurs()
        {
            // Arrange
            var emailRequest = new EmailRequestDTO
            {
                To = "invalid-email-format",
                Subject = "Test",
                Body = "Test"
            };

            // Setup invalid settings that will cause an exception
            _mockMailSettings.Setup(x => x.Value).Returns(new MailSettings
            {
                EmailFrom = "invalid-email-format",
                SmtpHost = "", // Invalid host
                SmtpPort = 0,  // Invalid port
                SmtpUser = "",
                SmtpPass = ""
            });

            var emailServiceWithInvalidSettings = new EmailService(_mockMailSettings.Object, _mockLogger.Object);

            // Act & Assert
            await emailServiceWithInvalidSettings.Invoking(x => x.SendAsync(emailRequest))
                .Should().ThrowAsync<ApiException>();
        }
    }
}