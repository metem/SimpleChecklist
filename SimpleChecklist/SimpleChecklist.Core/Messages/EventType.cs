namespace SimpleChecklist.Core.Messages
{
    public enum EventType
    {
        Started,
        ApplicationDataLoadError,
        Suspending,
        Closing,
        ApplicationDataLoadFinished,
        InitializationCancelled,
        InitializationFromBackupRequested,
        DoneListRefreshRequested,
        LoadBackup,
        CreateBackup,
        AddTasksFromTextFile,
        SaveTasksToTextFile,
        InvertListOrder
    }
}