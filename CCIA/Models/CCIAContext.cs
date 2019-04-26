using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
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
        public virtual DbSet<Seedlab> Seedlab { get; set; }
        public virtual DbSet<SeedlabDodder> SeedlabDodder { get; set; }
        public virtual DbSet<SeedlabGermination> SeedlabGermination { get; set; }
        public virtual DbSet<SeedlabGerminationRead> SeedlabGerminationRead { get; set; }
        public virtual DbSet<SeedlabGerminationReplicate> SeedlabGerminationReplicate { get; set; }
        public virtual DbSet<SeedlabImpurity> SeedlabImpurity { get; set; }
        public virtual DbSet<SeedlabList> SeedlabList { get; set; }
        public virtual DbSet<SeedlabNoxiousWeed> SeedlabNoxiousWeed { get; set; }
        public virtual DbSet<SeedlabNoxiousWeedList> SeedlabNoxiousWeedList { get; set; }
        public virtual DbSet<SeedlabPurity> SeedlabPurity { get; set; }
        public virtual DbSet<SeedlabPurityLists> SeedlabPurityLists { get; set; }
        public virtual DbSet<SeedlabSeeds> SeedlabSeeds { get; set; }
        public virtual DbSet<StateProvince> StateProvince { get; set; }
        public virtual DbSet<SxLabResults> SxLabResults { get; set; }
        public virtual DbSet<VarFamily> VarFamily { get; set; }
        public virtual DbSet<VarOfficial> VarOfficial { get; set; }
        public virtual DbSet<VarFull> VarFull { get; set; }

        // Unable to generate entity type for table 'dbo.map_radish_isolation'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.fir_docs'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.seed_doc_types'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.map_cucumber_isolation'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.seedlab_task'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.map_cucurbita_isolation'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.field_history'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.turfgrass_certificates'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.idaho_brassica_radish_isolation'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.tag_series'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.ecoregions'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.map_objects'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.map_typelut'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.application_changes'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.oecd'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.map_sweetcorn_isolation'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.map_croppts_app_listing'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.my_customers'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.map_crop_access'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.renew_actions_trans'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.meridians'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.tag_docs'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.trs'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.bulk_sales_certificate_shares'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.seed_transfers'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.organization_deleted'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.random_seeds2015'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.organization'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.seeds_changes'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.tags'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.map_user_access'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.blend_docs'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.var_countries'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Sheet1$'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.bulk_sales_certificates'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.map_alfalfa_gefree_isolation'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.blend_components_changes'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.abbrev_tag_type'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.seeds'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.seed_docs'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.duplicateOrgs'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.oecd_changes'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.renew_fields'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.tags_changes'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.abbrev_oecd_class'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.standards'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.idaho_beta_isolation'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.idaho_lists'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.field_results'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.sx_lab_results_changes'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.po_cert_history'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.po_health_cert'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.abbrev_class_seeds'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.seeds_apps'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.map_croppts_app'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.ccia_employees'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.org_address'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.idaho_carrot_isolation'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.app_certificates'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.blend_indirt_components'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.map_onion_isolation'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.map_carrot_isolation'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.random_samples'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.map_brassica_isolation'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.crop_assign'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.Jobs'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.contact_map'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.org_map'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.bulk_sales_certificates_changes'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.notices'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.blend_components'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.idaho_onion_isolation'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.seed_transfer_changes'. Please see the warning messages.

        //  public static readonly LoggerFactory DbCommandConsoleLoggerFactory
        //     = new LoggerFactory (new [] {
        //         new ConsoleLoggerProvider ((category, level) => category == DbLoggerCategory.Database.Command.Name &&
        //             level == LogLevel.Debug, true)
        //     });
        

    public static readonly LoggerFactory James = new LoggerFactory(new [] {
        new ConsoleLoggerProvider((category, level) => category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Debug, true)
    });


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=cherry01;Database=CCIA-Azure-Dev;Trusted_Connection=True;");
            }
            optionsBuilder.UseLoggerFactory(James).EnableSensitiveDataLogging();
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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

               entity.HasOne(d => d.FHCrops).WithMany(p => p.FieldHistories).HasForeignKey(d => d.Crop).HasPrincipalKey(p => p.CropId);
               

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

                entity.Property(e => e.GrowerSameAsApplicant)
                    .HasColumnName("grower_same_as_applicant");
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

                entity.Property(e => e.Meridian).HasColumnName("meridian");

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

                entity.Property(e => e.Range)
                    .HasColumnName("range")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Renewal)
                    .HasColumnName("renewal")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Section)
                    .HasColumnName("section")
                    .HasMaxLength(10)
                    .IsUnicode(false);

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

                entity.Property(e => e.Township)
                    .HasColumnName("township")
                    .HasMaxLength(10)
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

                entity.HasOne(d => d.ClassProduced)
                    .WithMany(p => p.Applications)
                    .HasForeignKey(d => d.ClassProducedId);

                entity.HasOne(d => d.Crop)
                    .WithMany(p => p.Applications)
                    .HasForeignKey(d => d.CropId)
                    .HasConstraintName("FK_Applications_Crops");

                entity.HasOne(d => d.GrowerOrganization)
                    .WithMany(p => p.GrownApplications)
                    .HasForeignKey(d => d.GrowerId);

                entity.HasOne(d => d.ApplicantOrganization)
                    .WithMany(p => p.AppliedApplications)
                    .HasForeignKey(d => d.ApplicantId);



                entity.HasOne(d => d.TraceNavigation)
                    .WithMany(p => p.InverseTraceNavigation)
                    .HasForeignKey(d => d.Trace)
                    .HasConstraintName("FK_Applications_Applications2");

                entity.HasOne(d => d.County)
                    .WithMany(p => p.Applications)
                    .HasForeignKey(d => d.FarmCounty);

                entity.HasOne(d => d.Variety)
                    .WithMany(p => p.Applications)
                    .HasForeignKey(d => d.SelectedVarietyId);
                
                entity.HasOne(d => d.AppTypeTrans).WithMany(p => p.Applications).HasForeignKey(d => d.AppType).HasPrincipalKey(p => p.Abbreviation);

                entity.HasMany(d => d.Certificates).WithOne(p => p.Application).HasForeignKey(d => d.AppId);

                entity.HasMany(d => d.PlantingStocks).WithOne(p => p.Applications).HasForeignKey(d => d.AppId);

                entity.HasMany(d => d.FieldHistories).WithOne(p => p.Application).HasForeignKey(d => d.AppId).HasForeignKey(p => p.AppId);

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

                entity.Property(e => e.Variety).HasColumnName("variety");
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
                entity.HasKey(e => e.ContactId);

                entity.ToTable("contacts");

                entity.Property(e => e.ContactId).HasColumnName("contact_id");

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

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasMaxLength(50);

                entity.Property(e => e.FormOfAddr)
                    .HasColumnName("form_of_addr")
                    .HasMaxLength(25)
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

                entity.Property(e => e.OrgId).HasColumnName("org_id");

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

                entity.Property(e => e.Title)
                    .HasColumnName("title")
                    .HasMaxLength(50)
                    .IsUnicode(false);

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
                entity.HasKey(e => e.CountryId);

                entity.ToTable("countries");

                entity.Property(e => e.CountryId).HasColumnName("country_id");

                entity.Property(e => e.CountryCode)
                    .IsRequired()
                    .HasColumnName("country_code")
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.CountryName)
                    .IsRequired()
                    .HasColumnName("country_name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DateModified)
                    .HasColumnName("date_modified")
                    .HasColumnType("datetime");

                entity.Property(e => e.OecdMember)
                    .HasColumnName("oecd_member")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.UserModified)
                    .HasColumnName("user_modified")
                    .HasMaxLength(9)
                    .IsUnicode(false);
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

                entity.HasOne(d => d.GrownStateProvince).WithMany(p => p.GrownInPlantingStocks).HasForeignKey(d => d.StateCountryGrown);
                entity.HasOne(d => d.TaggedStateProvince).WithMany(p => p.TaggedInPlantingStocks).HasForeignKey(d => d.StateCountryTagIssued);
                
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

            modelBuilder.Entity<Seedlab>(entity =>
            {
                entity.HasKey(e => e.LabId)
                    .ForSqlServerIsClustered(false);

                entity.ToTable("seedlab");

                entity.Property(e => e.LabId).HasColumnName("lab_id");

                entity.Property(e => e.AppEnteredBy)
                    .IsRequired()
                    .HasColumnName("app_entered_by")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.AppEnteredDate)
                    .HasColumnName("app_entered_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.AppUpdatedBy)
                    .HasColumnName("app_updated_by")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.AppUpdatedDate)
                    .HasColumnName("app_updated_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.ClearlyMarkedForCert)
                    .HasColumnName("clearly_marked_for_cert")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Comments)
                    .HasColumnName("comments")
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.Condition)
                    .HasColumnName("condition")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DateDivided)
                    .HasColumnName("date_divided")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateReceived)
                    .HasColumnName("date_received")
                    .HasColumnType("date");

                entity.Property(e => e.DividedBy)
                    .HasColumnName("divided_by")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.HasSampleForm)
                    .HasColumnName("has_sample_form")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.InsufficientSizeSample)
                    .HasColumnName("insufficient_size_sample")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.LabYear).HasColumnName("lab_year");

                entity.Property(e => e.RecordedBy)
                    .IsRequired()
                    .HasColumnName("recorded_by")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.SampleType)
                    .HasColumnName("sample_type")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SeedsId).HasColumnName("seeds_id");

                entity.Property(e => e.Weight).HasColumnName("weight");
            });

            modelBuilder.Entity<SeedlabDodder>(entity =>
            {
                entity.HasKey(e => e.LabId)
                    .ForSqlServerIsClustered(false);

                entity.ToTable("seedlab_dodder");

                entity.Property(e => e.LabId)
                    .HasColumnName("lab_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Comments)
                    .HasColumnName("comments")
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.CompletedBy)
                    .HasColumnName("completed_by")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DateCompleted)
                    .HasColumnName("date_completed")
                    .HasColumnType("date");

                entity.Property(e => e.Dodder)
                    .HasColumnName("dodder")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.WeightDodder)
                    .HasColumnName("weight_dodder")
                    .HasColumnType("numeric(7, 2)");

                entity.Property(e => e.WeightWorkingSample)
                    .HasColumnName("weight_working_sample")
                    .HasColumnType("numeric(18, 4)");
            });

            modelBuilder.Entity<SeedlabGermination>(entity =>
            {
                entity.HasKey(e => e.LabId)
                    .ForSqlServerIsClustered(false);

                entity.ToTable("seedlab_germination");

                entity.Property(e => e.LabId)
                    .HasColumnName("lab_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CalcAbnormal).HasColumnName("calc_abnormal");

                entity.Property(e => e.CalcDead).HasColumnName("calc_dead");

                entity.Property(e => e.CalcDormant).HasColumnName("calc_dormant");

                entity.Property(e => e.CalcGerm).HasColumnName("calc_germ");

                entity.Property(e => e.CalcHard).HasColumnName("calc_hard");

                entity.Property(e => e.Comments)
                    .HasColumnName("comments")
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.DatePlanted)
                    .HasColumnName("date_planted")
                    .HasColumnType("date");

                entity.Property(e => e.InsufficientSizeGerm)
                    .HasColumnName("insufficient_size_germ")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.NumSeedsPlanted).HasColumnName("num_seeds_planted");

                entity.Property(e => e.PreChill)
                    .HasColumnName("pre_chill")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.PreChillDays).HasColumnName("pre_chill_days");

                entity.Property(e => e.Replicates).HasColumnName("replicates");

                entity.Property(e => e.ReportGerm).HasColumnName("report_germ");

                entity.Property(e => e.ReportHard).HasColumnName("report_hard");

                entity.Property(e => e.StartedBy)
                    .HasColumnName("started_by")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Substrate)
                    .HasColumnName("substrate")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Temperature)
                    .HasColumnName("temperature")
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SeedlabGerminationRead>(entity =>
            {
                entity.HasKey(e => e.ReadId)
                    .ForSqlServerIsClustered(false);

                entity.ToTable("seedlab_germination_read");

                entity.Property(e => e.ReadId).HasColumnName("read_id");

                entity.Property(e => e.Comments)
                    .HasColumnName("comments")
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.DateRead)
                    .HasColumnName("date_read")
                    .HasColumnType("date");

                entity.Property(e => e.Final)
                    .HasColumnName("final")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.LabId).HasColumnName("lab_id");

                entity.Property(e => e.ReadBy)
                    .HasColumnName("read_by")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SeedlabGerminationReplicate>(entity =>
            {
                entity.HasKey(e => e.RepId)
                    .ForSqlServerIsClustered(false);

                entity.ToTable("seedlab_germination_replicate");

                entity.Property(e => e.RepId).HasColumnName("rep_id");

                entity.Property(e => e.AbnormalSeed).HasColumnName("abnormal_seed");

                entity.Property(e => e.DeadSeed).HasColumnName("dead_seed");

                entity.Property(e => e.DormantFresh).HasColumnName("dormant_fresh");

                entity.Property(e => e.GermCount).HasColumnName("germ_count");

                entity.Property(e => e.HardCount).HasColumnName("hard_count");

                entity.Property(e => e.ReadId).HasColumnName("read_id");

                entity.Property(e => e.Remainder).HasColumnName("remainder");

                entity.Property(e => e.RepNum).HasColumnName("rep_num");
            });

            modelBuilder.Entity<SeedlabImpurity>(entity =>
            {
                entity.HasKey(e => e.ImpurityListId)
                    .ForSqlServerIsClustered(false);

                entity.ToTable("seedlab_impurity");

                entity.Property(e => e.ImpurityListId).HasColumnName("impurity_list_id");

                entity.Property(e => e.Fraction)
                    .IsRequired()
                    .HasColumnName("fraction")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ImpurityType)
                    .IsRequired()
                    .HasColumnName("impurity_type")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LabId).HasColumnName("lab_id");

                entity.Property(e => e.ListId).HasColumnName("list_id");

                entity.Property(e => e.NumberFound).HasColumnName("number_found");

                entity.Property(e => e.ReportRate).HasColumnName("report_rate");
            });

            modelBuilder.Entity<SeedlabList>(entity =>
            {
                entity.HasKey(e => e.ListId);

                entity.ToTable("seedlab_list");

                entity.HasIndex(e => new { e.Genus, e.Species, e.Subspecies, e.CommonName })
                    .HasName("IX_seedlab_list")
                    .IsUnique();

                entity.Property(e => e.ListId).HasColumnName("list_id");

                entity.Property(e => e.CommonName)
                    .IsRequired()
                    .HasColumnName("common_name")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Genus)
                    .IsRequired()
                    .HasColumnName("genus")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ListName)
                    .IsRequired()
                    .HasColumnName("list_name")
                    .HasMaxLength(655)
                    .IsUnicode(false)
                    .HasComputedColumnSql("(((([genus]+isnull(' '+[species],''))+isnull(' '+[subspecies],''))+' | ')+[common_name])");

                entity.Property(e => e.Noxious)
                    .HasColumnName("noxious")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.NoxiousType)
                    .HasColumnName("noxious_type")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ScientificName)
                    .IsRequired()
                    .HasColumnName("scientific_name")
                    .HasMaxLength(152)
                    .IsUnicode(false)
                    .HasComputedColumnSql("(([genus]+isnull(' '+[species],''))+isnull(' '+[subspecies],''))");

                entity.Property(e => e.Species)
                    .HasColumnName("species")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Subspecies)
                    .HasColumnName("subspecies")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SeedlabNoxiousWeed>(entity =>
            {
                entity.HasKey(e => e.LabId)
                    .ForSqlServerIsClustered(false);

                entity.ToTable("seedlab_noxious_weed");

                entity.Property(e => e.LabId)
                    .HasColumnName("lab_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Comments)
                    .HasColumnName("comments")
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.CompletedBy)
                    .HasColumnName("completed_by")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DateCompleted)
                    .HasColumnName("date_completed")
                    .HasColumnType("date");

                entity.Property(e => e.DateDodder)
                    .HasColumnName("date_dodder")
                    .HasColumnType("date");

                entity.Property(e => e.DodderComments)
                    .HasColumnName("dodder_comments")
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.DodderCompletedBy)
                    .HasColumnName("dodder_completed_by")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DodderCount).HasColumnName("dodder_count");

                entity.Property(e => e.DodderResult)
                    .HasColumnName("dodder_result")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InsufficientSizeDodder)
                    .HasColumnName("insufficient_size_dodder")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.InsufficientSizeNoxious)
                    .HasColumnName("insufficient_size_noxious")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.RedriceComments)
                    .HasColumnName("redrice_comments")
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.RedriceCompletedBy)
                    .HasColumnName("redrice_completed_by")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RedriceCount).HasColumnName("redrice_count");

                entity.Property(e => e.RedriceDate)
                    .HasColumnName("redrice_date")
                    .HasColumnType("date");

                entity.Property(e => e.RedriceInsufficientSize)
                    .HasColumnName("redrice_insufficient_size")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.RedriceResult)
                    .HasColumnName("redrice_result")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RedriceWeight).HasColumnName("redrice_weight");

                entity.Property(e => e.WeightDodder).HasColumnName("weight_dodder");

                entity.Property(e => e.WeightWorkingSample).HasColumnName("weight_working_sample");
            });

            modelBuilder.Entity<SeedlabNoxiousWeedList>(entity =>
            {
                entity.HasKey(e => e.NoxiousListId)
                    .ForSqlServerIsClustered(false);

                entity.ToTable("seedlab_noxious_weed_list");

                entity.Property(e => e.NoxiousListId).HasColumnName("noxious_list_id");

                entity.Property(e => e.LabId).HasColumnName("lab_id");

                entity.Property(e => e.ListId).HasColumnName("list_id");

                entity.Property(e => e.NumberFound).HasColumnName("number_found");

                entity.Property(e => e.ReportRate).HasColumnName("report_rate");
            });

            modelBuilder.Entity<SeedlabPurity>(entity =>
            {
                entity.HasKey(e => e.LabId)
                    .ForSqlServerIsClustered(false);

                entity.ToTable("seedlab_purity");

                entity.Property(e => e.LabId)
                    .HasColumnName("lab_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.BadlyDiscoloredGrams).HasColumnName("badly_discolored_grams");

                entity.Property(e => e.BushelWeight).HasColumnName("bushel_weight");

                entity.Property(e => e.CalcBadlyDiscolored)
                    .HasColumnName("calc_badly_discolored")
                    .HasColumnType("decimal(5, 2)");

                entity.Property(e => e.CalcChewingInsectDamage)
                    .HasColumnName("calc_chewing_insect_damage")
                    .HasColumnType("decimal(5, 2)");

                entity.Property(e => e.CalcForeignMaterial)
                    .HasColumnName("calc_foreign_material")
                    .HasColumnType("decimal(5, 2)");

                entity.Property(e => e.CalcInert)
                    .HasColumnName("calc_inert")
                    .HasColumnType("decimal(5, 2)");

                entity.Property(e => e.CalcOtherCrop)
                    .HasColumnName("calc_other_crop")
                    .HasColumnType("decimal(5, 2)");

                entity.Property(e => e.CalcPurity)
                    .HasColumnName("calc_purity")
                    .HasColumnType("decimal(5, 2)");

                entity.Property(e => e.CalcSplitsAndCracks)
                    .HasColumnName("calc_splits_and_cracks")
                    .HasColumnType("decimal(5, 2)");

                entity.Property(e => e.CalcWeed)
                    .HasColumnName("calc_weed")
                    .HasColumnType("decimal(5, 2)");

                entity.Property(e => e.ChewingInsectDamageGrams).HasColumnName("chewing_insect_damage_grams");

                entity.Property(e => e.Comments)
                    .HasColumnName("comments")
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.CompletedBy)
                    .HasColumnName("completed_by")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DateCompleted)
                    .HasColumnName("date_completed")
                    .HasColumnType("date");

                entity.Property(e => e.ForeignMaterialGrams).HasColumnName("foreign_material_grams");

                entity.Property(e => e.InertChaff)
                    .HasColumnName("inert_chaff")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.InertDescription)
                    .HasColumnName("inert_description")
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.InertDirt)
                    .HasColumnName("inert_dirt")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.InertGrams)
                    .HasColumnName("inert_grams")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.InertPlantFragments)
                    .HasColumnName("inert_plant_fragments")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.InertSeedFragments)
                    .HasColumnName("inert_seed_fragments")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.InertSoil)
                    .HasColumnName("inert_soil")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.InertStems)
                    .HasColumnName("inert_stems")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.InertTreatmentColorant)
                    .HasColumnName("inert_treatment_colorant")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.InsufficientSizePurity)
                    .HasColumnName("insufficient_size_purity")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.OtherCropCount).HasColumnName("other_crop_count");

                entity.Property(e => e.OtherCropGrams)
                    .HasColumnName("other_crop_grams")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.OtherVarietyCount).HasColumnName("other_variety_count");

                entity.Property(e => e.OtherVarietyGrams).HasColumnName("other_variety_grams");

                entity.Property(e => e.PureSeedGrams)
                    .HasColumnName("pure_seed_grams")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ReportBadlyDiscolored)
                    .HasColumnName("report_badly_discolored")
                    .HasColumnType("decimal(5, 2)");

                entity.Property(e => e.ReportChewingInsectDamage)
                    .HasColumnName("report_chewing_insect_damage")
                    .HasColumnType("decimal(5, 2)");

                entity.Property(e => e.ReportForeignMaterial)
                    .HasColumnName("report_foreign_material")
                    .HasColumnType("decimal(5, 2)");

                entity.Property(e => e.ReportInert)
                    .HasColumnName("report_inert")
                    .HasColumnType("decimal(5, 2)");

                entity.Property(e => e.ReportOtherCrop)
                    .HasColumnName("report_other_crop")
                    .HasColumnType("decimal(5, 2)");

                entity.Property(e => e.ReportOtherVariety)
                    .HasColumnName("report_other_variety")
                    .HasColumnType("decimal(5, 2)");

                entity.Property(e => e.ReportPurity)
                    .HasColumnName("report_purity")
                    .HasColumnType("decimal(5, 2)");

                entity.Property(e => e.ReportSplitsCracks)
                    .HasColumnName("report_splits_cracks")
                    .HasColumnType("decimal(5, 2)");

                entity.Property(e => e.ReportWeed)
                    .HasColumnName("report_weed")
                    .HasColumnType("decimal(5, 2)");

                entity.Property(e => e.SplitsCracksGrams).HasColumnName("splits_cracks_grams");

                entity.Property(e => e.WeedSeedCount).HasColumnName("weed_seed_count");

                entity.Property(e => e.WeedSeedGrams)
                    .HasColumnName("weed_seed_grams")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.WeightWorkingSample).HasColumnName("weight_working_sample");
            });

            modelBuilder.Entity<SeedlabPurityLists>(entity =>
            {
                entity.HasKey(e => e.PurityListId)
                    .ForSqlServerIsClustered(false);

                entity.ToTable("seedlab_purity_lists");

                entity.HasIndex(e => new { e.LabId, e.Type, e.ListId })
                    .HasName("IX_seedlab_purity_lists")
                    .IsUnique();

                entity.Property(e => e.PurityListId).HasColumnName("purity_list_id");

                entity.Property(e => e.Count).HasColumnName("count");

                entity.Property(e => e.Grams).HasColumnName("grams");

                entity.Property(e => e.LabId).HasColumnName("lab_id");

                entity.Property(e => e.ListId).HasColumnName("list_id");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasColumnName("type")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SeedlabSeeds>(entity =>
            {
                entity.HasKey(e => e.LabId)
                    .ForSqlServerIsClustered(false);

                entity.ToTable("seedlab_seeds");

                entity.Property(e => e.LabId)
                    .HasColumnName("lab_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CertNumber)
                    .HasColumnName("cert_number")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CondId).HasColumnName("cond_id");

                entity.Property(e => e.CondText)
                    .HasColumnName("cond_text")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CropId).HasColumnName("crop_id");

                entity.Property(e => e.LotNumber)
                    .HasColumnName("lot_number")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LotSize)
                    .HasColumnName("lot_size")
                    .HasColumnType("numeric(16, 2)");

                entity.Property(e => e.SeedLab)
                    .HasColumnName("seed_lab")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SubmittingLabNumber)
                    .HasColumnName("submitting_lab_number")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Treated)
                    .HasColumnName("treated")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.VarOffId).HasColumnName("var_off_id");

                entity.Property(e => e.VarietyName)
                    .HasColumnName("variety_name")
                    .HasMaxLength(50)
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
