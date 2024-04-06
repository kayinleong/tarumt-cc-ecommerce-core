using Ky.Web.CMS.SharedLibarary.Infrastructure.Requests;
using Ky.Web.CMS.SharedLibarary.Infrastructure.Requests.Admin;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Tarumt.CC.Ecommerce.Constants;

namespace Tarumt.CC.Ecommerce.Infrastructure.Models;

[Index(nameof(Username), IsUnique = true)]
[Index(nameof(Email), IsUnique = true)]
[Index(nameof(PhoneNumber), IsUnique = true)]
public class User : ModelBase
{
    [Required(ErrorMessage = "Username is required")]
    public required string Username { get; set; }

    [Required(ErrorMessage = "First Name is required")]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = "Last Name is required")]
    public required string LastName { get; set; }

    [Required(ErrorMessage = "Gender is required")]
    public required GenderTypes Gender { get; set; }

    [Required(ErrorMessage = "Date of Birth is required")]
    public required DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Email is required")]
    public required string Email { get; set; }

    [Required]
    public bool IsEmailVerified { get; set; } = false;

    [Required(ErrorMessage = "Phone Number is required")]
    public required string PhoneNumber { get; set; }

    [Required]
    public bool IsPhoneNumberVerified { get; set; } = false;

    public string? Address { get; set; }

    [Required]
    public string Culture { get; set; } = "en-GB";

    [Required]
    public string PasswordHashed { get; set; } = string.Empty;

    [Required]
    public UserMfa UserMfa { get; set; } = new();

    [Required]
    public bool IsAdmin { get; set; } = false;

    [Required]
    public bool IsSuspended { get; set; } = false;

    public static implicit operator User(UserCreateRequest userCreateRequest)
    {
        return new()
        {
            Username = userCreateRequest.Username,
            FirstName = userCreateRequest.FirstName,
            LastName = userCreateRequest.LastName,
            Gender = (GenderTypes)userCreateRequest.Gender,
            DateOfBirth = DateTime.ParseExact(
                userCreateRequest.DateOfBirth,
                "M/d/yyyy",
                CultureInfo.InvariantCulture),
            Email = userCreateRequest.Email,
            PhoneNumber = userCreateRequest.PhoneNumber,
            Culture = userCreateRequest.Culture
        };
    }

    public static implicit operator User(UserUpdateRequest userUpdateRequest)
    {
        return new()
        {
            Username = userUpdateRequest.Username,
            FirstName = userUpdateRequest.FirstName,
            LastName = userUpdateRequest.LastName,
            Gender = (GenderTypes)userUpdateRequest.Gender,
            DateOfBirth = DateTime.ParseExact(
                userUpdateRequest.DateOfBirth,
                "M/d/yyyy",
                CultureInfo.InvariantCulture),
            Email = userUpdateRequest.Email,
            PhoneNumber = userUpdateRequest.PhoneNumber,
            Culture = userUpdateRequest.Culture
        };
    }

    public static implicit operator User(UserCreateAdminRequest userCreateAdminRequest)
    {
        return new()
        {
            Username = userCreateAdminRequest.Username,
            FirstName = userCreateAdminRequest.FirstName,
            LastName = userCreateAdminRequest.LastName,
            Gender = (GenderTypes)userCreateAdminRequest.Gender,
            DateOfBirth = DateTime.ParseExact(
                userCreateAdminRequest.DateOfBirth,
                "M/d/yyyy",
                CultureInfo.InvariantCulture),
            Email = userCreateAdminRequest.Email,
            IsEmailVerified = userCreateAdminRequest.IsEmailVerified,
            PhoneNumber = userCreateAdminRequest.PhoneNumber,
            IsPhoneNumberVerified = userCreateAdminRequest.IsPhoneNumberVerified,
            Address = userCreateAdminRequest.Address,
            Culture = userCreateAdminRequest.Culture,
            IsAdmin = userCreateAdminRequest.IsAdmin,
            IsSuspended = userCreateAdminRequest.IsSuspended,
        };
    }

    public static implicit operator User(UserUpdateAdminRequest userUpdateAdminRequest)
    {
        return new()
        {
            Username = userUpdateAdminRequest.Username,
            FirstName = userUpdateAdminRequest.FirstName,
            LastName = userUpdateAdminRequest.LastName,
            Gender = (GenderTypes)userUpdateAdminRequest.Gender,
            DateOfBirth = DateTime.ParseExact(
                userUpdateAdminRequest.DateOfBirth,
                "M/d/yyyy",
                CultureInfo.InvariantCulture),
            Email = userUpdateAdminRequest.Email,
            IsEmailVerified = userUpdateAdminRequest.IsEmailVerified,
            PhoneNumber = userUpdateAdminRequest.PhoneNumber,
            IsPhoneNumberVerified = userUpdateAdminRequest.IsPhoneNumberVerified,
            Address = userUpdateAdminRequest.Address,
            Culture = userUpdateAdminRequest.Culture,
            IsAdmin = userUpdateAdminRequest.IsAdmin,
            IsSuspended = userUpdateAdminRequest.IsSuspended,
        };
    }
}