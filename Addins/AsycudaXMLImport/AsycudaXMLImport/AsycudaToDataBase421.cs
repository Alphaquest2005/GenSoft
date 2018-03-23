
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SystemInterfaces;
using Asycuda421;
using Common.DataEntites;
using Common.Dynamic;
using JB.Collections.Reactive;


namespace Asycuda
{
    public partial class AsycudaToDataBase421
    {
        private static readonly AsycudaToDataBase421 instance;
        static AsycudaToDataBase421()
        {
            instance = new AsycudaToDataBase421();
           
       
        }

         public static AsycudaToDataBase421 Instance
        {
            get { return instance; }
        }

        private dynamic da = new Expando();
        private ASYCUDA a;

        public Expando Import(ASYCUDA adoc, IDynamicEntity docSet)
        {

            try
            {

                a = adoc;
                var ads = docSet;
                
                da.xcuda_Item = new List<Expando>();
                da.ASYCUDA_Id = 0;
                da.xcuda_ASYCUDA_ExtendedProperties = new Expando();
                da.xcuda_Suppliers_documents = new List<Expando>();

                SaveGeneralInformation();
                SaveDeclarant();
                SaveTraders();
                SaveProperty();
                SaveIdentification();
                SaveTransport();
                SaveFinancial();
                Save_Warehouse();
                Save_Valuation();
                SaveContainer();


                Save_Items();

                if (!((List<Expando>)da.xcuda_Item).Any())
                {
                    return new Expando();
                }



                Save_Suppliers_Documents();
                da.xcuda_ASYCUDA_ExtendedProperties.ImportComplete = true;
                da.xcuda_ASYCUDA_ExtendedProperties.AsycudaDocumentSetId = docSet.Properties["Id"];
                foreach (var itm in da.xcuda_Item)
                {
                    if (itm.ImportComplete != false) continue;
                    da.xcuda_ASYCUDA_ExtendedProperties.ImportComplete = false;
                    break;
                }
                return da;

            }
            catch (Exception)
            {
                throw;
            }
            
        }

       

        private void SavePreviousItem(dynamic itm )
        {

            try
            {

                for (var i = 0; i < a.Prev_decl.Count; i++)
                {


                    var ai = a.Prev_decl.ElementAt(i);
                    if (ai == null) continue;
                    
                    dynamic pi = new Expando();// CreateDynamicEntity("xcuda_PreviousItem");
                    
                    itm.xcuda_PreviousItem = pi;
                    pi.xcuda_Item = itm;
                    
                    pi.Commodity_code = ai.Prev_decl_HS_prec;
                        pi.Current_item_number = ai.Prev_decl_current_item;
                        pi.Current_value = Convert.ToSingle(Math.Round(Convert.ToDouble(ai.Prev_decl_ref_value), 2));
                        pi.Goods_origin = ai.Prev_decl_country_origin;
                        pi.Hs_code = ai.Prev_decl_HS_code;
                        pi.Net_weight = Convert.ToSingle(ai.Prev_decl_weight);
                        pi.Packages_number = ai.Prev_decl_number_packages;
                        pi.Prev_net_weight = Convert.ToSingle(ai.Prev_decl_weight_written_off);
                        pi.Prev_reg_cuo = ai.Prev_decl_office_code;
                        pi.Prev_reg_dat = ai.Prev_decl_reg_year;
                        pi.Prev_reg_nbr = ai.Prev_decl_reg_number;
                        pi.Prev_reg_ser = ai.Prev_decl_reg_serial;
                       if(!string.IsNullOrEmpty(ai.Prev_decl_supp_quantity_written_off)) pi.Preveious_suplementary_quantity = Convert.ToSingle(ai.Prev_decl_supp_quantity_written_off);
                        pi.Previous_item_number = ai.Prev_decl_item_number;
                        pi.Previous_Packages_number = ai.Prev_decl_number_packages_written_off;
                        if (ai.Prev_decl_ref_value_written_off != null)
                            pi.Previous_value = (float)Math.Round(Convert.ToDouble(ai.Prev_decl_ref_value_written_off), 2);
                    if (!string.IsNullOrEmpty(ai.Prev_decl_supp_quantity)) pi.Suplementary_Quantity = Convert.ToSingle(ai.Prev_decl_supp_quantity);

                      

                }
            }
            catch (Exception Ex)
            {
                throw;
            }
        }


