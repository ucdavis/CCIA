using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace CCIA.Models
{
    public partial class CCIAContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<AbbrevAppType> AbbrevAppType { get; set; }
        public virtual DbSet<AbbrevClassProduced> AbbrevClassProduced { get; set; }
        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<Applications> Applications { get; set; }
        public virtual DbSet<BlendRequests> BlendRequests { get; set; }
        public virtual DbSet<CertRad> CertRad { get; set; }
        public virtual DbSet<Certs> Certs { get; set; }
        public virtual DbSet<ChangeRequests> ChangeRequests { get; set; }
        public virtual DbSet<Charges> Charges { get; set; }
        public virtual DbSet<CondStatus> CondStatus { get; set; }
        public virtual DbSet<ContactAddress> ContactAddress { get; set; }
        public virtual DbSet<Contacts> Contacts { get; set; }
        public virtual DbSet<ContactToOrg> ContactToOrg { get; set; }
        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<County> County { get; set; }
        public virtual DbSet<Crops> Crops { get; set; }
        public virtual DbSet<CropsRates> CropsRates { get; set; }
        public virtual DbSet<CropStandards> CropStandards { get; set; }
        public virtual DbSet<DistrictCounty> DistrictCounty { get; set; }
        public virtual DbSet<Districts> Districts { get; set; }
        public virtual DbSet<Fees> Fees { get; set; }
        public virtual DbSet<FieldInspect> FieldInspect { get; set; }
        public virtual DbSet<FieldMaps> FieldMaps { get; set; }
        public virtual DbSet<LotBlends> LotBlends { get; set; }
        public virtual DbSet<Organizations> Organizations { get; set; }
        public virtual DbSet<PlantingStocks> PlantingStocks { get; set; }
        public virtual DbSet<Rates> Rates { get; set; }

        public virtual DbSet<StateProvince> StateProvince { get; set; }
        public virtual DbSet<SxLabResults> SxLabResults { get; set; }
        public virtual DbSet<VarFamily> VarFamily { get; set; }
        public virtual DbSet<VarOfficial> VarOfficial { get; set; }
        public virtual DbSet<VarFull> VarFull { get; set; }
        public virtual DbSet<Seeds> Seeds { get; set; }

        public virtual DbSet<AbbrevClassSeeds> AbbrevClassSeeds { get; set; }

        public virtual DbSet<Tags> Tags { get; set; }

        public virtual DbSet<AbbrevTagType>  AbbrevTagType { get; set; }

        public virtual DbSet<OECD> OECD { get; set; }

        public virtual DbSet<AbbrevOECDClass> AbbrevOECDClass { get; set; }

        public virtual DbSet<BulkSalesCertificates> BulkSalesCertificates { get; set; }

        public virtual DbSet<CCIAEmployees> CCIAEmployees { get; set; }

        public virtual DbSet<BulkSalesCertificatesShares> BulkSalesCertificatesShares { get; set; }

        public virtual DbSet<SeedTransfers> SeedTransfers { get; set; }

        // Unable to generate entity type for table 'dbo.map_radish_isolation'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.fir_docs'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.seed_doc_types'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.map_cucumber_isolation'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.map_cucurbita_isolation'. Please see the warning messages.
        
        // Unable to generate entity type for table 'dbo.turfgrass_certificates'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.idaho_brassica_radish_isolation'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.tag_series'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.ecoregions'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.map_objects'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.map_typelut'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.application_changes'. Please see the warning messages.
       
        // Unable to generate entity type for table 'dbo.map_sweetcorn_isolation'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.map_croppts_app_listing'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.my_customers'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.map_crop_access'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.renew_actions_trans'. Please see the warning messages.
        
        // Unable to generate entity type for table 'dbo.tag_docs'. Please see the warning messages.
      
        
       
       
        // Unable to generate entity type for table 'dbo.random_seeds2015'. Please see the warning messages.
        
        // Unable to generate entity type for table 'dbo.seeds_changes'. Please see the warning messages.
       
        // Unable to generate entity type for table 'dbo.map_user_access'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.blend_docs'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.var_countries'. Please see the warning messages.
       
       
        // Unable to generate entity type for table 'dbo.map_alfalfa_gefree_isolation'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.blend_components_changes'. Please see the warning messages.
      
        // Unable to generate entity type for table 'dbo.seed_docs'. Please see the warning messages.
       
        // Unable to generate entity type for table 'dbo.oecd_changes'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.renew_fields'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.tags_changes'. Please see the warning messages.
        
        // Unable to generate entity type for table 'dbo.standards'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.idaho_beta_isolation'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.idaho_lists'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.field_results'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.sx_lab_results_changes'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.po_cert_history'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.po_health_cert'. Please see the warning messages.

        // Unable to generate entity type for table 'dbo.seeds_apps'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.map_croppts_app'. Please see the warning messages.
        
        // Unable to generate entity type for table 'dbo.org_address'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.idaho_carrot_isolation'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.app_certificates'. Please see the warning messages.
       
        // Unable to generate entity type for table 'dbo.map_onion_isolation'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.map_carrot_isolation'. Please see the warning messages.
      
        // Unable to generate entity type for table 'dbo.map_brassica_isolation'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.crop_assign'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Jobs'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.contact_map'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.org_map'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.bulk_sales_certificates_changes'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.notices'. Please see the warning messages.
       
        // Unable to generate entity type for table 'dbo.idaho_onion_isolation'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.seed_transfer_changes'. Please see the warning messages.

       

        private ILoggerFactory GetLoggerFactory()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder =>
                   builder.AddConsole()
                          .AddFilter(DbLoggerCategory.Database.Command.Name,
                                     LogLevel.Information));
            return serviceCollection.BuildServiceProvider()
                    .GetService<ILoggerFactory>();
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=cherry01;Database=CCIA-Azure-Dev;Trusted_Connection=True;");
            }
            optionsBuilder.UseLoggerFactory(GetLoggerFactory());

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Seeds>(entity =>
            {
                entity.ToTable("Seeds");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("seeds_id");

                entity.Property(e => e.CertProgram).HasColumnName("cert_program");
                entity.Property(e => e.AppId).HasColumnName("app_id");
                entity.Property(e => e.SampleFormNumber).HasColumnName("sx_form_no");
                entity.Property(e => e.SampleFormDate).HasColumnName("sx_form_date");
                entity.Property(e => e.SampleFormCertNumber).HasColumnName("sx_form_cert_no");
                entity.Property(e => e.SampleFormRad).HasColumnName("sx_form_rad");
                entity.Property(e => e.CertYear).HasColumnName("cert_year");
                entity.Property(e => e.ApplicantId).HasColumnName("applicant_id");
                entity.Property(e => e.ConditionerId).HasColumnName("conditioner_id");
                entity.Property(e => e.SampleFormVarietyId).HasColumnName("sx_form_variety_id");
                entity.Property(e => e.OfficialVarietyId).HasColumnName("official_variety_id");
                entity.Property(e => e.LotNumber).HasColumnName("lot_num");
                entity.Property(e => e.PoundsLot).HasColumnName("lbs_lot");
                entity.Property(e => e.Class).HasColumnName("class");
                entity.Property(e => e.ClassAccession).HasColumnName("class_produced_accession");
                entity.Property(e => e.Status).HasColumnName("status");
                entity.Property(e => e.CountyDrawn).HasColumnName("county_drawn");
                entity.Property(e => e.OriginState).HasColumnName("origin_state");
                entity.Property(e => e.OriginCountry).HasColumnName("origin_country");
                entity.Property(e => e.Bulk).HasColumnName("sx_bulk");
                entity.Property(e => e.OriginalRun).HasColumnName("original_run");
                entity.Property(e => e.Remill).HasColumnName("remill");
                entity.Property(e => e.Treated).HasColumnName("treated");
                entity.Property(e => e.OECDTestRequired).HasColumnName("oecd_test_req");
                entity.Property(e => e.Resampled).HasColumnName("resample");
                entity.Property(e => e.CCIAAuth).HasColumnName("ccia_auth");
                entity.Property(e => e.Remarks).HasColumnName("remarks");
                entity.Property(e => e.SampleDrawnBy).HasColumnName("sx_drawn_by");
                entity.Property(e => e.CertId).HasColumnName("cert_id");
                entity.Property(e => e.SampleId).HasColumnName("sample_id");
                entity.Property(e => e.OECDLot).HasColumnName("oecd_lot");
                entity.Property(e => e.Rush).HasColumnName("rush");
                entity.Property(e => e.InDirt).HasColumnName("in_dirt");
                entity.Property(e => e.BlendNumber).HasColumnName("blend_num");
                entity.Property(e => e.DateSampleReceived).HasColumnName("date_sample_recd");
                entity.Property(e => e.CropFee).HasColumnName("crop_fee");
                entity.Property(e => e.CertFee).HasColumnName("cert_fee");
                entity.Property(e => e.ResearchFee).HasColumnName("research_fee");
                entity.Property(e => e.MinimumFee).HasColumnName("min_fee");
                entity.Property(e => e.BillTable).HasColumnName("bill_tbl");
                entity.Property(e => e.LotCertOk).HasColumnName("lot_cert_cert_ok");
                entity.Property(e => e.UserEntered).HasColumnName("user_entered");
                entity.Property(e => e.Submitted).HasColumnName("submitted");
                entity.Property(e => e.Confirmed).HasColumnName("confirmed");
                entity.Property(e => e.ConfirmedAt).HasColumnName("date_confirmed");
                entity.Property(e => e.Docs).HasColumnName("docs");
                entity.Property(e => e.EmployeeModified).HasColumnName("emp_modified");
                entity.Property(e => e.NotFinallyCertified).HasColumnName("not_finally_certified");
                entity.Property(e => e.ChargeFullFees).HasColumnName("charge_full_fees");

                entity.HasOne(d => d.ApplicantOrganization);

                entity.HasOne(v => v.Variety);

                entity.HasOne(s => s.ConditionerOrganization);

                entity.HasOne(s => s.ClassProduced);

                entity.HasOne(s => s.LabResults);

                entity.HasOne(d => d.AppTypeTrans).WithMany(p => p.Seeds).HasForeignKey(d => d.CertProgram).HasPrincipalKey(p => p.Abbreviation);

            });

            modelBuilder.Entity<FieldHistory>(entity =>
            {
                entity.ToTable("field_history");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("history_id");

                entity.Property(e => e.AppId).HasColumnName("app_id");

                entity.Property(e => e.Year);

                entity.Property(e => e.Crop);

                entity.Property(e => e.Variety).HasColumnName("entered_variety");

                entity.Property(e => e.AppNumber).HasColumnName("app_num");

                entity.HasOne(d => d.FHCrops);


            });

            modelBuilder.Entity<SeedTransfers>(entity => 
            {
                entity.ToTable("seed_transfers");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("stid");

                entity.Property(e => e.Type).HasColumnName("transfer_type");

                entity.Property(e => e.OriginatingOrganizationId).HasColumnName("originating_org");

                entity.Property(e => e.OriginatingCountyId).HasColumnName("originating_county");

                entity.Property(e => e.ApplicationId).HasColumnName("app_id");

                entity.Property(e => e.SeedsID).HasColumnName("sid");

                entity.Property(e => e.BlendId).HasColumnName("bid");

                entity.Property(e => e.CertificateDate).HasColumnName("certificate_date");

                entity.Property(e => e.CreatedById).HasColumnName("created_by");

                entity.Property(e => e.CreatedOn).HasColumnName("created_on");

                entity.Property(e => e.Pounds).HasColumnName("transfer_lbs");

                entity.Property(e => e.ClassId).HasColumnName("transfer_class");

                entity.Property(e => e.SeedstockLotNumbers).HasColumnName("seedstock_lot_numbers");

                entity.Property(e => e.SubmittedForAnalysis).HasColumnName("submitted_for_analysis");

                entity.Property(e => e.DestinationOrganizationId).HasColumnName("destination_org");

                 entity.Property(e => e.PurchaserName).HasColumnName("purch_name");

                entity.Property(e => e.PurchaserAddressLine1).HasColumnName("purch_address_line_1");

                entity.Property(e => e.PurchaserAddressLine2).HasColumnName("purch_address_line_2");

                entity.Property(e => e.PurchaserCity).HasColumnName("purch_city");

                entity.Property(e => e.PurchaserCountyId).HasColumnName("purch_county");

                entity.Property(e => e.PurchaserStateId).HasColumnName("purch_state_id");

                entity.Property(e => e.PurchaserCountryId).HasColumnName("purch_country");

                entity.Property(e => e.PurchaserZip).HasColumnName("purch_zip");

                entity.Property(e => e.PurchaserPhone).HasColumnName("purch_phone");

                entity.Property(e => e.PurchaserEmail).HasColumnName("purch_email");

                entity.Property(e => e.StageInDirt).HasColumnName("stage_indirt");

                entity.Property(e => e.StageFromField).HasColumnName("stage_from_field");

                entity.Property(e => e.StageFromFieldNumberOfAcres).HasColumnName("stage_from_field_num_acres");

                entity.Property(e => e.StageFromStorage).HasColumnName("stage_from_storage");

                entity.Property(e => e.StageConditioned).HasColumnName("stage_conditioned");

                entity.Property(e => e.StageNotFinallyCertified).HasColumnName("stage_nfc");

                entity.Property(e => e.StageCertifiedSeed).HasColumnName("stage_certified_seed");

                entity.Property(e => e.StageTreatment).HasColumnName("stage_treatment");

                entity.Property(e => e.StageBagging).HasColumnName("stage_bagging");

                entity.Property(e => e.StageTagging).HasColumnName("stage_tagging");

                entity.Property(e => e.StageBlending).HasColumnName("stage_blending");

                entity.Property(e => e.StageStorage).HasColumnName("stage_storage");

                entity.Property(e => e.StageOther).HasColumnName("stage_other");

                entity.Property(e => e.StageOtherValue).HasColumnName("stage_other_value");

                entity.Property(e => e.TypeRetail).HasColumnName("type_retail");

                entity.Property(e => e.TypeTote).HasColumnName("type_tote");

                entity.Property(e => e.TypeBulk).HasColumnName("type_bulk");

                entity.Property(e => e.NumberOfTrucks).HasColumnName("number_of_trucks");

                entity.Property(e => e.AgricultureCommissionerAccurate).HasColumnName("ag_comm_accurate");

                entity.Property(e => e.AgricultureCommissionerInaccurate).HasColumnName("ag_comm_inaccurate");

                entity.Property(e => e.AgricultureCommissionerApprove).HasColumnName("ag_comm_approve");

                entity.Property(e => e.AgricultureCommissionerDateRespond).HasColumnName("ag_comm_date_respond");

                entity.Property(e => e.AgricultureCommissionerContactRespondId).HasColumnName("contact_respond");

                entity.Property(e => e.AdminUpdatedId).HasColumnName("employee_update_id");

                entity.Property(e => e.AdminUpdatedDate).HasColumnName("employee_update_date");

                entity.Property(e => e.AdminUpdated).HasColumnName("update_by_admin");

                entity.HasOne(e => e.OriginatingOrganization);

                entity.HasOne(e => e.OriginatingCounty);

                entity.HasOne(e => e.Seeds);

                entity.HasOne(e => e.Blend);

                entity.HasOne(e => e.Application);

                entity.HasOne(e => e.CreatedByContact);

                entity.HasOne(e => e.SeedClass);

                entity.HasOne(e => e.AppClass);

                entity.HasOne(e => e.DestinationOrganization);

                entity.HasOne(e => e.PurchaserState);

                entity.HasOne(e => e.PurchaserCounty);

                entity.HasOne(e => e.AgricultureCommissionerContactRespond);

                entity.HasOne(e => e.AdminEmployee);

            });

            modelBuilder.Entity<BulkSalesCertificatesShares>(entity => 
            {
                entity.ToTable("bulk_sales_certificate_shares");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("bsc_share_id");

                entity.Property(e => e.BulkSalesCertificatesId).HasColumnName("bsc_id");

                entity.Property(e => e.ShareOrganizationId).HasColumnName("share_org_id");

                entity.HasOne(e => e.ShareOrganization);

            });

            modelBuilder.Entity<CCIAEmployees>(entity => 
            {
                entity.ToTable("ccia_employees");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("employee_id");

                entity.Property(e => e.FirstName).HasColumnName("first_name");

                entity.Property(e => e.LastName).HasColumnName("last_name");

                entity.Property(e => e.UCDMaildID).HasColumnName("ucd_mailid");

                entity.Property(e => e.CampusRoom).HasColumnName("campus_room");

                entity.Property(e => e.CampusBuilding).HasColumnName("campus_bldg");

                entity.Property(e => e.CampusPhone).HasColumnName("campus_phone");

                entity.Property(e => e.CellPhone).HasColumnName("cell_phone");

                entity.Property(e => e.KerberosId).HasColumnName("kerberos_id");

                entity.Property(e => e.Current).HasColumnName("current");

                entity.Property(e => e.CCIAAccess).HasColumnName("ccia_access");

                entity.Property(e => e.CoreStaff).HasColumnName("core_staff");

                entity.Property(e => e.FieldInspector).HasColumnName("field_inspect");

                entity.Property(e => e.SeedLotInform).HasColumnName("seed_lot_inform");

                entity.Property(e => e.EditVarieties).HasColumnName("edit_varieties");

                entity.Property(e => e.BillingAccess).HasColumnName("billing_access");

                entity.Property(e => e.SeedLab).HasColumnName("seed_lab");

                entity.Property(e => e.SeasonalEmployee).HasColumnName("seasonal_employee");

                entity.Property(e => e.NewTag).HasColumnName("new_tag");

                entity.Property(e => e.TagPrint).HasColumnName("tag_print");

                entity.Property(e => e.HeritageGrainQA).HasColumnName("heritage_grain_qa");

                entity.Property(e => e.PrevarietyGermplasm).HasColumnName("prevariety_germplasm");

                entity.Property(e => e.OECDInvoicePrinter).HasColumnName("oecd_invoice_printer");

            });

            modelBuilder.Entity<BulkSalesCertificates>(entity => 
            {
                entity.ToTable("bulk_sales_certificates");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("Id");

                entity.Property(e => e.ConditionerOrganizationId).HasColumnName("cond_org");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.SeedsID).HasColumnName("sid");

                entity.Property(e => e.BlendId).HasColumnName("bid");

                entity.Property(e => e.CertProgram).HasColumnName("cert_program");

                entity.Property(e => e.PurchaserName).HasColumnName("purch_name");

                entity.Property(e => e.PurchaserAddressLine1).HasColumnName("purch_address_line_1");

                entity.Property(e => e.PurchaserAddressLine2).HasColumnName("purch_address_line_2");

                entity.Property(e => e.PurchaserCity).HasColumnName("purch_city");

                entity.Property(e => e.PurchaserStateId).HasColumnName("purch_state_id");

                entity.Property(e => e.PurchaserCountryId).HasColumnName("purch_country");

                entity.Property(e => e.PurchaserZip).HasColumnName("purch_zip");

                entity.Property(e => e.PurchaserPhone).HasColumnName("purch_phone");

                entity.Property(e => e.PurchaserEmail).HasColumnName("purch_email");

                entity.Property(e => e.Pounds).HasColumnName("cert_lbs");

                entity.Property(e => e.ClassId).HasColumnName("class");

                entity.Property(e => e.CreatedById).HasColumnName("created_by");

                entity.Property(e => e.CreatedOn).HasColumnName("created_on");

                entity.Property(e => e.AdminUpdatedId).HasColumnName("admin_updated");

                entity.Property(e => e.AdminUpdatedDate).HasColumnName("admin_update_date");

                entity.Property(e => e.NotificationSent).HasColumnName("notification_sent");                


                entity.HasOne(e => e.Seeds);

                entity.HasOne(e => e.Blend);

                entity.HasOne(e => e.PurchaserState);

                entity.HasOne(e => e.PurchaserCountry);

                entity.HasOne(e => e.Class);

                entity.HasOne(e => e.CreatedByContact);

                entity.HasOne(e => e.ConditionerOrganization);

                entity.HasOne(e => e.AdminEmployee);

                entity.HasMany(e => e.BulkSalesCertificatesShares);

            });

            modelBuilder.Entity<AbbrevOECDClass>(entity => 
            {
                entity.ToTable("abbrev_oecd_class");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("oecd_class_id");

                entity.Property(e => e.Class).HasColumnName("oecd_trans");

                entity.Property(e => e.SortOrder).HasColumnName("sort_order");

            });

            modelBuilder.Entity<OECD>(entity => 
            {
                entity.ToTable("oecd");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("file_num");

                entity.Property(e => e.SeedsId).HasColumnName("seeds_id");

                entity.Property(e => e.VarietyId).HasColumnName("variety_id");

                entity.Property(e => e.TagId).HasColumnName("tag_id_link");

                entity.Property(e => e.Pounds).HasColumnName("lbs_oecd");

                entity.Property(e => e.CertNumber).HasColumnName("cert_num");

                entity.Property(e => e.OECDNumber).HasColumnName("oecd_num");

                entity.Property(e => e.ClassId).HasColumnName("class");

                entity.Property(e => e.CloseDate).HasColumnName("close_date");

                entity.Property(e => e.ConditionerId).HasColumnName("conditioner_id");

                entity.Property(e => e.CountryId).HasColumnName("country");

                entity.Property(e => e.IssueDate).HasColumnName("issue_date");

                entity.Property(e => e.LotNumber).HasColumnName("lot_num");

                entity.Property(e => e.ShipperId).HasColumnName("shipper_id");

                entity.Property(e => e.DateRequested).HasColumnName("date_requested");

                entity.Property(e => e.TotalFee).HasColumnName("total_fee");

                entity.Property(e => e.NotCertified).HasColumnName("not_cert");

                entity.Property(e => e.DataEntryDate).HasColumnName("data_entry_date");

                entity.Property(e => e.DataEntryUser).HasColumnName("data_entry_user");

                entity.Property(e => e.UpdateDate).HasColumnName("update_date");

                entity.Property(e => e.UpdateUser).HasColumnName("update_user");

                entity.Property(e => e.DomesticOrigin).HasColumnName("domestic_origin");

                entity.Property(e => e.Canceled).HasColumnName("canceled");

                entity.Property(e => e.Comments).HasColumnName("comments");

                entity.Property(e => e.AdminComments).HasColumnName("admin_comments");

                entity.Property(e => e.DatePrinted).HasColumnName("date_printed");

                entity.Property(e => e.ReferenceNumber).HasColumnName("ref_num");

                entity.Property(e => e.USDAReported).HasColumnName("usda_reported");

                entity.Property(e => e.USDAReportDate).HasColumnName("usda_report_date");

                entity.Property(e => e.TagsRequested).HasColumnName("tags_requested");

                entity.Property(e => e.CertificateFee).HasColumnName("certificate_fee");

                entity.Property(e => e.OECDFee).HasColumnName("oecd_fee");

                entity.Property(e => e.NotFinallyCertifiedFee).HasColumnName("nfc_fee");

                entity.Property(e => e.ClientNotified).HasColumnName("client_notified");

                entity.HasOne(e => e.Seeds);

                entity.HasOne(e => e.Class);

                entity.HasOne(e => e.Country);


            });

            modelBuilder.Entity<AbbrevTagType>(entity =>
            {
                entity.ToTable("abbrev_tag_type");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("tag_type_id");

                entity.Property(e => e.TagTypeTrans).HasColumnName("tag_type_trans");

                entity.Property(e => e.SortOrder).HasColumnName("sort_order");

                entity.Property(e => e.StandardTagForm).HasColumnName("standard_tag_form");

                entity.Property(e => e.OECD).HasColumnName("oecd");

                entity.Property(e => e.PotatoTag).HasColumnName("po_tag");



            });
            

            modelBuilder.Entity<BlendInDirtComponents>(entity => 
            {
                entity.ToTable("blend_indirt_components");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("comp_id");

                entity.Property(e => e.BlendId).HasColumnName("bid");

                entity.Property(e => e.AppId).HasColumnName("app_id");

                entity.Property(e => e.Weight).HasColumnName("weight");

                entity.Property(e => e.ApplicantId).HasColumnName("applicant_id");

                entity.Property(e => e.CropId).HasColumnName("crop_id");

                entity.Property(e => e.OfficialVarietyId).HasColumnName("official_variety_id");

                entity.Property(e => e.CertYear).HasColumnName("cert_year");

                entity.Property(e => e.CountryOfOrigin).HasColumnName("country_of_origin");

                entity.Property(e => e.StateOfOrigin).HasColumnName("state_of_origin");

                entity.Property(e => e.CertNumber).HasColumnName("cert_number");

                entity.Property(e => e.LotNumber).HasColumnName("lot_number");

                entity.Property(e => e.Class).HasColumnName("class");

                entity.Property(e => e.LastEditBy).HasColumnName("last_edit_by");

                entity.HasOne(e => e.Application);

                entity.HasOne(e => e.Variety);

                entity.HasOne(e => e.Crop);


            });
            


            modelBuilder.Entity<Tags>(entity =>
            {
                entity.ToTable("tags");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("tag_id");

                entity.Property(e => e.SeedsID).HasColumnName("seeds_id");

                entity.Property(e => e.BlendId).HasColumnName("blend_id");

                entity.Property(e => e.PotatoAppId).HasColumnName("pot_app_id");

                entity.Property(e => e.OECDId).HasColumnName("oecd_file_num");

                entity.Property(e => e.TagClass).HasColumnName("tag_class");

                entity.Property(e => e.DateRequested).HasColumnName("date_requested");

                entity.Property(e => e.DateNeeded).HasColumnName("date_needed");

                entity.Property(e => e.DateRun).HasColumnName("date_run");

                entity.Property(e => e.LotWeightBagged).HasColumnName("lot_weight_bagged");

                entity.Property(e => e.CoatingPercent).HasColumnName("coating_percent");

                entity.Property(e => e.CountRequested).HasColumnName("count_requested");

                entity.Property(e => e.CountUsed).HasColumnName("count_used");
                
                entity.Property(e => e.TagType).HasColumnName("tag_type");

                entity.Property(e => e.ExtrasOverrun).HasColumnName("tag_extras_overrun");

                entity.Property(e => e.Statement).HasColumnName("tag_statement");

                entity.Property(e => e.BagSize).HasColumnName("bag_size");

                entity.Property(e => e.WeightUnit).HasColumnName("weight_unit");

                entity.Property(e => e.Comments).HasColumnName("comments");

                entity.Property(e => e.Contact).HasColumnName("order_contact");

                entity.Property(e => e.UserPrinted).HasColumnName("user_printed");

                entity.Property(e => e.UserEntered).HasColumnName("user_entered");

                entity.Property(e => e.DateEntered).HasColumnName("date_entered");

                entity.Property(e => e.UserModified).HasColumnName("user_modified");

                entity.Property(e => e.DateModified).HasColumnName("date_modified");

                entity.Property(e => e.TaggingOrg).HasColumnName("tagging_org");

                entity.Property(e => e.Bulk).HasColumnName("bulk_request");

                entity.Property(e => e.Pretagging).HasColumnName("pretagging");

                entity.Property(e => e.SeriesNumbered).HasColumnName("series_numbered");

                entity.Property(e => e.AnalysisRequested).HasColumnName("analysis_request");

                entity.Property(e => e.HowDeliver).HasColumnName("how_deliver");

                entity.Property(e => e.TrackingNumber).HasColumnName("tracking_number");

                entity.Property(e => e.Stage).HasColumnName("stage");

                entity.Property(e => e.UserApproved).HasColumnName("user_apporved");

                entity.Property(e => e.ApprovedDate).HasColumnName("approved_date");

                entity.Property(e => e.PrintedDate).HasColumnName("printed_date");

                entity.Property(e => e.Alias).HasColumnName("requested_alias");

                entity.Property(e => e.OECD).HasColumnName("oecd_request");

                entity.Property(e => e.PlantingStockNumber).HasColumnName("ps_number");

                entity.Property(e => e.OECDTagType).HasColumnName("oecd_tag_type");

                entity.Property(e => e.DateSealed).HasColumnName("date_sealed");

                entity.Property(e => e.OECDCountry).HasColumnName("oecd_country");

                entity.Property(e => e.AdminComments).HasColumnName("admin_comments");

                entity.Property(e => e.SeriesRequest).HasColumnName("series_request");

                entity.Property(e => e.BulkCropId).HasColumnName("bulk_crop_id");

                entity.Property(e => e.BulkVarietyId).HasColumnName("bulk_var_off_id");

                entity.HasOne(e => e.Seeds);

                entity.HasOne(e => e.Blend);

                entity.HasOne(e => e.BulkCrop);

                entity.HasOne(e => e.BulkVariety);

                entity.HasOne(e => e.TagAbbrevClass);

                entity.HasOne(e => e.AbbrevTagType);


            });

            modelBuilder.Entity<AppCertificates>(entity =>
           {
               entity.ToTable("app_certificates");

               entity.HasKey(e => e.CertId);

               entity.Property(e => e.CertId).HasColumnName("cert_id");

               entity.Property(e => e.AppId).HasColumnName("app_id");

               entity.Property(e => e.Name).HasColumnName("cert_name");

               entity.Property(e => e.Link).HasColumnName("cert_link");

           });

            modelBuilder.Entity<VarFull>(entity =>
            {
                entity.ToTable("var_full");

                entity.HasKey(v => v.Id);

                entity.Property(v => v.Name).HasColumnName("var_name");

                entity.Property(v => v.Type).HasColumnName("var_type");

                entity.Property(v => v.CropId).HasColumnName("crop_id");

                entity.Property(v => v.Category).HasColumnName("var_category");

                entity.Property(v => v.Status).HasColumnName("var_status");

                //entity.Property(v => v.Table).HasColumnName("tblname");

                entity.Property(v => v.Certified).HasColumnName("ccia_certified");

                //entity.Property(v => v.DateCertified).HasColumnName("date_certified");

                entity.Property(v => v.RiceQa).HasColumnName("rice_qa");

                entity.Property(v => v.RiceColor).HasColumnName("rice_qa_color");

                entity.Property(v => v.ParendId).HasColumnName("parent_id");

                entity.Property(v => v.Turfgrass).HasColumnName("turfgrass");

                entity.HasOne(v => v.Crop);


            });
            modelBuilder.Entity<AbbrevAppType>(entity =>
        {
            entity.HasKey(e => e.AppTypeId);

            entity.ToTable("abbrev_app_type");

            entity.Property(e => e.AppTypeId).HasColumnName("app_type_id");

            entity.Property(e => e.Abbreviation)
                .HasColumnName("abbreviation")
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.AppTypeTrans)
                .HasColumnName("app_type_trans")
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.Property(e => e.CertificateTitle)
                .HasColumnName("certificate_title")
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.Property(e => e.NumberTitle)
                .HasColumnName("number_title")
                .HasMaxLength(500)
                .IsUnicode(false);

            entity.Property(e => e.ProcessTitle)
                .HasColumnName("process_title")
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.ProducedTitle)
                .HasColumnName("produced_title")
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.ShowType).HasColumnName("show_type");

            entity.Property(e => e.SirTitle)
                .HasColumnName("sir_title")
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.SpeciesOrCrop)
                .HasColumnName("species_or_crop")
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.Property(e => e.VarietyTitle)
                .HasColumnName("variety_title")
                .HasMaxLength(50)
                .IsUnicode(false);
        });

            modelBuilder.Entity<AbbrevClassProduced>(entity =>
            {
                entity.HasKey(e => e.ClassProducedId);

                entity.ToTable("abbrev_class_produced");

                entity.Property(e => e.ClassProducedId)
                    .HasColumnName("class_produced_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.AppType).HasColumnName("app_type");

                entity.Property(e => e.ClassAbbrv)
                    .HasColumnName("class_abbrv")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.ClassProducedTrans)
                    .HasColumnName("class_produced_trans")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SortOrder).HasColumnName("sort_order");
            });

            modelBuilder.Entity<AbbrevClassSeeds>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("abbrev_class_seeds");

                entity.Property(e => e.Id)
                    .HasColumnName("class_certified_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Abbrv)
                    .HasColumnName("class_abbrv")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Class)
                    .HasColumnName("class_certified_trans")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SortOrder).HasColumnName("sort_order");
            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("address");

                entity.Property(e => e.AddressId).HasColumnName("address_id");

                entity.Property(e => e.Address1)
                    .IsRequired()
                    .HasColumnName("address1")
                    .HasMaxLength(100);

                entity.Property(e => e.Address2)
                    .HasColumnName("address2")
                    .HasMaxLength(100);

                entity.Property(e => e.Address3)
                    .HasColumnName("address3")
                    .HasMaxLength(100);

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasMaxLength(250);

                entity.Property(e => e.CountryId).HasColumnName("country_id");

                entity.Property(e => e.CountyId).HasColumnName("county_id");

                entity.Property(e => e.DateModified)
                    .HasColumnName("date_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.OcId).HasColumnName("oc_id");

                entity.Property(e => e.PostalCode)
                    .HasColumnName("postal_code")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ReferenceId).HasColumnName("reference_id");

                entity.Property(e => e.StateProvinceId).HasColumnName("state_province_id");

                entity.Property(e => e.UserModified)
                    .HasColumnName("user_modified")
                    .HasMaxLength(9)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Applications>(entity =>
            {
                entity.HasKey(e => e.AppId);

                entity.ToTable("applications");

                entity.HasIndex(e => new { e.AppId, e.AppType, e.ApplicantId, e.GrowerId, e.CropId, e.AppCancelled, e.Tags, e.PoLotNum, e.FieldName, e.FarmCounty, e.DatePlanted, e.AcresApplied, e.SelectedVarietyId, e.ClassProducedId, e.AppSubmitable, e.Status, e.AppApproved, e.Maps, e.CertYear })
                    .HasName("IX_applications_cert_year");

                entity.Property(e => e.AppId).HasColumnName("app_id");

                entity.Property(e => e.AcresApplied)
                    .HasColumnName("acres_applied")
                    .HasColumnType("decimal(14, 2)");

                entity.Property(e => e.AppApproved)
                    .HasColumnName("app_approved")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.AppApprover)
                    .HasColumnName("app_approver")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AppCancelled)
                    .HasColumnName("app_cancelled")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.AppCancelledBy)
                    .HasColumnName("app_cancelled_by")
                    .HasMaxLength(9)
                    .IsUnicode(false);

                entity.Property(e => e.AppCompleteDt)
                    .HasColumnName("app_complete_dt")
                    .HasColumnType("datetime");

                entity.Property(e => e.AppDateAppr)
                    .HasColumnName("app_date_appr")
                    .HasColumnType("datetime");

                entity.Property(e => e.AppDateDenied)
                    .HasColumnName("app_date_denied")
                    .HasColumnType("datetime");

                entity.Property(e => e.AppDeadline)
                    .HasColumnName("app_deadline")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.AppDenied)
                    .HasColumnName("app_denied")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.AppFee)
                    .HasColumnName("app_fee")
                    .HasColumnType("smallmoney");

                entity.Property(e => e.AppOriginalCertYear).HasColumnName("app_original_cert_year");

                entity.Property(e => e.AppPkgComplete)
                    .HasColumnName("app_pkg_complete")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.AppPostmark)
                    .HasColumnName("app_postmark")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.AppReceived)
                    .HasColumnName("app_received")
                    .HasColumnType("datetime");

                entity.Property(e => e.AppRejector)
                    .HasColumnName("app_rejector")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AppSubmitable)
                    .HasColumnName("app_submitable")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.AppType)
                    .HasColumnName("app_type")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.ApplicantComments)
                    .HasColumnName("applicant_comments")
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.ApplicantId).HasColumnName("applicant_id");

                entity.Property(e => e.ApplicantNotes)
                    .HasColumnName("applicant_notes")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Billable)
                    .HasColumnName("billable")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CertNum).HasColumnName("cert_num");

                entity.Property(e => e.CertYear).HasColumnName("cert_year");

                entity.Property(e => e.Charged)
                    .HasColumnName("charged")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ClassProducedAccession).HasColumnName("class_produced_accession");

                entity.Property(e => e.ClassProducedId).HasColumnName("class_produced_id");

                entity.Property(e => e.Comments)
                    .HasColumnName("comments")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.CropId).HasColumnName("crop_id");

                entity.Property(e => e.DateNotified)
                    .HasColumnName("date_notified")
                    .HasColumnType("datetime");

                entity.Property(e => e.DatePlanted)
                    .HasColumnName("date_planted")
                    .HasColumnType("datetime");

                entity.Property(e => e.Ecoregion)
                    .HasColumnName("ecoregion")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.EnteredVariety)
                    .HasColumnName("entered_variety")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FarmCounty).HasColumnName("farm_county");

                entity.Property(e => e.FeeCofactor)
                    .HasColumnName("fee_cofactor")
                    .HasColumnType("decimal(5, 4)")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.FieldElevation)
                    .HasColumnName("field_elevation")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FieldHardiness)
                    .HasColumnName("field_hardiness")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FieldId).HasColumnName("field_id");

                entity.Property(e => e.FieldName)
                    .HasColumnName("field_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.GeoTextField)
                    .HasColumnName("geo_text_field")
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.GrowerId).HasColumnName("grower_id");

                entity.Property(e => e.IncompleteFee)
                    .HasColumnName("incomplete_fee")
                    .HasColumnType("smallmoney");

                entity.Property(e => e.LateFee)
                    .HasColumnName("late_fee")
                    .HasColumnType("smallmoney");

                entity.Property(e => e.LotNo)
                    .HasColumnName("lot_no")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MapCenterLat)
                    .HasColumnName("map_center_lat")
                    .HasColumnType("numeric(25, 15)");

                entity.Property(e => e.MapCenterLong)
                    .HasColumnName("map_center_long")
                    .HasColumnType("numeric(25, 15)");

                entity.Property(e => e.MapUploadFile)
                    .HasColumnName("map_upload_file")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MapVe)
                    .HasColumnName("map_ve")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.MapZoom).HasColumnName("map_zoom");

                entity.Property(e => e.Maps)
                    .HasColumnName("maps")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.MapsSubDt)
                    .HasColumnName("maps_sub_dt")
                    .HasColumnType("datetime");                

                entity.Property(e => e.NotifyDate)
                    .HasColumnName("notify_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.NotifyNeeded)
                    .HasColumnName("notify_needed")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.OverrideLateFee)
                    .HasColumnName("override_late_fee")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PaperAppNum).HasColumnName("paper_app_num");

                entity.Property(e => e.PoLotNum)
                    .HasColumnName("po_lot_num")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PvgSelectionId)
                    .HasColumnName("pvg_selectionId")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PvgSource)
                    .HasColumnName("pvg_source")
                    .HasMaxLength(50)
                    .IsUnicode(false);               

                entity.Property(e => e.Renewal)
                    .HasColumnName("renewal")
                    .HasDefaultValueSql("((0))");

               
                entity.Property(e => e.SelectedVarietyId).HasColumnName("selected_variety_id");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Tags)
                    .HasColumnName("tags")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TextField)
                    .HasColumnName("text_field")
                    .HasMaxLength(3000)
                    .IsUnicode(false);               

                entity.Property(e => e.Trace).HasColumnName("trace");

                entity.Property(e => e.UserAppDataentry).HasColumnName("user_app_dataentry");

                entity.Property(e => e.UserAppModDt)
                    .HasColumnName("user_app_mod_dt")
                    .HasColumnType("datetime");

                entity.Property(e => e.UserAppModifed).HasColumnName("user_app_modifed");

                entity.Property(e => e.UserEmpDateMod)
                    .HasColumnName("user_emp_date_mod")
                    .HasColumnType("datetime");

                entity.Property(e => e.UserEmpModified)
                    .HasColumnName("user_emp_modified")
                    .HasMaxLength(9)
                    .IsUnicode(false);

                entity.Property(e => e.WarningFlag)
                    .HasColumnName("warning_flag")
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.ClassProduced);

                entity.HasOne(d => d.Crop)
                    .WithMany(p => p.Applications)
                    .HasForeignKey(d => d.CropId)
                    .HasConstraintName("FK_Applications_Crops");

                entity.HasOne(d => d.GrowerOrganization);

                entity.HasOne(d => d.ApplicantOrganization)
                    .WithMany(p => p.AppliedApplications)
                    .HasForeignKey(d => d.ApplicantId);

                entity.HasOne(d => d.TraceNavigation)
                    .WithMany(p => p.InverseTraceNavigation)
                    .HasForeignKey(d => d.Trace)
                    .HasConstraintName("FK_Applications_Applications2");

                entity.HasOne(d => d.County);

                entity.HasOne(d => d.Variety);

                entity.HasOne(d => d.AppTypeTrans).WithMany(p => p.Applications).HasForeignKey(d => d.AppType).HasPrincipalKey(p => p.Abbreviation);

                entity.HasMany(d => d.Certificates);

                entity.HasMany(d => d.PlantingStocks);

                entity.HasMany(d => d.FieldHistories);

            });

            modelBuilder.Entity<BlendRequests>(entity =>
            {
                entity.HasKey(e => e.BlendId);

                entity.ToTable("blend_requests");

                entity.Property(e => e.BlendId).HasColumnName("blend_id");

                entity.Property(e => e.ApproveDate)
                    .HasColumnName("approve_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Approved)
                    .HasColumnName("approved")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ApprovedBy)
                    .HasColumnName("approved_by")
                    .HasMaxLength(9)
                    .IsUnicode(false);

                entity.Property(e => e.BlendType)
                    .IsRequired()
                    .HasColumnName("blend_type")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Class).HasColumnName("class");

                entity.Property(e => e.Comments)
                    .HasColumnName("comments")
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.ConditionerId).HasColumnName("conditioner_id");

                entity.Property(e => e.DateNeeded)
                    .HasColumnName("date_needed")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateSubmitted)
                    .HasColumnName("date_submitted")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeliveryAddress)
                    .HasColumnName("delivery_address")
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.HowDeliver)
                    .HasColumnName("how_deliver")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LbsLot)
                    .HasColumnName("lbs_lot")
                    .HasColumnType("numeric(16, 2)");

                entity.Property(e => e.RequestStarted)
                    .HasColumnName("request_started")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Submitted)
                    .HasColumnName("submitted")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TagCountRequested).HasColumnName("tag_count_requested");

                entity.Property(e => e.TagType).HasColumnName("tag_type");

                entity.Property(e => e.UserEntered).HasColumnName("user_entered");

                entity.Property(e => e.VarietyId).HasColumnName("variety");

                entity.HasMany(e => e.LotBlends);

                entity.HasMany(e => e.InDirtBlends);

                entity.HasOne(e => e.Variety);
            });

            modelBuilder.Entity<CertRad>(entity =>
            {
                entity.HasKey(e => new { e.CertNum, e.CertYear, e.Rad })
                    .ForSqlServerIsClustered(false);

                entity.ToTable("cert_rad");

                entity.Property(e => e.CertNum).HasColumnName("cert_num");

                entity.Property(e => e.CertYear).HasColumnName("cert_year");

                entity.Property(e => e.Rad).HasColumnName("rad");
            });

            modelBuilder.Entity<Certs>(entity =>
            {
                entity.HasKey(e => e.CertNum);

                entity.ToTable("certs");

                entity.Property(e => e.CertNum).HasColumnName("cert_num");

                entity.Property(e => e.CertYear).HasColumnName("cert_year");

                entity.Property(e => e.ClassProduced).HasColumnName("class_produced");

                entity.Property(e => e.ClassProducedChar)
                    .HasColumnName("class_produced_char")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.DateEntered)
                    .HasColumnName("date_entered")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateModified)
                    .HasColumnName("date_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.OfficialVarietyId).HasColumnName("official_variety_id");

                entity.Property(e => e.OrgId).HasColumnName("org_id");
            });

            modelBuilder.Entity<ChangeRequests>(entity =>
            {
                entity.HasKey(e => e.ChangeId)
                    .ForSqlServerIsClustered(false);

                entity.ToTable("change_requests");

                entity.Property(e => e.ChangeId).HasColumnName("change_id");

                entity.Property(e => e.Completed)
                    .HasColumnName("completed")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CompletedDate)
                    .HasColumnName("completed_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Priority).HasColumnName("priority");

                entity.Property(e => e.SubmitDate)
                    .HasColumnName("submit_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Text)
                    .HasColumnName("text")
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Charges>(entity =>
            {
                entity.HasKey(e => e.ChgId);

                entity.ToTable("charges");

                entity.Property(e => e.ChgId).HasColumnName("chg_id");

                entity.Property(e => e.Approval)
                    .HasColumnName("approval")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Approver)
                    .HasColumnName("approver")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Batchno)
                    .HasColumnName("batchno")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.ChargeAmt)
                    .HasColumnName("charge_amt")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.ChgCategory)
                    .IsRequired()
                    .HasColumnName("chg_category")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Correction)
                    .HasColumnName("correction")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.DateApplied)
                    .HasColumnName("date_applied")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateEntered)
                    .HasColumnName("date_entered")
                    .HasColumnType("datetime");

                entity.Property(e => e.Delcharge)
                    .HasColumnName("delcharge")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Holdchk)
                    .HasColumnName("holdchk")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Holddt)
                    .HasColumnName("holddt")
                    .HasColumnType("datetime");

                entity.Property(e => e.Holdinit)
                    .HasColumnName("holdinit")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LinkId).HasColumnName("link_id");

                entity.Property(e => e.LinkType)
                    .IsRequired()
                    .HasColumnName("link_type")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Note)
                    .HasColumnName("note")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.OrgId).HasColumnName("org_id");
            });

            modelBuilder.Entity<CondStatus>(entity =>
            {
                entity.ToTable("cond_status");

                entity.Property(e => e.CondStatusId).HasColumnName("cond_status_id");

                entity.Property(e => e.AllowPretag)
                    .HasColumnName("allow_pretag")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CondStatus1)
                    .IsRequired()
                    .HasColumnName("cond_status")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.CondUpdate)
                    .HasColumnName("cond_update")
                    .HasColumnType("datetime");

                entity.Property(e => e.CondYear).HasColumnName("cond_year");

                entity.Property(e => e.DatePretagApproved)
                    .HasColumnName("date_pretag_approved")
                    .HasColumnType("datetime");

                entity.Property(e => e.OrgId).HasColumnName("org_id");

                entity.Property(e => e.PrintSeries)
                    .HasColumnName("print_series")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.RequestCciaPrintSeries)
                    .HasColumnName("request_ccia_print_series")
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<ContactAddress>(entity =>
            {
                entity.HasKey(e => new { e.ContactId, e.AddressId });

                entity.ToTable("contact_address");

                entity.Property(e => e.ContactId).HasColumnName("contact_id");

                entity.Property(e => e.AddressId).HasColumnName("address_id");

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Billing)
                    .HasColumnName("billing")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ContaddId)
                    .HasColumnName("contadd_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.DateModified)
                    .HasColumnName("date_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.Delivery)
                    .HasColumnName("delivery")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Mailing)
                    .HasColumnName("mailing")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PhysicalLoc)
                    .HasColumnName("physical_loc")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.UserModified)
                    .HasColumnName("user_modified")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Organizations>(entity =>
            {
                entity.HasKey(e => e.OrgId);

                entity.ToTable("Organization");

                entity.Property(e => e.OrgId).HasColumnName("org_id");

                entity.Property(e => e.OrgName).HasColumnName("org_name");

                entity.Property(e => e.AddressId).HasColumnName("address_id");
            });

            modelBuilder.Entity<Contacts>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("contacts");

                entity.Property(e => e.Id).HasColumnName("contact_id");

                 entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasMaxLength(50);

                entity.Property(e => e.FormOfAddr)
                    .HasColumnName("form_of_addr")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.OrgId).HasColumnName("org_id");
                
                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(50)
                    .IsUnicode(false);
                   
                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.AgCommissioner)
                    .HasColumnName("ag_commissioner")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.AlfalfaLastYearAgreement)
                    .HasColumnName("alfalfa_last_year_agreement")
                    .HasDefaultValueSql("((2000))");

                entity.Property(e => e.AllowApps)
                    .HasColumnName("allow_apps")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.AllowPinning)
                    .HasColumnName("allow_pinning")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.AllowSeeds)
                    .HasColumnName("allow_seeds")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.AuditNotify)
                    .HasColumnName("audit_notify")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.BoardActive)
                    .HasColumnName("board_active")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.BoardMember)
                    .HasColumnName("board_member")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.BoardRepresent)
                    .HasColumnName("board_represent")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BoardTitle)
                    .HasColumnName("board_title")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BusPhone)
                    .HasColumnName("bus_phone")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.BusPhoneExt)
                    .HasColumnName("bus_phone_ext")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CciaMember)
                    .HasColumnName("ccia_member")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CciaMemberYear).HasColumnName("ccia_member_year");

                entity.Property(e => e.CertifiedSeedSx)
                    .HasColumnName("certified_seed_sx")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CertifiedSeedSxNo)
                    .HasColumnName("certified_seed_sx_no")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Comments)
                    .HasColumnName("comments")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ContactType)
                    .HasColumnName("contact_type")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreateApps)
                    .HasColumnName("create_apps")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CurrentYearReview)
                    .HasColumnName("current_year_review")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.DateAdded)
                    .HasColumnName("date_added")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateModified)
                    .HasColumnName("date_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.DeputyCommissioner)
                    .HasColumnName("deputy_commissioner")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.EmailAddr)
                    .HasColumnName("email_addr")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FarmAdvisor)
                    .HasColumnName("farm_advisor")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.FaxNo)
                    .HasColumnName("fax_no")
                    .HasMaxLength(30)
                    .IsUnicode(false);

               

                entity.Property(e => e.HomePhone)
                    .HasColumnName("home_phone")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.IdahoVegetableLastYearAgreement).HasColumnName("idaho_vegetable_last_year_agreement");

                entity.Property(e => e.LabContact)
                    .HasColumnName("lab_contact")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasMaxLength(50);

                entity.Property(e => e.MailListGrBook)
                    .HasColumnName("mail_list_gr_book")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.MailListSeednotes)
                    .HasColumnName("mail_list_seednotes")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.MemberSince)
                    .HasColumnName("member_since")
                    .HasColumnType("datetime");

                entity.Property(e => e.Mi)
                    .HasColumnName("mi")
                    .HasColumnType("char(1)");

                entity.Property(e => e.MobilePhone)
                    .HasColumnName("mobile_phone")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                

                entity.Property(e => e.PagerNo)
                    .HasColumnName("pager_no")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Suffix)
                    .HasColumnName("suffix")
                    .HasMaxLength(50);

                entity.Property(e => e.SweetCornLastYearAgreement)
                    .HasColumnName("sweet_corn_last_year_agreement")
                    .HasDefaultValueSql("((2000))");

                

                entity.Property(e => e.UserAdding).HasColumnName("user_adding");

                entity.Property(e => e.UserEmpModDt)
                    .HasColumnName("user_emp_mod_dt")
                    .HasColumnType("datetime");

                entity.Property(e => e.UserEmpModified)
                    .HasColumnName("user_emp_modified")
                    .HasMaxLength(9)
                    .IsUnicode(false);

                entity.Property(e => e.UserModified).HasColumnName("user_modified");
            });

            modelBuilder.Entity<ContactToOrg>(entity =>
            {
                entity.HasKey(e => e.ContOrgId);

                entity.ToTable("contact_to_org");

                entity.Property(e => e.ContOrgId).HasColumnName("cont_org_id");

                entity.Property(e => e.Comments)
                    .HasColumnName("comments")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ContactId).HasColumnName("contact_id");

                entity.Property(e => e.OrgId)
                    .HasColumnName("org_id")
                    .HasColumnType("nchar(10)");
            });

            modelBuilder.Entity<Countries>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("countries");

                entity.Property(e => e.Id).HasColumnName("country_id");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasColumnName("country_code")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("country_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);                

                entity.Property(e => e.OecdMember)
                    .HasColumnName("oecd_member")
                    .HasDefaultValueSql("((0))");                
            });

            modelBuilder.Entity<County>(entity =>
            {
                entity.ToTable("county");

                entity.Property(e => e.CountyId).HasColumnName("county_id");

                entity.Property(e => e.AgCommOrg).HasColumnName("ag_comm_org");

                entity.Property(e => e.CountyName)
                    .IsRequired()
                    .HasColumnName("county_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DateModified)
                    .HasColumnName("date_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.District)
                    .HasColumnName("district")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.Fips)
                    .HasColumnName("fips")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.StateProvinceId).HasColumnName("StateProvinceID");

                entity.Property(e => e.UserModified)
                    .HasColumnName("user_modified")
                    .HasMaxLength(9)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Crops>(entity =>
            {
                entity.HasKey(e => e.CropId);

                entity.ToTable("crops");

                entity.Property(e => e.CropId).HasColumnName("crop_id");

                entity.Property(e => e.Annual)
                    .HasColumnName("annual")
                    .HasColumnType("char(1)");

                entity.Property(e => e.AppDue).HasColumnName("app_due");


                entity.Property(e => e.CertifiedCrop)
                    .HasColumnName("certified_crop")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Crop)
                    .HasColumnName("crop")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CropKind)
                    .HasColumnName("crop_kind")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CropRenewYears).HasColumnName("crop_renew_years");

                entity.Property(e => e.DateEntered)
                    .HasColumnName("date_entered")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateModified)
                    .HasColumnName("date_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.Fov4Map)
                    .HasColumnName("fov4_map")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Genus)
                    .IsRequired()
                    .HasColumnName("genus")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Heritage)
                    .HasColumnName("heritage")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IdahoCropName)
                    .HasColumnName("idaho_crop_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdahoFieldType)
                    .HasColumnName("idaho_field_type")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdahoVegetable)
                    .HasColumnName("idaho_vegetable")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IsolationCertified)
                    .HasColumnName("isolation_certified")
                    .HasColumnType("numeric(10, 1)");

                entity.Property(e => e.IsolationCrop)
                    .HasColumnName("isolation_crop")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.IsolationFoundation)
                    .HasColumnName("isolation_foundation")
                    .HasColumnType("numeric(10, 1)");

                entity.Property(e => e.IsolationHybrid)
                    .HasColumnName("isolation_hybrid")
                    .HasColumnType("numeric(10, 1)");

                entity.Property(e => e.IsolationParentA)
                    .HasColumnName("isolation_parent_a")
                    .HasColumnType("numeric(10, 1)");

                entity.Property(e => e.IsolationParentB)
                    .HasColumnName("isolation_parent_b")
                    .HasColumnType("numeric(10, 1)");

                entity.Property(e => e.IsolationParentR)
                    .HasColumnName("isolation_parent_r")
                    .HasColumnType("numeric(10, 1)");

                entity.Property(e => e.IsolationRegistered)
                    .HasColumnName("isolation_registered")
                    .HasColumnType("numeric(10, 1)");

                entity.Property(e => e.PreVarietyGermplasm)
                    .HasColumnName("pre_variety_germplasm")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.QbClass)
                    .HasColumnName("qb_class")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.QbInvitem)
                    .HasColumnName("qb_invitem")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ReportGroup)
                    .HasColumnName("report_group")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Species)
                    .IsRequired()
                    .HasColumnName("species")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserEntered)
                    .HasColumnName("user_entered")
                    .HasMaxLength(9)
                    .IsUnicode(false);

                entity.Property(e => e.UserModified)
                    .HasColumnName("user_modified")
                    .HasMaxLength(9)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CropsRates>(entity =>
            {
                entity.HasKey(e => e.CropRateId);

                entity.ToTable("crops_rates");

                entity.Property(e => e.CropRateId).HasColumnName("crop_rate_id");

                entity.Property(e => e.CropId).HasColumnName("crop_id");

                entity.Property(e => e.DateSet)
                    .HasColumnName("date_set")
                    .HasColumnType("datetime");

                entity.Property(e => e.RateId).HasColumnName("rate_id");

                entity.Property(e => e.SubType)
                    .HasColumnName("sub_type")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CropStandards>(entity =>
            {
                entity.HasKey(e => new { e.CropId, e.StdId });

                entity.ToTable("crop_standards");

                entity.Property(e => e.CropId).HasColumnName("crop_id");

                entity.Property(e => e.StdId).HasColumnName("std_id");
            });

            modelBuilder.Entity<DistrictCounty>(entity =>
            {
                entity.HasKey(e => e.DistCountyId);

                entity.ToTable("district_county");

                entity.Property(e => e.DistCountyId).HasColumnName("dist_county_id");

                entity.Property(e => e.Comments)
                    .HasColumnName("comments")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CountyId).HasColumnName("county_id");

                entity.Property(e => e.DateEntered)
                    .HasColumnName("date_entered")
                    .HasColumnType("datetime");

                entity.Property(e => e.DistId).HasColumnName("dist_id");

                entity.Property(e => e.UserModified)
                    .HasColumnName("user_modified")
                    .HasMaxLength(9)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Districts>(entity =>
            {
                entity.HasKey(e => e.DistId);

                entity.ToTable("districts");

                entity.Property(e => e.DistId).HasColumnName("dist_id");

                entity.Property(e => e.Comments)
                    .HasColumnName("comments")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.DistCode)
                    .HasColumnName("dist_code")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.DistLeader).HasColumnName("dist_leader");

                entity.Property(e => e.DistName)
                    .HasColumnName("dist_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DistOffice).HasColumnName("dist_office");
            });

            modelBuilder.Entity<Fees>(entity =>
            {
                entity.HasKey(e => e.FeeId);

                entity.ToTable("fees");

                entity.Property(e => e.FeeId)
                    .HasColumnName("fee_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.DateModified)
                    .HasColumnName("date_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.FeeAmount)
                    .HasColumnName("fee_amount")
                    .HasColumnType("money");

                entity.Property(e => e.FeeCategory)
                    .HasColumnName("fee_category")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Item)
                    .HasColumnName("item")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.ItemId).HasColumnName("item_id");

                entity.Property(e => e.LinkType)
                    .HasColumnName("link_type")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Unit)
                    .HasColumnName("unit")
                    .HasColumnType("decimal(8, 2)");

                entity.Property(e => e.UserModified)
                    .HasColumnName("user_modified")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FieldInspect>(entity =>
            {
                entity.HasKey(e => e.FldinspId);

                entity.ToTable("field_inspect");

                entity.HasIndex(e => new { e.AcresApproved, e.AppId })
                    .HasName("IX_field_inpect_app_id");

                entity.Property(e => e.FldinspId).HasColumnName("fldinsp_id");

                entity.Property(e => e.AcresApproved)
                    .HasColumnName("acres_approved")
                    .HasColumnType("decimal(14, 2)");

                entity.Property(e => e.AcresCancelled)
                    .HasColumnName("acres_cancelled")
                    .HasColumnType("decimal(14, 2)");

                entity.Property(e => e.AcresFld)
                    .HasColumnName("acres_fld")
                    .HasColumnType("decimal(14, 2)");

                entity.Property(e => e.AcresGrowout)
                    .HasColumnName("acres_growout")
                    .HasColumnType("decimal(14, 2)");

                entity.Property(e => e.AcresInspOnly)
                    .HasColumnName("acres_insp_only")
                    .HasColumnType("decimal(14, 2)");

                entity.Property(e => e.AcresNoCrop)
                    .HasColumnName("acres_no_crop")
                    .HasColumnType("decimal(14, 2)");

                entity.Property(e => e.AcresRefund)
                    .HasColumnName("acres_refund")
                    .HasColumnType("decimal(14, 2)");

                entity.Property(e => e.AcresRejected)
                    .HasColumnName("acres_rejected")
                    .HasColumnType("decimal(14, 2)");

                entity.Property(e => e.AppId).HasColumnName("app_id");

                entity.Property(e => e.Charged)
                    .HasColumnName("charged")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Complete)
                    .HasColumnName("complete")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CompleteBy)
                    .HasColumnName("complete_by")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DateComplete)
                    .HasColumnName("date_complete")
                    .HasColumnType("date");

                entity.Property(e => e.DateEntered)
                    .HasColumnName("date_entered")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateFldRpt)
                    .HasColumnName("date_fld_rpt")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateModified)
                    .HasColumnName("date_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.FldInspectComments)
                    .HasColumnName("fld_inspect_comments")
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.OldCountyId).HasColumnName("old_county_id");

                entity.Property(e => e.OldFieldName)
                    .HasColumnName("old_field_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PassClass)
                    .HasColumnName("pass_class")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Passed)
                    .HasColumnName("passed")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PathCms)
                    .HasColumnName("path_cms")
                    .HasDefaultValueSql("((255))");

                entity.Property(e => e.PathComments)
                    .HasColumnName("path_comments")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.PathDate)
                    .HasColumnName("path_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.PathErw)
                    .HasColumnName("path_erw")
                    .HasDefaultValueSql("((255))");

                entity.Property(e => e.PathLab)
                    .HasColumnName("path_lab")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PathNumPlants).HasColumnName("path_num_plants");

                entity.Property(e => e.PathNumSamples).HasColumnName("path_num_samples");

                entity.Property(e => e.PathPlrv)
                    .HasColumnName("path_plrv")
                    .HasDefaultValueSql("((255))");

                entity.Property(e => e.PathPstvd)
                    .HasColumnName("path_pstvd")
                    .HasDefaultValueSql("((255))");

                entity.Property(e => e.PathPva)
                    .HasColumnName("path_pva")
                    .HasDefaultValueSql("((255))");

                entity.Property(e => e.PathPvm)
                    .HasColumnName("path_pvm")
                    .HasDefaultValueSql("((255))");

                entity.Property(e => e.PathPvs)
                    .HasColumnName("path_pvs")
                    .HasDefaultValueSql("((255))");

                entity.Property(e => e.PathPvx)
                    .HasColumnName("path_pvx")
                    .HasDefaultValueSql("((255))");

                entity.Property(e => e.PathPvy)
                    .HasColumnName("path_pvy")
                    .HasDefaultValueSql("((255))");

                entity.Property(e => e.PathSentVia)
                    .HasColumnName("path_sent_via")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ReportGenDt)
                    .HasColumnName("report_gen_dt")
                    .HasColumnType("datetime");

                entity.Property(e => e.ReportGenerated)
                    .HasColumnName("report_generated")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.UserEntered)
                    .HasColumnName("user_entered")
                    .HasMaxLength(9)
                    .IsUnicode(false);

                entity.Property(e => e.UserModified)
                    .HasColumnName("user_modified")
                    .HasMaxLength(9)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FieldMaps>(entity =>
            {
                entity.HasKey(e => e.MapptId);

                entity.ToTable("field_maps");

                entity.Property(e => e.MapptId).HasColumnName("mappt_id");

                entity.Property(e => e.AppId).HasColumnName("app_id");

                entity.Property(e => e.Latitude)
                    .HasColumnName("latitude")
                    .HasColumnType("numeric(25, 15)");

                entity.Property(e => e.Longitude)
                    .HasColumnName("longitude")
                    .HasColumnType("numeric(25, 15)");
            });

            modelBuilder.Entity<LotBlends>(entity =>
            {
                entity.HasKey(e => e.CompId);

                entity.ToTable("lot_blends");

                entity.Property(e => e.CompId).HasColumnName("comp_id");

                entity.Property(e => e.BlendId).HasColumnName("blend_id");

                entity.Property(e => e.Sid).HasColumnName("sid");

                entity.Property(e => e.Weight)
                    .HasColumnName("weight")
                    .HasColumnType("numeric(16, 2)");

                entity.HasOne(e => e.Seeds);
            });

            modelBuilder.Entity<PlantingStocks>(entity =>
            {
                entity.HasKey(e => e.PsId);

                entity.ToTable("planting_stocks");

                entity.Property(e => e.PsId).HasColumnName("ps_id");

                entity.Property(e => e.AppId).HasColumnName("app_id");

                entity.Property(e => e.DateEntered)
                    .HasColumnName("date_entered")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateModified)
                    .HasColumnName("date_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.OfficialVarietyId).HasColumnName("official_variety_id");

                entity.Property(e => e.PoundsPlanted)
                    .HasColumnName("pounds_planted")
                    .HasColumnType("decimal(14, 2)");

                entity.Property(e => e.PsAccession).HasColumnName("ps_accession");

                entity.Property(e => e.PsCertNum)
                    .HasColumnName("ps_cert_num")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PsClass).HasColumnName("ps_class");

                entity.Property(e => e.PsEnteredVariety)
                    .HasColumnName("ps_entered_variety")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PvxTest)
                    .HasColumnName("PVX_test")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.SeedPurchasedFrom)
                    .HasColumnName("seed_purchased_from")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.StateCountryGrown).HasColumnName("state_country_grown");

                entity.Property(e => e.StateCountryTagIssued).HasColumnName("state_country_tag_issued");

                entity.Property(e => e.UserCreator).HasColumnName("user_creator");

                entity.Property(e => e.UserModified)
                    .HasColumnName("user_modified")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.WinterTest)
                    .HasColumnName("winter_test")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ThcPercent).HasColumnName("thc_percent");

                entity.Property(e => e.PlantsPerAcre).HasColumnName("plants_per_acre");

                entity.HasOne(d => d.PsClassNavigation)
                    .WithMany(p => p.PlantingStocks)
                    .HasForeignKey(d => d.PsClass)
                    .HasConstraintName("FK_planting_stocks_farm_field");

                entity.HasOne(d => d.GrownStateProvince);
                entity.HasOne(d => d.TaggedStateProvince);

            });

            modelBuilder.Entity<Rates>(entity =>
            {
                entity.HasKey(e => e.RateId)
                    .ForSqlServerIsClustered(false);

                entity.ToTable("rates");

                entity.Property(e => e.RateId).HasColumnName("rate_id");

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Comments)
                    .HasColumnName("comments")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Cost)
                    .HasColumnName("cost")
                    .HasColumnType("money");

                entity.Property(e => e.Item)
                    .HasColumnName("item")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Modified)
                    .HasColumnName("modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Unit)
                    .HasColumnName("unit")
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<StateProvince>(entity =>
            {
                entity.ToTable("state_province");

                entity.Property(e => e.StateProvinceId).HasColumnName("StateProvinceID");

                entity.Property(e => e.CountryId).HasColumnName("country_id");

                entity.Property(e => e.DateModified)
                    .HasColumnName("date_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.StateProvinceCode).HasMaxLength(2);

                entity.Property(e => e.StateProvinceName)
                    .IsRequired()
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<SxLabResults>(entity =>
            {
                entity.HasKey(e => e.SeedsId)
                    .ForSqlServerIsClustered(false);

                entity.ToTable("sx_lab_results");

                entity.Property(e => e.SeedsId)
                    .HasColumnName("seeds_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.AssayResults)
                    .HasColumnName("assay_results")
                    .HasColumnType("char(1)");

                entity.Property(e => e.AssayResults2)
                    .HasColumnName("assay_results2")
                    .HasColumnType("char(1)");

                entity.Property(e => e.AssayTest)
                    .HasColumnName("assay_test")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.AssayTest2)
                    .HasColumnName("assay_test2")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.BadlyDiscoloredPercent)
                    .HasColumnName("badly_discolored_percent")
                    .HasColumnType("numeric(8, 7)");

                entity.Property(e => e.BushelWeight)
                    .HasColumnName("bushel_weight")
                    .HasColumnType("numeric(7, 2)");

                entity.Property(e => e.CciaConfirmed)
                    .HasColumnName("ccia_confirmed")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CciaGerm).HasColumnName("ccia_germ");

                entity.Property(e => e.ChewingInsectDamagePercent)
                    .HasColumnName("chewing_insect_damage_percent")
                    .HasColumnType("numeric(8, 7)");

                entity.Property(e => e.ConfirmDate)
                    .HasColumnName("confirm_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ConfirmUser)
                    .HasColumnName("confirm_user")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DataEntryDate)
                    .HasColumnName("data_entry_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.DataEntryUser)
                    .HasColumnName("data_entry_user")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DateComplete)
                    .HasColumnName("date_complete")
                    .HasColumnType("datetime");

                entity.Property(e => e.DodderGrams)
                    .HasColumnName("dodder_grams")
                    .HasColumnType("numeric(7, 2)");

                entity.Property(e => e.ForeignMaterialPercent)
                    .HasColumnName("foreign_material_percent")
                    .HasColumnType("numeric(8, 7)");

                entity.Property(e => e.ForeignMaterialsComments)
                    .HasColumnName("foreign_materials_comments")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.GermDays).HasColumnName("germ_days");

                entity.Property(e => e.GermHardSeed)
                    .HasColumnName("germ_hard_seed")
                    .HasColumnType("numeric(8, 7)");

                entity.Property(e => e.GermPercent)
                    .HasColumnName("germ_percent")
                    .HasColumnType("numeric(8, 7)");

                entity.Property(e => e.GermResults)
                    .HasColumnName("germ_results")
                    .HasColumnType("char(1)");

                entity.Property(e => e.GermTemp)
                    .HasColumnName("germ_temp")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.InertComments)
                    .HasColumnName("inert_comments")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.InertPercent)
                    .HasColumnName("inert_percent")
                    .HasColumnType("numeric(8, 7)");

                entity.Property(e => e.LbsCanceled)
                    .HasColumnName("lbs_canceled")
                    .HasColumnType("numeric(16, 2)");

                entity.Property(e => e.LbsPassed)
                    .HasColumnName("lbs_passed")
                    .HasColumnType("numeric(16, 2)");

                entity.Property(e => e.LbsRejected)
                    .HasColumnName("lbs_rejected")
                    .HasColumnType("numeric(16, 2)");

                entity.Property(e => e.NoxiousComments)
                    .HasColumnName("noxious_comments")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NoxiousCount).HasColumnName("noxious_count");

                entity.Property(e => e.NoxiousGrams)
                    .HasColumnName("noxious_grams")
                    .HasColumnType("numeric(7, 2)");

                entity.Property(e => e.NoxiousPercent)
                    .HasColumnName("noxious_percent")
                    .HasColumnType("numeric(8, 7)");

                entity.Property(e => e.OtherCropComments)
                    .HasColumnName("other_crop_comments")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.OtherCropCount).HasColumnName("other_crop_count");

                entity.Property(e => e.OtherCropPercent)
                    .HasColumnName("other_crop_percent")
                    .HasColumnType("numeric(8, 7)");

                entity.Property(e => e.OtherVarietyComments)
                    .HasColumnName("other_variety_comments")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.OtherVarietyCount).HasColumnName("other_variety_count");

                entity.Property(e => e.OtherVarietyPercent)
                    .HasColumnName("other_variety_percent")
                    .HasColumnType("numeric(8, 7)");

                entity.Property(e => e.PrivateLabDate)
                    .HasColumnName("private_lab_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.PrivateLabId).HasColumnName("private_lab_id");

                entity.Property(e => e.PrivateLabName)
                    .HasColumnName("private_lab_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PrivateLabNumber)
                    .HasColumnName("private_lab_number")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.PurityComments)
                    .HasColumnName("purity_comments")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PurityGrams)
                    .HasColumnName("purity_grams")
                    .HasColumnType("numeric(7, 2)");

                entity.Property(e => e.PurityPercent)
                    .HasColumnName("purity_percent")
                    .HasColumnType("numeric(8, 7)");

                entity.Property(e => e.PurityResults)
                    .HasColumnName("purity_results")
                    .HasColumnType("char(1)");

                entity.Property(e => e.SampleComments)
                    .HasColumnName("sample_comments")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.SplitsAndCracksPercent)
                    .HasColumnName("splits_and_cracks_percent")
                    .HasColumnType("numeric(8, 7)");

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("update_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdateUser)
                    .HasColumnName("update_user")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.WeedSeedComments)
                    .HasColumnName("weed_seed_comments")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.WeedSeedCount).HasColumnName("weed_seed_count");

                entity.Property(e => e.WeedSeedPercent)
                    .HasColumnName("weed_seed_percent")
                    .HasColumnType("numeric(8, 7)");

                entity.HasOne(d => d.LabOrganization);


            });

            modelBuilder.Entity<VarFamily>(entity =>
            {
                entity.HasKey(e => e.VarFamId);

                entity.ToTable("var_family");

                entity.Property(e => e.VarFamId)
                    .HasColumnName("var_fam_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Alias)
                    .HasColumnName("alias")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Confidential)
                    .HasColumnName("confidential")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.DateEntered)
                    .HasColumnName("date_entered")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("datetime");

                entity.Property(e => e.DescHyperlink)
                    .HasColumnName("desc_hyperlink")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Experimental)
                    .HasColumnName("experimental")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.InUse)
                    .HasColumnName("in_use")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Oecd)
                    .HasColumnName("oecd")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.OecdCountry).HasColumnName("oecd_country");

                entity.Property(e => e.OrigOfficialId).HasColumnName("orig_official_id");

                entity.Property(e => e.PrivateCode)
                    .HasColumnName("private_code")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.UpdateComments)
                    .HasColumnName("update_comments")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.UserEntered)
                    .HasColumnName("user_entered")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserUpdated)
                    .HasColumnName("user_updated")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.VarComments)
                    .HasColumnName("var_comments")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.VarFamName)
                    .IsRequired()
                    .HasColumnName("var_fam_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.VarOffId).HasColumnName("var_off_id");

                entity.Property(e => e.VarietyType)
                    .HasColumnName("variety_type")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.VarOff)
                    .WithMany(p => p.VarFamily)
                    .HasForeignKey(d => d.VarOffId)
                    .HasConstraintName("FK_Var_Family_Var_Official");
            });

            modelBuilder.Entity<VarOfficial>(entity =>
            {
                entity.HasKey(e => e.VarOffId);

                entity.ToTable("var_official");

                entity.Property(e => e.VarOffId)
                    .HasColumnName("var_off_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.BriefDescription)
                    .HasColumnName("brief_description")
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.CciaCertified)
                    .HasColumnName("ccia_certified")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CciaCertifiedDate)
                    .HasColumnName("ccia_certified_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.CciaCertifier)
                    .HasColumnName("ccia_certifier")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Comments)
                    .HasColumnName("comments")
                    .HasMaxLength(2500)
                    .IsUnicode(false);

                entity.Property(e => e.Confidential)
                    .HasColumnName("confidential")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CropId).HasColumnName("crop_id");

                entity.Property(e => e.CtcApproved)
                    .HasColumnName("ctc_approved")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CtcDateApproved)
                    .HasColumnName("ctc_date_approved")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateEntered)
                    .HasColumnName("date_entered")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("date_updated")
                    .HasColumnType("datetime");

                entity.Property(e => e.DescHyperlink)
                    .HasColumnName("desc_hyperlink")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.DescriptionOnFile)
                    .HasColumnName("description_on_file")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Experimental)
                    .HasColumnName("experimental")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.GenBreeder)
                    .HasColumnName("gen_breeder")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.GenCertified)
                    .HasColumnName("gen_certified")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.GenFoundation)
                    .HasColumnName("gen_foundation")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.GenRegistered)
                    .HasColumnName("gen_registered")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Generation)
                    .HasColumnName("generation")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.GermplasmEntity)
                    .HasColumnName("germplasm_entity")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.HistoricalName)
                    .HasColumnName("historical_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Oecd)
                    .HasColumnName("oecd")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.OtherStateCert)
                    .HasColumnName("other_state_cert")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.OwnerId).HasColumnName("owner_id");

                entity.Property(e => e.PendingCertification)
                    .HasColumnName("pending_certification")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PlantPatent)
                    .HasColumnName("plant_patent")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PlantPatentDate)
                    .HasColumnName("plant_patent_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.PlantPatentNum).HasColumnName("plant_patent_num");

                entity.Property(e => e.PrivateCode)
                    .HasColumnName("private_code")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ProducerId).HasColumnName("producer_id");

                entity.Property(e => e.Pvp)
                    .HasColumnName("pvp")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PvpDate)
                    .HasColumnName("pvp_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.PvpExpDate)
                    .HasColumnName("pvp_exp_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.PvpNumber).HasColumnName("pvp_number");

                entity.Property(e => e.PvpYears).HasColumnName("pvp_years");

                entity.Property(e => e.ReasonForInactive)
                    .HasColumnName("reason_for_inactive")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.RiceQa)
                    .HasColumnName("rice_qa")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.RiceQaColor)
                    .HasColumnName("rice_qa_color")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TitleV)
                    .HasColumnName("title_v")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Turfgrass)
                    .HasColumnName("turfgrass")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.UserEntered)
                    .HasColumnName("user_entered")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserUpdated)
                    .HasColumnName("user_updated")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.VarCategory)
                    .HasColumnName("var_category")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.VarOffName)
                    .IsRequired()
                    .HasColumnName("var_off_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.VarReviewBoard)
                    .HasColumnName("var_review_board")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.VarReviewBoardDate)
                    .HasColumnName("var_review_board_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.VarStatus)
                    .HasColumnName("var_status")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Crop)
                    .WithMany(p => p.VarOfficial)
                    .HasForeignKey(d => d.CropId)
                    .HasConstraintName("FK_Var_Official_Crops");
            });
        }
    }
}
