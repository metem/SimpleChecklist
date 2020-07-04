using System;

namespace SimpleChecklist.Common
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }

        DateTime UtcNow { get; }
    }
}