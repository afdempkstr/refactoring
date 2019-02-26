using System;

namespace Refactoring
{
    public class App
    {
        private MenuManager menus;
        private DataBaseManager dBManager;
        InputManager inputManager;
        MessageManager messageManager;

        public App()
        {
            menus = new MenuManager();
            dBManager = new DataBaseManager();
            inputManager = new InputManager();
            messageManager = new MessageManager();
        }

        public void CreateUserMessage(string usernameLogin, ConsoleColor color)
        {
            Console.Clear();
            Console.WriteLine("======= Create new Message =======");
            Console.WriteLine();
            Console.WriteLine("Please type the username of the recipient of the message:\n");
            string recipient = inputManager.InputUserName();
            if (recipient is null) //if ESC is pressed
            {
                return;
            }

            // check if recipient username exists in database
            bool recipientExists = dBManager.DoesUsernameExist(recipient);
            if (!recipientExists)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine();
                Console.WriteLine($"A user with username '{recipient}' does not exist.");
                Console.ForegroundColor = color;
                Console.WriteLine("\nPress any key to go back to the user menu");
                Console.ResetColor();
                Console.ReadKey();
            }
            else // the recipient exists. Go on to create and send message
            {
                // check if recipient is active
                bool recipientActive = dBManager.IsUserActive(recipient);
                if (!recipientActive)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nThe recipient you chose is no longer active.");
                    Console.WriteLine("Try sending a message to another user.");
                    Console.ForegroundColor = color;
                    Console.WriteLine("\nPress any key to go back to the user menu");
                    Console.ResetColor();
                    Console.ReadKey();
                }
                else
                {
                    messageManager.CreateMessage(usernameLogin, recipient);
                    Console.ForegroundColor = color;
                    Console.WriteLine("\nPress any key to go back to the user menu");
                    Console.ResetColor();
                    Console.ReadKey();
                }
            }
        }

        public bool ValidatePassword(string usernameLogin)
        {
            Console.WriteLine("\nPlease enter your password.");
            bool isPasswordCorrect = false;
            int numberOfAttempts = 0; // Holds the number of attempts for password input
            int maxNumberOfAttempts = 3;
            string passwordLogin;

            while (!isPasswordCorrect && numberOfAttempts < maxNumberOfAttempts)
            {
                passwordLogin = inputManager.InputLoginPassword();
                numberOfAttempts += 1;
                isPasswordCorrect = dBManager.IsPasswordCorrect(usernameLogin, passwordLogin);
                if (!isPasswordCorrect)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Wrong password. Try again.");
                    Console.ResetColor();
                }
            }

            if (!isPasswordCorrect && (numberOfAttempts == maxNumberOfAttempts))
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Maximum number of attemps reached!");
                Console.ResetColor();
                Console.WriteLine("\nPress any key to go back.");
                Console.ReadKey();
                return false;
            }

            return true;
        }

        public bool ValidateActiveUser(string usernameLogin)
        {
            if (!dBManager.IsUserActive(usernameLogin))
            {
                // user not active
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"The user with username '{usernameLogin}' is no longer active.");
                Console.WriteLine("Press any key to go back");
                Console.ReadKey();
                Console.ResetColor();
                return false;
            }

            return true;
        }

        public bool ValidateUsername(string usernameLogin)
        {
            if (!dBManager.DoesUsernameExist(usernameLogin))
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("The username you entered does not exist in the database. Please SignUp.");
                Console.WriteLine("Press any key to go back to the Main Menu.");
                Console.ResetColor();
                Console.ReadKey();
                return false;
            }

            return true;
        }

        public void SignUpUser()
        {
            bool itExists = false;
            string usernameSignup;
            do
            {
                menus.SignUpMenu();
                usernameSignup = inputManager.InputUserName();
                if (usernameSignup is null)
                {
                    break;
                }

                itExists = dBManager.DoesUsernameExist(usernameSignup); // Check if username already exists in database
                if (itExists)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The username you entered already exists. Please choose another.");
                    Console.ResetColor();
                    Console.ReadKey();
                }
            } while (itExists);

            if (usernameSignup != null) // username is null if ESC is pressed
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The username you entered is not taken. Nice choice!");
                Console.ResetColor();

                Console.WriteLine();
                string password = inputManager.InputPassword();
                dBManager.AddUser(usernameSignup, password); // add new user to database
                Console.WriteLine("\nPress any key to go back to Main Menu.");
                Console.ReadKey();
            }
        }
    }
}
