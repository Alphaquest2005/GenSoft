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
	public partial class xcuda_Suppliers_documents: BaseEntity, Ixcuda_Suppliers_documents
	{
		public virtual int ASYCUDA_Id { get; set; }
		public virtual string Suppliers_document_city { get; set; }
		public virtual string Suppliers_document_code { get; set; }
		public virtual string Suppliers_document_country { get; set; }
		public virtual string Suppliers_document_date { get; set; }
		public virtual string Suppliers_document_fax { get; set; }
		public virtual string Suppliers_document_invoice_amt { get; set; }
		public virtual string Suppliers_document_invoice_nbr { get; set; }
		public virtual string Suppliers_document_itmlink { get; set; }
		public virtual string Suppliers_document_name { get; set; }
		public virtual string Suppliers_document_street { get; set; }
		public virtual string Suppliers_document_telephone { get; set; }
		public virtual string Suppliers_document_type_code { get; set; }
		public virtual string Suppliers_document_zip_code { get; set; }

		//-------------------Navigation Properties -------------------------------//
			// ---------Child Relationships
		
			// ---------Parent Relationships
				public virtual xcuda_ASYCUDA xcuda_ASYCUDA {get; set;}
	

	}
}
