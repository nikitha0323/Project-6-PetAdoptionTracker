# Pet Adoption Tracker - Project 6

For this project, I added Azure Functions to my pet adoption
tracker from Project 5.

## Projects

- PetAdoptionTracker.Api - API controllers and business layer
- PetAdoptionTracker.Client - Console client
- PetAdoptionTracker.Data - Database models and data access
- PetAdoptionTracker.Functions - The two Azure functions
- ChecklistFiles - Dog and cat checklist text files

## How to Run

1. Open Project6.sln inside the Project6 folder in Visual Studio.
2. Restore NuGet packages.
3. Reuse the Project 5 database or set it up using the SQL scripts.
4. Check the database connection string in the API's appsettings.json.
5. Set AzureFunctions: BaseUrl to the deployed Function App address.
6. Set the API and Client as multiple startup projects.
7. Use the API's HTTPS profile and press Ctrl + F5.
8. Wait for the API to start, then use the console menu.

The client uses https://localhost:7140/.
**Update Program.cs if the API uses a different port.
**
## Azure Functions

- CalculateAdoptionFee - Returns a sample fee based on age.
- GetCareChecklist - Reads a dog or cat checklist from Blob Storage.

The console client calls the API, which calls the business layer.
The business layer then calls Azure using HttpClient.


## Testing

- Option 9 with age 3 returns a fee of $100.00.
- Option 10 with dog or cat returns the matching checklist.
- Invalid input returns a validation message.

