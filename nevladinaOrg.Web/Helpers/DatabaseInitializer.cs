using Core.Entities.Base;
using DAL.Contexts;
using Microsoft.Extensions.DependencyInjection;
using nevladinaOrg.Web.Constants;
using System;
using System.Collections.Generic;
using System.Linq;

namespace nevladinaOrg.Web.Helpers
{
    public static class DatabaseInitializer
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<NevladinaOrgContext>();

            var baseDbCreated = context.Database.EnsureCreated();
            if (baseDbCreated)
                Console.WriteLine("BASE DATABASE :: CREATED !");
            else
                Console.WriteLine("BASE DATABASE :: ALREADY EXISTS OR NOT CREATED !");


            var ajdin = new int();
            var omer = new int();

            var sa = new int();
            var mo = new int();
            var regijaSa = new int();
            var regijaHnk = new int();
            var bih = new int();
            var cro = new int();

            var saBase = new int();
            var moBase = new int();
            var regijaSaBase = new int();
            var regijaHnkBase = new int();
            var bihBase = new int();
            var croBase = new int();

            var stateInstitution = new int();
            var localInstitution = new int();
            var bussinessEntityOrganizationType = new int();
            var nonProfitOrganizationType = new int();

            var opstinaVg = new int();
            var opstinaHz = new int();
            var zebe = new int();
            var uLogorasa = new int();

            if (!context.Countries.Any())
            {
                var bosnia = new Country
                {
                    Name = "Bosna i Hercegovina"
                };
                var croatia = new Country
                {
                    Name = "Hrvatska"
                };

                context.Countries.AddRange(bosnia, croatia);

                if (!context.Regions.Any())
                {
                    var regionSarajevo = new Region("Kanton Sarajevo", bosnia.Id);
                    var regionSrednjabosna = new Region("Srednjobosanski kanton", bosnia.Id);
                    var regionHercegovackonertvanski = new Region("Hercegovačko-neretvanski kanton", bosnia.Id);

                    context.Regions.AddRange(regionSarajevo, regionSrednjabosna, regionHercegovackonertvanski);

                    regijaSa = regionSarajevo.Id;
                    regijaHnk = regionHercegovackonertvanski.Id;

                    if (!context.Cities.Any())
                    {
                        var sarajevo = new City
                        {
                            Name = "Sarajevo",
                            RegionId = regionSarajevo.Id,
                            CountryId = regionSarajevo.CountryId
                        };

                        var travnik = new City
                        {
                            Name = "Travnik",
                            RegionId = regionSrednjabosna.Id,
                            CountryId = regionSrednjabosna.CountryId
                        };

                        var bugojno = new City
                        {
                            Name = "Bugojno",
                            RegionId = regionSrednjabosna.Id,
                            CountryId = regionSrednjabosna.CountryId
                        };

                        var mostar = new City
                        {
                            Name = "Mostar",
                            RegionId = regionHercegovackonertvanski.Id,
                            CountryId = regionHercegovackonertvanski.CountryId
                        };

                        context.Cities.AddRange(sarajevo, travnik, bugojno, mostar);
                        context.SaveChanges();

                        sa = sarajevo.Id;
                        mo = mostar.Id;
                    }
                }

                context.SaveChanges();

                bih = bosnia.Id;
                cro = croatia.Id;
            }

