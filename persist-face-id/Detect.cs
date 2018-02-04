using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CSHttpClientSample
{
        [DataContract]
        public class DetectModel{
        [DataMember] 
        public string faceId { get; set;} 
    }
}
