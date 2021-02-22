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
            Guid AdminRoleId = Guid.Parse("194DF880-D4CE-4997-96C9-878102EB6E0E");
            Guid RegularUserRoleId = Guid.Parse("728569AA-7A1F-45C9-B9D4-94BCC176BD0C");
            Guid FirstCityId = Guid.Parse("9171F23E-ADF2-4698-B73F-05C6FD7AD1BE");
            Guid SecondCityId = Guid.Parse("9346B8C4-1B3B-435F-9C35-35DE3A76FCF9");


            modelBuilder.Entity<Role>().HasData(
            new
            {
                RoleId = AdminRoleId,
                RoleName = "Admin"
            },
            new
            {
                RoleId = RegularUserRoleId,
                RoleName = "Regular user"
            });


            modelBuilder.Entity<City>().HasData(
            new
            {
                CityId = FirstCityId,
                CityName = "Novi Sad"
            },
            new
            {
                CityId = SecondCityId,
                CityName = "Beograd"
            });

            modelBuilder.Entity<PersonalUser>().HasData(
            new
            {
                UserId = Guid.Parse("CE593D02-C615-4AF6-A794-C450B79E9B4D"),
                Email = "nata@gmail.com",
                Password = "pass123",
                IsActive = true,
                Telephone = "+3816928749275",
                Username = "NatalijaG",
                FirstName = "Natalija",
                LastName = "Gajic",
                CityId = FirstCityId,
                RoleId = AdminRoleId
            },
            new
            {
                UserId = Guid.Parse("FF0C9396-7C4C-4BF5-A12E-6AA79C272413"),
                Email = "vladika@gmail.com",
                Password = "pass123",
                IsActive = true,
                Telephone = "+3816968749275",
                Username = "VladikaF",
                FirstName = "Vladimir",
                LastName = "Filipovic",
                CityId = FirstCityId,
                RoleId = RegularUserRoleId
            },
            new
            {
                UserId = Guid.Parse("8C349E7B-1C97-486D-AA2E-E58205D11577"),
                Email = "stefke@gmail.com",
                Password = "pass123",
                IsActive = true,
                Telephone = "+3816928749275",
                Username = "StefanO",
                FirstName = "Stefan",
                LastName = "Ostojic",
                CityId = SecondCityId,
                RoleId = RegularUserRoleId
            });

            modelBuilder.Entity<Corporation>().HasData(
            new
            {
                UserId = Guid.Parse("33253633-10E4-45C8-9B8E-84020A5C8C58"),
                Email = "financial_corpo@gmail.com",
                Password = "pass123",
                IsActive = true,
                Telephone = "+3816228749275",
                Username = "Financial Corporation",
                CorporationName = "Financial Corporation",
                Pib = "187398",
                HeadquartersCity = "Novi Sad",
                HeadquartersAddress = "Radnicka 1",
                CityId = FirstCityId,
                RoleId = RegularUserRoleId
            },
            new
            {
                UserId = Guid.Parse("987268E5-F880-4F81-B1BF-5B9704604E26"),
                Email = "billing_corpo@gmail.com",
                Password = "pass123",
                IsActive = true,
                Telephone = "+3816228749275",
                Username = "Billing Corporation",
                CorporationName = "Billing Corporation",
                Pib = "1844398",
                HeadquartersCity = "Novi Sad",
                HeadquartersAddress = "Danila Kisa 15",
                CityId = FirstCityId,
                RoleId = RegularUserRoleId
            });
        }
    }
}
