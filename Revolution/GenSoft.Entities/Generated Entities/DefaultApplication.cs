﻿// <autogenerated>
//   This file was generated by T4 code generator MRManger-DataEntities.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

using Common.DataEntites;
using GenSoft.Interfaces;

namespace GenSoft.Entities
{
	public partial class DefaultApplication: BaseEntity, IDefaultApplication
	{

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
		
			// ---------Parent Relationships
				public virtual Application Application {get; set;}
	

	}
}