        private void Save_Suppliers_Documents()
        {
            if (a.Supplier_documents.Count > 0 && a.Supplier_documents[0] == null) return;
            for (int i = 0; i < a.Supplier_documents.Count; i++)
            {
                var asd = a.Supplier_documents.ElementAt(i);

                dynamic s = new Expando();
                s.ASYCUDA_Id = da.ASYCUDA_Id;

                da.xcuda_Suppliers_documents.Add(s);
                
                
                if (asd.Invoice_supplier_city.Text.Count > 0)
                    s.Suppliers_document_city = asd.Invoice_supplier_city.Text[0];

               

                if (asd.Invoice_supplier_country.Text.Count > 0)
                    s.Suppliers_document_country = asd.Invoice_supplier_country.Text[0];

                if (asd.Invoice_supplier_fax.Text.Count > 0)
                    s.Suppliers_document_fax = asd.Invoice_supplier_fax.Text[0];

                if (asd.Invoice_supplier_name.Text.Count > 0)
                    s.Suppliers_document_name = asd.Invoice_supplier_name.Text[0];

                if (asd.Invoice_supplier_street.Text.Count > 0)
                    s.Suppliers_document_street = asd.Invoice_supplier_street.Text[0];

                if (asd.Invoice_supplier_telephone.Text.Count > 0)
                    s.Suppliers_document_telephone = asd.Invoice_supplier_telephone.Text[0];

                
                if (asd.Invoice_supplier_zip_code.Text.Count > 0)
                    s.Suppliers_document_zip_code = asd.Invoice_supplier_zip_code.Text[0];

                

            }
        }



        private void Save_Items()
        {
            try
            {

          da.xcuda_ASYCUDA_ExtendedProperties.DocumentLines = a.Item.Count;
          for (var i = 0; i < a.Item.Count; i++)
           // Parallel.For(0, a.Item.Count, i =>
            {
                var ai = a.Item.ElementAt(i);
                dynamic di = new Expando();
                di.ASYCUDA_Id = da.ASYCUDA_Id;
                di.ImportComplete = false ;
                da.xcuda_Item.Add(di);

              

                if (!string.IsNullOrEmpty(a.Identification.Registration.Number))
                {
                    di.IsAssessed = true;
                }



                di.LineNumber = i + 1;
                di.SalesFactor = 1;

                if (ai.Licence_number.Text.Count > 0)
                {
                    di.Licence_number = ai.Licence_number.Text[0];
                    di.Amount_deducted_from_licence = ai.Amount_deducted_from_licence;
                    di.Quantity_deducted_from_licence = ai.Quantity_deducted_from_licence;
                }

                SavePreviousItem(di);

                Save_Item_Suppliers_link(di, ai);
               
                Save_Item_Attached_documents(di, ai);

                Save_Item_Packages(di, ai);

                


                Save_Item_Tarification(di, ai);
                
                Save_Item_Goods_description(di, ai);
                Save_Item_Previous_doc(di, ai);
                Save_Item_Taxation(di, ai);
                Save_Item_Valuation_item(di, ai);
               

                

                di.ImportComplete = true;
               
                

            }
            //    );
            }
            catch (Exception)
            {

                throw;
            }
        }

        
        private void Save_Item_Valuation_item(dynamic di, ASYCUDAItem ai)
        {
            var vi = di.xcuda_Valuation_item;
            if (vi == null)
            {
                vi = new Expando();
                vi.Item_Id = di.Item_Id;
                di.xcuda_Valuation_item = vi;
            }
            if (ai.Valuation_item.Alpha_coeficient_of_apportionment != "")
                vi.Alpha_coeficient_of_apportionment = ai.Valuation_item.Alpha_coeficient_of_apportionment;
            if (ai.Valuation_item.Rate_of_adjustement != "")
                vi.Rate_of_adjustement = Convert.ToDouble(ai.Valuation_item.Rate_of_adjustement);
            if (ai.Valuation_item.Statistical_value != "")
                vi.Statistical_value = Convert.ToSingle(ai.Valuation_item.Statistical_value);
            if (ai.Valuation_item.Total_CIF_itm != "")
                vi.Total_CIF_itm = Convert.ToSingle(ai.Valuation_item.Total_CIF_itm);
            if (ai.Valuation_item.Total_cost_itm != "")
                vi.Total_cost_itm = Convert.ToSingle(ai.Valuation_item.Total_cost_itm);

            Save_Item_Invoice(vi, ai);
            Save_item_External_freight(vi, ai);
            Save_Weight_Item(vi, ai);

           
        }

        private void Save_Weight_Item(dynamic vi, ASYCUDAItem ai)
        {
            var wi = vi.xcuda_Weight_itm;
            if (wi == null)
            {
                wi = new Expando();
                wi.Valuation_item_Id = vi.Item_Id;
                vi.xcuda_Weight_itm = wi;
            }
            if (ai.Valuation_item.Weight_itm.Gross_weight_itm != "")
                wi.Gross_weight_itm = Convert.ToSingle(ai.Valuation_item.Weight_itm.Gross_weight_itm);

            if (ai.Valuation_item.Weight_itm.Net_weight_itm != "")
                wi.Net_weight_itm = Convert.ToSingle(ai.Valuation_item.Weight_itm.Net_weight_itm);

        }

