using System;
using System.Linq;
using PetAdoptionTracker.Data.Models;

namespace PetAdoptionTracker.Data
{
    public static class DbSeeder
    {
        public static void Seed(AdoptionDbContext context)
        {
            // Avoid adding the sample data again.
            if (context.Shelters.Any())
            {
                return;
            }

            var shelters = new[]
            {
                new Shelter
                {
                    Name = "Happy Paws Shelter",
                    Address = "100 Oak Street, Columbia, SC",
                    Phone = "803-555-0101"
                },
                new Shelter
                {
                    Name = "Second Chance Rescue",
                    Address = "200 Pine Road, Lexington, SC",
                    Phone = "803-555-0102"
                },
                new Shelter
                {
                    Name = "Forever Friends Shelter",
                    Address = "300 Maple Avenue, Irmo, SC",
                    Phone = "803-555-0103"
                }
            };

            var breeds = new[]
            {
                new Breed { Name = "Labrador Retriever", Species = "Dog" },
                new Breed { Name = "Beagle", Species = "Dog" },
                new Breed { Name = "Mixed Breed", Species = "Dog" },
                new Breed { Name = "Domestic Shorthair", Species = "Cat" },
                new Breed { Name = "Siamese", Species = "Cat" }
            };

            var pets = new[]
            {
                new Pet
                {
                    Name = "Buddy", Age = 3,
                    Description = "Friendly dog who enjoys walks and fetch.",
                    Shelter = shelters[0], Breed = breeds[0]
                },
                new Pet
                {
                    Name = "Luna", Age = 2,
                    Description = "Gentle cat who enjoys quiet afternoons.",
                    Shelter = shelters[0], Breed = breeds[3]
                },
                new Pet
                {
                    Name = "Max", Age = 4,
                    Description = "Playful beagle who loves exploring outdoors.",
                    Shelter = shelters[1], Breed = breeds[1]
                },
                new Pet
                {
                    Name = "Bella", Age = 1,
                    Description = "Energetic dog who enjoys playing with toys.",
                    Shelter = shelters[1], Breed = breeds[2]
                },
                new Pet
                {
                    Name = "Oliver", Age = 5,
                    Description = "Calm cat who enjoys sitting by the window.",
                    Shelter = shelters[2], Breed = breeds[4]
                },
                new Pet
                {
                    Name = "Daisy", Age = 2,
                    Description = "Affectionate dog who loves attention.",
                    Shelter = shelters[2], Breed = breeds[0]
                },
                new Pet
                {
                    Name = "Charlie", Age = 6,
                    Description = "Relaxed beagle who enjoys short walks.",
                    Shelter = shelters[0], Breed = breeds[1]
                },
                new Pet
                {
                    Name = "Milo", Age = 1,
                    Description = "Curious cat who loves climbing and toys.",
                    Shelter = shelters[1], Breed = breeds[3]
                },
                new Pet
                {
                    Name = "Lucy", Age = 3,
                    Description = "Social cat who enjoys spending time with people.",
                    Shelter = shelters[2], Breed = breeds[4]
                },
                new Pet
                {
                    Name = "Rocky", Age = 4,
                    Description = "Active dog who enjoys hiking and running.",
                    Shelter = shelters[0], Breed = breeds[2]
                },
                new Pet
                {
                    Name = "Cleo", Age = 7,
                    Description = "Quiet cat looking for a peaceful home.",
                    Shelter = shelters[1], Breed = breeds[3]
                },
                new Pet
                {
                    Name = "Teddy", Age = 2,
                    Description = "Friendly dog who enjoys learning new tricks.",
                    Shelter = shelters[2], Breed = breeds[0]
                }
            };

            var adopters = new[]
            {
                new Adopter
                {
                    Name = "Emma Davis",
                    Email = "emma@example.com",
                    Phone = "803-555-0201"
                },
                new Adopter
                {
                    Name = "Noah Wilson",
                    Email = "noah@example.com",
                    Phone = "803-555-0202"
                },
                new Adopter
                {
                    Name = "Olivia Brown",
                    Email = "olivia@example.com",
                    Phone = "803-555-0203"
                },
                new Adopter
                {
                    Name = "Liam Taylor",
                    Email = "liam@example.com",
                    Phone = "803-555-0204"
                },
                new Adopter
                {
                    Name = "Ava Johnson",
                    Email = "ava@example.com",
                    Phone = "803-555-0205"
                },
                new Adopter
                {
                    Name = "Ethan Miller",
                    Email = "ethan@example.com",
                    Phone = "803-555-0206"
                },
                new Adopter
                {
                    Name = "Sophia Clark",
                    Email = "sophia@example.com",
                    Phone = "803-555-0207"
                },
                new Adopter
                {
                    Name = "James Lewis",
                    Email = "james@example.com",
                    Phone = "803-555-0208"
                }
            };

            var applications = new[]
            {
                new AdoptionApplication
                {
                    Pet = pets[0], Adopter = adopters[0],
                    ApplicationDate = DateTime.Today.AddDays(-6),
                    Status = "Pending"
                },
                new AdoptionApplication
                {
                    Pet = pets[1], Adopter = adopters[1],
                    ApplicationDate = DateTime.Today.AddDays(-5),
                    Status = "Pending"
                },
                new AdoptionApplication
                {
                    Pet = pets[2], Adopter = adopters[2],
                    ApplicationDate = DateTime.Today.AddDays(-4),
                    Status = "Pending"
                },
                new AdoptionApplication
                {
                    Pet = pets[3], Adopter = adopters[3],
                    ApplicationDate = DateTime.Today.AddDays(-3),
                    Status = "Pending"
                },
                new AdoptionApplication
                {
                    Pet = pets[4], Adopter = adopters[4],
                    ApplicationDate = DateTime.Today.AddDays(-2),
                    Status = "Pending"
                },
                new AdoptionApplication
                {
                    Pet = pets[5], Adopter = adopters[5],
                    ApplicationDate = DateTime.Today.AddDays(-1),
                    Status = "Pending"
                }
            };

            context.Shelters.AddRange(shelters);
            context.Breeds.AddRange(breeds);
            context.Pets.AddRange(pets);
            context.Adopters.AddRange(adopters);
            context.AdoptionApplications.AddRange(applications);

            context.SaveChanges();
        }
    }
}