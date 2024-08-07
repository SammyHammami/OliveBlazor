// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using OliveBlazor.Core.Application.Services.Email;
using OliveBlazor.Core.Application.Services.IdentityManagement;
using OliveBlazor.Infrastructure.Indentity;

namespace OliveBlazor.UI.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<UserIdentity> _signInManager;
        private readonly UserManager<UserIdentity> _userManager;
        private readonly IUserStore<UserIdentity> _userStore;
        private readonly IUserEmailStore<UserIdentity> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailService _emailService;

        private readonly IIdentityService _identityService;
          private readonly IConfiguration _configuration;
  

        public RegisterModel(
            UserManager<UserIdentity> userManager,
            IUserStore<UserIdentity> userStore,
            SignInManager<UserIdentity> signInManager,
            ILogger<RegisterModel> logger,
            IEmailService emailService,
            IIdentityService identityService, IConfiguration configuration)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailService = emailService;
            _identityService = identityService;
            _configuration = configuration;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {


            [Required]
           
            [Display(Name = "First Name")]
            public string FirstName { get; set; }
            
            
            [Required]
           
            [Display(Name = "Last Name")]
            public string LastName { get; set; }



            [Required]
            
            [Display(Name = "UserName")]
            public string UserName { get; set; }



            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }



        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                var dto = new UserRegistrationDto
                {
                    Password = Input.Password,
                    Email = Input.Email,
                    UserName = Input.UserName,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                };

                var result = await _identityService.CreateUserAsync(dto);

                if (result.Success)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var emailConfirmationEnabled = _configuration.GetValue<bool>("EmailConfirmation:Enabled");
                    var registrationData = result.Data;
                    var userId = registrationData.UserId;
                    var code = registrationData.EmailConfirmationToken;
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    var emailTextMessage = $"Please confirm your account by clicking here:\n {callbackUrl}";

                    if (emailConfirmationEnabled)
                    {
                        // Send the confirmation email if email confirmation is enabled
                        await _emailService.SendEmailAsync(Input.Email, "Confirm your email", emailTextMessage);

                        // Redirect to register confirmation page
                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                        }
                    }
                    else
                    {
                        // Directly confirm the email if email confirmation is disabled
                        var user = await _userManager.FindByIdAsync(userId);
                        if (user != null)
                        {
                            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                            var confirmResult = await _userManager.ConfirmEmailAsync(user, decodedToken);

                            if (!confirmResult.Succeeded)
                            {
                                _logger.LogWarning("Failed to automatically confirm email for user {UserId}: {Errors}", userId, confirmResult.Errors);
                            }

                            await _identityService.SignInAsync(userId);
                        }
                        return LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }

           
            return Page();
        }


        private UserIdentity CreateUser()
        {
            try
            {
                return Activator.CreateInstance<UserIdentity>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(UserIdentity)}'. " +
                    $"Ensure that '{nameof(UserIdentity)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<UserIdentity> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<UserIdentity>)_userStore;
        }
    }
}