        private void Save_item_External_freight(dynamic vi, ASYCUDAItem ai)
        {
            var i = vi.xcuda_item_external_freight;
            if (i == null)
            {
                i = new Expando();
                i.Valuation_item_Id = vi.Item_Id;
                vi.xcuda_item_external_freight = i;
            }
            if (ai.Valuation_item.item_external_freight.Amount_foreign_currency != "")
                i.Amount_foreign_currency = Convert.ToSingle(ai.Valuation_item.item_external_freight.Amount_foreign_currency);
            if (ai.Valuation_item.item_external_freight.Amount_national_currency != "")
                i.Amount_national_currency = Convert.ToSingle(ai.Valuation_item.item_external_freight.Amount_national_currency);

            i.Currency_code = ai.Valuation_item.item_external_freight.Currency_code.Text.FirstOrDefault();

            if (ai.Valuation_item.item_external_freight.Currency_rate != "")
                i.Currency_rate = Convert.ToSingle(ai.Valuation_item.item_external_freight.Currency_rate);

        }

        private void Save_Item_Invoice(dynamic vi, ASYCUDAItem ai)
        {
            var i = vi.xcuda_Item_Invoice;
            if (i == null)
            {
                i = new Expando();
                i.Valuation_item_Id = vi.Item_Id;
                vi.xcuda_Item_Invoice = i;
            }
            if (ai.Valuation_item.Item_Invoice.Amount_foreign_currency != "")
                i.Amount_foreign_currency = Convert.ToSingle(ai.Valuation_item.Item_Invoice.Amount_foreign_currency);
            if (ai.Valuation_item.Item_Invoice.Amount_national_currency != "")
                i.Amount_national_currency = Convert.ToSingle(ai.Valuation_item.Item_Invoice.Amount_national_currency);
            if (ai.Valuation_item.Item_Invoice.Currency_code?.Text?.FirstOrDefault() != null)
                i.Currency_code = ai.Valuation_item.Item_Invoice.Currency_code.Text.FirstOrDefault();
            if (ai.Valuation_item.Item_Invoice.Currency_rate != "")
                i.Currency_rate = Convert.ToSingle(ai.Valuation_item.Item_Invoice.Currency_rate);

        }

        private void Save_Item_Taxation(dynamic di, ASYCUDAItem ai)
        {
            var t = di.xcuda_Taxation;
            if (t == null)
            {

                t = new Expando();
                t.Item_Id = di.Item_Id;
                t.xcuda_Taxation_line = new List<Expando>();
                di.xcuda_Taxation = t;
               
            }

            t.Counter_of_normal_mode_of_payment = ai.Taxation.Counter_of_normal_mode_of_payment;
            t.Displayed_item_taxes_amount = ai.Taxation.Displayed_item_taxes_amount;
            if (ai.Taxation.Item_taxes_amount != "")
                t.Item_taxes_amount = Convert.ToSingle(ai.Taxation.Item_taxes_amount);
            t.Item_taxes_guaranted_amount = ai.Taxation.Item_taxes_guaranted_amount;
            if (ai.Taxation.Item_taxes_mode_of_payment.Text.Count > 0)
                t.Item_taxes_mode_of_payment = ai.Taxation.Item_taxes_mode_of_payment.Text[0];


            Save_Taxation_line(t, ai);

            
        }

        private void Save_Taxation_line(dynamic t, ASYCUDAItem ai)
        {
            for (var i = 0; i < ai.Taxation.Taxation_line.Count; i++)
            {
                var au = ai.Taxation.Taxation_line.ElementAt(i);

                if (au.Duty_tax_code.Text.Count == 0) break;

                dynamic tl = new Expando();
                    t.xcuda_Taxation_line.Add(tl);
                    
               

                tl.Duty_tax_amount = Convert.ToDouble(au.Duty_tax_amount);
                tl.Duty_tax_Base = au.Duty_tax_Base;
                tl.Duty_tax_code = au.Duty_tax_code.Text[0];

                if (au.Duty_tax_MP.Text.Count > 0)
                    tl.Duty_tax_MP = au.Duty_tax_MP.Text[0];

                tl.Duty_tax_rate = Convert.ToDouble(au.Duty_tax_rate);

            }
        }

        private void Save_Item_Previous_doc(dynamic di, ASYCUDAItem ai)
        {
            var pd = di.xcuda_Previous_doc;
            if (pd == null)
            {
                pd = new Expando();
                pd.Item_Id = di.Item_Id;
                di.xcuda_Previous_doc = pd;
            }
            pd.Summary_declaration = ai.Previous_doc.Summary_declaration.Text.FirstOrDefault();
            if (da.xcuda_ASYCUDA_ExtendedProperties.BLNumber == null && ai.Previous_doc.Summary_declaration != null)
                da.xcuda_ASYCUDA_ExtendedProperties.BLNumber = ai.Previous_doc.Summary_declaration.Text.FirstOrDefault();

            
        }

