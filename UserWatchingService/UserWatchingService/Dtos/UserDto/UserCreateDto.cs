﻿namespace UserWatchingService.Dtos.UserDto
{
    public class UserCreateDto
    {
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Is active
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Phone
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; set; }
    }
}