            if (!context.InstitutionTypes.Any())
            {
                var state = new InstitutionType
                {
                    Name = "Državna"
                };
                var regional = new InstitutionType
                {
                    Name = "Regionalna"
                };
                var local = new InstitutionType
                {
                    Name = "Gradska"
                };

                context.InstitutionTypes.AddRange(state, regional, local);
                context.SaveChanges();

                stateInstitution = state.Id;
                localInstitution = local.Id;

            }
            if (!context.EmployeeStatuses.Any())
            {
                var employed = new EmployeeStatus()
                {
                    Name = "Employed"
                };
                var unEmployed = new EmployeeStatus()
                {
                    Name = "Unemployed"
                };
                context.EmployeeStatuses.AddRange(employed, unEmployed);
                context.SaveChanges();
            }
            if (!context.AcademicTitles.Any())
            {
                var academicTitles = new List<AcademicTitle>();
                academicTitles.Add(new AcademicTitle { Name = "Professor", IsDeleted = false });
                academicTitles.Add(new AcademicTitle { Name = "Associate Professor", IsDeleted = false });
                academicTitles.Add(new AcademicTitle { Name = "Assistant Professor", IsDeleted = false });
                academicTitles.Add(new AcademicTitle { Name = "Lecturer", IsDeleted = false });

                context.AcademicTitles.AddRange(academicTitles);
                context.SaveChanges();
            }
            if (!context.AcademicDegrees.Any())
            {
                var academicDegrees = new List<AcademicDegree>();
                academicDegrees.Add(new AcademicDegree { Name = "Bachelor's Degree", IsDeleted = false });
                academicDegrees.Add(new AcademicDegree { Name = "Master's Degree", IsDeleted = false });
                academicDegrees.Add(new AcademicDegree { Name = "Doctorat", IsDeleted = false });

                context.AcademicDegrees.AddRange(academicDegrees);
                context.SaveChanges();
            }
            if (!context.MaritalStatuses.Any())
            {
                var maritalStatuses = new List<MaritalStatus>();

                maritalStatuses.Add(new MaritalStatus { Name = "Married", IsDeleted = false });
                maritalStatuses.Add(new MaritalStatus { Name = "Single", IsDeleted = false });
                maritalStatuses.Add(new MaritalStatus { Name = "Divorced", IsDeleted = false });
                maritalStatuses.Add(new MaritalStatus { Name = "Widowed", IsDeleted = false });

                context.MaritalStatuses.AddRange(maritalStatuses);
                context.SaveChanges();
            }
            if (!context.ContactTypes.Any())
            {
                var contactType = new ContactType { Name = "Email", Active = true, IsDeleted = false }; //Id = 1
                var contactType2 = new ContactType { Name = "Phone", Active = true, IsDeleted = false }; //Id = 2
                var contactType3 = new ContactType { Name = "Fax", Active = true, IsDeleted = false };

                context.ContactTypes.AddRange(contactType, contactType2, contactType3);
                context.SaveChanges();
            }


            if (!context.Citizenships.Any())
            {
                var citizenchip_BIH = new Citizenship
                {
                    Name = "BiH"
                };
                var citizenchip_CRO = new Citizenship
                {
                    Name = "CRO"
                };
                context.Citizenships.AddRange(citizenchip_BIH, citizenchip_CRO);
                context.SaveChanges();
            }


            if (!context.OrganizationTypes.Any())
            {
                var bussinessEntity = new OrganizationType() { Name = "Business entity" };
                var militaryForces = new OrganizationType() { Name = "Military forces" };
                var nonProfit = new OrganizationType() { Name = "Non profit organizations" };

                context.OrganizationTypes.AddRange(bussinessEntity, militaryForces, nonProfit);
                context.SaveChanges();

                bussinessEntityOrganizationType = bussinessEntity.Id;
                nonProfitOrganizationType = nonProfit.Id;
            }

            if (!context.Institutions.Any())
            {
                var opstinaVogosca = new Institution
                {
                    InstitutionTypeId = stateInstitution,
                    Name = "Opština Vogošća",
                    Address = "Kranjčevića 12",
                    ContactPhone = "387 33 285 101",
                    ContactEmail = "vogosca@gmail.com",
                    WebsiteURL = "www.vogosca.ba",
                    CountryId = bih,
                    CityId = sa,
                    Active = true
                };
                var opstinaHadzici = new Institution
                {
                    InstitutionTypeId = localInstitution,
                    Name = "Opština Hadzici",
                    Address = "Bolnička 25",
                    ContactPhone = "387 33 297 000",
                    ContactEmail = "hadzici@gmail.com",
                    WebsiteURL = "www.hadzici.ba",
                    CountryId = bih,
                    CityId = sa,
                    Active = true
                };

                context.Institutions.AddRange(opstinaVogosca, opstinaHadzici);
                context.SaveChanges();
                opstinaVg = opstinaVogosca.Id;
                opstinaHz = opstinaHadzici.Id;
            }
            if (!context.Organizations.Any())
            {
                var organization = new Organization()
                {
                    OrganizationTypeId = bussinessEntityOrganizationType,
                    Name = "Zelene Beretke Vogosca",
                    CountryId = bih,
                    CityId = sa,
                    Address = "Address",
                    Active = true,
                    IsDeleted = false,
                    InstitutionId = opstinaVg
                };
                var organization1 = new Organization()
                {
                    OrganizationTypeId = nonProfitOrganizationType,
                    ParentId = 1,
                    Name = "Udruzenje Logorasa Hadzici",
                    CountryId = bih,
                    CityId = sa,
                    Address = "Address",
                    Active = true,
                    IsDeleted = false,
                    InstitutionId = opstinaHz
                };
                var organization2 = new Organization()
                {
                    OrganizationTypeId = bussinessEntityOrganizationType,
                    ParentId = 1,
                    Name = "Sportsko rekreativni centar 'Ajdinovići'",
                    CountryId = bih,
                    CityId = mo,
                    Address = "Address",
                    Active = true,
                    IsDeleted = false,
                    InstitutionId = opstinaVg
                };
                context.Organizations.AddRange(organization, organization1, organization2);
                context.SaveChanges();

                zebe = organization.Id;
                uLogorasa = organization1.Id;
            }

