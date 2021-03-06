﻿using Core.Entities.Base;
using Core.Entities.Base.DTO;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DAL.Contexts
{
    public class NevladinaOrgContext : DbContext
    {
        #region Entities
        public virtual DbSet<ActivityLog> ActivityLogs { get; set; }
        public virtual DbSet<LogActivity> LogActivities { get; set; }
        public virtual DbSet<Institution> Institutions { get; set; }
        public virtual DbSet<Functionality> Functionalities { get; set; }
        public virtual DbSet<RoleFunctionality> RoleFunctionalities { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<PersonContact> PersonContacts { get; set; }
        public virtual DbSet<PersonDetail> PersonDetails { get; set; }
        public virtual DbSet<PersonPhoto> PersonPhotos { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<OrganizationInstitutionUser> OrganizationInstitutionUsers { get; set; }
        public virtual DbSet<InstitutionUser> InstitutionUsers { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<InstitutionType> InstitutionTypes { get; set; }
        public virtual DbSet<EmployeeStatus> EmployeeStatuses { get; set; }
        public virtual DbSet<AcademicDegree> AcademicDegrees { get; set; }
        public virtual DbSet<AcademicTitle> AcademicTitles { get; set; }
        public virtual DbSet<MaritalStatus> MaritalStatuses { get; set; }
        public virtual DbSet<ContactType> ContactTypes{ get; set; }
        public virtual DbSet<Citizenship> Citizenships{ get; set; }
        public virtual DbSet<OrganizationType> OrganizationTypes{ get; set; }
        public virtual DbSet<Member> Members{ get; set; }
        public virtual DbSet<Nationality> Nationalities { get; set; }
        public virtual DbSet<Announcement> Announcements { get; set; }
        public virtual DbSet<AnnouncementDocument> AnnouncementDocuments { get; set; }
        public virtual DbSet<AnnouncementOrganization> AnnouncementOrganizations { get; set; }
        public virtual DbSet<AnnouncementPhoto> AnnouncementPhotos { get; set; }
        public virtual DbSet<AnnouncementType> AnnouncementTypes { get; set; }
        public virtual DbSet<AnnouncementUser> AnnouncementUsers { get; set; }


        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<EventDocument> EventDocuments{ get; set; }
        public virtual DbSet<EventImage> EventImages{ get; set; }
        public virtual DbSet<EventItem> EventItems{ get; set; }
        public virtual DbSet<EventItemType> EventItemTypes { get; set; }
        public virtual DbSet<EventItemEventType> EventItemEventTypes { get; set; }
        public virtual DbSet<EventRegistration> EventRegistrations { get; set; }
        public virtual DbSet<EventSponsor> EventSponsors { get; set; }
        public virtual DbSet<EventUser> EventUsers { get; set; }
        public virtual DbSet<Lecture> Lectures{ get; set; }
        public virtual DbSet<Lecturer> Lecturers{ get; set; }
        public virtual DbSet<LectureLecturer> LectureLecturers{ get; set; }
        public virtual DbSet<Payment> Payments{ get; set; }
        public virtual DbSet<Sponsor> Sponsors{ get; set; }
        public virtual DbSet<LicensePeriod> LicensePeriods{ get; set; }
        public virtual DbSet<LicenseType> LicenseTypes { get; set; }
        public virtual DbSet<MemberEmployement> MemberEmployements { get; set; }
        public virtual DbSet<MemberLicense> MemberLicenses { get; set; }

        #endregion

        public NevladinaOrgContext(DbContextOptions<NevladinaOrgContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /* Remove all cascade relationships */
            modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()).ToList().ForEach(r => r.DeleteBehavior = DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}