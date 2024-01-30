using System;
using System.Timers;
using Microsoft.Win32.TaskScheduler;

class Program
{
    static void Main(string[] args)
    {
        // Task Scheduler에 작업 등록
        using (var ts = new TaskService())
        {
            // Create a new task definition and assign properties
            TaskDefinition td = ts.NewTask();
            td.RegistrationInfo.Description = "y혁";

            // TimeTrigger를 사용하여 매일 특정 시간에 실행되도록 설정
            TimeTrigger dailyTrigger = new TimeTrigger
            {
                StartBoundary = DateTime.Now // 예시: 매일 정오에 실행
                


            };
            dailyTrigger.Repetition = new RepetitionPattern(TimeSpan.FromMinutes(1),TimeSpan.Zero);   

            td.Triggers.Add(dailyTrigger);
            td.Settings.AllowDemandStart = true;
            
            // Create an action that will launch the specified executable whenever the trigger fires
            td.Actions.Add(new ExecAction(@"C:\\Users\\BIT\\Desktop\\용혁바보\\OTP 생성기(디자인 완성)\\OTP 생성기(SITE)\\bin\\Release\\OTP 생성기(SITE).exe"));
            //td.Triggers.Add();
            // Register the task in the root folder
            ts.RootFolder.RegisterTaskDefinition("yh", td);

            // 등록된 작업 실행
            //Task task = ts.AddAutomaticMaintenanceTask("yh", TimeSpan.Zero, TimeSpan.Zero, @"C:\\Users\\BIT\\Desktop\\용혁바보\\OTP 생성기(디자인 완성)\\OTP 생성기(SITE)\\bin\\Release\\OTP 생성기(SITE).exe");
            Task task = ts.GetTask("yh");
            // 작업을 바로 실행하거나 주기적으로 실행하도록 설정
            RunTask(task);

            // 주기적으로 실행할 타이머 설정 (예: 5분마다 실행)
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Elapsed += TimerElapsed;
            timer.Interval = 1000; // 5분마다 실행
            timer.Start();

            // 프로그램이 종료되지 않도록 대기
            Console.WriteLine("프로그램을 종료하려면 'Enter'를 누르세요.");
            Console.ReadLine();
        }
    }

    private static void TimerElapsed(object sender, ElapsedEventArgs e)
    {
        // 타이머에 의해 주기적으로 실행되는 작업
        using (var ts = new TaskService())
        {
            Task task = ts.GetTask("yh");
            RunTask(task);
        }
    }

    private static void RunTask(Task task)
    {
        DateTime date = task.LastRunTime;
        if (date != DateTime.Now)
        {
            task.Run();
        }
    }
}
