using SimpleChecklist.Common;
using System;

namespace SimpleChecklist.Tests
{
    public class MockedDateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => UtcNow.ToLocalTime();

        public DateTime UtcNow { get; private set; } = DateTime.UtcNow;

        public void AddSeconds(int seconds)
        {
            UtcNow = UtcNow.AddSeconds(seconds);
        }
    }
}