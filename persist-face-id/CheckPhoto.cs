using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CSHttpClientSample
{
    [DataContract]
    public class CheckPhotoModel
    {
        [DataMember] 
        public string faceId { get; set;} 
        [DataMember] 
        public List<CandidateModel> candidates { get; set;} 
    }

    [DataContract]
    public class CandidateModel{
        [DataMember] 
        public string personId { get;set; }
    }
    
}

