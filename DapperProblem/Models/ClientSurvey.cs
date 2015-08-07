using System;
using System.Runtime.Serialization;

namespace DapperProblem.Models
{
    /// <summary>
    /// A survey performed for a client.
    /// </summary>
    [DataContract]
    public class ClientSurvey
    {
        /// <summary>
        /// Unique ID.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int Id { get; set; }

        /// <summary>
        /// The ID of the client that performed the survey.
        /// </summary>
        [DataMember]
        public int ClientId { get; set; }

        /// <summary>
        /// ID of the survey.
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public int SurveyId { get; set; }

        /// <summary>
        /// The custom name given to the survey by the client.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// The start date of the survey.
        /// </summary>
        [DataMember]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// The end date of the survey.
        /// </summary>
        [DataMember]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// If this survey hasn't ended, then the patient should be automatically directed to this survey.
        /// </summary>
        [DataMember]
        public bool IsPatientDefault { get; set; }

        /// <summary>
        /// Is the survey locked? If so, changes may not be made to the survey.
        /// </summary>
        [DataMember]
        public bool IsLocked { get; set; }

        /// <summary>
        /// The full survey the client is performing.
        /// </summary>
        //[DataMember]
        //public Survey Survey { get; set; }

        /// <summary>
        /// The intro text of the survey.  This overrides Surveys.IntroHtml
        /// </summary>
        [DataMember]
        public string IntroHtml { get; set; }

        /// <summary>
        /// The outro text of the survey. This overrides Surveys.OutroHtml
        /// </summary>
        [DataMember]
        public string OutroHtml { get; set; }

        /// <summary>
        /// Maximum number of times this survey can be completed by a patient
        /// </summary>
        [DataMember]
        public int MaxResponses { get; set; }
    }
}
