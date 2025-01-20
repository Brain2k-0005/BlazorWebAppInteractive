# Project Setup

This document provides a step-by-step guide to set up the project, including database configuration and mail service setup.

---

## Database Setup

1. Open your project in your preferred IDE (e.g., Visual Studio).
2. Navigate to the **Package Manager Console**.
3. Run the following command to initialize the database:
   ```bash
   Update-Database
   ```
4. The database path is specified in the `appsettings.json` file under the property `DefaultConnection`. Ensure the connection string is configured correctly.

---

## Mail Service Setup

1. Navigate to the backend of the project:
   ```
   Backend -> Services -> EmailSender.cs
   ```
2. Open the `EmailSender.cs` file.
3. Locate the `Sender` method.
4. Update the method with your email credentials (e.g., Gmail credentials). Ensure the following information is provided:
   - Email address
   - Password or App-Specific Password

   Example configuration:
   ```csharp
   smtpClient.Credentials = new NetworkCredential("your-email@gmail.com", "your-password");
   ```

   **Note**: For Gmail accounts, ensure you have enabled less secure app access or configured an app password.
