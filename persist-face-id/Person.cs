using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CSHttpClientSample
{
    [DataContract]
    public class PersonModel{
        [DataMember] 
        public string personId { get;set;}
        [DataMember] 
        public List<string> persistedFaceIds {get;set;}
        [DataMember] 
        public string name {get;set;}
        [DataMember] 
        public string userData {get;set;}
    }
}