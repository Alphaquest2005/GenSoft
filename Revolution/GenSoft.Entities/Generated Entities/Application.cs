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
	public partial class Application: BaseEntity, IApplication
	{
		public virtual string Name { get; set; }

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
				public virtual ICollection<DomainProcess> DomainProcess {get; set;}
				public virtual ICollection<ApplicationSetting> ApplicationSetting {get; set;}
				public virtual ICollection<EntityType> EntityType {get; set;}
				public virtual DatabaseInfo DatabaseInfo {get; set;}
				public virtual DefaultApplication DefaultApplication {get; set;}
		
			// ---------Parent Relationships
	

	}
}
