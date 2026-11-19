using PetAdoptionTracker.Data;
using PetAdoptionTracker.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace PetAdoptionTracker.Api.Business
{
    public class BusinessManager
    {
        private readonly DataProvider _dataProvider;
        private readonly HttpClient _azureClient;

        public BusinessManager(
            DataProvider dataProvider,
            IHttpClientFactory httpClientFactory)
        {
            _dataProvider = dataProvider;
            _azureClient = httpClientFactory.CreateClient("AzureFunctions");
        }
        public async Task<List<Pet>> GetPetsAsync()
        {
            return await _dataProvider.GetAllAsync<Pet>();
        }

        public async Task<Pet?> GetPetByIdAsync(int id)
        {
            return await _dataProvider.GetByIdAsync<Pet>(id);
        }

        public async Task<Pet> SavePetAsync(Pet pet)
        {
            if (pet.PetId < 0)
            {
                throw new ArgumentException("Pet ID cannot be negative.");
            }

            if (string.IsNullOrWhiteSpace(pet.Name) || pet.Name.Length > 100)
            {
                throw new ArgumentException(
                    "Pet name is required and cannot exceed 100 characters.");
            }

            if (pet.Age < 0 || pet.Age > 100)
            {
                throw new ArgumentException("Age must be between 0 and 100.");
            }

            if (string.IsNullOrWhiteSpace(pet.Description)
                || pet.Description.Length > 1000)
            {
                throw new ArgumentException(
                    "Description is required and cannot exceed 1000 characters.");
            }

            var allowedStatuses = new[] { "Available", "Pending", "Adopted" };

            if (!allowedStatuses.Contains(pet.AdoptionStatus))
            {
                throw new ArgumentException(
                    "Status must be Available, Pending, or Adopted.");
            }

            var shelter = await _dataProvider
                .GetByIdAsync<Shelter>(pet.ShelterId);

            if (shelter == null)
            {
                throw new ArgumentException("The selected shelter does not exist.");
            }

            var breed = await _dataProvider
                .GetByIdAsync<Breed>(pet.BreedId);

            if (breed == null)
            {
                throw new ArgumentException("The selected breed does not exist.");
            }

            pet.Name = pet.Name.Trim();
            pet.Description = pet.Description.Trim();

            if (pet.PetId == 0)
            {
                return await _dataProvider.InsertAsync(pet);
            }

            var updated = await _dataProvider.UpdateAsync(pet.PetId, pet);

            if (!updated)
            {
                throw new KeyNotFoundException("Pet not found.");
            }

            return pet;
        }
        public async Task<bool> DeletePetAsync(int id)
        {
            var applications = await _dataProvider
                .GetAllAsync<AdoptionApplication>();

            if (applications.Any(a => a.PetId == id))
            {
                throw new InvalidOperationException(
                    "Cannot delete a pet that has adoption applications.");
            }

            return await _dataProvider.DeleteAsync<Pet>(id);
        }

        public async Task<List<Adopter>> GetAdoptersAsync()
        {
            return await _dataProvider.GetAllAsync<Adopter>();
        }

        public async Task<Adopter?> GetAdopterByIdAsync(int id)
        {
            return await _dataProvider.GetByIdAsync<Adopter>(id);
        }
        public async Task<Adopter> SaveAdopterAsync(Adopter adopter)
        {
            if (adopter.AdopterId < 0)
            {
                throw new ArgumentException("Adopter ID cannot be negative.");
            }

            var errors = new List<ValidationResult>();

            bool valid = Validator.TryValidateObject(
                adopter,
                new ValidationContext(adopter),
                errors,
                validateAllProperties: true);

            if (!valid)
            {
                throw new ArgumentException(
                    string.Join(" ", errors.Select(e => e.ErrorMessage)));
            }

            adopter.Name = adopter.Name.Trim();
            adopter.Email = adopter.Email.Trim();
            adopter.Phone = adopter.Phone.Trim();

            var adopters = await _dataProvider.GetAllAsync<Adopter>();

            if (adopters.Any(a =>
                a.AdopterId != adopter.AdopterId &&
                string.Equals(a.Email.Trim(), adopter.Email,
                    StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException(
                    "An adopter with this email already exists.");
            }

            if (adopter.AdopterId == 0)
            {
                return await _dataProvider.InsertAsync(adopter);
            }

            var updated = await _dataProvider.UpdateAsync(
                adopter.AdopterId, adopter);

            if (!updated)
            {
                throw new KeyNotFoundException("Adopter not found.");
            }

            return adopter;
        }
        public async Task<bool> DeleteAdopterAsync(int id)
        {
            var applications = await _dataProvider
                .GetAllAsync<AdoptionApplication>();

            if (applications.Any(a => a.AdopterId == id))
            {
                throw new InvalidOperationException(
                    "Cannot delete an adopter who has adoption applications.");
            }

            return await _dataProvider.DeleteAsync<Adopter>(id);
        }
        public async Task<List<AdoptionApplication>> GetApplicationsAsync()
        {
            return await _dataProvider.GetAllAsync<AdoptionApplication>();
        }

        public async Task<AdoptionApplication?> GetApplicationByIdAsync(int id)
        {
            return await _dataProvider.GetByIdAsync<AdoptionApplication>(id);
        }
        public async Task<AdoptionApplication> SaveApplicationAsync(
    AdoptionApplication application)
        {
            if (application.AdoptionApplicationId < 0)
            {
                throw new ArgumentException(
                    "Application ID cannot be negative.");
            }

            var allowedStatuses = new[] { "Pending", "Approved", "Rejected" };

            if (!allowedStatuses.Contains(application.Status))
            {
                throw new ArgumentException(
                    "Status must be Pending, Approved, or Rejected.");
            }

            if (application.ApplicationDate == default ||
                application.ApplicationDate.Date > DateTime.Today)
            {
                throw new ArgumentException(
                    "Enter an application date that is not in the future.");
            }

            var pet = await _dataProvider.GetByIdAsync<Pet>(
                application.PetId);

            if (pet == null)
            {
                throw new ArgumentException("The selected pet does not exist.");
            }

            var adopter = await _dataProvider.GetByIdAsync<Adopter>(
                application.AdopterId);

            if (adopter == null)
            {
                throw new ArgumentException(
                    "The selected adopter does not exist.");
            }

            var applications = await _dataProvider
                .GetAllAsync<AdoptionApplication>();

            if (application.Status == "Pending" &&
                applications.Any(a =>
                    a.AdoptionApplicationId != application.AdoptionApplicationId &&
                    a.PetId == application.PetId &&
                    a.AdopterId == application.AdopterId &&
                    a.Status == "Pending"))
            {
                throw new ArgumentException(
                    "This adopter already has a pending application for this pet.");
            }

            if (application.AdoptionApplicationId == 0)
            {
                return await _dataProvider.InsertAsync(application);
            }

            var updated = await _dataProvider.UpdateAsync(
                application.AdoptionApplicationId, application);

            if (!updated)
            {
                throw new KeyNotFoundException("Application not found.");
            }

            return application;
        }
        public async Task<bool> DeleteApplicationAsync(int id)
        {
            return await _dataProvider
                .DeleteAsync<AdoptionApplication>(id);
        }
        public async Task<List<Shelter>> GetSheltersAsync()
        {
            return await _dataProvider.GetAllAsync<Shelter>();
        }

        public async Task<Shelter?> GetShelterByIdAsync(int id)
        {
            return await _dataProvider.GetByIdAsync<Shelter>(id);
        }
        public async Task<List<Breed>> GetBreedsAsync()
        {
            return await _dataProvider.GetAllAsync<Breed>();
        }

        public async Task<Breed?> GetBreedByIdAsync(int id)
        {
            return await _dataProvider.GetByIdAsync<Breed>(id);
        }
        public async Task<string> CalculateAdoptionFeeAsync(int age)
        {
            if (age < 0 || age > 100)
            {
                throw new ArgumentException(
                    "Age must be between 0 and 100.");
            }

            return await _azureClient.GetStringAsync(
                $"api/CalculateAdoptionFee?age={age}");
        }

        public async Task<string> GetCareChecklistAsync(string species)
        {
            if (string.IsNullOrWhiteSpace(species))
            {
                throw new ArgumentException("Species is required.");
            }

            species = species.Trim().ToLowerInvariant();

            if (species != "dog" && species != "cat")
            {
                throw new ArgumentException(
                    "Species must be dog or cat.");
            }

            return await _azureClient.GetStringAsync(
                $"api/GetCareChecklist?species={Uri.EscapeDataString(species)}");
        }
    }
}