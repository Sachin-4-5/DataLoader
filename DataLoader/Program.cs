using DataLoader.Interfaces;
using DataLoader.Logger;
using DataLoader.Templates;
using DataLoader.Services;
using DataLoader.Archive;
using DataLoader.Mailer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.CommandLine;
using System.Globalization;

try
{
    // Read Configuration with Argument
    var argsForNow = new[] { "--TemplatePath=C:\\Users\\pr667\\OneDrive\\Desktop\\Projects\\2. Console Apps (.NET Core)\\ProdData\\Template\\TransactionTemplate.xml" };
    var configuration = BuildConfiguration(argsForNow);


    // Initialize Logger
    var logPath = configuration["Paths:LogFilePath"];
    if (string.IsNullOrWhiteSpace(logPath))
        throw new Exception("Logging:LogFilePath missing in appsettings.json");
    ErrorLogger.Initialize(logPath);
    ErrorLogger.Log(nameof(Program), "Startup", "DataLoader application started", ErrorLogger.LogLevel.INFO);


    // Read Template Path
    var templatePath = configuration["TemplatePath"];
    if (string.IsNullOrWhiteSpace(templatePath))
        throw new Exception("TemplatePath argument is missing");
    ErrorLogger.Log(nameof(Program), "Template", $"Template path: {templatePath}", ErrorLogger.LogLevel.INFO);


    // Load Template
    var templateReader = new TemplateReader();
    var template = templateReader.ReadTemplate(templatePath);
    ErrorLogger.Log(nameof(Program),  "Template", "Template loaded successfully", ErrorLogger.LogLevel.INFO);


    // Input Directory Validation
    if (!Directory.Exists(template.InputFilePath))
        throw new DirectoryNotFoundException(template.InputFilePath);


    // Fetch input files
    var inputFiles = Directory.GetFiles(template.InputFilePath, template.InputFilePattern, SearchOption.TopDirectoryOnly);
    if (!inputFiles.Any())
        throw new Exception("No input files found");


    // Search Latest Input File
    var latestFile = GetLatestDatedFile(inputFiles) ?? throw new Exception("No valid dated file found");
    ErrorLogger.Log(nameof(Program), "InputFile", $"Latest file selected: {Path.GetFileName(latestFile)}", ErrorLogger.LogLevel.INFO);
    Console.WriteLine($"Processing file started: {latestFile}");


    // Read lates transaction xml file
    IXmlReader xmlReader = new XmlTransactionParser();
    var transactions = xmlReader.Read(latestFile, template.IterationXPath);
    if (!transactions.Any())
        ErrorLogger.Log(nameof(Program), "XMLRead", "No transactions found in XML file", ErrorLogger.LogLevel.INFO);
    else
        ErrorLogger.Log(nameof(Program), "XMLRead", $"Total transactions loaded: {transactions.Count}", ErrorLogger.LogLevel.INFO);
    Console.WriteLine($"Transactions parsed: {transactions.Count}");


    // Db insertion via stored procedure
    var connectionString = configuration.GetConnectionString("DBCS");
    IDataInserter inserter = new StoredProcedureInserter();
    foreach (var transaction in transactions)
    {
        inserter.Insert(transaction, template, connectionString, Path.GetFileName(latestFile));
    }
    ErrorLogger.Log(nameof(Program), "XMLRead", "Records inserted successfully!", ErrorLogger.LogLevel.INFO);


    // Archive logic
    var archiveProcess = new ArchiveProcess();
    archiveProcess.ArchiveFile(latestFile, configuration["Paths:ArchivePath"]);


    // Email Notification
    var successBody = $"""
        Transaction file processed successfully.
        File Name: {Path.GetFileName(latestFile)}
        Records: {transactions.Count}
        Date: {DateTime.Now}

        Regards,
        DataLoader 
    """;
    MailSender mail = new MailSender(configuration);
    mail.SendMail("Transaction Loader - SUCCESS", successBody);
    ErrorLogger.Log(nameof(Program), "Complete", "Email Sent Successfully!", ErrorLogger.LogLevel.INFO);
    ErrorLogger.Log(nameof(Program), "Complete", "Processing completed successfully!", ErrorLogger.LogLevel.INFO);
}
catch (Exception ex)
{
    ErrorLogger.Log(nameof(Program), "Fatal", "Unhandled exception occurred", ErrorLogger.LogLevel.ERROR, ex);
    Console.WriteLine("Application failed. Check logs.");
}





// --------------
// Helper Methods
// --------------

static IConfiguration BuildConfiguration(string[] args)
{
    return new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: false)
        .AddCommandLine(args)
        .Build();
}

static string? GetLatestDatedFile(string[] files)
{
    return files
        .Select(f => new { File = f, Date = ExtractDateFromFileName(f) })
        .Where(x => x.Date.HasValue)
        .OrderByDescending(x => x.Date)
        .FirstOrDefault()?.File;
}

static DateTime? ExtractDateFromFileName(string filePath)
{
    var token = Path.GetFileName(filePath)
        .Split('.', '_')
        .FirstOrDefault(t => t.Length == 8 && t.All(char.IsDigit));
    return DateTime.TryParseExact(token, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date) ? date : null;
}