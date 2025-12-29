## DataLoader Application  

### 📘 Overview  
DataLoader is a console-based ETL utility application developed using .NET 8.0 (LTS).
It reads structured XML transaction files, parses and validates the data, and loads it into a SQL Server database using stored procedures via ADO.NET.
<br /><br />
The application is designed to follow enterprise-grade batch processing standards, including logging, archiving, error handling, and email notifications. 
Also this tool is designed to automate XML processing and database insertion efficiently.

---
<br />



### 🚀 Features  
✅ Reads XML files from a configurable input directory. <br />
✅ Dynamically loads processing rules from a Template XML. <br />
✅ Parses and validates transaction data. <br />
✅ Inserts data using SQL Server stored procedures (ADO.NET). <br />
✅ Logs each processing step to daily log files. <br />
✅ Archives successfully processed files. <br />
✅ Sends success/failure email notifications. <br />
✅ Batch-job ready (supports command-line arguments). <br />

---
<br />



### 🎯 Software Installation
1️⃣ Download VS-2022 from Google. <br />
2️⃣ Select .NET desktop development and click install. <br />
3️⃣ This installs - .NET SDK, C# Compiler, Console templates and MSBuild (CLI). <br />
4️⃣ Install Microsoft SQL Server 2022. <br />
5️⃣ Install SSMS (SQL Server Management Studio). <br />

---
<br />



### 📌 Project Configuration
1️⃣ Project Name: DataLoader <br />
2️⃣ Solution Name: DataLoader.sln <br />
3️⃣ Framework: .NET 8 (LTS) <br />
4️⃣ Application Type: Console Application <br />
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
│   └── ErrorLogger.cs
│
├── Models/
│   ├── Transaction.cs
│
├── Interfaces/
│   ├── IXmlReader.cs
│   ├── IDataInserter.cs
│
├── Templates/
│   ├── DataLoadTemplate.cs
│   ├── ColumnMapping.cs
│   └── TemplateReader.cs
│
├── Services/
│   ├── StoredProcedureInserter.cs
│   ├── XmlTransactionParser.cs
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



### 📍 Project Architecture Highlights
🔹 SOLID principles <br />
🔹 Dependency Injection (DI) <br />
🔹 Interface-based programming <br />
🔹 Separation of Concerns <br />
🔹 Security & Maintainability <br />
🔹 Future extensibility (CSV, Excel, REST, etc.) <br />

---
<br />



### 💡 Future Enhancements
🔹 Multi-threading / parallel file processing <br />
🔹 Support for CSV, Excel, JSON inputs <br />
🔹 Support for multiple databases (MySQL, PostgreSQL) <br />
🔹 Unit testing using NUnit Framework <br />
🔹 Retry & recovery mechanism <br />
🔹 Scheduler / Windows Task integration <br />

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



### 📄 License
This project is intended for learning and demonstration purposes. <br />
You are free to modify and extend it for personal or educational use.

---
<br />
<br />



