            if (!context.Users.Any())
            {
                var ajdinh = new Person
                {
                    FirstName = "Ajdin",
                    ParentName = "Ime roditelja",
                    LastName = "Halilović",
                    DateOfBirth = DateTime.Now.AddYears(-21).AddDays(-55),
                    Gender = "M",
                    CountryId = bih,
                    CityId = sa
                };

                var omerr = new Person
                {
                    FirstName = "Omer",
                    ParentName = "Ime roditelja",
                    LastName = "Rahmanović",
                    DateOfBirth = DateTime.Now.AddYears(-21),
                    Gender = "M",
                    CountryId = bih,
                    CityId = mo
                };

                context.Persons.AddRange(omerr, ajdinh);
                context.SaveChanges();

                var ajdinUser = new User
                {
                    Id = ajdinh.Id,
                    Username = "ajdin",
                    Email = "ajdin@demo.ba",
                    PasswordSalt = "0yw6zgUlzFTJBkk7LBE25Q==",
                    PasswordHash = "XrRigoPosbJmqjXbWlc0ctnDqogGsy4NBxbJ332x8gA=", //= test
                    CreatedDate = DateTime.Now,
                    CultureName = "en-us"
                };

                var omerUser = new User
                {
                    Id = omerr.Id,
                    Username = "omer",
                    Email = "omer@demo.ba",
                    PasswordSalt = "0yw6zgUlzFTJBkk7LBE25Q==",
                    PasswordHash = "XrRigoPosbJmqjXbWlc0ctnDqogGsy4NBxbJ332x8gA=", //= test
                    CreatedDate = DateTime.Now,
                    CultureName = "en-us"
                };

                context.Users.AddRange(ajdinUser, omerUser);
                context.SaveChanges();

                var ajdinUserDetails = new PersonDetail
                {
                    AcademicDegreeId = 1,
                    AcademicTitleId = 1,
                    EmploymentStatusId = 1,
                    MartialStatusId = 1,
                    Id = ajdinh.Id
                };
                var omerUserDetails = new PersonDetail
                {
                    AcademicDegreeId = 1,
                    AcademicTitleId = 1,
                    EmploymentStatusId = 1,
                    MartialStatusId = 1,
                    Id = omerr.Id
                };
                context.PersonDetails.AddRange(ajdinUserDetails, omerUserDetails);

                var ajdinUserPrimaryEmailContact = new PersonContact
                {
                    PersonId = ajdinh.Id,
                    ContactTypeId = 1,
                    Contact = "ajdin.haliloic@edu.fit.ba"
                };
                var ajdinUserPhoneContact = new PersonContact
                {
                    PersonId = ajdinh.Id,
                    ContactTypeId = 3,
                    Contact = "062673287"
                };
                var omerUserPrimaryEmailContact = new PersonContact
                {
                    PersonId = omerr.Id,
                    ContactTypeId = 1,
                    Contact = "omer.rahamnovic@edu.fit.ba"
                };
                var omerUserPhoneContact = new PersonContact
                {
                    PersonId = omerr.Id,
                    ContactTypeId = 3,
                    Contact = "062545685"
                };
                context.PersonContacts.AddRange(ajdinUserPrimaryEmailContact, ajdinUserPhoneContact, omerUserPrimaryEmailContact, omerUserPhoneContact);
                context.SaveChanges();

                ajdin = ajdinh.Id;
                omer = omerr.Id;

                var ajdinVGInstitutionUser = new InstitutionUser
                {
                    UserId = ajdinUser.Id,
                    InstitutionId = opstinaVg,
                    IsInstitutionUser = true,
                    IsOrganizationUser = true,
                    Token = Guid.NewGuid(),
                    Active = true,
                    LastLogin = DateTime.Now
                };

                var omerVGInstitutionUser = new InstitutionUser
                {
                    UserId = omerUser.Id,
                    InstitutionId = opstinaVg,
                    IsInstitutionUser = true,
                    IsOrganizationUser = false,
                    Token = Guid.NewGuid(),
                    Active = true,
                    LastLogin = DateTime.Now
                };

                var ajdinHZInstitutionUser = new InstitutionUser
                {
                    UserId = ajdinUser.Id,
                    InstitutionId = opstinaHz,
                    IsInstitutionUser = true,
                    IsOrganizationUser = true,
                    Token = Guid.NewGuid(),
                    Active = true,
                    LastLogin = DateTime.Now
                };

                var omerHZHInstitutionUser = new InstitutionUser
                {
                    UserId = omerUser.Id,
                    InstitutionId = opstinaHz,
                    IsInstitutionUser = true,
                    IsOrganizationUser = false,
                    Token = Guid.NewGuid(),
                    Active = true,
                    LastLogin = DateTime.Now
                };

                context.InstitutionUsers.AddRange(ajdinVGInstitutionUser, omerVGInstitutionUser, ajdinHZInstitutionUser, omerHZHInstitutionUser);
                context.SaveChanges();

                var ajdinVgZebeOrganizationInstitutionUser = new OrganizationInstitutionUser
                {
                    InstitutionUserId = ajdinVGInstitutionUser.Id,
                    OrganizationId = zebe,
                    Token = Guid.NewGuid(),
                    Active = true
                };

                var omerHZUdrLogorasaOrganizationInstitutionUser = new OrganizationInstitutionUser
                {
                    InstitutionUserId = omerHZHInstitutionUser.Id,
                    OrganizationId = uLogorasa,
                    Employed = true,
                    Token = Guid.NewGuid(),
                    Active = true
                };

                context.OrganizationInstitutionUsers.AddRange(ajdinVgZebeOrganizationInstitutionUser, omerHZUdrLogorasaOrganizationInstitutionUser);
                context.SaveChanges();

                var ajdinVGMember = new Member
                {
                    Id = ajdinVgZebeOrganizationInstitutionUser.Id,
                    Note = "Član opštine"
                };
                context.Members.Add(ajdinVGMember);
                context.SaveChanges();

                var license = new MemberLicense
                {
                    Active = true,
                    CreatedDate = DateTime.Now,
                    LicenceNumber = "XYC-225",
                    MemberId = ajdinVGMember.Id,
                    Permanent = false
                };
                context.MemberLicenses.Add(license);
                context.SaveChanges();

                var licenseType = new LicenseType
                {
                    Name = "Prolongation"
                };
                var licenseType2 = new LicenseType
                {
                    Name = "Ban"
                };
                context.LicenseTypes.AddRange(licenseType, licenseType2);
                context.SaveChanges();

                var licensePeriod = new LicensePeriod
                {
                    Active = true,
                    CreatedDate = DateTime.Now,
                    EndDate = DateTime.Now.AddDays(160),
                    LicenseTypeId = 1,
                    MemberLicenseId = license.Id
                };
                context.LicensePeriods.Add(licensePeriod);
                context.SaveChanges();
            }

