﻿// <autogenerated>
//   This file was generated by T4 code generator MRManger-DataEntities.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

using Common.DataEntites;
using GenSoft.Interfaces;

namespace GenSoft.Entities
{
	public partial class EntityTypeViewModelCommand: BaseEntity, IEntityTypeViewModelCommand
	{
		public virtual int EntityTypeId { get; set; }
		public virtual int ViewModelCommandId { get; set; }

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
		
			// ---------Parent Relationships
				public virtual EntityType EntityType {get; set;}
				public virtual ViewModelCommands ViewModelCommands {get; set;}
	

	}
}
