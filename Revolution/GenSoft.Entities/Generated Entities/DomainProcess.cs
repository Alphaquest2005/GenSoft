﻿// <autogenerated>
//   This file was generated by T4 code generator MRManger-DataEntities.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

using System;
using System.Collections.Generic;
using Common.DataEntites;
using GenSoft.Entities;
using GenSoft.Interfaces;

namespace GenSoft.Entities
{
	public partial class DomainProcess: BaseEntity, IDomainProcess
	{
		public virtual int ApplicationId { get; set; }
		public virtual int Priority { get; set; }

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
				public virtual ICollection<ProcessStep> ProcessStep {get; set;}
		
			// ---------Parent Relationships
				public virtual Application Application {get; set;}
				public virtual SystemProcess SystemProcess {get; set;}
	

	}
}
