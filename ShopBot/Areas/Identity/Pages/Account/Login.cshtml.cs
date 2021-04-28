﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace ShopBot.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<IdentityUser> signInManager, 
            ILogger<LoginModel> logger,
            UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            private string[] Qreturn { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            protected internal String Strhash { get; set; }
            protected internal byte[] Bhash { get; set; }
            private byte[] Hash
            {
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

            private string Verify { get; set; }
            private string Salt { get; set; }

            private string password;
            [Required]
            [DataType(DataType.Password)]
            public string Password {
                get
                {
                    return password;
                }
                set 
                {
                    Qreturn = GetQReturn();
                    //Console.WriteLine("Q: " + Qreturn);
                    Verify = Qreturn[1];
                    Salt = Qreturn[2];
                    password = value;
                    Hash = GetHash(password + Salt);
                    Console.WriteLine("Password: " + password);
                    Console.WriteLine("Salt: " + Salt);
                    Console.WriteLine("Verify: " + Verify);
                    Console.WriteLine("Hash: " + Strhash);
                    if (Strhash == Verify)
                    {
                        Console.WriteLine("Hash Matches Verify");
                    }
                    else
                    {
                        Console.WriteLine("Hash Does Not Match Verify");
                    }
                } 
            }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }

            private string [] GetQReturn()
            {
                string[] output = { "null", "null", "null"};
                string ConnectionStr = "Server= rst-db-do-user-8696039-0.b.db.ondigitalocean.com;Port = 25060;Database=RST_DB;Uid=doadmin;Pwd=wwd0oli7w2rplovh;SslMode=Required;";
                MySqlConnection connect = new MySqlConnection(ConnectionStr);
                MySqlCommand login = connect.CreateCommand();
                login.CommandText = GetLoginDetailsQueary();
                connect.Open();
                try
                {
                    MySqlDataReader connection = login.ExecuteReader();
                    if (connection.HasRows)
                    {
                        connection.Read();
                        if (connection.FieldCount != 3)
                        {
                            connect.Close();
                            Console.WriteLine("Too many Fields: ", connection.FieldCount);
                            return output;
                        }
                        else
                        {
                            string[] results = { (string)connection.GetValue(0),
                                                (string)connection.GetValue(1),
                                                (string)connection.GetValue(2)};
                            Console.WriteLine(results[1], results[2]);
                            connect.Close();
                            return results;
                        }

                    }
                    else
                    {
                        connect.Close();
                        Console.WriteLine("Invalid Email Address");
                        return output;
                    }
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                connect.Close();
                Console.WriteLine("Query failed");
                return output;
            }

            private string GetLoginDetailsQueary()
            {
                String queary = "SELECT * FROM `RST_DB`.`user_login_info` WHERE `user_email` = '" + Email +"'";
                return queary;
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

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
