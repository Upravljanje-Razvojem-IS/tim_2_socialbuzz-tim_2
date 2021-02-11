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
            Guid AdminRoleId = Guid.NewGuid();
            Guid RegularUserRoleId = Guid.NewGuid();
            Guid FirstCityId = Guid.NewGuid();
            Guid SecondCityId = Guid.NewGuid();


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
                UserId = Guid.NewGuid(),
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
                UserId = Guid.NewGuid(),
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
                UserId = Guid.NewGuid(),
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
                UserId = Guid.NewGuid(),
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
                UserId = Guid.NewGuid(),
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
