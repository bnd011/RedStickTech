using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
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
using MySql.Data.MySqlClient;

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


            protected internal string _salt { get; set; }
            private string Salt
            {
                get
                {
                    return _salt;
                }
                set
                {
                    Console.WriteLine("Trust no man unless you have eaten much salt with him");
                    _salt = value;
                }
            }
            protected internal String Strhash { get; set; }
            protected internal byte[] Bhash { get; set; }
            private byte[] Hash {
                get
                {
                    return Bhash;
                }
                set
                {
                    Bhash = value;
                    Strhash = Convert.ToBase64String(value);
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
                    password = value;
                    Salt = GetSalt();
                    Hash = GetHash(password + Salt);
                    Console.WriteLine("Password: " + password);
                    Console.WriteLine("Salt: " + Salt);
                    Console.WriteLine("Hash Length: " + Hash.Length);
                    Console.WriteLine("Hash String: " + Strhash);
                    Console.WriteLine("Animal Crackers in my Soup!");
                    // to user_login_info => username , Salt, password
                    string ConnectionStr = "Server= rst-db-do-user-8696039-0.b.db.ondigitalocean.com;Port = 25060;Database=RST_DB;Uid=doadmin;Pwd=wwd0oli7w2rplovh;SslMode=Required;";
                    MySqlConnection connect = new MySqlConnection(ConnectionStr);
                    MySqlCommand first = connect.CreateCommand();
                    //first.CommandText = "INSERT INTO `RST_DB`.`login`(`user_email`, `pass`) VALUES('dummy4@email.com', 'passsword'); ";
                    first.CommandText = GetLoginQueary();
                    connect.Open();
                    try
                    {
                        string results = (string)first.ExecuteScalar();
                    }catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    connect.Close();
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

                    confirmPassword = value;
                    Console.WriteLine(confirmPassword);
                }
            }

            [Required]
            [Display(Name = "I verify that I am over the age of 18")]
            [VerifyChecked(ErrorMessage = "This box must be checked")]
            public Boolean AgeVerification { get; set; }

            // Creates a 64 digit salt            
            private static string GetSalt()
            {
                Random r = new Random();
                String salt = "";
                for(int i=0; i<20; i++)
                {
                    char curr = 'a';
                    int rand_char_offset = r.Next(0, 26);
                    curr += (char)rand_char_offset;
                    salt += curr;
                    //Console.WriteLine("RCO: " + rand_char_offset);
                    //Console.WriteLine("Current Char: " + curr);
                    //Console.WriteLine("i: " + i + " Salt* " + salt);
                }
                //implement Salt 64
                return salt;
            }

            //https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.rsacryptoserviceprovider?view=net-5.0
            private static byte[] GetHash(string input)
            {
                try
                {
                    //Create a UnicodeEncoder to convert between byte array and string.
                    UnicodeEncoding ByteConverter = new UnicodeEncoding();

                    //Create byte arrays to hold original, encrypted, and decrypted data.
                    byte[] dataToEncrypt = ByteConverter.GetBytes(input);
                    byte[] encryptedData;

                    //Create a new instance of RSACryptoServiceProvider to generate
                    //public and private key data.
                    using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                    {

                        //Pass the data to ENCRYPT, the public key information 
                        //(using RSACryptoServiceProvider.ExportParameters(false),
                        //and a boolean flag specifying no OAEP padding.
                        encryptedData = RSAEncrypt(dataToEncrypt, RSA.ExportParameters(false), false);
                        //https://stackoverflow.com/questions/1003275/how-to-convert-utf-8-byte-to-string
                        //string encryptedString = Encoding.UTF8.GetString(encryptedData);
                        //return encryptedString;
                        return encryptedData;
                    }
                }
                catch (ArgumentNullException)
                {
                    //Catch this exception in case the encryption did
                    //not succeed.
                    UnicodeEncoding ByteConverter = new UnicodeEncoding();
                    Console.WriteLine("Encryption failed.");
                    return ByteConverter.GetBytes("default");
                }
            }

            public static byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
            {
                try
                {
                    byte[] encryptedData;
                    //Create a new instance of RSACryptoServiceProvider.
                    using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                    {

                        //Import the RSA Key information. This only needs
                        //toinclude the public key information.
                        RSA.ImportParameters(RSAKeyInfo);

                        //Encrypt the passed byte array and specify OAEP padding.  
                        //OAEP padding is only available on Microsoft Windows XP or
                        //later.  
                        encryptedData = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
                    }
                    return encryptedData;
                }
                //Catch and display a CryptographicException  
                //to the console.
                catch (CryptographicException e)
                {
                    Console.WriteLine(e.Message);

                    return null;
                }
            }

            public static byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
            {
                try
                {
                    byte[] decryptedData;
                    //Create a new instance of RSACryptoServiceProvider.
                    using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                    {
                        //Import the RSA Key information. This needs
                        //to include the private key information.
                        RSA.ImportParameters(RSAKeyInfo);

                        //Decrypt the passed byte array and specify OAEP padding.  
                        //OAEP padding is only available on Microsoft Windows XP or
                        //later.  
                        decryptedData = RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
                    }
                    return decryptedData;
                }
                //Catch and display a CryptographicException  
                //to the console.
                catch (CryptographicException e)
                {
                    Console.WriteLine(e.ToString());

                    return null;
                }
            }
            
            private string GetLoginQueary()
            {
                String queary = "INSERT INTO `RST_DB`.`user_login_info`(`user_email`, `verify`, `salt`) VALUES('" + Email +"', '" + Strhash +"', '" + Salt +"'); ";
                return queary;
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
                Console.WriteLine("EGGGGGGGGGs: "+test);
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