            if (!context.Functionalities.Any())
            {
                var functionality1 = new Core.Entities.Base.Functionality
                {
                    Name = "Role - Index",
                    ControllerName = "Roles",
                    FunctionNumber = 1,
                    IsDeleted = false
                };
                var functionality2 = new Core.Entities.Base.Functionality
                {
                    Name = "Role - Add",
                    ControllerName = "Roles",
                    FunctionNumber = 2,
                    IsDeleted = false
                };
                var functionality3 = new Core.Entities.Base.Functionality
                {
                    Name = "Role - Edit",
                    ControllerName = "Roles",
                    FunctionNumber = 3,
                    IsDeleted = false
                };
                var functionality4 = new Core.Entities.Base.Functionality
                {
                    Name = "Role - Delete",
                    ControllerName = "Roles",
                    FunctionNumber = 4,
                    IsDeleted = false
                };
                context.Functionalities.AddRange(functionality1, functionality2, functionality3, functionality4);
                context.SaveChanges();
            }

            /*if (!context.Roles.Any())
            {
                var sadmin = new Core.Entities.Base.Role
                {
                    Name = "Super Administrator",
                    Code = (int)Enumerations.WebRoles.SuperAdministrator,
                    RoleLevel = (int)Enumerations.RoleLevels.System,
                    Active = true
                };
                var admin = new Core.Entities.Base.Role
                {
                    Name = "Administrator",
                    Code = (int)Enumerations.WebRoles.Administrator,
                    RoleLevel = (int)Enumerations.RoleLevels.System,
                    Active = true
                };
                var institutionAdmin = new Core.Entities.Base.Role
                {
                    Name = "Institution Administrator",
                    Code = (int)Enumerations.WebRoles.InstitutionAdministrator,
                    RoleLevel = (int)Enumerations.RoleLevels.Institution,
                    Active = true
                };
                var institutionStaff = new Core.Entities.Base.Role
                {
                    Name = "Institution Staff",
                    Code = (int)Enumerations.WebRoles.InstitutionStaff,
                    RoleLevel = (int)Enumerations.RoleLevels.Institution,
                    Active = true
                };
                var organizationAdministrator = new Core.Entities.Base.Role
                {
                    Name = "Organization Administrator",
                    Code = (int)Enumerations.WebRoles.OrganizationAdministrator,
                    RoleLevel = (int)Enumerations.RoleLevels.Organization,
                    Active = true
                };
                var organizationManagement = new Core.Entities.Base.Role
                {
                    Name = "Organization Management",
                    Code = (int)Enumerations.WebRoles.OrganizationManagement,
                    RoleLevel = (int)Enumerations.RoleLevels.Organization,
                    Active = true
                };
                var organizationMember = new Core.Entities.Base.Role
                {
                    Name = "Organization Member",
                    Code = (int)Enumerations.WebRoles.OrganizationMember,
                    RoleLevel = (int)Enumerations.RoleLevels.Organization,
                    Active = true
                };
                
                context.Roles.AddRange(sadmin, admin, institutionAdmin, institutionStaff, organizationAdministrator, organizationManagement, organizationMember);
                context.SaveChanges();
            }*/

