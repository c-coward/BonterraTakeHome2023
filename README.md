## Chloe Coward's solution to the Bonterra 2023 Summer Internship Take-Home Problem.

### Deployment Information
This project is written in F# and targets the .NET 6.0 framework.
Ensure you have the [.NET 6.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) installed to build this project from the source.

To run this project, clone the repository and navigate to the project directory:
```
git clone https://github.com/c-coward/BonterraTakeHome2023.git
cd BonterraTakeHome2023
```

Next, you must add valid API credentials. There are two simple ways of accomplishing this:
1. Add a secret file.  
Create a file called `secret.txt` in the project directory, and paste your credentials into it.
2. Modify `Program.fs` directly.  
In `src/Program.fs`, modify line 14 to be the following:
```fs
let key = "YOUR_CREDENTIALS_HERE"
```

Finally, you can execute the project by running `dotnet run` from within the project directory. This will create or overwrite a file called `EmailReport.csv` in the same directory to contain the solution based on the problem description.