        private void Save_Item_Goods_description(dynamic di, ASYCUDAItem ai)
        {
            var g = di.xcuda_Goods_description;
            if (g == null)
            {
                g = new Expando();
                g.Item_Id = di.Item_Id;
                di.xcuda_Goods_description = g;
            }
            g.Commercial_Description = ai.Goods_description.Commercial_Description;
            g.Country_of_origin_code = ai.Goods_description.Country_of_origin_code;
            g.Description_of_goods = ai.Goods_description.Description_of_goods.Text.FirstOrDefault();
            
        }

        private void Save_Item_Tarification(dynamic di, ASYCUDAItem ai)
        {
            var t = di.xcuda_Tarification;
            if (t == null)
            {
                t = new Expando();
                t.Item_Id = di.Item_Id;
                t.Unordered_xcuda_Supplementary_unit = new List<Expando>();
                di.xcuda_Tarification = t;

            }

            t.Extended_customs_procedure = ai.Tarification.Extended_customs_procedure;
            t.National_customs_procedure = ai.Tarification.National_customs_procedure;
            if (ai.Tarification.Item_price != "")
                t.Item_price = Convert.ToSingle(ai.Tarification.Item_price);
            if (ai.Tarification.Value_item.Text.Count > 0)
                t.Value_item = ai.Tarification.Value_item.Text[0];

            Save_Supplementary_unit(t, ai);
            
            if (ai.Tarification.Attached_doc_item.Text.Count > 0)
                t.Attached_doc_item = ai.Tarification.Attached_doc_item.Text[0];
            
            Save_HScode(t, ai);
            
        }


        private void Save_Supplementary_unit(dynamic t, ASYCUDAItem ai)
        {
            for (var i = 0; i < ai.Tarification.Supplementary_unit.Count; i++)
            {
                var au = ai.Tarification.Supplementary_unit.ElementAt(i);

                if (au.Suppplementary_unit_code.Text.Count == 0) continue;

               
                   dynamic su = new Expando();
                    su.Tarification_Id = t.Item_Id;
                    t.Unordered_xcuda_Supplementary_unit.Add(su);
               

                su.Suppplementary_unit_quantity = Convert.ToDouble(string.IsNullOrEmpty(au.Suppplementary_unit_quantity)
                    ? "0"
                    : au.Suppplementary_unit_quantity);

                if (au.Suppplementary_unit_code.Text.Count > 0)
                    su.Suppplementary_unit_code = au.Suppplementary_unit_code.Text[0];

                if (au.Suppplementary_unit_name.Text.Count > 0)
                    su.Suppplementary_unit_name = au.Suppplementary_unit_name.Text[0];

                if (i == 0) su.IsFirstRow = true;
            }
        }



        private void Save_HScode(dynamic t, ASYCUDAItem ai)
        {
            var h = t.xcuda_HScode;
            if (h == null)
            {
                h = new Expando();
                h.Item_Id = t.Item_Id;
                t.xcuda_HScode = h;
            }

            h.Commodity_code = ai.Tarification.HScode.Commodity_code;
            h.Precision_1 = ai.Tarification.HScode.Precision_1;
            if (ai.Tarification.HScode.Precision_4.Text.FirstOrDefault() != null)
            {
                h.Precision_4 = ai.Tarification.HScode.Precision_4.Text.FirstOrDefault();
            }
            
        }

        private void Save_Item_Packages(dynamic di, ASYCUDAItem ai)
        {
            var p = di.xcuda_Packages;
            if (p == null)
            {
                p = new Expando();
                p.Item_Id = di.Item_Id;
                di.xcuda_Packages = p;
            }
            p.Kind_of_packages_code = ai.Packages.Kind_of_packages_code;
            p.Kind_of_packages_name = ai.Packages.Kind_of_packages_name;
            p.Number_of_packages = Convert.ToSingle(ai.Packages.Number_of_packages);

            if (ai.Packages.Marks1_of_packages.Text.Count > 0)
                p.Marks1_of_packages = ai.Packages.Marks1_of_packages.Text[0];

            if (ai.Packages.Marks2_of_packages.Text.Count > 0)
                p.Marks2_of_packages = ai.Packages.Marks2_of_packages.Text[0];
            
        }

        private void Save_Item_Suppliers_link(dynamic di, ASYCUDAItem ai)
        {
            var sl = di.xcuda_Suppliers_link;
            if (sl == null)
            {
                sl = new Expando();
                sl.Item_Id = di.Item_Id;
                di.xcuda_Suppliers_link = sl;
            }

            sl.Suppliers_link_code = ai.Suppliers_link.Suppliers_link_code;
            
        }