            if (!context.Roles.Any())
            {
                var admin = new Core.Entities.Base.Role
                {
                    InstitutionId = 1,
                    Name = "Administrator",
                    Active = true
                };
                var def = new Core.Entities.Base.Role
                {
                    InstitutionId = 1,
                    Name = "Demo default role",
                    Active = true
                };
                context.Roles.AddRange(admin, def);
                context.SaveChanges();
            }
            if (!context.RoleFunctionalities.Any())
            {
                var adminRoleFunct1 = new Core.Entities.Base.RoleFunctionality
                {
                    RoleId = 1,
                    FunctionalityId = 1,
                    AssignmentDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    IsDeleted = false
                };
                var adminRoleFunct2 = new Core.Entities.Base.RoleFunctionality
                {
                    RoleId = 1,
                    FunctionalityId = 2,
                    AssignmentDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    IsDeleted = false
                };
                var adminRoleFunct3 = new Core.Entities.Base.RoleFunctionality
                {
                    RoleId = 1,
                    FunctionalityId = 3,
                    AssignmentDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    IsDeleted = false
                };
                var adminRoleFunct4 = new Core.Entities.Base.RoleFunctionality
                {
                    RoleId = 1,
                    FunctionalityId = 4,
                    AssignmentDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    IsDeleted = false
                };
                var defRoleFunct = new Core.Entities.Base.RoleFunctionality
                {
                    RoleId = 2,
                    FunctionalityId = 1,
                    AssignmentDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    IsDeleted = false
                };
                context.RoleFunctionalities.AddRange(adminRoleFunct1, adminRoleFunct2, adminRoleFunct3, adminRoleFunct4, defRoleFunct);
                context.SaveChanges();
            }
            if (!context.UserRoles.Any())
            {
                var userAdmin = new Core.Entities.Base.UserRole
                {
                    InstitutionUserId = 1,
                    OrganizationInstitutionUserId=1,
                    RoleId = 2,
                };
                var userDefault = new Core.Entities.Base.UserRole
                {
                    InstitutionUserId = 2,
                    RoleId = 1
                };
                context.UserRoles.AddRange(userAdmin, userDefault);
                context.SaveChanges();
            }

        }
    }
}