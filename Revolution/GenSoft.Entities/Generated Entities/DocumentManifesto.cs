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
	public partial class DocumentManifesto: BaseEntity, IDocumentManifesto
	{
		public virtual int ManifestoId { get; set; }

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
		
			// ---------Parent Relationships
				public virtual xcuda_ASYCUDA_ExtendedProperties xcuda_ASYCUDA_ExtendedProperties {get; set;}
				public virtual ManifestInfo ManifestInfo {get; set;}
	

	}
}
