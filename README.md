## DataLoader Application  

### 📘 Overview  
DataLoader is a console based utility application developed using .NET 8.0 (LTS) that reads XML files and loads the data into a database using ADO.NET technique. This tool is designed to automate XML processing and database insertion efficiently.

---
<br />



### 🚀 Features  
✅ Reads structured XML files from a specified directory  
✅ Parses and validates XML content  
✅ Inserts data into the configured SQL Server database  
✅ Logs processing details for troubleshooting to a logs folder <br />
✅ Moves processed files to an archive folder <br />
✅ Sends success/failure email notification after processing 

---
<br />



### 🎯 Prerequisite Installation
1️⃣ Download VS-2022 from Google. <br />
2️⃣ Select .NET desktop development and click install. <br />
3️⃣ This installs - .NET SDK, C# Compiler, Console templates and MSBuild (CLI). <br />

---
<br />



### 📌 Project Configuration
1️⃣ Project Name: DataLoader <br />
2️⃣ Solution Name: DataLoader.sln <br />
3️⃣ Framework: .NET 8 (LTS) <br />
4️⃣ Template: Console Application <br />
5️⃣ Language: C# <br />
6️⃣ Database: MS SQL Server 2022 <br />
7️⃣ Library: ADO.NET <br />
8️⃣ Tools: Visual Studio 2022, SSMS <br />

---
<br />



### 🎓 Project structure
```
DataLoader
│
├── bin/
├── obj/
├── Program.cs
├── appsettings.json
│
├── Archive/
│   └── ArchiveProcess.cs
│
├── Logger/
│   ├── IAppLogger.cs
│   └── ErrorLogger.cs
│
├── Models/
│   ├── Transaction.cs
│   ├── DataLoadResult.cs
│
├── Interfaces/
│   ├── ITemplateReader.cs
│   ├── IFileSelector.cs
│   ├── IXmlDataReader.cs
│   ├── IDataInserter.cs
│   ├── IArchiveService.cs
│   ├── INotificationService.cs
│
├── Templates/
│   ├── DataLoadTemplate.cs
│   ├── ColumnMapping.cs
│   └── TemplateReader.cs
│
├── Services/
│   ├── FileSelectorService.cs
│   ├── XmlDataReader.cs
│   ├── StoredProcedureInserter.cs
│   ├── DataLoadOrchestrator.cs
│   ├── MailNotificationService.cs
│
└── Utilities/
    ├── FileHashUtility.cs
    └── DateParser.cs

├── ProdData/
│   ├── Archive/
│   ├── Logs/
│   ├── Input/
│   ├── Template/
│   ├── Error/

```

---
<br />



### 📍 Project Architecture Feature
🔹 SOLID principles <br />
🔹 Dependency Injection (DI) <br />
🔹 Separation of Concerns <br />
🔹 Security & Maintainability <br />
🔹 Future extensibility (CSV, Excel, REST, etc.) <br 

---
<br />



### 💡 Future Enhancements
🔹 Implement multi-threading/parallel processing for faster processing <br />
🔹 Add support for multiple data sources like - excel, json, etc. <br />
🔹 Add support for multiple database types (MySQL, PostgreSQL) <br />
🔹 Implement unit testing using NUnit Framework. <br />

---
<br />



### 🤝 Contribution
Pull requests are welcome! To contribute:

1️⃣ Fork the repo <br />
2️⃣ Create a feature branch (git checkout -b feature-xyz) <br />
3️⃣ Commit changes (git commit -m "Added feature xyz") <br />
4️⃣ Push to your branch (git push origin feature-xyz) <br />
5️⃣ Create a pull request 

---
<br />
<br />



















