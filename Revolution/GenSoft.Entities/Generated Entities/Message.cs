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
	public partial class Message: BaseEntity, IMessage
	{
		public virtual int MessageSourceId { get; set; }
		public virtual int ProcessId { get; set; }
		public virtual DateTime EntryDateTime { get; set; }

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
				public virtual Event Event {get; set;}
		
			// ---------Parent Relationships
				public virtual MessageSource MessageSource {get; set;}
				public virtual Process Process {get; set;}
	

	}
}