        private void Save_Item_Attached_documents(dynamic di, ASYCUDAItem ai)
        {
            for (var i = 0; i < ai.Attached_documents.Count; i++)
            {
                if (ai.Attached_documents[i].Attached_document_code.Text.Count == 0) break;

                dynamic ad =  new Expando();
                    ad.Item_Id = di.Item_Id;
                    di.xcuda_Attached_documents.Add(ad);
                

                ad.Attached_document_date = ai.Attached_documents[i].Attached_document_date;

                if (ai.Attached_documents[i].Attached_document_code.Text.Count != 0)
                    ad.Attached_document_code = ai.Attached_documents[i].Attached_document_code.Text[0];

                if (ai.Attached_documents[i].Attached_document_from_rule.Text.Count != 0)
                    ad.Attached_document_from_rule = Convert.ToInt32(ai.Attached_documents[i].Attached_document_from_rule.Text[0]);

                if (ai.Attached_documents[i].Attached_document_name.Text.Count != 0)
                    ad.Attached_document_name = ai.Attached_documents[i].Attached_document_name.Text[0];

                if (ai.Attached_documents[i].Attached_document_reference.Text.Count != 0)
                    ad.Attached_document_reference = ai.Attached_documents[i].Attached_document_reference.Text[0];
                
            }
        }

        private void SaveContainer()
        {
            foreach (var ac in a.Container)
            {

                dynamic c = new Expando();
                c.ASYCUDA_Id = da.ASYCUDA_Id;
                da.xcuda_Container.Add(c);
                c.Container_identity = ac.Container_identity;
                c.Container_type = ac.Container_type;
                c.Empty_full_indicator = ac.Empty_full_indicator;
                c.Goods_description = ac.Goods_description;
                c.Gross_weight = Convert.ToSingle(ac.Gross_weight.Text.FirstOrDefault());
                c.Item_Number = ac.Item_Number;
                c.Packages_number = ac.Packages_number;
                c.Packages_type = ac.Packages_type;
                c.Packages_weight = Convert.ToSingle(ac.Packages_weight);
            }

        }

        private void Save_Valuation()
        {
            var v = da.xcuda_Valuation;
            if (v == null)
            {
                v = new Expando();
                v.ASYCUDA_Id = da.ASYCUDA_Id;
                da.xcuda_Valuation = v;
            }
            v.Calculation_working_mode = a.Valuation.Calculation_working_mode;
            v.Total_CIF = Convert.ToSingle(a.Valuation.Total_CIF);
            v.Total_cost = Convert.ToSingle(a.Valuation.Total_cost);

            Save_Valuation_Weight(v);
            Save_Gs_Invoice(v);
            Save_Gs_External_freight(v);
            Save_Total(v);
            
        }

        private void Save_Total(dynamic v)
        {
            var t = v.xcuda_Total;
            if (t == null)
            {
                t = new Expando();
                t.Valuation_Id = v.ASYCUDA_Id;
                v.xcuda_Total = t;
            }
            t.Total_invoice = Convert.ToSingle(a.Valuation.Total.Total_invoice);
            t.Total_weight = Convert.ToSingle(a.Valuation.Total.Total_weight);
        }

        private void Save_Gs_External_freight(dynamic v)
        {
            var gf = v.xcuda_Gs_external_freight;
            if (gf == null)
            {
                gf = new Expando();
                gf.Valuation_Id = v.ASYCUDA_Id;
                v.xcuda_Gs_external_freight = gf;
            }

            gf.Amount_foreign_currency = Convert.ToSingle(a.Valuation.Gs_external_freight.Amount_foreign_currency);
            gf.Amount_national_currency = Convert.ToSingle(a.Valuation.Gs_external_freight.Amount_national_currency);
            gf.Currency_code = a.Valuation.Gs_external_freight.Currency_code.Text.FirstOrDefault();
            gf.Currency_name = a.Valuation.Gs_external_freight.Currency_name;
            gf.Currency_rate = Convert.ToSingle(a.Valuation.Gs_external_freight.Currency_rate);


        }

        private void Save_Gs_Invoice(dynamic v)
        {
            var gi = v.xcuda_Gs_Invoice;
            if (gi == null)
            {
                gi = new Expando();
                gi.Valuation_Id = v.ASYCUDA_Id;
                v.xcuda_Gs_Invoice = gi;
            }

            gi.Amount_foreign_currency = Convert.ToSingle(a.Valuation.Gs_Invoice.Amount_foreign_currency);
            gi.Amount_national_currency = Convert.ToSingle(a.Valuation.Gs_Invoice.Amount_national_currency);
            gi.Currency_code = a.Valuation.Gs_Invoice.Currency_code.Text.FirstOrDefault();
            gi.Currency_rate = Convert.ToSingle(a.Valuation.Gs_Invoice.Currency_rate);
            if (a.Valuation.Gs_Invoice.Currency_name.Text.Count != 0)
                gi.Currency_name = a.Valuation.Gs_Invoice.Currency_name.Text[0];
        }

        private void Save_Valuation_Weight(dynamic v)
        {
            var w = v.xcuda_Weight;
            if (w == null)
            {
                w = new Expando();
                w.Valuation_Id = v.ASYCUDA_Id;
                v.xcuda_Weight = w;
            }
            w.Gross_weight = a.Valuation.Weight.Gross_weight;
        }

