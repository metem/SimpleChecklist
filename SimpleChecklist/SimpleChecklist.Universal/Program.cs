namespace SimpleChecklist.Universal
{
    /// <summary>
    /// Program class
    /// </summary>
    public static class Program
    {
        static void Main(string[] args)
        {
            Windows.UI.Xaml.Application.Start(callbackParams =>
            {
                BootstrapperUniversal.Configure();
            });
        }
    }
}