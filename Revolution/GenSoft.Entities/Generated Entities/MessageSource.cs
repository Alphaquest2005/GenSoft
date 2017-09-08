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
	public partial class MessageSource: BaseEntity, IMessageSource
	{
		public virtual int MachineId { get; set; }
		public virtual string Name { get; set; }
		public virtual int ProcessId { get; set; }
		public virtual Guid SourceGuid { get; set; }
		public virtual int SourceTypeId { get; set; }

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
				public virtual ICollection<Message> Message {get; set;}
		
			// ---------Parent Relationships
				public virtual Machine Machine {get; set;}
				public virtual Process Process {get; set;}
				public virtual SourceType SourceType {get; set;}
	

	}
}
