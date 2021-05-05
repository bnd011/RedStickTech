using System;
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
            public InputModel()
            {
                publicKey = Controllers.HomeController.publicKey;
                GenerateKeys();
            }
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
            protected internal String Strhash2 { get; set; }
            protected internal byte[] Bhash2 { get; set; }
            public byte[] Hash2
            {
                get
                {
                    return Bhash2;
                }
                set
                {
                    Bhash2 = value;
                    Strhash2 = Convert.ToBase64String(value);
                }
            }

            public static RSAParameters publicKey;

            private static RSAParameters privateKey;

            
            public static Boolean KeyExists { get; set; } = false;

            //private static RSAParameters test;
            private string Verify { get; set; }
            private string Salt { get; set; }

            private string password;
            [Required]
            [DataType(DataType.Password)]
            public string Password
            {
                get
                {
                    return password;
                }
                set
                {
                    Controllers.HomeController.passEmail(Email);
                    //ShopBot.Controllers.HomeController.ViewBag.account = Email;
                    Qreturn = GetQReturn();
                    //Console.WriteLine("Q: " + Qreturn);
                    Verify = Qreturn[1];
                    Salt = Qreturn[2];
                    password = value;
                    //publicKey = Controllers.HomeController.publicKey;
                    Strhash = GetHash(Verify);
                    Strhash2 = GetHash(Verify);
                    Console.WriteLine("Password: " + password);
                    Console.WriteLine("Salt: " + Salt);
                    Console.WriteLine("Verify: " + Verify);
                    Console.WriteLine("Hash: " + Strhash);
                    Console.WriteLine("Hash 2: " + Strhash2);
                    if (Strhash == password + Salt)
                    {
                        Console.WriteLine("Hash Matches Verify");
                        
                    }
                    else
                    {
                        Console.WriteLine("Hash Does Not Match Verify");
                        FailedLogin();
                    }
                }
            }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }

            private string[] GetQReturn()
            {
                string[] output = { "null", "null", "null" };
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
                            //Console.WriteLine("GQR!: " + results[1] + " " + results[2]);
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
            private void FailedLogin()
            {
                string ConnectionStr = "Server= rst-db-do-user-8696039-0.b.db.ondigitalocean.com;Port = 25060;Database=RST_DB;Uid=doadmin;Pwd=wwd0oli7w2rplovh;SslMode=Required;";
                MySqlConnection connect = new MySqlConnection(ConnectionStr);
                MySqlCommand login = connect.CreateCommand();
                login.CommandText = GetFailedLoginRequestQuery();
                connect.Open();
                try
                {
                    MySqlDataReader connection = login.ExecuteReader();
                    // the Failed Login Table has an existing Row with the Email
                    if (connection.HasRows)
                    {
                        connection.Read();
                        if (connection.FieldCount != 3)
                        {
                            connect.Close();
                            Console.WriteLine("Too many Fields: ", connection.FieldCount);
                        }
                        else
                        {
                            int prevFail = (int)connection.GetValue(1);
                            string last_time = (string)connection.GetValue(2);
                            connect.Close();
                            MySqlConnection connect2 = new MySqlConnection(ConnectionStr);
                            MySqlCommand login2 = connect2.CreateCommand();
                            login2.CommandText = GetFailedLoginModifyQuery(prevFail);
                            connect2.Open();
                            Console.WriteLine("Last Login Failure on: ", last_time);
                            string connection2 = (string)login2.ExecuteScalar();
                            connect2.Close();
                        }
                    }
                    // the Failed Login Table does not have an existing Row with the Email
                    else
                    {
                        MySqlConnection connect3 = new MySqlConnection(ConnectionStr);
                        MySqlCommand login3 = connect3.CreateCommand();
                        login3.CommandText = GetFailedLoginNewQuery();
                        connect3.Open();
                        string connection3 = (string)login3.ExecuteScalar();
                        connect3.Close();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                connect.Close();
                Console.WriteLine("Failed Login Stub");
            }

            private string GetLoginDetailsQueary()
            {
                String queary = "SELECT * FROM `RST_DB`.`user_login_info` WHERE `user_email` = '" + Email + "';";
                return queary;
            }

            private string GetFailedLoginRequestQuery()
            {
                String queary = "SELECT * FROM `RST_DB`.`failed_login` WHERE `user_email` = '" + Email + "';";
                return queary;
            }

            private string GetFailedLoginModifyQuery(int value)
            {
                DateTime now = DateTime.Now;
                String queary = "UPDATE `RST_DB`.`failed_login` SET `failed_num` = '" + value + "', `time_of_try` = '" + now + "' WHERE `user_email` = '" + Email + "';";
                return queary;
            }

            private string GetFailedLoginNewQuery()
            {
                DateTime now = DateTime.Now;
                String queary = "INSERT INTO `RST_DB`.`failed_login`(`user_email`, `failed_num`, `time_of_try`) VALUES('" + Email + "', '1', '" + now + "');";
                return queary;
            }


            //https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.rsacryptoserviceprovider?view=net-5.0
            //https://www.youtube.com/watch?v=EA5jF_7FteM
            private static string GetHash(string input)
            {
                try
                {
                    Console.WriteLine(input.Length);
                    input = input.Trim(new char[] { ' ', '=' });
                    Console.WriteLine(input);
                    //Create a UnicodeEncoder to convert between byte array and string.
                    UnicodeEncoding ByteConverter = new();

                    //Create byte arrays to hold original, encrypted, and decrypted data.
                    byte[] dataToEncrypt = ByteConverter.GetBytes(input);
                    byte[] encryptedData;
                    byte[] encryptedData2;
                    byte[] decryptedData;
                    byte[] decryptedData2;

                    //Create a new instance of RSACryptoServiceProvider to generate
                    //public and private key data.
                    using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                    {
                        /*
                        RSAParameters myRSAParameters = RSA.ExportParameters(false);
                        myRSAParameters.Modulus = publicKey.Modulus;
                        myRSAParameters.Exponent = ByteConverter.GetBytes("65537");
                        myRSAParameters.D = publicKey.D;
                        /

                        //Pass the data to ENCRYPT, the public key information 
                        //(using RSACryptoServiceProvider.ExportParameters(false),
                        //and a boolean flag specifying no OAEP padding.
                        Console.WriteLine("Login Encryption:");
                        //Console.WriteLine("Input: " + input);

                        //Console.WriteLine("Public Key : " + publicKey);
                        Console.WriteLine("Public Key Modulus A: " + Convert.ToBase64String(publicKey.Modulus));
                        Console.WriteLine("Public Key Exponant A: " + Convert.ToBase64String(publicKey.Exponent));
                        //Console.WriteLine("Public Key P A: " + Convert.ToBase64String(publicKey.P));
                        //Console.WriteLine("Public Key Q A: " + Convert.ToBase64String(publicKey.Q));
                        //Console.WriteLine("Public Key D" + publicKey.D.ToString());
                        //encryptedData = RSAEncrypt(dataToEncrypt, publicKey, false); //return\
                        Console.WriteLine("Public Key Modulus B: " + Convert.ToBase64String(publicKey.Modulus));
                        Console.WriteLine("Public Key Exponant B: " + Convert.ToBase64String(publicKey.Exponent));
                        //Console.WriteLine("Public Key P B: " + Convert.ToBase64String(publicKey.P));
                        //Console.WriteLine("Public Key Q B: " + Convert.ToBase64String(publicKey.Q));
                        //encryptedData2 = RSAEncrypt(dataToEncrypt, publicKey, false); //return
                        Console.WriteLine("Public Key Modulus C: " + Convert.ToBase64String(publicKey.Modulus));
                        Console.WriteLine("Public Key Exponant C: " + Convert.ToBase64String(publicKey.Exponent));
                        //Console.WriteLine("Public Key P C: " + Convert.ToBase64String(publicKey.P));
                        //Console.WriteLine("Public Key Q C: " + Convert.ToBase64String(publicKey.P));

                        //encryptedData = RSAEncrypt(dataToEncrypt, myRSAParameters, false); //return\
                        //encryptedData2 = RSAEncrypt(dataToEncrypt, myRSAParameters, false); //return
                        //Console.WriteLine("E1: " + Convert.ToBase64String(encryptedData));
                       // Console.WriteLine("E2: " + Convert.ToBase64String(encryptedData2));

                        //decryptedData = RSADecrypt(encryptedData, privateKey, false);
                        //decryptedData2 = RSADecrypt(encryptedData2, privateKey, false);
                        */
                        decryptedData = RSADecrypt(dataToEncrypt, privateKey, false);
                        //decryptedData2 = RSADecrypt(dataToEncrypt, privateKey, false);

                        Console.WriteLine("D1: " + ByteConverter.GetString(decryptedData));
                        //Console.WriteLine("D2: " + ByteConverter.GetString(decryptedData2));

                        //string encryptedString = Encoding.UTF8.GetString(encryptedData);
                        //Console.WriteLine("Encrypted String: " + encryptedString);
                        return ByteConverter.GetString(decryptedData);
                    }
                }
                catch (ArgumentNullException)
                {
                    //Catch this exception in case the encryption did
                    //not succeed.
                    UnicodeEncoding ByteConverter = new UnicodeEncoding();
                    Console.WriteLine("Encryption failed.");
                    return "default";
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
            //https://www.youtube.com/watch?v=EA5jF_7FteM
            public static void GenerateKeys()
            {
                KeyExists = true;
                //UnicodeEncoding ByteConverter = new UnicodeEncoding();
                using (var rsa = new RSACryptoServiceProvider(2048))
                {
                    rsa.PersistKeyInCsp = false; //don't store keys in a container
                    publicKey = rsa.ExportParameters(false);
                    privateKey = rsa.ExportParameters(true);
                    //publicKey.Modulus = Convert.FromBase64String("utDXrcbgXmFJ1uJobk5xrkgUD9gaQetZTPADKDFLOOsEXtpChftLSdJn80Ovc3T1DHmljpzxcGAbjibJVV7QGHgUVrp0XEb0fRTNtHamm7nJQrZmRfmGVpCVMwQ8sWPLGaM2FTCCRgWmnrW151kESimzj5MQqL0JGfpnNh9zhkCWihKtjRyilkSBwUJ+RODDwqGFE8gaGAgxyKG4Tm5GIfYnOW54DPZNHq5ZHPmWTp2LV6cbuNAkMBVLtShMlwLfmOFgYOhBwSefW7CEn7r5pU9esqAR9rCla+1xXrJ6hWLWqRcBviSlk47jbGxbDcmziyNNK4HY0DVCQ");
                    //publicKey.Exponent = ByteConverter.GetBytes("65537");
                    //publicKey.D = privateKey.D;

                }
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
            /*
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
            */
            return RedirectToPage(returnUrl);
        }
    }
}
