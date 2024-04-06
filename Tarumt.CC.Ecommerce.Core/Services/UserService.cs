﻿using Ky.Web.CMS.SharedLibarary.Infrastructure.Requests;
using Ky.Web.CMS.SharedLibarary.Infrastructure.Requests.Admin;
using Ky.Web.CMS.SharedLibarary.Infrastructure.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tarumt.CC.Ecommerce.Infrastructure.Context;
using Tarumt.CC.Ecommerce.Infrastructure.Models;
using static Ky.Web.SharedLibrary.Utils.RegexHelper;

namespace Tarumt.CC.Ecommerce.Services
{
    public class UserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly CoreContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(ILogger<UserService> logger, CoreContext context, IPasswordHasher<User> passwordHasher)
        {
            _logger = logger;
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public PagedList<User> GetAll(int pageNumber, int pageSize, string keyword, bool isDeleted, bool isSuspended)
        {
            _logger.LogDebug($"[USER GET ALL] Page Number: {pageNumber}; Page Size: {pageSize}; Keyword: {keyword}");

            if (string.IsNullOrEmpty(keyword))
            {
                return PagedList<User>.ToPagedList(
                    _context.Set<User>()
                        .Where(m => m.IsDeleted == isDeleted)
                        .Where(m => m.IsSuspended == isSuspended)
                        .OrderBy(m => m.CreatedAt),
                    pageNumber, pageSize);
            }
            else
            {
                return PagedList<User>.ToPagedList(
                    _context.Set<User>()
                        .Where(m => m.IsDeleted == isDeleted)
                        .Where(m => m.IsSuspended == isSuspended)
                        .Where(m => m.Username.Contains(keyword))
                        .OrderBy(m => m.CreatedAt),
                    pageNumber, pageSize);
            }
        }

        public async Task<User> GetByIdAsync(string id, bool isDeleted, bool isSuspended)
        {
            _logger.LogInformation($"[USER GET] ID: {id}");

            return await _context.Users
                .Where(m => m.IsDeleted == isDeleted)
                .Where(m => m.IsSuspended == isSuspended)
                .SingleAsync(m => m.Id == id) ?? throw new InvalidOperationException("User not found");
        }

        public async Task<User> GetByUsernameAsync(string username, bool isDeleted, bool isSuspended)
        {
            _logger.LogInformation($"[USER GET] Username: {username}");

            return await _context.Users
                .Where(m => m.IsDeleted == isDeleted)
                .Where(m => m.IsSuspended == isSuspended)
                .SingleAsync(m => m.Username == username) ?? throw new InvalidOperationException("User not found");
        }

        public async Task<User> GetByEmailAsync(string email, bool isDeleted, bool isSuspended)
        {
            _logger.LogInformation($"[USER GET] Email: {email}");

            return await _context.Users
                .Where(m => m.IsDeleted == isDeleted)
                .Where(m => m.IsSuspended == isSuspended)
                .SingleAsync(m => m.Email == email) ?? throw new InvalidOperationException("User not found");
        }

        public async Task<bool> CreateAsync(User user)
        {
            _logger.LogInformation($"[USER CREATE] Username: {user.Username}");

            user.UserMfa = new UserMfa();

            await _context.Users.AddAsync(user);
            return await _context.SaveChangesAsync() != 0;
        }

        public async Task<bool> UpdateAsync(User user, string id, bool isDeleted, bool isSuspended)
        {
            _logger.LogInformation($"[USER UPDATE] Username: {user.Username}");

            User requestedUser = await GetByIdAsync(id, isDeleted, isSuspended);
            requestedUser.Username = user.Username;
            requestedUser.FirstName = user.FirstName;
            requestedUser.LastName = user.LastName;
            requestedUser.Gender = user.Gender;
            requestedUser.DateOfBirth = user.DateOfBirth;
            requestedUser.Email = user.Email;
            requestedUser.PhoneNumber = user.PhoneNumber;
            requestedUser.IsPhoneNumberVerified = user.IsPhoneNumberVerified;
            requestedUser.Email = user.Email;
            requestedUser.IsEmailVerified = user.IsEmailVerified;
            requestedUser.Address = user.Address;
            requestedUser.Culture = user.Culture;
            requestedUser.IsAdmin = user.IsAdmin;
            requestedUser.IsSuspended = user.IsSuspended;

            _context.Users.Update(requestedUser);
            return await _context.SaveChangesAsync() != 0;
        }

        public async Task<bool> DeleteAsync(string id, bool isSuspended)
        {
            _logger.LogInformation($"[USER DELETE] ID: {id}");

            User requestedUser = await GetByIdAsync(id, false, isSuspended);
            requestedUser.IsDeleted = true;
            _context.Users.Update(requestedUser);
            return await _context.SaveChangesAsync() != 0;
        }

        public async Task<bool> RegisterAsync(UserCreateRequest userCreateRequest)
        {
            _logger.LogInformation($"[USER REGISTER] Username: {userCreateRequest.Username}");

            if (!ValidateEmail(userCreateRequest.Email))
            {
                throw new InvalidOperationException("Email is invalid");
            }

            if (!ValidateMalaysianPhoneNumber(userCreateRequest.PhoneNumber))
            {
                throw new InvalidOperationException("Phone Number is invalid");
            }

            User user = (User)userCreateRequest;
            user.PasswordHashed = _passwordHasher.HashPassword(user, userCreateRequest.Password);

            return await CreateAsync(user);
        }

        public async Task<bool> RegisterAsync(UserCreateAdminRequest userCreateAdminRequest)
        {
            _logger.LogInformation($"[USER REGISTER] Username: {userCreateAdminRequest.Username}");

            if (!ValidateEmail(userCreateAdminRequest.Email))
            {
                throw new InvalidOperationException("Email is invalid");
            }

            if (!ValidateMalaysianPhoneNumber(userCreateAdminRequest.PhoneNumber))
            {
                throw new InvalidOperationException("Phone Number is invalid");
            }

            User user = (User)userCreateAdminRequest;
            user.PasswordHashed = _passwordHasher.HashPassword(user, userCreateAdminRequest.Password);

            return await CreateAsync(user);
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            User user = await GetByUsernameAsync(username, false, false);
            return Verify(user, password);
        }

        private bool Verify(User user, string rawPassword)
        {
            PasswordVerificationResult result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHashed, rawPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}