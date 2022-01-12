using System;
using Microsoft.EntityFrameworkCore;
//using Thinktecture;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Configuration;

namespace CCIA.Models
{
    public partial class CCIAContext : DbContext
    {
          public CCIAContext(DbContextOptions<CCIAContext> options)
            : base(options)
        { }
       
        [DbFunction("sid_standards_msg","dbo")]
        public static string GetStandardsMessage(int seed_id)
        {
            throw new NotImplementedException();
        }

        [DbFunction("sid_standards_msg_assay","dbo")]
        public static string GetAssayMessage(int seed_id)
        {
            throw new NotImplementedException();
        }

        public virtual DbSet<AbbrevAppType> AbbrevAppType { get; set; }

        public virtual DbSet<IsolationConflicts> IsolationConflicts { get; set; }

        public virtual DbSet<ProcessTag> ProcessTag { get; set;}
        public virtual DbSet<VarCountries> VarCountires { get; set; }
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
        public virtual DbSet<OrganizationAddress> OrganizationAddress { get; set; }
        public virtual DbSet<Contacts> Contacts { get; set; }
       
        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<County> County { get; set; }
        public virtual DbSet<Crops> Crops { get; set; }
        public virtual DbSet<CropsRates> CropsRates { get; set; }
        public virtual DbSet<CropStandards> CropStandards { get; set; }
        public virtual DbSet<DistrictCounty> DistrictCounty { get; set; }
        public virtual DbSet<Districts> Districts { get; set; }
        public virtual DbSet<Ecoregions> Ecoregions { get; set; }
        public virtual DbSet<Fees> Fees { get; set; }
        public virtual DbSet<FieldInspectionReport> FieldInspectionReport { get; set; }
        public virtual DbSet<FieldInspection> FieldInspection { get; set;}
        // public virtual DbSet<FieldMaps> FieldMaps { get; set; }
        public virtual DbSet<LotBlends> LotBlends { get; set; }
        public virtual DbSet<BlendInDirtComponents> BlendInDirtComponents { get; set; }
        public virtual DbSet<LotBlendSummary> LotBlendSummary { get; set; }
        public virtual DbSet<InDirtBlendSummary> InDirtBlendSummary { get; set; }
        public virtual DbSet<VarietyBlendComponents> VarietyBlendComponents { get; set; }
        public virtual DbSet<BlendComponentChanges> BlendComponentChanges { get; set; }
        public virtual DbSet<BlendDocuments>   BlendDocuments { get; set; }
        public virtual DbSet<Organizations> Organizations { get; set; }
        public virtual DbSet<PlantingStocks> PlantingStocks { get; set; }
        public virtual DbSet<Rates> Rates { get; set; }
        public virtual DbSet<SeedsApplications> SeedsApplications { get; set; }
        public virtual DbSet<StateProvince> StateProvince { get; set; }
        public virtual DbSet<SampleLabResults> SampleLabResults { get; set; }
        public virtual DbSet<VarFamily> VarFamily { get; set; }
        public virtual DbSet<VarOfficial> VarOfficial { get; set; }
        public virtual DbSet<VarFull> VarFull { get; set; }
        public virtual DbSet<Seeds> Seeds { get; set; }

        public virtual DbSet<SeedDocuments> SeedDocuments { get; set; }

        public virtual DbSet<SeedsDocumentTypes> SeedsDocumentTypes { get; set; }

        public virtual DbSet<AbbrevClassSeeds> AbbrevClassSeeds { get; set; }

        public virtual DbSet<Tags> Tags { get; set; }

        public virtual DbSet<AbbrevTagType>  AbbrevTagType { get; set; }

        public virtual DbSet<FieldHistory> FieldHistory { get; set; }

        public virtual DbSet<OECD> OECD { get; set; }

        public virtual DbSet<AbbrevOECDClass> AbbrevOECDClass { get; set; }

        public virtual DbSet<BulkSalesCertificates> BulkSalesCertificates { get; set; }

        public virtual DbSet<BulkSalesCertificateChanges> BulkSalesCertificateChanges { get; set; }

        public virtual DbSet<CCIAEmployees> CCIAEmployees { get; set; }

        public virtual DbSet<BulkSalesCertificatesShares> BulkSalesCertificatesShares { get; set; }

        public virtual DbSet<SeedTransfers> SeedTransfers { get; set; }

        public virtual DbSet<SeedTransferChanges> SeedTransferChanges { get; set; }

        public virtual DbSet<Standards>  Standards { get; set; }

        public virtual DbSet<TurfgrassCertificates> TurfgrassCertificates { get; set; }

        public virtual DbSet<MyCustomers> MyCustomers { get; set; }

        public virtual DbSet<RenewFields> RenewFields { get; set; }

        public virtual DbSet<AppChanges> AppChanges { get; set; }

        public virtual DbSet<OECDChanges> OECDChanges { get; set; }

        public virtual DbSet<TagChanges> TagChanges { get; set; }

        public virtual DbSet<TagBagging> TagBagging { get; set; }

        public virtual DbSet<TagSeries> TagSeries { get; set; }

        public virtual DbSet<OrgMaps> OrgMaps {get; set; }

        public virtual DbSet<Maps> Maps { get; set; }

        public virtual DbSet<OrgMapCrops> OrgMapCrops {get; set;}

        public virtual DbSet<ApplicationReport> ApplicationReport { get; set; }

        public virtual DbSet<SeedsReport> SeedsReport { get; set; }

        public virtual DbSet<OECDReport> OECDReport { get; set; }

        public virtual DbSet<CropAssignmentByLeadBackup> CropAssignmentByLeadBackup { get; set; }

        public virtual DbSet<CropAssignmentByName> CropAssignmentByName { get; set; }

        public virtual DbSet<CropGroups> CropGroups { get; set; }   

        public virtual DbSet<Notifications> Notifications { get; set; }

        public virtual DbSet<PotatoHealthCertificates> PotatoHealthCertificates { get; set; }

