How to setup the application locally.

1.) Open project.
	Double click on the ElyriaAlumniAssociation.sln file to open the project.
	
2.) Connect to database.
	If it is not already present, create a file called appsettings.json in the project's root directory. The files text should be as follows, replacing <your_connection_string> with the connection string for your SQL server database.
	
	{
  "ConnectionStrings": {
    "DefaultConnection": "<your_connection_string>"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}

3.) Create database tables.
	Run the following commands in Visual Studio's Package Manager Console:
	
	add-migration FirstMigration -Context ApplicationDbContext
	update-database -Context ApplicationDbContext
	add-migration FirstMigration -Context DeletedAlumnusDbContext
	update-database -Context DeletedAlumnusDbContext

4.) Create initial admin account by doing the following:
	Run the following commands in Visual Studio's Package Manager Console:
	
	add-migration AddAdminAccount -Context ApplicationDbContext
	
	A file will open whose name begins with a series of numbers followed by the _AddAdminAccount. This is a migration file.
	Open the file named AddAdminAccountMigrationCode.txt
	Copy the entire text of AddAdminAccountMigrationCode, delete everything in the _AddAdminAccount migration file, and copy the text of AddAdminAccountMigrationCode into the now empty _AddAdminAccount migration file.
	
	Run the following command:
	
	update-database -Context ApplicationDbContext
	
5.) Setup SendGrid integration
	Go to https://sendgrid.com , register for an account, and generate an API Key. If you have questions on how to do this please reference this page: https://docs.sendgrid.com/for-developers/sending-email/api-getting-started
	Open the Developer PowerShell window in Visual Studio and run the following commands substituting your API Key for <key> in the first command :
	
	dotnet user-secrets set SendGridKey <key>
	Install-Package SendGrid
	
	Open the EmailSender.cs file and replace the email address on lines 45 and 66 with the email address that you would like the applications emails to come from.
	
	*Please note, this step will differ for a cloud deployment, and the steps above will not work for any non-local deployment. You will need a solution for vaulting and retrieving API Keys that works in the cloud environment of your choosing.
	 This may also involve small code changes to the AuthMessageSenderOptions.cs file.
	
6.) Run the application.
	In Visual Studio click on the green play arrow with the application name next to it and the application will launch.
	