        private void Save_Warehouse()
        {
            var w = da.xcuda_Warehouse;
            if (w == null)
            {
                w = new Expando();
                w.ASYCUDA_Id = da.ASYCUDA_Id;
                da.xcuda_Warehouse = w;
            }
            w.Identification = a.Warehouse.Identification.Text.FirstOrDefault();
            w.Delay = a.Warehouse.Delay;
            
        }

        private void SaveFinancial()
        {
            var f = da.xcuda_Financial;
            if (f == null)
            {
                f = new Expando();
                    f.ASYCUDA_Id = da.ASYCUDA_Id;
               
                da.xcuda_Financial=f;
            }
            if (a.Financial.Deffered_payment_reference.Text.Count != 0)
                f.Deffered_payment_reference = a.Financial.Deffered_payment_reference.Text[0];

            f.Mode_of_payment = a.Financial.Mode_of_payment;

            Save_Amounts(f);
            Save_Guarantee(f);
        }

        public ASYCUDA A
        {
            get { return a; }
            set { a = value; }
        }

        private void Save_Guarantee(dynamic f)
        {
            var g = f.xcuda_Financial_Guarantee;
            if (g == null)
            {
                g = new Expando();
                g.Financial_Id = f.Financial_Id;
                f.xcuda_Financial_Guarantee = g;
            }
            if (a.Financial.Guarantee.Amount != "")
                g.Amount = Convert.ToDecimal(a.Financial.Guarantee.Amount);
                g.Date = a.Financial.Guarantee.Date;
        }

        private void Save_Amounts(dynamic f)
        {
            
               dynamic fa = new Expando();
                fa.Financial_Id = f.Financial_Id;
                f.xcuda_Financial_Amounts = fa;
           
            if (a.Financial.Amounts.Global_taxes != "")
                fa.Global_taxes = Convert.ToDecimal(a.Financial.Amounts.Global_taxes);
            // fa.Total_manual_taxes = a.Financial.Amounts.Total_manual_taxes;
            if (a.Financial.Amounts.Totals_taxes != "")
                fa.Totals_taxes = Convert.ToDecimal(a.Financial.Amounts.Totals_taxes);
        }

        private void SaveTransport()
        {
            var t = da.xcuda_Transport;
            if (t == null)
            {
                t = new Expando();
                t.ASYCUDA_Id = da.ASYCUDA_Id;
                da.xcuda_Transport = t;
            }
            t.Container_flag = a.Transport.Container_flag;
            t.Single_waybill_flag = a.Transport.Single_waybill_flag;
            if (a.Transport.Location_of_goods.Text.Count != 0)
            {
                t.Location_of_goods = a.Transport.Location_of_goods.Text[0];
            }
            SaveMeansofTransport(t);
            Save_Delivery_terms(t);
            Save_Border_office(t);
            
        }

        private void Save_Border_office(dynamic t)
        {
            var bo = t.xcuda_Border_office;
            if (bo == null)
            {
                bo = new Expando();
                bo.Transport_Id = t.Transport_Id;
                t.xcuda_Border_office = bo;
            }
            if (a.Transport.Border_office.Code.Text.Count != 0)
                bo.Code = a.Transport.Border_office.Code.Text[0];

            if (a.Transport.Border_office.Name.Text.Count != 0)
                bo.Name = a.Transport.Border_office.Name.Text[0];

        }

        private void Save_Delivery_terms(dynamic t)
        {
            var d = t.xcuda_Delivery_terms;
            if (d == null)
            {
                d = new Expando();
                d.Transport_Id = t.Transport_Id;
                t.xcuda_Delivery_terms = d;
            }
            if (a.Transport.Delivery_terms.Code.Text.Count != 0)
                d.Code = a.Transport.Delivery_terms.Code.Text[0];
            //d.Place = a.Transport.Delivery_terms.Place
        }

        private void SaveMeansofTransport(dynamic t)
        {
            var m = t.xcuda_Means_of_transport;
            if (m == null)
            {
                m = new Expando();
                m.Transport_Id = t.Transport_Id;
                t.xcuda_Means_of_transport = m;

            }

            SaveDepartureArrivalInformation(m);
            SaveBorderInformation(m);
            //m.Inland_mode_of_transport = a.Transport.Means_of_transport.Inland_mode_of_transport.

        }



        private void SaveBorderInformation(dynamic m)
        {
            var d = m.xcuda_Border_information;
            if (d == null)
            {
                d = new Expando();
                d.Means_of_transport_Id = m.Means_of_transport_Id;
                m.xcuda_Border_information = d;
            }
            //if (a.Transport.Means_of_transport.Border_information.Nationality.ToString() != null)
            //    d.Nationality = a.Transport.Means_of_transport.Departure_arrival_information.Nationality.Text[0];

            //if (a.Transport.Means_of_transport.Departure_arrival_information.Identity.Text.Count != 0)
            //    d.Identity = a.Transport.Means_of_transport.Departure_arrival_information.Identity.Text[0];
            if (a.Transport.Means_of_transport.Border_information.Mode.Text.Count != 0)
                d.Mode = Convert.ToInt32(a.Transport.Means_of_transport.Border_information.Mode.Text[0]);
        }

