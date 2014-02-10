#region License
/* 
 * Copyright 2001-2009 Terracotta, Inc. 
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not 
 * use this file except in compliance with the License. You may obtain a copy 
 * of the License at 
 * 
 *   http://www.apache.org/licenses/LICENSE-2.0 
 *   
 * Unless required by applicable law or agreed to in writing, software 
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT 
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the 
 * License for the specific language governing permissions and limitations 
 * under the License.
 * 
 */
#endregion

namespace JasonSoft.Components.Quartz
{
    /// <summary>
    /// Scheduler constants.
    /// </summary>
    /// <author>Marko Lahma (.NET)</author>
    public struct SchedulerConstants
    {
        /// <summary>
        /// A (possibly) useful constant that can be used for specifying the group
        /// that <see cref="IJob" /> and <see cref="Trigger" /> instances belong to.
        /// </summary>
        public const string DefaultGroup = "DEFAULT";

        /// <summary>
        /// A constant <see cref="Trigger" /> group name used internally by the
        /// scheduler - clients should not use the value of this constant
        /// ("MANUAL_TRIGGER") for thename of a <see cref="Trigger" />'s group.
        /// </summary>
        public const string DefaultManualTriggers = "MANUAL_TRIGGER";

        /// <summary>
        /// A constant <see cref="Trigger" /> group name used internally by the
        /// scheduler - clients should not use the value of this constant
        /// ("RECOVERING_JOBS") for thename of a <see cref="Trigger" />'s group.
        /// </summary>
        public const string DefaultRecoveryGroup = "RECOVERING_JOBS";

        /// <summary>
        /// A constant <see cref="Trigger" /> group name used internally by the
        /// scheduler - clients should not use the value of this constant
        /// ("FAILED_OVER_JOBS") for thename of a <see cref="Trigger" />'s group.
        /// </summary>
        public const string DefaultFailOverGroup = "FAILED_OVER_JOBS";


        /// <summary>
        ///  A constant <see cref="JobDataMap" /> key that can be used to retrieve the
        /// name of the original <see cref="Trigger" /> from a recovery trigger's
        /// data map in the case of a job recovering after a failed scheduler
        /// instance.
        /// </summary>
        /// <seealso cref="JobDetail.RequestsRecovery" />
        public const string FailedJobOriginalTriggerName = "QRTZ_FAILED_JOB_ORIG_TRIGGER_NAME";

        /// <summary>
        /// A constant <see cref="JobDataMap" /> key that can be used to retrieve the
        /// group of the original <see cref="Trigger" /> from a recovery trigger's
        /// data map in the case of a job recovering after a failed scheduler
        /// instance.
        /// </summary>
        /// <seealso cref="JobDetail.RequestsRecovery" />
        public const string FailedJobOriginalTriggerGroup = "QRTZ_FAILED_JOB_ORIG_TRIGGER_GROUP";

        /// <summary>
        ///  A constant <see cref="JobDataMap" /> key that can be used to retrieve the
        /// scheduled fire time of the original <see cref="Trigger" /> from a recovery
        /// trigger's data map in the case of a job recovering after a failed scheduler
        /// instance.
        /// </summary>
        /// <seealso cref="JobDetail.RequestsRecovery" />
        public const string FailedJobOriginalTriggerFiretimeInMillisecoonds = "QRTZ_FAILED_JOB_ORIG_TRIGGER_FIRETIME_IN_MILLISECONDS_AS_STRING";

    }
}