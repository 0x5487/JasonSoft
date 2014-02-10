using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;


namespace JasonSoft.Services.JobSchelude
{
    [DataContract]
    [Table(Name = "Schedule_JobHistory")]
    public class JobHistory
    {
        public JobHistory()
        {
            this.Message = String.Empty;
        }

        private int _id;

        [DataMember]
        [Column(Storage = "_id", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public Int32 ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _instanceName;

        [DataMember]
        [Column(Storage = "_instanceName", DbType = "VarChar(80) NOT NULL", CanBeNull = false)]
        public String InstanceName
        {
            get { return _instanceName; }
            set { _instanceName = value; }
        }

        private string _jobName;

        [DataMember]
        [Column(Storage = "_jobName", DbType = "VarChar(80) NOT NULL", CanBeNull = false)]
        public String JobName
        {
            get { return _jobName; }
            set { _jobName = value; }
        }

        private string _jobGroupName;

        [DataMember]
        [Column(Storage = "_jobGroupName", DbType = "VarChar(80) NOT NULL", CanBeNull = false)]
        public String JobGroupName
        {
            get { return _jobGroupName; }
            set { _jobGroupName = value; }
        }


        private Mode _mode;

        [DataMember]
        [Column(Storage = "_mode", DbType = "TinyInt NOT NULL")]
        public Mode Mode
        {
            get { return _mode; }
            set { _mode = value; }
        }

        private State _state;

        [DataMember]
        [Column(Storage = "_state", DbType = "TinyInt NOT NULL")]
        public State State
        {
            get { return _state; }
            set { _state = value; }
        }

        private DateTime _beginDateUTC;

        [DataMember]
        [Column(Storage = "_beginDateUTC", DbType = "DateTime NOT NULL")]
        public DateTime BeginDateUTC
        {
            get { return _beginDateUTC; }
            set { _beginDateUTC = value; }
        }

        private DateTime? _endDateUTC;

        [DataMember]
        [Column(Storage="_endDateUTC", DbType="DateTime")]
        public DateTime? EndDateUTC
        {
            get { return _endDateUTC; }
            set { _endDateUTC = value; }
        }

        private int _period;

        [DataMember]
        [Column(Storage="_period", DbType="Int NOT NULL")]
        public Int32 Period
        {
            get { return _period; }
            set { _period = value; }
        }

        private string _message;

        [DataMember]
        [Column(Storage = "_message", DbType = "NVarChar(2000) NOT NULL", CanBeNull = false)]
        public String Message
        {
            get { return _message; }
            set { _message = value; }
        }

    }
}
