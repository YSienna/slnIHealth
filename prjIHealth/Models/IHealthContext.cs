using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace prjIHealth.Models
{
    public partial class IHealthContext : DbContext
    {
        public IHealthContext()
        {
        }

        public IHealthContext(DbContextOptions<IHealthContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TAuthority> TAuthorities { get; set; }
        public virtual DbSet<TAvailableTime> TAvailableTimes { get; set; }
        public virtual DbSet<TBodyRecord> TBodyRecords { get; set; }
        public virtual DbSet<TCalorieIntake> TCalorieIntakes { get; set; }
        public virtual DbSet<TCandidate> TCandidates { get; set; }
        public virtual DbSet<TCity> TCities { get; set; }
        public virtual DbSet<TCoach> TCoaches { get; set; }
        public virtual DbSet<TCoachAvailableTime> TCoachAvailableTimes { get; set; }
        public virtual DbSet<TCoachContact> TCoachContacts { get; set; }
        public virtual DbSet<TCoachExperience> TCoachExperiences { get; set; }
        public virtual DbSet<TCoachLicense> TCoachLicenses { get; set; }
        public virtual DbSet<TCoachRate> TCoachRates { get; set; }
        public virtual DbSet<TCoachSkill> TCoachSkills { get; set; }
        public virtual DbSet<TContactText> TContactTexts { get; set; }
        public virtual DbSet<TCourse> TCourses { get; set; }
        public virtual DbSet<TDiscount> TDiscounts { get; set; }
        public virtual DbSet<TFoodCalory> TFoodCalories { get; set; }
        public virtual DbSet<TMember> TMembers { get; set; }
        public virtual DbSet<TNews> TNews { get; set; }
        public virtual DbSet<TNewsCategory> TNewsCategories { get; set; }
        public virtual DbSet<TNewsComment> TNewsComments { get; set; }
        public virtual DbSet<TNewsImage> TNewsImages { get; set; }
        public virtual DbSet<TOrder> TOrders { get; set; }
        public virtual DbSet<TOrderDetail> TOrderDetails { get; set; }
        public virtual DbSet<TPaymentCategory> TPaymentCategories { get; set; }
        public virtual DbSet<TProblem> TProblems { get; set; }
        public virtual DbSet<TProblemCategroie> TProblemCategroies { get; set; }
        public virtual DbSet<TProduct> TProducts { get; set; }
        public virtual DbSet<TProductCategory> TProductCategories { get; set; }
        public virtual DbSet<TProductsImage> TProductsImages { get; set; }
        public virtual DbSet<TRegion> TRegions { get; set; }
        public virtual DbSet<TReply> TReplies { get; set; }
        public virtual DbSet<TReservation> TReservations { get; set; }
        public virtual DbSet<TSkill> TSkills { get; set; }
        public virtual DbSet<TStatus> TStatuses { get; set; }
        public virtual DbSet<TTrackList> TTrackLists { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=IHealth;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Chinese_Taiwan_Stroke_CI_AS");

            modelBuilder.Entity<TAuthority>(entity =>
            {
                entity.HasKey(e => e.FAutorityId);

                entity.ToTable("tAuthority");

                entity.Property(e => e.FAutorityId).HasColumnName("fAutorityID");

                entity.Property(e => e.FAuthorityName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("fAuthorityName");

                entity.Property(e => e.FRemarks).HasColumnName("fRemarks");
            });

            modelBuilder.Entity<TAvailableTime>(entity =>
            {
                entity.HasKey(e => e.FAvailableTimeId);

                entity.ToTable("tAvailableTimes");

                entity.HasIndex(e => e.FAvailableTimeNum, "IX_tAvailableTimes")
                    .IsUnique();

                entity.Property(e => e.FAvailableTimeId).HasColumnName("fAvailableTimeID");

                entity.Property(e => e.FAvailableTime)
                    .HasMaxLength(50)
                    .HasColumnName("fAvailableTime");

                entity.Property(e => e.FAvailableTimeNum)
                    .IsRequired()
                    .HasColumnName("fAvailableTimeNum");
            });

            modelBuilder.Entity<TBodyRecord>(entity =>
            {
                entity.HasKey(e => e.FBodyRecordId);

                entity.ToTable("tBodyRecords");

                entity.Property(e => e.FBodyRecordId).HasColumnName("fBodyRecordID");

                entity.Property(e => e.FHeight).HasColumnName("fHeight");

                entity.Property(e => e.FMemberId).HasColumnName("fMemberID");

                entity.Property(e => e.FRecordDate)
                    .HasMaxLength(50)
                    .HasColumnName("fRecordDate");

                entity.Property(e => e.FWeight).HasColumnName("fWeight");

                entity.Property(e => e.FWorkload).HasColumnName("fWorkload");

                entity.HasOne(d => d.FMember)
                    .WithMany(p => p.TBodyRecords)
                    .HasForeignKey(d => d.FMemberId)
                    .HasConstraintName("FK_tBodyRecords_tMember");
            });

            modelBuilder.Entity<TCalorieIntake>(entity =>
            {
                entity.HasKey(e => e.FCalorieIntakeId);

                entity.ToTable("tCalorieIntake");

                entity.Property(e => e.FCalorieIntakeId).HasColumnName("fCalorieIntakeID");

                entity.Property(e => e.FFoodId).HasColumnName("fFoodID");

                entity.Property(e => e.FIntakeTime)
                    .HasMaxLength(50)
                    .HasColumnName("fIntakeTime");

                entity.Property(e => e.FMeal)
                    .HasMaxLength(50)
                    .HasColumnName("fMeal");

                entity.Property(e => e.FMemberId).HasColumnName("fMemberID");

                entity.Property(e => e.FQuantity).HasColumnName("fQuantity");

                entity.Property(e => e.FRemarks)
                    .HasMaxLength(50)
                    .HasColumnName("fRemarks");

                entity.HasOne(d => d.FFood)
                    .WithMany(p => p.TCalorieIntakes)
                    .HasForeignKey(d => d.FFoodId)
                    .HasConstraintName("FK_tCalorieIntake_tFoodCalories");

                entity.HasOne(d => d.FMember)
                    .WithMany(p => p.TCalorieIntakes)
                    .HasForeignKey(d => d.FMemberId)
                    .HasConstraintName("FK_tCalorieIntake_tMember");
            });

            modelBuilder.Entity<TCandidate>(entity =>
            {
                entity.HasKey(e => e.FCandidateId);

                entity.ToTable("tCandidates");

                entity.Property(e => e.FCandidateId).HasColumnName("fCandidateID");

                entity.Property(e => e.FCoachId).HasColumnName("fCoachID");

                entity.Property(e => e.FMemberId).HasColumnName("fMemberID");

                entity.HasOne(d => d.FCoach)
                    .WithMany(p => p.TCandidates)
                    .HasForeignKey(d => d.FCoachId)
                    .HasConstraintName("FK_tCandidates_tCoaches");

                entity.HasOne(d => d.FMember)
                    .WithMany(p => p.TCandidates)
                    .HasForeignKey(d => d.FMemberId)
                    .HasConstraintName("FK_tCandidates_tMember");
            });

            modelBuilder.Entity<TCity>(entity =>
            {
                entity.HasKey(e => e.FCityId);

                entity.ToTable("tCities");

                entity.Property(e => e.FCityId).HasColumnName("fCityID");

                entity.Property(e => e.FCityName)
                    .HasMaxLength(50)
                    .HasColumnName("fCityName");

                entity.Property(e => e.FCityOrder).HasColumnName("fCityOrder");
            });

            modelBuilder.Entity<TCoach>(entity =>
            {
                entity.HasKey(e => e.FCoachId);

                entity.ToTable("tCoaches");

                entity.Property(e => e.FCoachId).HasColumnName("fCoachID");

                entity.Property(e => e.FApplyDate)
                    .HasMaxLength(50)
                    .HasColumnName("fApplyDate");

                entity.Property(e => e.FCityId).HasColumnName("fCityID");

                entity.Property(e => e.FCoachDescription).HasColumnName("fCoachDescription");

                entity.Property(e => e.FCoachFee).HasColumnName("fCoachFee");

                entity.Property(e => e.FCoachImage)
                    .HasMaxLength(50)
                    .HasColumnName("fCoachImage");

                entity.Property(e => e.FCoachName)
                    .HasMaxLength(50)
                    .HasColumnName("fCoachName");

                entity.Property(e => e.FCourseCount).HasColumnName("fCourseCount");

                entity.Property(e => e.FMemberId).HasColumnName("fMemberID");

                entity.Property(e => e.FSlogan).HasColumnName("fSlogan");

                entity.Property(e => e.FStatusNumber).HasColumnName("fStatusNumber");

                entity.Property(e => e.FVisible).HasColumnName("fVisible");

                entity.HasOne(d => d.FCity)
                    .WithMany(p => p.TCoaches)
                    .HasForeignKey(d => d.FCityId)
                    .HasConstraintName("FK_tCoaches_tCities");

                entity.HasOne(d => d.FMember)
                    .WithMany(p => p.TCoaches)
                    .HasForeignKey(d => d.FMemberId)
                    .HasConstraintName("FK_tCoaches_tMember");

                entity.HasOne(d => d.FStatusNumberNavigation)
                    .WithMany(p => p.TCoaches)
                    .HasPrincipalKey(p => p.FStatusNumber)
                    .HasForeignKey(d => d.FStatusNumber)
                    .HasConstraintName("FK_tCoaches_tStatus");
            });

            modelBuilder.Entity<TCoachAvailableTime>(entity =>
            {
                entity.HasKey(e => e.FCoachTimeId);

                entity.ToTable("tCoachAvailableTime");

                entity.Property(e => e.FCoachTimeId).HasColumnName("fCoachTimeID");

                entity.Property(e => e.FAvailableTimeId).HasColumnName("fAvailableTimeID");

                entity.Property(e => e.FCoachId).HasColumnName("fCoachID");

                entity.HasOne(d => d.FAvailableTime)
                    .WithMany(p => p.TCoachAvailableTimes)
                    .HasForeignKey(d => d.FAvailableTimeId)
                    .HasConstraintName("FK_tCoachAvailableTime_tAvailableTimes");

                entity.HasOne(d => d.FCoach)
                    .WithMany(p => p.TCoachAvailableTimes)
                    .HasForeignKey(d => d.FCoachId)
                    .HasConstraintName("FK_tCoachAvailableTime_tCoaches");
            });

            modelBuilder.Entity<TCoachContact>(entity =>
            {
                entity.HasKey(e => e.FCoachContactId);

                entity.ToTable("tCoachContacts");

                entity.Property(e => e.FCoachContactId).HasColumnName("fCoachContactID");

                entity.Property(e => e.FAvailableTimeNum).HasColumnName("fAvailableTimeNum");

                entity.Property(e => e.FCoachId).HasColumnName("fCoachID");

                entity.Property(e => e.FContactDate)
                    .HasMaxLength(50)
                    .HasColumnName("fContactDate");

                entity.Property(e => e.FMemberId).HasColumnName("fMemberID");

                entity.Property(e => e.FStatusNumber).HasColumnName("fStatusNumber");

                entity.HasOne(d => d.FAvailableTimeNumNavigation)
                    .WithMany(p => p.TCoachContacts)
                    .HasPrincipalKey(p => p.FAvailableTimeNum)
                    .HasForeignKey(d => d.FAvailableTimeNum)
                    .HasConstraintName("FK_tCoachContacts_tAvailableTimes");

                entity.HasOne(d => d.FCoach)
                    .WithMany(p => p.TCoachContacts)
                    .HasForeignKey(d => d.FCoachId)
                    .HasConstraintName("FK_tCoachContacts_tCoaches");

                entity.HasOne(d => d.FMember)
                    .WithMany(p => p.TCoachContacts)
                    .HasForeignKey(d => d.FMemberId)
                    .HasConstraintName("FK_tCoachContacts_tMember");

                entity.HasOne(d => d.FStatusNumberNavigation)
                    .WithMany(p => p.TCoachContacts)
                    .HasPrincipalKey(p => p.FStatusNumber)
                    .HasForeignKey(d => d.FStatusNumber)
                    .HasConstraintName("FK_tCoachContacts_tStatus");
            });

            modelBuilder.Entity<TCoachExperience>(entity =>
            {
                entity.HasKey(e => e.FExperienceId)
                    .HasName("PK_tExperience");

                entity.ToTable("tCoachExperience");

                entity.Property(e => e.FExperienceId).HasColumnName("fExperienceID");

                entity.Property(e => e.FCoachId).HasColumnName("fCoachID");

                entity.Property(e => e.FExperience).HasColumnName("fExperience");

                entity.HasOne(d => d.FCoach)
                    .WithMany(p => p.TCoachExperiences)
                    .HasForeignKey(d => d.FCoachId)
                    .HasConstraintName("FK_tCoachExperience_tCoaches");
            });

            modelBuilder.Entity<TCoachLicense>(entity =>
            {
                entity.HasKey(e => e.FLicenseId);

                entity.ToTable("tCoachLicense");

                entity.Property(e => e.FLicenseId).HasColumnName("fLicenseID");

                entity.Property(e => e.FCoachId).HasColumnName("fCoachID");

                entity.Property(e => e.FLicense).HasColumnName("fLicense");

                entity.HasOne(d => d.FCoach)
                    .WithMany(p => p.TCoachLicenses)
                    .HasForeignKey(d => d.FCoachId)
                    .HasConstraintName("FK_tCoachLicense_tCoaches");
            });

            modelBuilder.Entity<TCoachRate>(entity =>
            {
                entity.HasKey(e => e.FRateId);

                entity.ToTable("tCoachRates");

                entity.Property(e => e.FRateId).HasColumnName("fRateID");

                entity.Property(e => e.FCoachId).HasColumnName("fCoachID");

                entity.Property(e => e.FMemberId).HasColumnName("fMemberID");

                entity.Property(e => e.FRateDate)
                    .HasMaxLength(50)
                    .HasColumnName("fRateDate");

                entity.Property(e => e.FRateStar).HasColumnName("fRateStar");

                entity.Property(e => e.FRateText).HasColumnName("fRateText");

                entity.Property(e => e.FVisible).HasColumnName("fVisible");

                entity.HasOne(d => d.FCoach)
                    .WithMany(p => p.TCoachRates)
                    .HasForeignKey(d => d.FCoachId)
                    .HasConstraintName("FK_tCoachRates_tCoaches");

                entity.HasOne(d => d.FMember)
                    .WithMany(p => p.TCoachRates)
                    .HasForeignKey(d => d.FMemberId)
                    .HasConstraintName("FK_tCoachRates_tMember");
            });

            modelBuilder.Entity<TCoachSkill>(entity =>
            {
                entity.HasKey(e => e.FCoachSkillId);

                entity.ToTable("tCoachSkill");

                entity.Property(e => e.FCoachSkillId).HasColumnName("fCoachSkillID");

                entity.Property(e => e.FCoachId).HasColumnName("fCoachID");

                entity.Property(e => e.FSkillId).HasColumnName("fSkillID");

                entity.HasOne(d => d.FCoach)
                    .WithMany(p => p.TCoachSkills)
                    .HasForeignKey(d => d.FCoachId)
                    .HasConstraintName("FK_tCoachSkill_tCoaches");

                entity.HasOne(d => d.FSkill)
                    .WithMany(p => p.TCoachSkills)
                    .HasForeignKey(d => d.FSkillId)
                    .HasConstraintName("FK_tCoachSkill_tSkills");
            });

            modelBuilder.Entity<TContactText>(entity =>
            {
                entity.HasKey(e => e.FContactTextId);

                entity.ToTable("tContactText");

                entity.Property(e => e.FContactTextId).HasColumnName("fContactTextID");

                entity.Property(e => e.FCoachContactId).HasColumnName("fCoachContactID");

                entity.Property(e => e.FContactText).HasColumnName("fContactText");

                entity.Property(e => e.FContactTextTime)
                    .HasMaxLength(50)
                    .HasColumnName("fContactTextTime");

                entity.Property(e => e.FIsCoach).HasColumnName("fIsCoach");

                entity.HasOne(d => d.FCoachContact)
                    .WithMany(p => p.TContactTexts)
                    .HasForeignKey(d => d.FCoachContactId)
                    .HasConstraintName("FK_tContactText_tCoachContacts");
            });

            modelBuilder.Entity<TCourse>(entity =>
            {
                entity.HasKey(e => e.FCourseId);

                entity.ToTable("tCourses");

                entity.Property(e => e.FCourseId).HasColumnName("fCourseID");

                entity.Property(e => e.FAvailableTimeNum).HasColumnName("fAvailableTimeNum");

                entity.Property(e => e.FCoachContactId).HasColumnName("fCoachContactID");

                entity.Property(e => e.FCourseName)
                    .HasMaxLength(50)
                    .HasColumnName("fCourseName");

                entity.Property(e => e.FCourseTotal).HasColumnName("fCourseTotal");

                entity.Property(e => e.FStatusNumber).HasColumnName("fStatusNumber");

                entity.Property(e => e.FVisible).HasColumnName("fVisible");

                entity.HasOne(d => d.FAvailableTimeNumNavigation)
                    .WithMany(p => p.TCourses)
                    .HasPrincipalKey(p => p.FAvailableTimeNum)
                    .HasForeignKey(d => d.FAvailableTimeNum)
                    .HasConstraintName("FK_tCourses_tAvailableTimes");

                entity.HasOne(d => d.FCoachContact)
                    .WithMany(p => p.TCourses)
                    .HasForeignKey(d => d.FCoachContactId)
                    .HasConstraintName("FK_tCourses_tCoachContacts");

                entity.HasOne(d => d.FStatusNumberNavigation)
                    .WithMany(p => p.TCourses)
                    .HasPrincipalKey(p => p.FStatusNumber)
                    .HasForeignKey(d => d.FStatusNumber)
                    .HasConstraintName("FK_tCourses_tStatus");
            });

            modelBuilder.Entity<TDiscount>(entity =>
            {
                entity.HasKey(e => e.FDiscountId);

                entity.ToTable("tDiscount");

                entity.Property(e => e.FDiscountId).HasColumnName("fDiscountID");

                entity.Property(e => e.FDiscountCode)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("fDiscountCode");

                entity.Property(e => e.FDiscountValue)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("fDiscountValue");

                entity.Property(e => e.FDiscretion)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("fDiscretion");
            });

            modelBuilder.Entity<TFoodCalory>(entity =>
            {
                entity.HasKey(e => e.FFoodId);

                entity.ToTable("tFoodCalories");

                entity.Property(e => e.FFoodId).HasColumnName("fFoodID");

                entity.Property(e => e.FCalories).HasColumnName("fCalories");

                entity.Property(e => e.FFoodName)
                    .HasMaxLength(50)
                    .HasColumnName("fFoodName");

                entity.Property(e => e.FUnit)
                    .HasMaxLength(50)
                    .HasColumnName("fUnit");
            });

            modelBuilder.Entity<TMember>(entity =>
            {
                entity.HasKey(e => e.FMemberId);

                entity.ToTable("tMember");

                entity.Property(e => e.FMemberId).HasColumnName("fMemberID");

                entity.Property(e => e.FAddress)
                    .HasMaxLength(250)
                    .HasColumnName("fAddress");

                entity.Property(e => e.FAuthorityId).HasColumnName("fAuthorityID");

                entity.Property(e => e.FBirthday)
                    .HasMaxLength(50)
                    .HasColumnName("fBirthday");

                entity.Property(e => e.FDisabled).HasColumnName("fDisabled");

                entity.Property(e => e.FEmail)
                    .HasMaxLength(50)
                    .HasColumnName("fEmail");

                entity.Property(e => e.FGender).HasColumnName("fGender");

                entity.Property(e => e.FMemberName)
                    .HasMaxLength(50)
                    .HasColumnName("fMemberName");

                entity.Property(e => e.FPassword)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("fPassword");

                entity.Property(e => e.FPhone)
                    .HasMaxLength(50)
                    .HasColumnName("fPhone");

                entity.Property(e => e.FPicturePath)
                    .HasMaxLength(50)
                    .HasColumnName("fPicturePath");

                entity.Property(e => e.FRegisterDate)
                    .HasMaxLength(50)
                    .HasColumnName("fRegisterDate");

                entity.Property(e => e.FRemarks)
                    .HasMaxLength(250)
                    .HasColumnName("fRemarks");

                entity.Property(e => e.FUserName)
                    .HasMaxLength(50)
                    .HasColumnName("fUserName");

                entity.HasOne(d => d.FAuthority)
                    .WithMany(p => p.TMembers)
                    .HasForeignKey(d => d.FAuthorityId)
                    .HasConstraintName("FK_tMember_tAuthority");
            });

            modelBuilder.Entity<TNews>(entity =>
            {
                entity.HasKey(e => e.FNewsId);

                entity.ToTable("tNews");

                entity.Property(e => e.FNewsId).HasColumnName("fNewsID");

                entity.Property(e => e.FContent).HasColumnName("fContent");

                entity.Property(e => e.FMemberId).HasColumnName("fMemberID");

                entity.Property(e => e.FNewsCategoryId).HasColumnName("fNewsCategoryID");

                entity.Property(e => e.FNewsDate)
                    .HasMaxLength(50)
                    .HasColumnName("fNewsDate");

                entity.Property(e => e.FThumbnailPath)
                    .HasMaxLength(50)
                    .HasColumnName("fThumbnailPath");

                entity.Property(e => e.FTitle)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("fTitle");

                entity.Property(e => e.FVideoUrl).HasColumnName("fVideoURL");

                entity.Property(e => e.FViews).HasColumnName("fViews");

                entity.HasOne(d => d.FMember)
                    .WithMany(p => p.TNews)
                    .HasForeignKey(d => d.FMemberId)
                    .HasConstraintName("FK_tNews_tMember");

                entity.HasOne(d => d.FNewsCategory)
                    .WithMany(p => p.TNews)
                    .HasForeignKey(d => d.FNewsCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tNews_tNewsCategories");
            });

            modelBuilder.Entity<TNewsCategory>(entity =>
            {
                entity.HasKey(e => e.FNewsCategoryId);

                entity.ToTable("tNewsCategories");

                entity.Property(e => e.FNewsCategoryId).HasColumnName("fNewsCategoryID");

                entity.Property(e => e.FCategoryName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("fCategoryName");
            });

            modelBuilder.Entity<TNewsComment>(entity =>
            {
                entity.HasKey(e => e.FNewsCommentId);

                entity.ToTable("tNewsComment");

                entity.Property(e => e.FNewsCommentId).HasColumnName("fNewsCommentID");

                entity.Property(e => e.FMemberId).HasColumnName("fMemberID");

                entity.Property(e => e.FNewsId).HasColumnName("fNewsID");

                entity.Property(e => e.FNewsReply)
                    .HasMaxLength(50)
                    .HasColumnName("fNewsReply");

                entity.HasOne(d => d.FMember)
                    .WithMany(p => p.TNewsComments)
                    .HasForeignKey(d => d.FMemberId)
                    .HasConstraintName("FK_tNewsComment_tMember");

                entity.HasOne(d => d.FNews)
                    .WithMany(p => p.TNewsComments)
                    .HasForeignKey(d => d.FNewsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tNewsComment_tNews");
            });

            modelBuilder.Entity<TNewsImage>(entity =>
            {
                entity.HasKey(e => e.FNewsImageId);

                entity.ToTable("tNewsImages");

                entity.Property(e => e.FNewsImageId).HasColumnName("fNewsImageID");

                entity.Property(e => e.FImagePath)
                    .HasMaxLength(50)
                    .HasColumnName("fImagePath");

                entity.Property(e => e.FNewsId).HasColumnName("fNewsID");

                entity.HasOne(d => d.FNews)
                    .WithMany(p => p.TNewsImages)
                    .HasForeignKey(d => d.FNewsId)
                    .HasConstraintName("FK_tNewsImages_tNews");
            });

            modelBuilder.Entity<TOrder>(entity =>
            {
                entity.HasKey(e => e.FOrderId);

                entity.ToTable("tOrders");

                entity.Property(e => e.FOrderId).HasColumnName("fOrderID");

                entity.Property(e => e.FAddress)
                    .IsRequired()
                    .HasColumnName("fAddress");

                entity.Property(e => e.FContact)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("fContact");

                entity.Property(e => e.FDate)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("fDate");

                entity.Property(e => e.FMemberId).HasColumnName("fMemberID");

                entity.Property(e => e.FPaymentCategoryId).HasColumnName("fPaymentCategoryID");

                entity.Property(e => e.FPhone)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("fPhone");

                entity.Property(e => e.FRemarks).HasColumnName("fRemarks");

                entity.Property(e => e.FStatusNumber).HasColumnName("fStatusNumber");

                entity.HasOne(d => d.FMember)
                    .WithMany(p => p.TOrders)
                    .HasForeignKey(d => d.FMemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tOrders_tMember");

                entity.HasOne(d => d.FPaymentCategory)
                    .WithMany(p => p.TOrders)
                    .HasForeignKey(d => d.FPaymentCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tOrders_tPaymentCategories");

                entity.HasOne(d => d.FStatusNumberNavigation)
                    .WithMany(p => p.TOrders)
                    .HasPrincipalKey(p => p.FStatusNumber)
                    .HasForeignKey(d => d.FStatusNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tOrders_tStatus");
            });

            modelBuilder.Entity<TOrderDetail>(entity =>
            {
                entity.HasKey(e => e.FOrderDetailsId);

                entity.ToTable("tOrderDetails");

                entity.Property(e => e.FOrderDetailsId).HasColumnName("fOrderDetailsID");

                entity.Property(e => e.FDiscountId).HasColumnName("fDiscountID");

                entity.Property(e => e.FOrderId).HasColumnName("fOrderID");

                entity.Property(e => e.FProductId).HasColumnName("fProductID");

                entity.Property(e => e.FQuantity).HasColumnName("fQuantity");

                entity.Property(e => e.FUnitprice)
                    .HasColumnType("money")
                    .HasColumnName("fUnitprice");

                entity.HasOne(d => d.FDiscount)
                    .WithMany(p => p.TOrderDetails)
                    .HasForeignKey(d => d.FDiscountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tOrderDetails_tDiscount");

                entity.HasOne(d => d.FOrder)
                    .WithMany(p => p.TOrderDetails)
                    .HasForeignKey(d => d.FOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tOrderDetails_tOrders");

                entity.HasOne(d => d.FProduct)
                    .WithMany(p => p.TOrderDetails)
                    .HasForeignKey(d => d.FProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tOrderDetails_tProducts");
            });

            modelBuilder.Entity<TPaymentCategory>(entity =>
            {
                entity.HasKey(e => e.FPaymentCategoryId);

                entity.ToTable("tPaymentCategories");

                entity.Property(e => e.FPaymentCategoryId).HasColumnName("fPaymentCategoryID");

                entity.Property(e => e.FPaymentCategory)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("fPaymentCategory");
            });

            modelBuilder.Entity<TProblem>(entity =>
            {
                entity.HasKey(e => e.FProblemId);

                entity.ToTable("tProblems");

                entity.Property(e => e.FProblemId).HasColumnName("fProblemID");

                entity.Property(e => e.FContactPhone)
                    .HasMaxLength(50)
                    .HasColumnName("fContactPhone");

                entity.Property(e => e.FEmail)
                    .HasMaxLength(50)
                    .HasColumnName("fEmail");

                entity.Property(e => e.FFilePath).HasColumnName("fFilePath");

                entity.Property(e => e.FMemberId).HasColumnName("fMemberID");

                entity.Property(e => e.FOrderId).HasColumnName("fOrderID");

                entity.Property(e => e.FPicturePath)
                    .HasColumnType("image")
                    .HasColumnName("fPicturePath");

                entity.Property(e => e.FProblemCategoryId).HasColumnName("fProblemCategoryID");

                entity.Property(e => e.FProblemContent)
                    .IsRequired()
                    .HasColumnName("fProblemContent");

                entity.Property(e => e.FProblemTime)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("fProblemTime");

                entity.Property(e => e.FStatusNumber).HasColumnName("fStatusNumber");

                entity.HasOne(d => d.FMember)
                    .WithMany(p => p.TProblems)
                    .HasForeignKey(d => d.FMemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tProblems_tMember");

                entity.HasOne(d => d.FProblemCategory)
                    .WithMany(p => p.TProblems)
                    .HasForeignKey(d => d.FProblemCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tProblems_tProblemCategroies");

                entity.HasOne(d => d.FStatusNumberNavigation)
                    .WithMany(p => p.TProblems)
                    .HasPrincipalKey(p => p.FStatusNumber)
                    .HasForeignKey(d => d.FStatusNumber)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tProblems_tStatus");
            });

            modelBuilder.Entity<TProblemCategroie>(entity =>
            {
                entity.HasKey(e => e.FProblemCategoryId);

                entity.ToTable("tProblemCategroies");

                entity.Property(e => e.FProblemCategoryId).HasColumnName("fProblemCategoryID");

                entity.Property(e => e.FProblemCategory)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("fProblemCategory");
            });

            modelBuilder.Entity<TProduct>(entity =>
            {
                entity.HasKey(e => e.FProductId);

                entity.ToTable("tProducts");

                entity.Property(e => e.FProductId).HasColumnName("fProductID");

                entity.Property(e => e.FCategoryId).HasColumnName("fCategoryID");

                entity.Property(e => e.FCoverImage)
                    .HasMaxLength(50)
                    .HasColumnName("fCoverImage");

                entity.Property(e => e.FDescription)
                    .HasMaxLength(50)
                    .HasColumnName("fDescription");

                entity.Property(e => e.FProductName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("fProductName");

                entity.Property(e => e.FUnitprice)
                    .HasColumnType("money")
                    .HasColumnName("fUnitprice");

                entity.Property(e => e.FVisible).HasColumnName("fVisible");

                entity.HasOne(d => d.FCategory)
                    .WithMany(p => p.TProducts)
                    .HasForeignKey(d => d.FCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tProducts_tProductCategories");
            });

            modelBuilder.Entity<TProductCategory>(entity =>
            {
                entity.HasKey(e => e.FCategoryId);

                entity.ToTable("tProductCategories");

                entity.Property(e => e.FCategoryId).HasColumnName("fCategoryID");

                entity.Property(e => e.FCategoryName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("fCategoryName");

                entity.Property(e => e.FDescription)
                    .HasMaxLength(50)
                    .HasColumnName("fDescription");

                entity.Property(e => e.FImage)
                    .HasMaxLength(50)
                    .HasColumnName("fImage");
            });

            modelBuilder.Entity<TProductsImage>(entity =>
            {
                entity.HasKey(e => e.FProductImageId);

                entity.ToTable("tProductsImages");

                entity.Property(e => e.FProductImageId).HasColumnName("fProductImageID");

                entity.Property(e => e.FImage)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("fImage");

                entity.Property(e => e.FProductId).HasColumnName("fProductID");

                entity.HasOne(d => d.FProduct)
                    .WithMany(p => p.TProductsImages)
                    .HasForeignKey(d => d.FProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tProductsImages_tProducts");
            });

            modelBuilder.Entity<TRegion>(entity =>
            {
                entity.HasKey(e => e.FRegionId);

                entity.ToTable("tRegions");

                entity.Property(e => e.FRegionId).HasColumnName("fRegionID");

                entity.Property(e => e.FCityId).HasColumnName("fCityID");

                entity.Property(e => e.FRegion)
                    .HasMaxLength(50)
                    .HasColumnName("fRegion");

                entity.Property(e => e.Postal)
                    .HasMaxLength(50)
                    .HasColumnName("postal");

                entity.HasOne(d => d.FCity)
                    .WithMany(p => p.TRegions)
                    .HasForeignKey(d => d.FCityId)
                    .HasConstraintName("FK_tRegions_tCities");
            });

            modelBuilder.Entity<TReply>(entity =>
            {
                entity.HasKey(e => e.FReplyId);

                entity.ToTable("tReplies");

                entity.Property(e => e.FReplyId).HasColumnName("fReplyID");

                entity.Property(e => e.FProblemId).HasColumnName("fProblemID");

                entity.Property(e => e.FReplierId).HasColumnName("fReplierID");

                entity.Property(e => e.FReplyContent).HasColumnName("fReplyContent");

                entity.Property(e => e.FReplyTime)
                    .HasMaxLength(50)
                    .HasColumnName("fReplyTime");

                entity.Property(e => e.FReplyType)
                    .HasMaxLength(5)
                    .HasColumnName("fReplyType");

                entity.HasOne(d => d.FProblem)
                    .WithMany(p => p.TReplies)
                    .HasForeignKey(d => d.FProblemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tReplies_tProblems");

                entity.HasOne(d => d.FReplier)
                    .WithMany(p => p.TReplies)
                    .HasForeignKey(d => d.FReplierId)
                    .HasConstraintName("FK_tReplies_tMember");
            });

            modelBuilder.Entity<TReservation>(entity =>
            {
                entity.HasKey(e => e.FReservationId);

                entity.ToTable("tReservations");

                entity.Property(e => e.FReservationId).HasColumnName("fReservationID");

                entity.Property(e => e.FCourseId).HasColumnName("fCourseID");

                entity.Property(e => e.FCourseTime)
                    .HasMaxLength(50)
                    .HasColumnName("fCourseTime");

                entity.Property(e => e.FStatusNumber).HasColumnName("fStatusNumber");

                entity.HasOne(d => d.FCourse)
                    .WithMany(p => p.TReservations)
                    .HasForeignKey(d => d.FCourseId)
                    .HasConstraintName("FK_tReservations_tCourses");

                entity.HasOne(d => d.FStatusNumberNavigation)
                    .WithMany(p => p.TReservations)
                    .HasPrincipalKey(p => p.FStatusNumber)
                    .HasForeignKey(d => d.FStatusNumber)
                    .HasConstraintName("FK_tReservations_tStatus");
            });

            modelBuilder.Entity<TSkill>(entity =>
            {
                entity.HasKey(e => e.FSkillId);

                entity.ToTable("tSkills");

                entity.Property(e => e.FSkillId).HasColumnName("fSkillID");

                entity.Property(e => e.FSkillDescription)
                    .HasMaxLength(50)
                    .HasColumnName("fSkillDescription");

                entity.Property(e => e.FSkillImage)
                    .HasMaxLength(50)
                    .HasColumnName("fSkillImage");

                entity.Property(e => e.FSkillName)
                    .HasMaxLength(50)
                    .HasColumnName("fSkillName");
            });

            modelBuilder.Entity<TStatus>(entity =>
            {
                entity.HasKey(e => e.FStatusId);

                entity.ToTable("tStatus");

                entity.HasIndex(e => e.FStatusNumber, "IX_tStatus")
                    .IsUnique();

                entity.Property(e => e.FStatusId).HasColumnName("fStatusID");

                entity.Property(e => e.FStatus)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("fStatus");

                entity.Property(e => e.FStatusNumber).HasColumnName("fStatusNumber");
            });

            modelBuilder.Entity<TTrackList>(entity =>
            {
                entity.HasKey(e => e.FTrackId);

                entity.ToTable("tTrackList");

                entity.Property(e => e.FTrackId).HasColumnName("fTrackID");

                entity.Property(e => e.FMemberId).HasColumnName("fMemberID");

                entity.Property(e => e.FProductId).HasColumnName("fProductID");

                entity.HasOne(d => d.FMember)
                    .WithMany(p => p.TTrackLists)
                    .HasForeignKey(d => d.FMemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tTrackList_tMember");

                entity.HasOne(d => d.FProduct)
                    .WithMany(p => p.TTrackLists)
                    .HasForeignKey(d => d.FProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tTrackList_tProducts");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
