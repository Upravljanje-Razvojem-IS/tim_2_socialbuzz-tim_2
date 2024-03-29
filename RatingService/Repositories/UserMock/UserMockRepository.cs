﻿using RatingService.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RatingService.Repositories.UserMock
{
    public class UserMockRepository : IUserMockRepository
    {
        public static List<UserMockDTO> Users { get; set; } = new List<UserMockDTO>();

        public UserMockRepository()
        {
            FillData();
        }
        private void FillData()
        {
            Users.AddRange(new List<UserMockDTO>
            {
                new UserMockDTO //prati ga samo user 4
                {
                    UserID = 1,
                    Email = "milica.jovanovic.2000@gmail.com",
                    IsActive = true,
                    Telephone = "032/8738628",
                    Username = "Milica Jovanovic",
                    Role = "user",
                    City = "Novi Sad"
                },
               new UserMockDTO
                {
                    UserID = 2,
                    Email = "nemanja.petrovic.1990@gmail.com",
                    IsActive = true,
                    Telephone = "027/9113228",
                    Username = "Nemanja Petrovic",
                    Role = "user",
                    City = "Kragujevac"
                },
                new UserMockDTO //prate ga 1 i 2, a on prati 2
                {
                    UserID = 3,
                    Email = "aleksa.mitrovic.1988@gmail.com",
                    IsActive = true,
                    Telephone = "044/3860668",
                    Username = "Aleksa Mitrovic",
                    Role = "user",
                    City = "Beograd"
                },
                 new UserMockDTO
                {
                    UserID = 4,
                    Email = "sara.mikic.1999@gmail.com",
                    IsActive = true,
                    Telephone = "030/38245691",
                    Username = "Sara Mikic",
                    Role = "user",
                    City = "Beograd"
                }
            });
        }

        public List<UserMockDTO> GetAllUsers()
        {
            return Users;
        }

        public UserMockDTO GetUserByID(int userID)
        {
            return Users.FirstOrDefault(e => e.UserID == userID);
        }
    }
}
