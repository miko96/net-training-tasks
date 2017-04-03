﻿using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Serialization.Tasks
{
    // TODO : Make Company class xml-serializable using DataContractSerializer 
    // Employee.Manager should be serialized as reference
    // Company class has to be forward compatible with all derived versions

    [DataContract]
    public class Company : IExtensibleDataObject
    {
        public ExtensionDataObject ExtensionData { get; set; }

        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public IList<Employee> Employee { get; set; }
    }

    [DataContract(IsReference =true)]
    [KnownType(typeof(Worker))]
    [KnownType(typeof(Manager))]
    public abstract class Employee
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public Manager Manager { get; set; }
    }


    public class Worker : Employee
    {
        [DataMember]
        public int Salary { get; set; }
    }

    public class Manager : Employee
    {
        [DataMember]
        public int YearBonusRate { get; set; }
    }

}
