﻿// <autogenerated>
//   This file was generated by T4 code generator MRManger-DataEntities.tt.
//   Any changes made to this file manually will be lost next time the file is regenerated.
// </autogenerated>

using System;
using System.Collections.Generic;
using Common.DataEntites;
using EF.Entities;
using Interfaces;

namespace EF.Entities
{
	public partial class Addresses: BaseEntity, IAddresses
	{
		public virtual DateTime EntryDateTime { get; set; }

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
				public virtual AddressCities AddressCities {get; set;}
				public virtual AddressCountries AddressCountries {get; set;}
				public virtual ICollection<AddressLines> AddressLines {get; set;}
				public virtual AddressParishes AddressParishes {get; set;}
				public virtual AddressStates AddressStates {get; set;}
				public virtual AddressZipCodes AddressZipCodes {get; set;}
				public virtual ICollection<ForeignAddresses> ForeignAddresses {get; set;}
				public virtual ICollection<OrganisationAddress> OrganisationAddress {get; set;}
				public virtual ICollection<PersonAddresses> PersonAddresses {get; set;}
		
			// ---------Parent Relationships
	

	}
}
