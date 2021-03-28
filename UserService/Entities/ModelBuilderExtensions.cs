using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Entities
{
    public static class ModelBuilderExtensions
    {
       
        public static void Seed(this ModelBuilder modelBuilder)
        {
            //TODO: Latest migration doesn't have data, it failed to update with seeding,
            //also, unique is not working 
            Guid adminRoleId = Guid.Parse("194DF880-D4CE-4997-96C9-878102EB6E0E");
            Guid regularUserRoleId = Guid.Parse("728569AA-7A1F-45C9-B9D4-94BCC176BD0C");
            Guid firstCityId = Guid.Parse("9171F23E-ADF2-4698-B73F-05C6FD7AD1BE");
            Guid secondCityId = Guid.Parse("9346B8C4-1B3B-435F-9C35-35DE3A76FCF9");
            Guid firstPersonalUserAdminId = Guid.Parse("CE593D02-C615-4AF6-A794-C450B79E9B4D");
            Guid secondPersonalUserRegularUserId = Guid.Parse("FF0C9396-7C4C-4BF5-A12E-6AA79C272413");
            Guid thirdPersonalUserRegularUserId = Guid.Parse("8C349E7B-1C97-486D-AA2E-E58205D11577");
            Guid firstCorporationUserRegularUserId = Guid.Parse("33253633-10E4-45C8-9B8E-84020A5C8C58");
            Guid secondCorporationUserRegularUserId = Guid.Parse("987268E5-F880-4F81-B1BF-5B9704604E26");

            modelBuilder.Entity<Role>().HasData(
            new
            {
                RoleId = adminRoleId,
                RoleName = "Admin"
            },
            new
            {
                RoleId = regularUserRoleId,
                RoleName = "Regular user"
            });


            modelBuilder.Entity<City>().HasData(
            new
            {
                CityId = firstCityId,
                CityName = "Novi Sad"
            },
            new
            {
                CityId = secondCityId,
                CityName = "Beograd"
            });

            modelBuilder.Entity<PersonalUser>().HasData(
            new
            {
                UserId = firstPersonalUserAdminId,
                Email = "nata@gmail.com",
                Password = "pass123",
                IsActive = true,
                Telephone = "+3816928749275",
                Username = "NatalijaG",
                FirstName = "Natalija",
                LastName = "Gajic",
                CityId = firstCityId,
                RoleId = adminRoleId
            },
            new
            {
                UserId = secondPersonalUserRegularUserId,
                Email = "vladika@gmail.com",
                Password = "pass123",
                IsActive = true,
                Telephone = "+3816968749275",
                Username = "VladikaF",
                FirstName = "Vladimir",
                LastName = "Filipovic",
                CityId = firstCityId,
                RoleId = regularUserRoleId
            },
            new
            {
                UserId = thirdPersonalUserRegularUserId,
                Email = "stefke@gmail.com",
                Password = "pass123",
                IsActive = true,
                Telephone = "+3816928749275",
                Username = "StefanO",
                FirstName = "Stefan",
                LastName = "Ostojic",
                CityId = secondCityId,
                RoleId = regularUserRoleId
            });

            modelBuilder.Entity<Corporation>().HasData(
            new
            {
                UserId = firstCorporationUserRegularUserId,
                Email = "financial_corpo@gmail.com",
                Password = "pass123",
                IsActive = true,
                Telephone = "+3816228749275",
                Username = "Financial Corporation",
                CorporationName = "Financial Corporation",
                Pib = "187398",
                HeadquartersCity = "Novi Sad",
                HeadquartersAddress = "Radnicka 1",
                CityId = firstCityId,
                RoleId = regularUserRoleId
            },
            new
            {
                UserId = secondCorporationUserRegularUserId,
                Email = "billing_corpo@gmail.com",
                Password = "pass123",
                IsActive = true,
                Telephone = "+3816228749275",
                Username = "Billing Corporation",
                CorporationName = "Billing Corporation",
                Pib = "1844398",
                HeadquartersCity = "Novi Sad",
                HeadquartersAddress = "Danila Kisa 15",
                CityId = firstCityId,
                RoleId = regularUserRoleId
            });
        }

        public static void SeedIdentity(this ModelBuilder modelBuilder)
        {
            Guid adminRoleId = Guid.Parse("194DF880-D4CE-4997-96C9-878102EB6E0E");
            Guid regularUserRoleId = Guid.Parse("728569AA-7A1F-45C9-B9D4-94BCC176BD0C");
            Guid firstPersonalUserAdminId = Guid.Parse("CE593D02-C615-4AF6-A794-C450B79E9B4D");
            Guid secondPersonalUserRegularUserId = Guid.Parse("FF0C9396-7C4C-4BF5-A12E-6AA79C272413");
            Guid thirdPersonalUserRegularUserId = Guid.Parse("8C349E7B-1C97-486D-AA2E-E58205D11577");
            Guid firstCorporationUserRegularUserId = Guid.Parse("33253633-10E4-45C8-9B8E-84020A5C8C58");
            Guid secondCorporationUserRegularUserId = Guid.Parse("987268E5-F880-4F81-B1BF-5B9704604E26");
            //TODO: updating username in PersonalUser/CorporationUser updates AccountInfo table as well
            // also updating the role makes changes
            //TODO: what happens after update in Auth database with username
            //TODO: which user has which role
            modelBuilder.Entity<AccountInfo>().HasData(
                new AccountInfo("NatalijaG", "nata@gmail.com", firstPersonalUserAdminId),
                new AccountInfo("VladikaF", "vladika@gmail.com", secondPersonalUserRegularUserId),
                new AccountInfo("StefanO", "stefke@gmail.com", thirdPersonalUserRegularUserId),
                new AccountInfo("Financial Corporation", "financial_corpo@gmail.com", firstCorporationUserRegularUserId),
                new AccountInfo("Billing Corporation", "billing_corpo@gmail.com", secondCorporationUserRegularUserId)
         );
            //TODO: Role VS AccountRole
            //TODO: update of AccountRole
            modelBuilder.Entity<AccountRole>().HasData(
                new AccountRole(adminRoleId, "Admin", "Role that enables root level privileges"),
                new AccountRole(regularUserRoleId, "Regular user", "Role that basic level privileges")
         );
        }
    }
}