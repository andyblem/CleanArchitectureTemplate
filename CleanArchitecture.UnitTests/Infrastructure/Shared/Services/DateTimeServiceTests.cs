using CleanArchitecture.Infrastructure.Shared.Services;
using FluentAssertions;
using System;
using Xunit;

namespace CleanArchitecture.UnitTests.Infrastructure.Shared.Services
{
    public class DateTimeServiceTests
    {
        private readonly DateTimeService _dateTimeService;

        public DateTimeServiceTests()
        {
            _dateTimeService = new DateTimeService();
        }

        [Fact]
        public void NowUtc_Should_Return_Current_UTC_Time()
        {
            // Arrange
            var beforeCall = DateTime.UtcNow;

            // Act
            var result = _dateTimeService.NowUtc;

            // Assert
            var afterCall = DateTime.UtcNow;
            
            result.Should().BeAfter(beforeCall.AddSeconds(-1));
            result.Should().BeBefore(afterCall.AddSeconds(1));
            result.Kind.Should().Be(DateTimeKind.Utc);
        }

        [Fact]
        public void NowUtc_Should_Return_Different_Values_When_Called_Multiple_Times()
        {
            // Act
            var firstCall = _dateTimeService.NowUtc;
            
            // Small delay to ensure time difference
            System.Threading.Thread.Sleep(1);
            
            var secondCall = _dateTimeService.NowUtc;

            // Assert
            secondCall.Should().BeAfter(firstCall);
        }

        [Fact]
        public void NowUtc_Should_Return_UTC_Kind_DateTime()
        {
            // Act
            var result = _dateTimeService.NowUtc;

            // Assert
            result.Kind.Should().Be(DateTimeKind.Utc);
        }

        [Fact]
        public void NowUtc_Should_Be_Close_To_System_UtcNow()
        {
            // Arrange
            var tolerance = TimeSpan.FromMilliseconds(100);

            // Act
            var serviceTime = _dateTimeService.NowUtc;
            var systemTime = DateTime.UtcNow;

            // Assert
            var difference = Math.Abs((serviceTime - systemTime).TotalMilliseconds);
            difference.Should().BeLessThan(tolerance.TotalMilliseconds);
        }

        [Fact]
        public void NowUtc_Should_Be_Consistent_Across_Multiple_Service_Instances()
        {
            // Arrange
            var service1 = new DateTimeService();
            var service2 = new DateTimeService();

            // Act
            var time1 = service1.NowUtc;
            var time2 = service2.NowUtc;

            // Assert
            // Both should return times very close to each other (within 1 second)
            var difference = Math.Abs((time1 - time2).TotalSeconds);
            difference.Should().BeLessThan(1.0);
        }

        [Fact]
        public void NowUtc_Property_Should_Be_Accessible()
        {
            // Act & Assert
            var result = _dateTimeService.NowUtc;
            
            // Should not throw and should return a valid DateTime
            result.Should().NotBe(default(DateTime));
        }

        [Theory]
        [InlineData(1000)] // 1 second
        [InlineData(5000)] // 5 seconds
        public void NowUtc_Should_Progress_Over_Time(int delayMilliseconds)
        {
            // Arrange
            var startTime = _dateTimeService.NowUtc;

            // Act
            System.Threading.Thread.Sleep(delayMilliseconds);
            var endTime = _dateTimeService.NowUtc;

            // Assert
            var expectedMinimumDifference = TimeSpan.FromMilliseconds(delayMilliseconds * 0.9); // Allow 10% tolerance
            var actualDifference = endTime - startTime;
            
            actualDifference.Should().BeGreaterThanOrEqualTo(expectedMinimumDifference);
        }
    }
}