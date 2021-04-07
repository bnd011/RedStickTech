using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using FluentEmail.Core;


namespace ShopBot.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }


            private string salt;
            private string Salt
            {
                get
                {
                    return salt;
                }
                set
                {
                    Console.WriteLine("bees are in my teeth");
                    salt = GetSalt();
                }
            }

            private string password;
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password
            {
                get
                {
                    return password;
                }
                set
                {
                    string prehash = value + Salt;
                    password = GetHash(prehash);
                    Console.WriteLine(password);
                }
            }

            private string confirmPassword;
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword
            {
                get
                {
                    return confirmPassword;
                }
                set
                {
                    string prehash = value + Salt;
                    confirmPassword = GetHash(prehash);
                    Console.WriteLine(confirmPassword);
                }
            }

            [Required]
            [Display(Name = "I verify that I am over the age of 18")]
            [VerifyChecked(ErrorMessage = "This box must be checked")]
            public Boolean AgeVerification { get; set; }

            private static string GetSalt()
            {
                Random r = new Random();
                String salt = "";
                for(int i=0; i<64; i++)
                {
                    char curr = 'a';
                    int rand_char_offset = r.Next(0, 26);
                    curr += (char)rand_char_offset;
                    salt.Append(curr);
                }
                Console.WriteLine(salt);
                //implement Salt 64
                return salt;
            }

            private static string GetHash(string input)
            {
                //implement RSA11
                return input;
            }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        /*public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            //return View();
        } */

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                Console.WriteLine("############# Async command stub #############");
                var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                var test = Input.Password;
                Console.WriteLine("EGGGGGGGGGs: ",test);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action("ConfirmEmail", "/Identity", new { userId = user.Id, token = code }, Request.Scheme);

                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);
                    var email = await Email 
                        .From("service@shopbot.com")
                        .To(Input.Email)
                        .Subject("Confirm your shopbot account")
                        .Body("Thank you registering a ShopBot account. Please use the link below to confirm your account: \n" + confirmationLink)
                        .SendAsync();


                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                       // $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }


    }

    internal class VerifyCheckedAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value is bool && (bool)value;
        }
    }
}
