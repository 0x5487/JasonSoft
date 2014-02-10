using System;
using JasonSoft.Components.Quartz;
using JasonSoft.Components.Quartz.Impl.Calendar;
using JasonSoft;

namespace JasonSoft.Services.JobSchelude
{
    public class TriggerAdapter
    {

        public TriggerAdapter(Trigger trigger) 
        {
            this.Trigger = trigger;
            
            if (trigger is RecurvedTrigger)
            {
                RecurvedTrigger targetTrigger = trigger as RecurvedTrigger;
                string cronExpression = targetTrigger.GetCronExpression();
        
                QuartzTrigger = new CronTrigger( targetTrigger.Name, targetTrigger.Group, targetTrigger.JobName, targetTrigger.JobGroupName, cronExpression) ;
                QuartzTrigger.StartTimeUtc = targetTrigger.StartDateUTC;
                QuartzTrigger.EndTimeUtc = targetTrigger.EndDateUTC;
                QuartzTrigger.MisfireInstruction = MisfireInstruction.CronTrigger.DoNothing;

                if (targetTrigger.DailyFrequency is RepeatedDailyFrequency)
                {
                    RepeatedDailyFrequency df = (RepeatedDailyFrequency)targetTrigger.DailyFrequency;
                    DateTime utcNow = DateTime.UtcNow;
                    DateTime start = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, df.StartingAtUTC.Hour, df.StartingAtUTC.Minute, df.StartingAtUTC.Second);
                    DateTime end = new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, df.EndingAtUTC.Hour, df.EndingAtUTC.Minute, df.EndingAtUTC.Second);

                    this.QuartzTrigger.CalendarName = trigger.Group + "." + trigger.Name; 

                    if (end > start)
                        this.QuartzCalendar = new DailyCalendar(start, end) { InvertTimeRange = true }; //watch out, If invertTimeRange is true, the time range is inverted: that is, all times outside the defined time range are excluded.
                    else
                        this.QuartzCalendar = new DailyCalendar(end, start) { InvertTimeRange = false };
                }
            }

        }

        public TriggerAdapter(JasonSoft.Components.Quartz.Trigger quartzTrigger, ICalendar quartzDailyCalendar) 
        {
            this.QuartzTrigger = quartzTrigger;
            this.QuartzCalendar = quartzDailyCalendar;

            if (quartzTrigger is CronTrigger)
            {
                CronTrigger cronTrigger = (CronTrigger)quartzTrigger;
                RecurvedTrigger recurvedTrigger = new RecurvedTrigger
                                                      {
                                                          Name = cronTrigger.Name,
                                                          Group = cronTrigger.Group,
                                                          JobName = cronTrigger.JobName,
                                                          JobGroupName = cronTrigger.JobGroup
                                                      };

                string[] timeParts = cronTrigger.CronExpressionString.Split(' ');

                //detect Frequency type
                Frequency frequency = null;

                if (timeParts[4] == "*" && timeParts[5] == "?")
                {
                    Daily daily = new Daily();
                    daily.RepeatedDays = timeParts[3].Substring(2).ChangeTypeTo<int>();
                    frequency = daily;
                }
                else if (timeParts[4] == "*" && timeParts[5] != "?")
                {
                    Weekly weekly = new Weekly();
                    weekly.RepeatedWeeks = timeParts[5].Substring(2).ChangeTypeTo<int>();

                    switch(timeParts[5].Substring(0,1).ChangeTypeTo<int>())
                    {
                        case 0:
                            weekly.DayOfWeek = DayOfWeek.Sunday;
                            break;
                        case 1:
                            weekly.DayOfWeek = DayOfWeek.Monday;
                            break;
                        case 2:
                            weekly.DayOfWeek = DayOfWeek.Tuesday;
                            break;
                        case 3:
                            weekly.DayOfWeek = DayOfWeek.Wednesday;
                            break;
                        case 4:
                            weekly.DayOfWeek = DayOfWeek.Thursday;
                            break;
                        case 5:
                            weekly.DayOfWeek = DayOfWeek.Friday;
                            break;
                        case 6:
                            weekly.DayOfWeek = DayOfWeek.Saturday;
                            break;
                    }

                    frequency = weekly;
                }
                else
                {
                    Monthly monthly = new Monthly();
                    String[] result = timeParts[4].Split('/');

                    monthly.DayOfMonth = timeParts[3].ChangeTypeTo<int>();
                    monthly.RepeatedMonths = result[1].ChangeTypeTo<int>();

                    frequency = monthly;
                }

                recurvedTrigger.Frequency = frequency;

                //detect DailyFrequency type
                DailyFrequency dailyFrequency = null;

                if (timeParts[2] == "*")
                {
                    RepeatedDailyFrequency repeatedDailyFrequency = new RepeatedDailyFrequency();
                    repeatedDailyFrequency.RepeatedMinutes = timeParts[1].Substring(2, 1).ChangeTypeTo<int>();

                    DailyCalendar calendar = quartzDailyCalendar as DailyCalendar;
  
                    repeatedDailyFrequency.StartingAtUTC = calendar.GetTimeRangeStartingTimeUtc(DateTime.UtcNow);
                    repeatedDailyFrequency.EndingAtUTC = calendar.GetTimeRangeEndingTimeUtc(DateTime.UtcNow);
                    
                    dailyFrequency = repeatedDailyFrequency;
                }
                else
                {
                    OnceDailyFrequency onceDailyFrequency = new OnceDailyFrequency();
                    onceDailyFrequency.OccurTime = new TimeSpan(timeParts[2].ChangeTypeTo<int>(), timeParts[1].ChangeTypeTo<int>(), timeParts[0].ChangeTypeTo<int>());
                    dailyFrequency = onceDailyFrequency;
                }

                recurvedTrigger.DailyFrequency = dailyFrequency;

                //duration
                recurvedTrigger.StartDateUTC = quartzTrigger.StartTimeUtc;
                recurvedTrigger.EndDateUTC = quartzTrigger.EndTimeUtc;

                this.Trigger = recurvedTrigger;
            }

        }

        public Trigger Trigger { get; set; }

        public JasonSoft.Components.Quartz.Trigger QuartzTrigger { get; set; }

        public ICalendar QuartzCalendar { get; set; }

    }
}