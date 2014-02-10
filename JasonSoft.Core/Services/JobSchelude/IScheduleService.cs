using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;


namespace JasonSoft.Services.JobSchelude
{
    [ServiceKnownType(typeof(Daily))]
    [ServiceKnownType(typeof(Weekly))]
    [ServiceKnownType(typeof(Monthly))]
    [ServiceKnownType(typeof(OnceDailyFrequency))]
    [ServiceKnownType(typeof(RepeatedDailyFrequency))]
    [ServiceKnownType(typeof(RecurvedTrigger))]
    [ServiceContract]
    public interface IScheduleService
    {
        [OperationContract]
        DateTime ScheduleJob(Job job, Trigger trigger);

        [OperationContract]
        Job GetJob(String jobName, String jobGroupName);

        [OperationContract]
        Job[] GetJobsByGroup(String jobGroupName);

        [OperationContract]
        void CreateJob(Job job);

        [OperationContract]
        void DeleteJob(String jobName, String jobGroupName);

        [OperationContract]
        void DeleteJobsByGroup(String jobGroupName);

        [OperationContract]
        void UpdateJob(String oldJobName, String oldJobGroupName, Job newJob);



        [OperationContract]
        Trigger GetTrigger(String triggerName, String triggerGroupName);

        [OperationContract]
        Trigger[] GetTriggersByJob(String jobName, String jobGroupName);

        [OperationContract]
        void CreateTrigger(Trigger trigger);

        [OperationContract]
        void DeleteTrigger(String triggerName, String triggerGroupName);

        [OperationContract]
        void UpdateTrigger(String oldTriggerName, String oldTriggerGroupName, Trigger newTrigger);


        [OperationContract]
        void UpdateJobAndTrigger(String oldJobName, String oldJobGroupName, Job newJob, String oldTriggerName, String oldTriggerGroupName, Trigger newTrigger);




        [OperationContract]
        void RunJobOnceNow(String jobName, String jobGroupName);

        [OperationContract]
        void EnableJob(String jobName, String jobGroupName);

        [OperationContract]
        void DisableJob(String jobName, String jobGroupName);

        [OperationContract]
        void StopRunningJob(String jobName, String jobGroupName);

        [OperationContract]
        JobHistory[] GetHistoryByJob(String jobName, String jobGroupName);

        [OperationContract]
        JobState GetStateByJob(String jobName, String jobGroupName);

        [OperationContract]
        JobState[] GetStatesByJobGroup(String jobGroupName);

        [OperationContract]
        void ClearJobHistory();

        [OperationContract]
        void ClearJobsAndTriggers();
    }
}