        public virtual DbSet<PotatoHealthCertificateHistory> PotatoHealthCertificateHistory { get; set; }

        public virtual DbSet<PotatoHealthCertificateInspections> PotatoHealthCertificateInspections { get; set; }

        public virtual DbSet<Jobs> Jobs { get; set; }

        public virtual DbSet<TagDocuments> TagDocuments { get; set;}

        public virtual DbSet<AppCertificates> AppCertificates { get; set; }
        public virtual DbSet<FIRDocuments> FIRDocuments { get; set; }
        public virtual DbSet<SeedsChanges> SeedsChanges { get; set; }

        public virtual DbSet<SampleLabResultsChanges> SampleLabResultChanges { get; set; }

        
       
        // Unable to generate entity type for table 'dbo.renew_actions_trans'. Please see the warning messages.
        
        
        
        // Unable to generate entity type for table 'dbo.var_countries'. Please see the warning messages.
      
        

       
        // Unable to generate entity type for table 'dbo.contact_map'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.org_map'. Please see the warning messages.
      
       

        public static ILoggerFactory GetLoggerFactory()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder =>
                   builder.AddConsole()
                          .AddFilter(DbLoggerCategory.Database.Command.Name,
                                     LogLevel.Information));
            return serviceCollection.BuildServiceProvider()
                    .GetService<ILoggerFactory>();
        }
       

        // public CCIAContext(IConfiguration configuration)
        // {
        //     Configuration = configuration;
        // }

        // public IConfiguration Configuration { get; }


        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     // if (!optionsBuilder.IsConfigured)
        //     // {
        //     //     optionsBuilder.UseSqlServer(Configuration.GetConnectionString("CCIACoreContext"), sqlOptions =>
        //     //     {                       
        //     //             sqlOptions.UseNetTopologySuite();
        //     //             sqlOptions.AddRowNumberSupport();
        //     //     });
        //     // }
        //     optionsBuilder.UseLoggerFactory(GetLoggerFactory());

        // }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Seeds>(entity =>
            {
                entity.ToTable("Seeds");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("seeds_id").ValueGeneratedOnAdd();

                entity.Property(e => e.CertProgram).HasColumnName("cert_program");
                entity.Property(e => e.AppId).HasColumnName("app_id");
                entity.Property(e => e.SampleFormNumber).HasColumnName("sx_form_no");
                entity.Property(e => e.SampleFormDate).HasColumnName("sx_form_date");
                entity.Property(e => e.SampleFormCertNumber).HasColumnName("sx_form_cert_no");
                entity.Property(e => e.SampleFormRad).HasColumnName("sx_form_rad");
                entity.Property(e => e.CertYear).HasColumnName("cert_year");
                entity.Property(e => e.YearConfirmed).HasColumnName("year_confirmed");
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
                entity.Property(e => e.Remarks).HasColumnName("remarks");
                entity.Property(e => e.SampleDrawnBy).HasColumnName("sx_drawn_by");
                entity.Property(e => e.SamplerID).HasColumnName("sampler_id");
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
                
                entity.HasOne(d => d.Application);

                entity.HasOne(d => d.StateOfOrigin);

                entity.HasOne(d => d.CountryOfOrigin);

                entity.HasOne(d => d.CountySampleDrawn);

                entity.HasOne(d => d.ContactEntered);

                entity.HasMany(d => d.Documents);

                entity.HasMany(d => d.OECDForm).WithOne(o => o.Seeds).HasForeignKey(o => o.SeedsId).HasPrincipalKey(s => s.Id);

            });

            modelBuilder.Entity<TagBagging>(entity => {
                entity.ToTable("tag_bagging");

                entity.HasKey(e => e.TagId);

                entity.Property(e => e.TagId).HasColumnName("tag_id");

                entity.Property(e => e.TotalBagged).HasColumnName("total_bagged");

            });

            modelBuilder.Entity<Jobs>(entity => {
                entity.ToTable("jobs");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.JobTitle).HasColumnName("JobTitle");

                entity.Property(e => e.JobInterval).HasColumnName("JobInterval");

                entity.Property(e => e.JobTime).HasColumnName("JobTime").HasColumnType("Time(0)");

                entity.Property(e => e.DateLastJobRan).HasColumnName("DateLastJobRan");

                entity.Property(e => e.DateNextJobStart).HasColumnName("DateNextJobStart");

                entity.Property(e => e.Section).HasColumnName("Section");

            });

               modelBuilder.Entity<PotatoHealthCertificates>(entity => {
                entity.ToTable("po_health_cert");

                entity.HasKey(e => e.AppId);

                entity.Property(e => e.AppId).HasColumnName("app_id");

                entity.Property(e => e.LotOriginatedFromTissueCulture).HasColumnName("lot_orig_cult");

                entity.Property(e => e.YearMicroPropagated).HasColumnName("yr_micropropagated");
                entity.Property(e => e.MicropropagatedBy).HasColumnName("micropropagated_by");
                entity.Property(e => e.NumberOfYearsProduced).HasColumnName("num_yrs_produced");
                entity.Property(e => e.PostHarvestLocation).HasColumnName("ph_location");
                entity.Property(e => e.PostHarvestLeafroll).HasColumnName("ph_leafroll");
                entity.Property(e => e.PostHarvestMosaic).HasColumnName("ph_mosaic");
                entity.Property(e => e.PostHarvestOtherVarieties).HasColumnName("ph_other_varieties");
                entity.Property(e => e.PostHarvestSampleNumber).HasColumnName("ph_sample_no");
                entity.Property(e => e.PostHarvestPlantCount).HasColumnName("ph_plant_count");
                entity.Property(e => e.PercentPVY).HasColumnName("percent_pvy");
                entity.Property(e => e.PercentPVX).HasColumnName("percent_pvx");
                entity.Property(e => e.BacterialRingRot).HasColumnName("bact_ring_rot");
                entity.Property(e => e.GoldenNematode).HasColumnName("golden_nematode");
                entity.Property(e => e.LateBlight).HasColumnName("late_blight");
                entity.Property(e => e.RootKnotNematode).HasColumnName("root_knot_nematode");
                entity.Property(e => e.PotatoRotNematode).HasColumnName("pot_rot_nematode");
                entity.Property(e => e.PotatoWart).HasColumnName("pot_wart");
                entity.Property(e => e.PowderScap).HasColumnName("powder_scab");
                entity.Property(e => e.PotatoSpindleTuberViroid).HasColumnName("pot_spindle_tuber_viroid");
                entity.Property(e => e.CorkyRingSpots).HasColumnName("corky_ring_spots");
                entity.Property(e => e.Notes).HasColumnName("notes");
                entity.HasMany(e => e.History);
            });

            modelBuilder.Entity<PotatoHealthCertificateHistory>(entity => {
                entity.ToTable("po_cert_history");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.AppId).HasColumnName("app_id");
                entity.Property(e => e.ProductionYear).HasColumnName("prod_year");
                entity.Property(e => e.Greenhouse).HasColumnName("greenhouse");
                entity.Property(e => e.Field).HasColumnName("field");
                entity.Property(e => e.CertNumber).HasColumnName("cert_no");
                entity.Property(e => e.CertifyingState).HasColumnName("cert_state");

            });

            modelBuilder.Entity<PotatoHealthCertificateInspections>(entity => {
                entity.HasNoKey();
            });

            modelBuilder.Entity<ApplicationReport>(entity => {
                entity.HasNoKey();
            });

            modelBuilder.Entity<SeedsReport>(entity => {
                entity.HasNoKey();
            });

            modelBuilder.Entity<OECDReport>(entity => {
                entity.HasNoKey();
            });

            modelBuilder.Entity<CropAssignmentByLeadBackup>(entity => {
                entity.HasNoKey();
            });

            modelBuilder.Entity<CropAssignmentByName>(entity => {
                entity.HasNoKey();
            });

            modelBuilder.Entity<CropGroups>(entity => {
                entity.HasNoKey();
            });

            modelBuilder.Entity<Maps>(entity => {
                entity.ToTable("map_croppts_app_listing");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("app_id");

                entity.Property(e => e.Name).HasColumnName("description");

                entity.Property(e => e.Url).HasColumnName("url");

            });

            modelBuilder.Entity<SeedsChanges>(entity => {
                entity.ToTable("seeds_changes");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.SID).HasColumnName("seeds_id");

                entity.Property(e => e.ColumnChange).HasColumnName("column_change");

                entity.Property(e => e.OldValue).HasColumnName("old_value");

                entity.Property(e => e.NewValue).HasColumnName("new_value");                

                entity.Property(e => e.UserChange).HasColumnName("user_change");

                entity.Property(e => e.DateChanged).HasColumnName("date_change");

                entity.HasOne(e => e.Employee);

            });

            modelBuilder.Entity<SampleLabResultsChanges>(entity => {
                entity.ToTable("sx_lab_results_changes");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.SID).HasColumnName("seeds_id");

                entity.Property(e => e.ColumnChange).HasColumnName("column_change");

                entity.Property(e => e.OldValue).HasColumnName("old_value");

                entity.Property(e => e.NewValue).HasColumnName("new_value");                

                entity.Property(e => e.UserChange).HasColumnName("user_change");

                entity.Property(e => e.DateChanged).HasColumnName("date_change");

                entity.HasOne(e => e.Employee);

            });

            modelBuilder.Entity<OrgMapCrops>(entity => {
                entity.ToTable("map_crop_access");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("access_id");
                entity.Property(e => e.OrgId).HasColumnName("org_id");
                entity.Property(e => e.CropId).HasColumnName("crop_id");
                entity.Property(e => e.Allow).HasColumnName("access");
            });

            modelBuilder.Entity<OrgMaps>(entity => {
                entity.ToTable("org_map");   

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.OrgId).HasColumnName("org_id");

                entity.Property(e => e.Map).HasColumnName("map_name");

                entity.Property(e => e.Allow).HasColumnName("allow_access");

            });

            modelBuilder.Entity<TagSeries>(entity => {
                entity.ToTable("tag_series");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("seriesID");
                entity.Property(e => e.TagId).HasColumnName("tag_id");
                entity.Property(d => d.Letter).HasColumnName("letter");
                entity.Property(d => d.Start).HasColumnName("start_value");
                entity.Property(d => d.End).HasColumnName("end_value");
                entity.Property(d => d.Void).HasColumnName("void");
                entity.Property(d => d.Count).HasColumnName("row_count");

                entity.HasOne(d => d.Tag).WithMany(t => t.TagSeries).HasForeignKey(s => s.TagId).HasPrincipalKey(t => t.Id);

            });

            modelBuilder.Entity<Standards>(entity => {
                entity.ToTable("standards");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("std_id");

                entity.Property(e => e.Name).HasColumnName("std_name");

                entity.Property(e => e.Category).HasColumnName("std_category");

                entity.Property(e => e.Description).HasColumnName("std_desc");

                entity.Property(e => e.Value).HasColumnName("value");

                entity.Property(e => e.MinValue).HasColumnName("min_value");

                entity.Property(e => e.MaxValue).HasColumnName("max_value");

                entity.Property(e => e.ValueType).HasColumnName("value_type");

                entity.Property(e => e.TextValue).HasColumnName("text_value");

                entity.Property(e => e.NegativeMessage).HasColumnName("neg_msg");

                entity.Property(e => e.PositiveMessage).HasColumnName("pos_msg");

                entity.Property(e => e.Program).HasColumnName("program");

            });

            modelBuilder.Entity<FieldInspection>(entity => {
                entity.ToTable("field_results");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("fld_res_id");

                entity.Property(e => e.AppId).HasColumnName("app_id");

                entity.Property(e => e.DateInspected).HasColumnName("date_inspected");

                entity.Property(e => e.InspectorId).HasColumnName("inspector");

                entity.Property(e => e.ApplicantContacted).HasColumnName("applicant_contacted");

                entity.Property(e => e.ApplicantPresent).HasColumnName("applicant_present");

                entity.Property(e => e.Weeds).HasColumnName("weed_comment");

                entity.Property(e => e.Comments).HasColumnName("insp_comments");

                entity.Property(e => e.TotalPlantsInspected).HasColumnName("total_plants_insp");

                entity.Property(e => e.OtherVarieties).HasColumnName("other_varieties");

                entity.Property(e => e.Mosaic).HasColumnName("mosaic");

                entity.Property(e => e.Leafroll).HasColumnName("leafroll");

                entity.Property(e => e.Blackleg).HasColumnName("blackleg");

                entity.Property(e => e.Calico).HasColumnName("calico");

                entity.Property(e => e.OtherDiseases).HasColumnName("other_disease");

                entity.Property(e => e.Insects).HasColumnName("insects");

                entity.Property(e => e.Maturity).HasColumnName("maturity_comment");

                entity.Property(e => e.Isolation).HasColumnName("isolation_comment");

                entity.Property(e => e.EstimatedYield).HasColumnName("estimated_yield_comment");

                entity.Property(e => e.OtherVarietiesComment).HasColumnName("other_varieties_comment");

                entity.Property(e => e.OtherCrop).HasColumnName("other_crops_comment");

                entity.Property(e => e.Disease).HasColumnName("disease_comment");

                entity.Property(e => e.Appearance).HasColumnName("appearance_comment");

                entity.HasOne(e => e.InspectorEmployee);


            });

            modelBuilder.Entity<AppChanges>(entity => {
                entity.ToTable("application_changes");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id);

                entity.Property(e => e.AppId).HasColumnName("app_id");

                entity.Property(e => e.ColumnChange).HasColumnName("column_change");

                entity.Property(e => e.OldValue).HasColumnName("old_value");

                entity.Property(e => e.NewValue).HasColumnName("new_value");                

                entity.Property(e => e.UserChange).HasColumnName("user_change");

                entity.Property(e => e.DateChanged).HasColumnName("date_change");

                entity.HasOne(e => e.Employee);
            });

            modelBuilder.Entity<BulkSalesCertificateChanges>(entity => {
                entity.ToTable("bulk_sales_certificates_changes");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id);

                entity.Property(e => e.BSCId).HasColumnName("bsc_id");

                entity.Property(e => e.ColumnChange).HasColumnName("column_change");

                entity.Property(e => e.OldValue).HasColumnName("old_value");

                entity.Property(e => e.NewValue).HasColumnName("new_value");                

                entity.Property(e => e.UserChange).HasColumnName("user_change");

                entity.Property(e => e.DateChanged).HasColumnName("date_change");

                entity.HasOne(e => e.Employee);
            });

            modelBuilder.Entity<SeedTransferChanges>(entity => {
                entity.ToTable("seed_transfer_changes");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id);

                entity.Property(e => e.STId).HasColumnName("stid");

                entity.Property(e => e.ColumnChange).HasColumnName("column_change");

                entity.Property(e => e.OldValue).HasColumnName("old_value");

                entity.Property(e => e.NewValue).HasColumnName("new_value");                

                entity.Property(e => e.UserChange).HasColumnName("user_change");

                entity.Property(e => e.DateChanged).HasColumnName("date_change");

                entity.Property(e => e.userIsAdmin).HasColumnName("user_admin");

                entity.HasOne(e => e.Employee);
            });

            modelBuilder.Entity<OECDChanges>(entity => {
                entity.ToTable("oecd_changes");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id);

                entity.Property(e => e.OECDId).HasColumnName("file_num");

                entity.Property(e => e.ColumnChange).HasColumnName("column_change");

                entity.Property(e => e.OldValue).HasColumnName("old_value");

                entity.Property(e => e.NewValue).HasColumnName("new_value");                

                entity.Property(e => e.UserChange).HasColumnName("user_change");

                entity.Property(e => e.DateChanged).HasColumnName("date_change");

                entity.HasOne(e => e.Employee);
            });

            modelBuilder.Entity<TagChanges>(entity => {
                entity.ToTable("tags_changes");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id);

                entity.Property(e => e.TagId).HasColumnName("tag_id");

                entity.Property(e => e.ColumnChange).HasColumnName("column_change");

                entity.Property(e => e.OldValue).HasColumnName("old_value");

                entity.Property(e => e.NewValue).HasColumnName("new_value");                

                entity.Property(e => e.UserChange).HasColumnName("user_change");

                entity.Property(e => e.DateChanged).HasColumnName("date_change");

                entity.HasOne(e => e.Employee);
            });


            modelBuilder.Entity<RenewFields>(entity => {
                entity.ToTable("renew_fields");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("counter");

                entity.Property(e => e.AppId).HasColumnName("app_num");

                entity.Property(e => e.Year).HasColumnName("renew_year");

                entity.Property(e => e.Action).HasColumnName("renew_action");

                entity.Property(e => e.DateRenewed).HasColumnName("action_date");

                entity.HasOne(e => e.Application);

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

                entity.Property(e => e.UserEmpModified).HasColumnName("user_emp_modified");

                entity.Property(e => e.DateModified).HasColumnName("date_modified");

                entity.HasOne(d => d.FHCrops);

                //entity.HasOne(d => d.Application);


            });

            modelBuilder.Entity<Ecoregions>(entity => 
            {
                entity.ToTable("ecoregions");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name");
            });

            modelBuilder.Entity<SeedsApplications>(entity =>
            {
                entity.ToTable("seeds_apps");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("seed_app_id");

                entity.Property(e => e.AppId).HasColumnName("app_id");

                entity.Property(e => e.SeedsId).HasColumnName("seeds_id");

                entity.HasOne(e => e.Seeds).WithMany(s => s.SeedsApplications).HasForeignKey(e => e.SeedsId);

                entity.HasOne(e => e.Application);

            });

            modelBuilder.Entity<MyCustomers>(entity =>
            {
                entity.ToTable("my_customers");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.OrganizationId).HasColumnName("org_id");

                entity.Property(e => e.Name).HasColumnName("cust_name");

                entity.Property(e => e.Address1).HasColumnName("cust_address_line_1");

                entity.Property(e => e.Address2).HasColumnName("cust_address_line_2");

                entity.Property(e => e.City).HasColumnName("cust_city");

                entity.Property(e => e.StateId).HasColumnName("cust_state_id");

                entity.Property(e => e.CountyId).HasColumnName("cust_county");

                entity.Property(e => e.CountryId).HasColumnName("cust_country");

                entity.Property(e => e.Zip).HasColumnName("cust_zip");

                entity.Property(e => e.Phone).HasColumnName("cust_phone");

                entity.Property(e => e.Email).HasColumnName("cust_email");

                entity.HasOne(e => e.State);

                entity.HasOne(e => e.County);

                entity.HasOne(e => e.Country);

                entity.HasOne(e => e.Organization);

            });

            modelBuilder.Entity<TurfgrassCertificates>(entity => 
            {
                entity.ToTable("turfgrass_certificates");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("certificate_id");

                entity.Property(e => e.AppId).HasColumnName("app_id");

                entity.Property(e => e.Sprigs).HasColumnName("sprigs");

                entity.Property(e => e.Sod).HasColumnName("sod");

                entity.Property(e => e.BillingInvoice).HasColumnName("billing_invoice");

                entity.Property(e => e.HarvestDate).HasColumnName("harvest_date");

                entity.Property(e => e.HarvestNumber).HasColumnName("harvest_number");

                

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

                entity.Property(e => e.TransferClassId).HasColumnName("transfer_class");

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

                entity.Property(e => e.SeasonalEmployee).HasColumnName("seasonal_employee");

                entity.Property(e => e.NewTag).HasColumnName("new_tag");

                entity.Property(e => e.TagPrint).HasColumnName("tag_print");

                entity.Property(e => e.HeritageGrainQA).HasColumnName("heritage_grain_qa");

                entity.Property(e => e.PrevarietyGermplasm).HasColumnName("prevariety_germplasm");

                entity.Property(e => e.OECDInvoicePrinter).HasColumnName("oecd_invoice_printer");

                entity.Property(e => e.Admin).HasColumnName("admin");

                entity.Property(e => e.ConditionerStatusUpdate).HasColumnName("ConditionerStatusUpdate");

                entity.Property(e => e.UpdateMapPermissions).HasColumnName("UpdateMapPermissions");

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

                entity.Property(e => e.CertProgramAbbreviation).HasColumnName("cert_program");

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

                entity.HasOne(e => e.CertProgram).WithMany(a => a.BulkSalesCertificates).HasForeignKey(e => e.CertProgramAbbreviation).HasPrincipalKey(a => a.Abbreviation);

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

                entity.HasOne(e => e.ShipperOrganization);

                entity.HasOne(e => e.ConditionerOrganization);

                entity.HasOne(e => e.Variety);
                entity.HasMany(e => e.Changes);
                entity.HasOne(e => e.EntryEmployee);
                entity.HasOne(e => e.UpdateEmployee);


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

            modelBuilder.Entity<VarietyBlendComponents>(entity => 
            {
                entity.ToTable("blend_components");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasColumnName("blend_comp_id");

                entity.Property(e => e.BlendVarietyId).HasColumnName("var_off_id");

                entity.Property(e => e.ComponentVarietyId).HasColumnName("comp_var_id");

                entity.Property(e => e.ComponentPercent).HasColumnName("comp_percent");

            });

            modelBuilder.Entity<BlendDocuments>(entity => 
            {
                entity.ToTable("blend_docs");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("blend_doc_id");
                entity.Property(e => e.BlendId).HasColumnName("blend_id");
                entity.Property(e => e.Name).HasColumnName("doc_name");
                entity.Property(e => e.Link).HasColumnName("doc_link");
            });

            modelBuilder.Entity<BlendComponentChanges>(entity =>
            {
                entity.ToTable("blend_components_changes");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.BlendId).HasColumnName("blend_id");
                entity.Property(e => e.ComponentId).HasColumnName("comp_id");
                entity.Property(e => e.ColumnChange).HasColumnName("column_change");

                entity.Property(e => e.OldValue).HasColumnName("old_value");

                entity.Property(e => e.NewValue).HasColumnName("new_value");                

                entity.Property(e => e.UserChange).HasColumnName("user_change");

                entity.Property(e => e.DateChanged).HasColumnName("date_change");

                entity.HasOne(e => e.Employee);

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

                entity.Property(e => e.OECDCountryId).HasColumnName("oecd_country");

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

                entity.HasOne(e => e.TaggingOrganization);
                entity.HasOne(e => e.ContactEntered);
                entity.HasOne(e => e.TagBagging).WithOne().HasForeignKey<TagBagging>();;                
                entity.HasOne(e => e.EmployeePrinted);
                entity.HasOne(e => e.OECDClass);
                entity.HasOne(e => e.OECDCountry);
                entity.HasMany(e => e.Changes);


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

            modelBuilder.Entity<FIRDocuments>(entity =>
           {
               entity.ToTable("fir_docs");

               entity.HasKey(e => e.Id);

               entity.Property(e => e.Id).HasColumnName("doc_id");

               entity.Property(e => e.AppId).HasColumnName("app_id");

               entity.Property(e => e.Name).HasColumnName("doc_name");

               entity.Property(e => e.Link).HasColumnName("doc_link");

           });


            modelBuilder.Entity<TagDocuments>(entity =>
           {
               entity.ToTable("tag_docs");

               entity.HasKey(e => e.Id);

               entity.Property(e => e.Id).HasColumnName("tag_doc_id");

               entity.Property(e => e.TagId).HasColumnName("tag_id");

               entity.Property(e => e.Link).HasColumnName("doc_link");

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

                entity.Property(v => v.TableName).HasColumnName("tblname");

                entity.Property(v => v.Certified).HasColumnName("ccia_certified");

                entity.Property(v => v.DateCertified).HasColumnName("date_certified");

                entity.Property(v => v.RiceQa).HasColumnName("rice_qa");

                entity.Property(v => v.RiceColor).HasColumnName("rice_qa_color");

                entity.Property(v => v.ParentId).HasColumnName("parent_id");

                entity.Property(v => v.Turfgrass).HasColumnName("turfgrass");
                entity.Property(v => v.EcoregionId).HasColumnName("ecoregion");
                entity.Property(v => v.CountyHarvestId).HasColumnName("county_harvested");
                entity.Property(v => v.Elevation).HasColumnName("elevation");
                entity.Property(v => v.NumberOfGenerationsPermitted).HasColumnName("number_of_generations_permitted");

                entity.HasOne(v => v.Crop);
                entity.HasOne(v => v.CountyHarvested);
                entity.HasOne(v => v.Ecoregion);
                entity.HasOne(v => v.VarietyFamily).WithOne(f => f.VarFull).IsRequired(false);


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

            entity.Property(e => e.FieldHistoryCount).HasColumnName("field_history_number");

            entity.Property(e => e.GrowerSameAsApplicant).HasColumnName("grower_same_as_applicant");

            entity.Property(e => e.QAProgram).HasColumnName("qa_program").HasDefaultValueSql("((0))");
            
        });

            modelBuilder.Entity<AbbrevClassProduced>(entity =>
            {
                entity.HasKey(e => e.ClassProducedId);

                entity.ToTable("abbrev_class_produced");

                entity.Property(e => e.ClassProducedId)
                    .HasColumnName("class_produced_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.AppTypeId).HasColumnName("app_type");

                entity.Property(e => e.ClassAbbrv)
                    .HasColumnName("class_abbrv")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.ClassProducedTrans)
                    .HasColumnName("class_produced_trans")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SortOrder).HasColumnName("sort_order");
                
                entity.HasOne(e => e.AppType);
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

                entity.Property(e => e.CertClass)
                    .HasColumnName("class_certified_trans")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Program).HasColumnName("program");

                entity.Property(e => e.SortOrder).HasColumnName("sort_order");
            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("address");

                entity.Property(e => e.Id).HasColumnName("address_id");

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

                entity.HasOne(e => e.County);
                entity.HasOne(e => e.StateProvince);
                entity.HasOne(e => e.Countries);
            });

            modelBuilder.Entity<Applications>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("applications");

                entity.HasIndex(e => new { e.Id, e.AppType, e.ApplicantId, e.GrowerId, e.CropId, e.Cancelled, e.Tags, e.PoLotNum, e.FieldName, e.FarmCounty, e.DatePlanted, e.AcresApplied, e.SelectedVarietyId, e.ClassProducedId, e.Submitable, e.Status, e.Approved, e.Maps, e.CertYear })                    
                    .HasDatabaseName("IX_applications_cert_year");


                entity.Property(e => e.Id).HasColumnName("app_id");

                entity.Property(e => e.AcresApplied)
                    .HasColumnName("acres_applied")
                    .HasColumnType("decimal(14, 2)");

                entity.Property(e => e.CountyPermit)
                    .HasColumnName("county_permit")
                    .HasColumnType("varchar(50)");

                entity.Property(e => e.Approved)
                    .HasColumnName("app_approved")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Approver)
                    .HasColumnName("app_approver")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Cancelled)
                    .HasColumnName("app_cancelled")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CancelledBy)
                    .HasColumnName("app_cancelled_by")
                    .HasMaxLength(9)
                    .IsUnicode(false);

                entity.Property(e => e.CompleteDate)
                    .HasColumnName("app_complete_dt")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateApproved)
                    .HasColumnName("app_date_appr")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateDenied)
                    .HasColumnName("app_date_denied")
                    .HasColumnType("datetime");

                entity.Property(e => e.Deadline)
                    .HasColumnName("app_deadline")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Denied)
                    .HasColumnName("app_denied")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Fee)
                    .HasColumnName("app_fee")
                    .HasColumnType("smallmoney");

                entity.Property(e => e.OriginalCertYear).HasColumnName("app_original_cert_year");

                entity.Property(e => e.PackageComplete)
                    .HasColumnName("app_pkg_complete")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Postmark)
                    .HasColumnName("app_postmark")
                    .HasColumnType("smalldatetime");

                entity.Property(e => e.Received)
                    .HasColumnName("app_received")
                    .HasColumnType("datetime");

                entity.Property(e => e.Rejector)
                    .HasColumnName("app_rejector")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Submitable)
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

                entity.Property(e => e.EcoregionId)
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

                entity.Property(e => e.GeoField).HasColumnName("geo_field");

                entity.Property(e => e.Maps)
                    .HasColumnName("maps")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.MapsSubmissionDate)
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

                entity.Property(e => e.UserDataentry).HasColumnName("user_app_dataentry");

                entity.Property(e => e.UserAppModDt)
                    .HasColumnName("user_app_mod_dt")
                    .HasColumnType("datetime");

                entity.Property(e => e.UserAppModifed).HasColumnName("user_app_modifed");

                entity.Property(e => e.AreaAcres).HasColumnName("geo_field_area_sq_m");

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

                
                entity.HasOne(d => d.County);

                entity.HasOne(d => d.Variety);

                entity.HasOne(d => d.AppTypeTrans).WithMany(p => p.Applications).HasForeignKey(d => d.AppType).HasPrincipalKey(p => p.Abbreviation);

                entity.HasMany(d => d.Certificates);

                entity.HasMany(d => d.PlantingStocks);

                entity.HasMany(d => d.FieldHistories);

                entity.HasMany(d => d.TurfgrassCertificates);

                entity.HasMany(d => d.FieldInspection);
                entity.HasOne(d => d.AppCertRad).WithMany(c => c.Applications).HasForeignKey(d => d.CertYear).HasPrincipalKey(c => c.CertYear);
                entity.HasOne(d => d.AppCertRad).WithMany(c => c.Applications).HasForeignKey(d => d.CertNum).HasPrincipalKey(c => c.CertNum);
                entity.HasMany(d => d.Changes);
                entity.HasOne(d => d.FieldInspectionReport);
                entity.HasOne(d => d.Ecoregion);
                entity.HasOne(d => d.PotatoHealthCertificate);

            });

            modelBuilder.Entity<BlendRequests>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("blend_requests");

                entity.Property(e => e.Id).HasColumnName("blend_id");

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

                entity.HasOne(e => e.Conditioner);
            });

            modelBuilder.Entity<CertRad>(entity =>
            {
                entity.HasKey(e => new { e.CertNum, e.CertYear, e.Rad });

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

                entity.HasOne(e => e.Variety);

                entity.HasOne(e => e.Class);

                entity.HasOne(e => e.ApplicantOrganization);
            });

            modelBuilder.Entity<ChangeRequests>(entity =>
            {
                entity.HasKey(e => e.ChangeId);

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

                entity.Property(e => e.BatchNumber)
                    .HasColumnName("batchno")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.ChargeAmount)
                    .HasColumnName("charge_amt")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.ChargeCategory)
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

                entity.Property(e => e.Deletecharge)
                    .HasColumnName("delcharge")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.HoldCheck)
                    .HasColumnName("holdchk")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.HoldDate)
                    .HasColumnName("holddt")
                    .HasColumnType("datetime");

               
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

                entity.Property(e => e.Id).HasColumnName("cond_status_id");

                entity.Property(e => e.AllowPretag)
                    .HasColumnName("allow_pretag")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("cond_status")
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.DateUpdated)
                    .HasColumnName("cond_update")
                    .HasColumnType("datetime");

                entity.Property(e => e.Year).HasColumnName("cond_year");

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
                entity.HasKey(e => e.Id);
                //entity.HasKey(e => new { e.ContactId, e.AddressId });

                entity.ToTable("contact_address");

                entity.Property(e => e.ContactId).HasColumnName("contact_id");

                entity.Property(e => e.AddressId).HasColumnName("address_id");

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Billing)
                    .HasColumnName("billing")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Id)
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

            modelBuilder.Entity<OrganizationAddress>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("organization_address");

                entity.Property(e => e.OrgId).HasColumnName("org_id");

                entity.Property(e => e.AddressId).HasColumnName("address_id");

                entity.Property(e => e.Active)
                    .HasColumnName("active");

                entity.Property(e => e.Billing)
                    .HasColumnName("billing")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();
               
                entity.Property(e => e.Delivery)
                    .HasColumnName("delivery")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Mailing)
                    .HasColumnName("mailing")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Physical)
                    .HasColumnName("physical")
                    .HasDefaultValueSql("((0))");
               
            });

            modelBuilder.Entity<Organizations>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("Organization");

                entity.Property(e => e.Id).HasColumnName("org_id");

                entity.Property(e => e.Name).HasColumnName("org_name");

                entity.Property(e => e.AddressId).HasColumnName("address_id");
                
                entity.Property(e => e.Phone).HasColumnName("main_phone");

                entity.Property(e => e.Email).HasColumnName("main_email");

                entity.Property(e => e.CountyId).HasColumnName("county_id");

                entity.Property(e => e.GermLab).HasColumnName("germination_lab");

                entity.Property(e => e.Fax).HasColumnName("main_fax");

                entity.Property(e => e.Website).HasColumnName("website");

                //entity.Property(e => e.FoundationSeedGrower).HasColumnName("foundation_seed_grower");

                entity.Property(e => e.DiagnosticLab).HasColumnName("diagnostic_lab");

                entity.Property(e => e.AgCommissioner).HasColumnName("ag_comm_off");

                entity.Property(e => e.District).HasColumnName("district");

                entity.Property(e => e.Member).HasColumnName("ccia_member");

                entity.Property(e => e.MemberYear).HasColumnName("member_year");

                entity.Property(e => e.MemberType).HasColumnName("member_type");

                entity.Property(e => e.LastMemberAgreement).HasColumnName("last_member_agreement");

                entity.Property(e => e.MemberSince).HasColumnName("member_since");

                entity.Property(e => e.RepresentativeContactId).HasColumnName("member_rep_id");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.UserModified).HasColumnName("user_modified");

                entity.Property(e => e.DateModified).HasColumnName("date_modified");

                entity.Property(e => e.Notes).HasColumnName("notes");

                entity.Property(e => e.AppYearAgree).HasColumnName("app_agree_accept");

               // entity.Property(e => e.LacYearAgree).HasColumnName("lac_agree_accept");

                entity.Property(e => e.AlfalfaGMOPinning).HasColumnName("allow_alfalfa_gmo_pinning");
                
                entity.HasOne(e => e.OrgCounty);

                entity.HasOne(e => e.Address);

                entity.HasOne(e => e.RepresentativeContact);

                entity.HasMany(e => e.Employees); 

                entity.HasMany(e => e.ConditionerStatus);  

                entity.HasMany(e => e.MapPermissions);             

                
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

                entity.Property(e => e.BusinessPhone)
                    .HasColumnName("bus_phone")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessPhoneExtension)
                    .HasColumnName("bus_phone_ext")
                    .HasMaxLength(50)
                    .IsUnicode(false);

              
               

                entity.Property(e => e.Email)
                    .HasColumnName("email_addr")
                    .HasMaxLength(100)
                    .IsUnicode(false);

               

                entity.Property(e => e.FaxNumber)
                    .HasColumnName("fax_no")
                    .HasMaxLength(30)
                    .IsUnicode(false);

               

                entity.Property(e => e.HomePhone)
                    .HasColumnName("home_phone")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.IdahoVegetableLastYearAgreement).HasColumnName("idaho_vegetable_last_year_agreement");

               

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasMaxLength(50);

               

                entity.Property(e => e.MiddleInitial)
                    .HasColumnName("mi")
                    .HasColumnType("char(1)");

                entity.Property(e => e.MobilePhone)
                    .HasColumnName("mobile_phone")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                

                entity.Property(e => e.PagerNumber)
                    .HasColumnName("pager_no")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordHash)
                    .HasColumnName("password_hash");

                entity.Property(e => e.Salt)
                    .HasColumnName("salt");
              
                entity.Property(e => e.Suffix)
                    .HasColumnName("suffix")
                    .HasMaxLength(50);

                entity.Property(e => e.SweetCornLastYearAgreement)
                    .HasColumnName("sweet_corn_last_year_agreement")
                    .HasDefaultValueSql("((2000))");               

               entity.HasMany(e => e.Addresses);

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

                entity.Property(e => e.US).HasColumnName("us").HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<County>(entity =>
            {
                entity.ToTable("county");

                entity.Property(e => e.CountyId).HasColumnName("county_id");

                entity.Property(e => e.AgCommOrg).HasColumnName("ag_comm_org");

                entity.Property(e => e.Name)
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

                entity.HasOne(e => e.Crops).WithMany(c => c.CropStandards).HasForeignKey(e => e.CropId);
                entity.HasOne(e => e.Standards).WithMany(s => s.CropStandards).HasForeignKey(e => e.StdId);
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

            modelBuilder.Entity<FieldInspectionReport>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.ToTable("field_inspect");

                entity.Property(e => e.Id).HasColumnName("fldinsp_id");

                entity.Property(e => e.AcresApproved)
                    .HasColumnName("acres_approved")
                    .HasColumnType("decimal(14, 2)");

                entity.Property(e => e.AcresCancelled)
                    .HasColumnName("acres_cancelled")
                    .HasColumnType("decimal(14, 2)");            

                entity.Property(e => e.AcresGrowout)
                    .HasColumnName("acres_growout")
                    .HasColumnType("decimal(14, 2)");

                entity.Property(e => e.AcresInspectionOnly)
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

                entity.Property(e => e.Comments)
                    .HasColumnName("fld_inspect_comments")
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.PassClass)
                    .HasColumnName("pass_class")
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

                entity.HasOne(e => e.CompleteEmployee);

                entity.HasOne(e => e.POPassClass);

                entity.Property(e => e.PotatoPoundsHarvested).HasColumnName("potato_pounds_harvested");
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

                entity.Property(e => e.UserEmpModified).HasColumnName("user_emp_modified");

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
                //entity.HasOne(d => d.Application);

            });

            modelBuilder.Entity<Rates>(entity =>
            {
                entity.HasKey(e => e.RateId);

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

                entity.Property(e => e.Name).HasColumnName("StateProvinceName")
                    .IsRequired()
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<SeedDocuments>(entity =>
            {
                entity.ToTable("seed_docs");

                entity.Property(e => e.Id).HasColumnName("seed_cert_id");

                entity.Property(e => e.SeedsId).HasColumnName("seeds_id");

                entity.Property(e => e.Name).HasColumnName("doc_name");

                entity.Property(e => e.Link).HasColumnName("doc_link");

                entity.Property(e => e.DocType).HasColumnName("doc_type");

                entity.HasOne(e => e.DocumentType);

            });

            modelBuilder.Entity<SeedsDocumentTypes>(entity => 
            {
                entity.ToTable("seed_doc_types");

                entity.Property(e => e.Id).HasColumnName("doc_type");

                entity.Property(e => e.Name).HasColumnName("type_trans");

                entity.Property(e => e.Order).HasColumnName("type_order");

                entity.Property(e =>e.Folder).HasColumnName("folder_name");

            });

            modelBuilder.Entity<VarCountries>(entity => 
            {
                entity.ToTable("var_countries");


                entity.Property(e => e.VarId).HasColumnName("var_id");
                

                entity.Property(e => e.CountryId).HasColumnName("country_id");

            });

            modelBuilder.Entity<SampleLabResults>(entity =>
            {
                entity.HasKey(e => e.SeedsId);

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

                entity.Property(e => e.HardSeedPercent)
                    .HasColumnName("germ_hard_seed")
                    .HasColumnType("numeric(8, 7)");

                entity.Property(e => e.GermPercent)
                    .HasColumnName("germ_percent")
                    .HasColumnType("numeric(8, 7)");

                entity.Property(e => e.GermResults)
                    .HasColumnName("germ_results")
                    .HasColumnType("char(1)");                

                entity.Property(e => e.InertComments)
                    .HasColumnName("inert_comments")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.InertPercent)
                    .HasColumnName("inert_percent")
                    .HasColumnType("numeric(8, 7)");                

                entity.Property(e => e.NoxiousComments)
                    .HasColumnName("noxious_comments")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.NoxiousCount).HasColumnName("noxious_count");

                entity.Property(e => e.NoxiousGrams)
                    .HasColumnName("noxious_grams")
                    .HasColumnType("numeric(7, 2)");               

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

                entity.Property(e => e.Comments)
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

                entity.Property(e => e.OtherKindPercent).HasColumnName("other_kind_percent").HasColumnType("numberic(8, 7)");

                entity.Property(e => e.OtherKindComments).HasColumnName("other_kind_comments");

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

                entity.Property(e => e.OECD)
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

                entity.Property(e => e.CCIACertified)
                    .HasColumnName("ccia_certified")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CCIACertifiedDate)
                    .HasColumnName("ccia_certified_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.CCIACertifier)
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

               

                entity.Property(e => e.GermplasmEntity)
                    .HasColumnName("germplasm_entity")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.HistoricalName)
                    .HasColumnName("historical_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.OECD)
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

                entity.Property(e =>e.Ecoregion).HasColumnName("ecoregion");

                entity.Property(e => e.Elevation).HasColumnName("elevation");

                entity.Property(e => e.HarvestCountyId).HasColumnName("county_harvested");

                entity.Property(e => e.StateHarvestedId).HasColumnName("state_harvested");
            });
        }
    }
}
