using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace FunctionApp2.timeTrigger
{
    public static class timerTrigger
    {
        [FunctionName("Function1")]
        public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }

    // Runs once every 5 minutes
    public static void CronJob([TimerTrigger("0 */5 * * * *")] TimerInfo timer)
    {
        Console.WriteLine("Cron job fired!");
    }

    // Runs immediately on startup, then every two hours thereafter
    public static void StartupJob(
        [TimerTrigger("0 0 */2 * * *", RunOnStartup = true)] TimerInfo timerInfo)
    {
        Console.WriteLine("Timer job fired!");
    }

    // Runs once every 30 seconds
    public static void TimerJob([TimerTrigger("00:00:30")] TimerInfo timer)
    {
        Console.WriteLine("Timer job fired!");
    }

    // Runs on a custom schedule. You implement Type MySchedule which is called on to
    // return the next occurrence time as needed
    public static void CustomJob([TimerTrigger(typeof(MySchedule))] TimerInfo timer)
    {
        Console.WriteLine("Custom job fired!");
    }



}