        private void SaveDepartureArrivalInformation(dynamic m)
        {
            var d = m.xcuda_Departure_arrival_information;
            if (d == null)
            {
                d = new Expando();
                d.Means_of_transport_Id = m.Means_of_transport_Id;
                m.xcuda_Departure_arrival_information = d;
            }
            if (a.Transport.Means_of_transport.Departure_arrival_information.Nationality.Text.Count != 0)
                d.Nationality = a.Transport.Means_of_transport.Departure_arrival_information.Nationality.Text[0];

            if (a.Transport.Means_of_transport.Departure_arrival_information.Identity.Text.Count != 0)
                d.Identity = a.Transport.Means_of_transport.Departure_arrival_information.Identity.Text[0];
        }

        private void SaveGeneralInformation()
        {
            var gi = da.xcuda_General_information;
            if (gi == null)
            {
                gi = new Expando();
                gi.ASYCUDA_Id = da.ASYCUDA_Id;
                da.xcuda_General_information = gi;
            }
            gi.Value_details = a.General_information.Value_details;
            gi.Comments_free_text = a.General_information.Comments_free_text.Text.FirstOrDefault();

            SetEffectiveAssessmentDate(da, gi.Comments_free_text);

            SaveCountry(gi);
            
        }

        private void SetEffectiveAssessmentDate(dynamic documentCt, string commentsFreeText)
        {
            if (string.IsNullOrEmpty(commentsFreeText)) return;
            documentCt.Document.xcuda_ASYCUDA_ExtendedProperties.EffectiveRegistrationDate = DateTime.ParseExact(commentsFreeText.Replace("EffectiveAssessmentDate:",""),"MMM-dd-yyyy",null);
        }

        private void SaveCountry(dynamic gi)
        {
            var c = gi.xcuda_Country;
            if (c == null)
            {
                c = new Expando();
                c.Country_Id = gi.ASYCUDA_Id;
                gi.xcuda_Country = c;
            }
            c.Country_first_destination = a.General_information.Country.Country_first_destination.Text.FirstOrDefault();
            c.Country_of_origin_name = a.General_information.Country.Country_of_origin_name;
            c.Trading_country = a.General_information.Country.Trading_country.Text.FirstOrDefault();
            SaveExport(c);
            SaveDestination(c);
        }

        private void SaveDestination(dynamic c)
        {
            var des = c.xcuda_Destination;
            if (des == null)
            {
                des = new Expando();
                des.Country_Id = c.Country_Id;
                c.xcuda_Destination = des;
                des.xcuda_Country = c;
                
            }
            des.Destination_country_code = a.General_information.Country.Destination.Destination_country_code.Text.FirstOrDefault();
            if (a.General_information.Country.Destination.Destination_country_name != null)
                des.Destination_country_name = a.General_information.Country.Destination.Destination_country_name.Text.FirstOrDefault();
            
        }

        private void SaveExport(dynamic c)
        {
            var Exp = c.xcuda_Export;
            if (Exp == null)
            {
                Exp = new Expando();
                Exp.Country_Id = c.Country_Id;
                c.xcuda_Export = Exp;
            }
            Exp.Export_country_code = a.General_information.Country.Export.Export_country_code;
            Exp.Export_country_name = a.General_information.Country.Export.Export_country_name;
            Exp.Export_country_region = a.General_information.Country.Export.Export_country_region.Text.FirstOrDefault();
        }

        private void SaveDeclarant()
        {
            try
            {
                var d = da.xcuda_Declarant;
                if (d == null)
                {
                    da.xcuda_Declarant = new Expando();
                    da.ASYCUDA_Id = da.ASYCUDA_Id;
                    d = da.xcuda_Declarant;
                    
                }

                d.Declarant_name = a.Declarant.Declarant_name;
                d.Declarant_representative = a.Declarant.Declarant_representative.Text.FirstOrDefault();
                d.Declarant_code = a.Declarant.Declarant_code;

               
                d.Number = a.Declarant.Reference.Number.Text.FirstOrDefault();
                
            }
            catch (Exception Ex)
            {
                throw new Exception("Declarant fail to import - " + a.Declarant.Reference.Number);
            }

        }

        private void SaveTraders()
        {
            var t = da.xcuda_Traders;
            if (t == null)
            {
                t = new Expando();
                t.Traders_Id = da.ASYCUDA_Id;
                da.xcuda_Traders = t;
            }
            SaveExporter(t);
            SaveConsignee(t);
            SaveTradersFinancial(t);
            //DBaseDataModel.Instance.Savexcuda_Traders(t);
        }

