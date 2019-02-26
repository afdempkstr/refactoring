using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Refactoring
{
    class Program
    {
        static void Main(string[] args)
        {
            MenuManager menus = new MenuManager();
            DataBaseManager dBManager = new DataBaseManager();
            InputManager inputManager = new InputManager();
            MessageManager messageManager = new MessageManager();
            App app = new App();

            bool shouldExit = false;
            do
            {
                Enums.MainMenuOptions mainMenuChoice = menus.MainMenu(); //Run the Main Menu

                switch (mainMenuChoice)
                {
                    case Enums.MainMenuOptions.Login:
                        string usernameLogin;
                        menus.LoginMenu(); // Includes console clear and welcome message-to check that
                        usernameLogin = inputManager.InputUserName(); // Returns a string or null if ESC is pressed

                        if (usernameLogin == null)
                        {
                            // Break to main menu if username is null after ESC is pressed
                            break;
                        }

                        // checks if username exists in database
                        if (!app.ValidateUsername(usernameLogin)) break;

                        // username exists in database, continue to ask for password
                        // check if user is active
                        if (!app.ValidateActiveUser(usernameLogin)) break;

                        Console.WriteLine($"\nWelcome {usernameLogin}!");

                        bool isPasswordCorrect = app.ValidatePassword(usernameLogin);
                        if (!isPasswordCorrect) break;
                    
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Correct Password.");
                        Console.ResetColor();

                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("\nPress ESC to log out, Ctrl Q to exit or Enter to proceed to User Menu.");
                        Console.ResetColor();

                        Enums.ExitOptions userExitOption = inputManager.InputExitChoice();

                        switch (userExitOption)
                        {
                            case Enums.ExitOptions.Esc:
                                Console.Clear();
                                Console.WriteLine("\nLogging out...");
                                System.Threading.Thread.Sleep(700);
                                break;

                            case Enums.ExitOptions.CtrlQ:
                                Console.Clear();
                                Console.WriteLine("\nClosing application...");
                                shouldExit = true;
                                break;

                            case Enums.ExitOptions.Enter:

                                bool isLogged = true;

                                Enums.UserTypes userType = dBManager.GetUserType(usernameLogin); //check user type

                                switch (userType)
                                {
                                    case Enums.UserTypes.User:
                                        do
                                        {
                                            Enums.UserMenuOptions userMenuOption = menus.UserMenu(usernameLogin);
                                            switch (userMenuOption)
                                            {
                                                case Enums.UserMenuOptions.CreateNewMessage:
                                                    app.CreateUserMessage(usernameLogin, ConsoleColor.Cyan);
                                                    break;

                                                case Enums.UserMenuOptions.Inbox:
                                                    messageManager.ShowInbox(usernameLogin);
                                                    break;

                                                case Enums.UserMenuOptions.SentMessages:
                                                    messageManager.ShowSentMessages(usernameLogin);
                                                    break;

                                                case Enums.UserMenuOptions.Info:
                                                    dBManager.GetUserInfo(usernameLogin);
                                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                                    Console.WriteLine("\nPress any key to go back");
                                                    Console.ResetColor();
                                                    Console.ReadKey();
                                                    break;

                                                case Enums.UserMenuOptions.ExitToMain:
                                                    Console.Clear();
                                                    isLogged = false;
                                                    Console.WriteLine("\nLogging out...");
                                                    System.Threading.Thread.Sleep(700);
                                                    break;

                                                case Enums.UserMenuOptions.Quit:
                                                    Console.Clear();
                                                    isLogged = false;
                                                    Console.WriteLine("\nClosing application...");
                                                    Environment.Exit(0);
                                                    break;
                                            }
                                        } while (isLogged);
                                        break;

                                    case Enums.UserTypes.JuniorAdmin:
                                        do
                                        {
                                            Enums.JuniorAdminMenuOptions juniorAdminMenuOption = menus.JuniorAdminMenu(usernameLogin);

                                            switch (juniorAdminMenuOption)
                                            {
                                                case Enums.JuniorAdminMenuOptions.CreateNewMessage:

                                                    app.CreateUserMessage(usernameLogin, ConsoleColor.Cyan);
                                                    break;

                                                case Enums.JuniorAdminMenuOptions.Inbox:

                                                    messageManager.ShowInbox(usernameLogin);
                                                    break;

                                                case Enums.JuniorAdminMenuOptions.SentMessages:

                                                    messageManager.ShowSentMessages(usernameLogin);
                                                    break;

                                                case Enums.JuniorAdminMenuOptions.Info:

                                                    dBManager.GetUserInfo(usernameLogin);
                                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                                    Console.WriteLine("\nPress any key to go back");
                                                    Console.ResetColor();
                                                    Console.ReadKey();
                                                    break;

                                                case Enums.JuniorAdminMenuOptions.ViewUserInfo:

                                                    dBManager.ViewUserInfo();
                                                    break;

                                                case Enums.JuniorAdminMenuOptions.ViewUserMessages:

                                                    messageManager.ViewUserMessages();
                                                    break;

                                                case Enums.JuniorAdminMenuOptions.ViewAllMessages:

                                                    messageManager.ViewAllMessages();
                                                    break;

                                                case Enums.JuniorAdminMenuOptions.EditMessages:

                                                    messageManager.EditMessage();
                                                    break;

                                                case Enums.JuniorAdminMenuOptions.ExitToMain:
                                                    Console.Clear();
                                                    isLogged = false;
                                                    Console.WriteLine("\nGoodbye Junior...");
                                                    System.Threading.Thread.Sleep(700);
                                                    break;

                                                case Enums.JuniorAdminMenuOptions.Quit:
                                                    Console.Clear();
                                                    isLogged = false;
                                                    Console.WriteLine("\nClosing application...");
                                                    Environment.Exit(0);
                                                    break;
                                            }

                                        } while (isLogged);
                                        break;

                                    case Enums.UserTypes.MasterAdmin:
                                        do
                                        {
                                            Enums.MasterAdminMenuOptions masterAdminMenuOption = menus.MasterAdminMenu(usernameLogin);

                                            switch (masterAdminMenuOption)
                                            {
                                                case Enums.MasterAdminMenuOptions.CreateNewMessage:

                                                    app.CreateUserMessage(usernameLogin, ConsoleColor.Blue);
                                                    break;

                                                case Enums.MasterAdminMenuOptions.Inbox:

                                                    messageManager.ShowInbox(usernameLogin);
                                                    break;

                                                case Enums.MasterAdminMenuOptions.SentMessages:

                                                    messageManager.ShowSentMessages(usernameLogin);
                                                    break;

                                                case Enums.MasterAdminMenuOptions.Info:

                                                    dBManager.GetUserInfo(usernameLogin);
                                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                                    Console.WriteLine("\nPress any key to go back");
                                                    Console.ResetColor();
                                                    Console.ReadKey();
                                                    break;

                                                case Enums.MasterAdminMenuOptions.ViewUserInfo:

                                                    dBManager.ViewUserInfo();
                                                    break;

                                                case Enums.MasterAdminMenuOptions.ViewUserMessages:

                                                    messageManager.ViewUserMessages();
                                                    break;

                                                case Enums.MasterAdminMenuOptions.ViewAllMessages:

                                                    messageManager.ViewAllMessages();
                                                    break;

                                                case Enums.MasterAdminMenuOptions.EditMessages:

                                                    messageManager.EditMessage();
                                                    break;

                                                case Enums.MasterAdminMenuOptions.DeleteMessages:

                                                    messageManager.DeleteMessage();
                                                    break;

                                                case Enums.MasterAdminMenuOptions.ExitToMain:
                                                    Console.Clear();
                                                    isLogged = false;
                                                    Console.WriteLine("\nGoodbye Admin...");
                                                    System.Threading.Thread.Sleep(700);
                                                    break;

                                                case Enums.MasterAdminMenuOptions.Quit:
                                                    Console.Clear();
                                                    isLogged = false;
                                                    Console.WriteLine("\nClosing application...");
                                                    Environment.Exit(0);
                                                    break;
                                            }

                                        } while (isLogged);

                                        break;

                                    case Enums.UserTypes.SuperAdmin:
                                        do
                                        {
                                            Enums.SuperAdminMenuOptions superAdminMenuOption = menus.SuperAdminMenu(usernameLogin);

                                            switch (superAdminMenuOption)
                                            {
                                                case Enums.SuperAdminMenuOptions.CreateNewMessage:

                                                    app.CreateUserMessage(usernameLogin, ConsoleColor.Cyan);
                                                    break;

                                                case Enums.SuperAdminMenuOptions.Inbox:

                                                    messageManager.ShowInbox(usernameLogin);
                                                    break;

                                                case Enums.SuperAdminMenuOptions.SentMessages:

                                                    messageManager.ShowSentMessages(usernameLogin);
                                                    break;

                                                case Enums.SuperAdminMenuOptions.Info:

                                                    dBManager.GetUserInfo(usernameLogin);
                                                    Console.ForegroundColor = ConsoleColor.Blue;
                                                    Console.WriteLine("\nPress any key to go back");
                                                    Console.ResetColor();
                                                    Console.ReadKey();
                                                    break;
                                                //==========================================================================================================================================//
                                                case Enums.SuperAdminMenuOptions.CreateNewUser:

                                                    dBManager.CreateNewUser();
                                                    break;

                                                case Enums.SuperAdminMenuOptions.DeleteUser:

                                                    dBManager.DeleteUser();
                                                    break;

                                                case Enums.SuperAdminMenuOptions.ActivateUser:

                                                    dBManager.ActivateUser();
                                                    break;

                                                case Enums.SuperAdminMenuOptions.EditUserType:

                                                    dBManager.EditUserType();
                                                    break;

                                                case Enums.SuperAdminMenuOptions.ViewUserInfo:

                                                    dBManager.ViewUserInfo();
                                                    break;

                                                case Enums.SuperAdminMenuOptions.ViewUserMessages:

                                                    messageManager.ViewUserMessages();
                                                    break;

                                                case Enums.SuperAdminMenuOptions.ViewAllMessages:

                                                    messageManager.ViewAllMessages();
                                                    break;

                                                case Enums.SuperAdminMenuOptions.DeleteMessages:

                                                    messageManager.DeleteMessage();
                                                    break;

                                                case Enums.SuperAdminMenuOptions.EditMessages:

                                                    messageManager.EditMessage();
                                                    break;

                                                case Enums.SuperAdminMenuOptions.ExitToMain:
                                                    Console.Clear();
                                                    isLogged = false;
                                                    Console.WriteLine("\nGoodbye Master...");
                                                    System.Threading.Thread.Sleep(700);
                                                    break;

                                                case Enums.SuperAdminMenuOptions.Quit:
                                                    Console.Clear();
                                                    isLogged = false;
                                                    Console.WriteLine("\nClosing application...");
                                                    Environment.Exit(0);
                                                    break;
                                            }

                                        } while (isLogged);

                                        break;
                                }
                                break;
                        }
                        break;

                    case Enums.MainMenuOptions.SignUp:
                        app.SignUpUser();
                        break;

                    case Enums.MainMenuOptions.Info:
                        dBManager.GetInfo();
                        break;

                    case Enums.MainMenuOptions.Exit:
                        Console.Clear();
                        Console.WriteLine("\nClosing application...");
                        shouldExit = true;
                        break;
                }
            } while (!shouldExit);
        }
    }
}