        private void SaveTradersFinancial(dynamic t)
        {
            if (a.Traders.Financial.Financial_code.Text.Count == 0) return;
            var f = t.xcuda_Traders_Financial;
            if (f == null)
            {
                f = new Expando();
                f.Traders_Id = t.Traders_Id;
                t.xcuda_Traders_Financial = f;
            }
            if (a.Traders.Financial.Financial_code.Text.Count != 0)
            {
                f.Financial_code = a.Traders.Financial.Financial_code.Text[0];
            }
            if (a.Traders.Financial.Financial_name.Text.Count != 0)
            {
                f.Financial_name = a.Traders.Financial.Financial_name.Text[0];
            }
        }

        private void SaveConsignee(dynamic t)
        {
            var c = t.xcuda_Consignee;
            if (c == null)
            {
                c = new Expando();
                c.Traders_Id = t.Traders_Id;
                t.xcuda_Consignee = c;
            }
            if (a.Traders.Consignee.Consignee_code.Text.Count != 0)
            {
                c.Consignee_code = a.Traders.Consignee.Consignee_code.Text[0];
            }
            if (a.Traders.Consignee.Consignee_name.Text.Count != 0)
            {
                c.Consignee_name = a.Traders.Consignee.Consignee_name.Text[0];
            }
        }

        private void SaveExporter(dynamic t)
        {
            var e = t.xcuda_Exporter;
            if (e == null)
            {
                e = new Expando();
                e.Traders_Id = t.Traders_Id;
                t.xcuda_Exporter = e;
            }

            if (a.Traders.Exporter.Exporter_name.Text.Count != 0)
            {
                e.Exporter_code = a.Traders.Exporter.Exporter_name.Text[0];
            }

            if (a.Traders.Exporter.Exporter_code.Text.Count != 0)
            {
                e.Exporter_code = a.Traders.Exporter.Exporter_code.Text[0];
            }
        }

        private void SaveProperty()
        {
            var p = da.xcuda_Property;

            if (p == null)
            {
                p = new Expando() {  };
                da.xcuda_Property = p;
                
            }
            
            SaveNbers(p);
            
        }

        private void SaveNbers(dynamic p)
        {

            var n = p.xcuda_Nbers;
            if (n == null)
            {
                n = new Expando();
                n.ASYCUDA_Id = p.ASYCUDA_Id;
                p.xcuda_Nbers = n;
                
            }
            n.Number_of_loading_lists = a.Property.Nbers.Number_of_loading_lists;
            n.Total_number_of_packages = Convert.ToSingle(string.IsNullOrEmpty(a.Property.Nbers.Total_number_of_packages) ? "0" : a.Property.Nbers.Total_number_of_packages);
            n.Total_number_of_items = a.Property.Nbers.Total_number_of_items;

        }

        private void SaveIdentification()
        {
            var di = da.xcuda_Identification;
            if (di == null)
            {
                di = new Expando() {  };
                da.xcuda_Identification = di;
                
            }

            SaveManifestReferenceNumber(di);
            SaveOfficeSegment(di);
            SaveRegistration(di);
            SaveType(di);

            

        }

        private void SaveType(dynamic di)
        {
            var t = di.xcuda_Type;
            if (t == null)
            {
                t = new Expando() {  };
                di.xcuda_Type = t;
            }

            t.Declaration_gen_procedure_code = a.Identification.Type.Declaration_gen_procedure_code;
            t.Type_of_declaration = a.Identification.Type.Type_of_declaration;


            dynamic dt = new Expando();
            dt.Type_of_declaration = t.Type_of_declaration;
            dt.Declaration_gen_procedure_code = t.Declaration_gen_procedure_code;
                    
            
            
            da.xcuda_ASYCUDA_ExtendedProperties.Document_Type = dt;

        }

        
        private void SaveManifestReferenceNumber(dynamic di)
        {
            
            if (a.Identification.Manifest_reference_number.Text.Count != 0)
                di.Manifest_reference_number = a.Identification.Manifest_reference_number.Text[0];

        }

        private void SaveOfficeSegment(dynamic di)
        {
            var o = di.xcuda_Office_segment;
            if (o == null)
            {
                o = new Expando();
                o.ASYCUDA_Id = di.ASYCUDA_Id;
                di.xcuda_Office_segment = o;
                
            }
            o.Customs_clearance_office_code = a.Identification.Office_segment.Customs_clearance_office_code.Text.FirstOrDefault();
            o.Customs_Clearance_office_name = a.Identification.Office_segment.Customs_Clearance_office_name.Text.FirstOrDefault();

        }

        private void SaveRegistration(dynamic di)
        {
            var r = di.xcuda_Registration;
            if (r == null)
            {
                r = new Expando();
                r.ASYCUDA_Id = di.ASYCUDA_Id;
                di.xcuda_Registration = r;
                
            }
            if (a.Identification.Registration.Date != "1/1/0001")
                r.Date = a.Identification.Registration.Date;
            if (a.Identification.Registration.Number != "")
                r.Number = a.Identification.Registration.Number;

        }
    }
}
