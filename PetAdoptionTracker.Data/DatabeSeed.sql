USE [PetAdoptionTrackerDb];
GO

SET XACT_ABORT ON;

-- Prevent duplicate data or changes to an existing populated database.
IF EXISTS (SELECT 1 FROM dbo.Shelters)
   OR EXISTS (SELECT 1 FROM dbo.Breeds)
   OR EXISTS (SELECT 1 FROM dbo.Pets)
   OR EXISTS (SELECT 1 FROM dbo.Adopters)
   OR EXISTS (SELECT 1 FROM dbo.AdoptionApplications)
BEGIN
    THROW 50001, 'Seed script requires empty tables. No data was changed.', 1;
END;

BEGIN TRANSACTION;

-- 3 shelters
INSERT INTO dbo.Shelters (Name, Address, Phone)
VALUES
('Happy Paws Shelter',
 '100 Oak Street, Columbia, SC', '803-555-0101'),
('Second Chance Rescue',
 '200 Pine Road, Lexington, SC', '803-555-0102'),
('Forever Friends Shelter',
 '300 Maple Avenue, Irmo, SC', '803-555-0103');

-- 5 breeds
INSERT INTO dbo.Breeds (Name, Species)
VALUES
('Labrador Retriever', 'Dog'),
('Beagle', 'Dog'),
('Mixed Breed', 'Dog'),
('Domestic Shorthair', 'Cat'),
('Siamese', 'Cat');

-- 12 pets; look up related IDs instead of assuming their values.
INSERT INTO dbo.Pets
    (Name, Age, Description, AdoptionStatus, ShelterId, BreedId)
SELECT
    sample.Name,
    sample.Age,
    sample.Description,
    'Available',
    shelter.ShelterId,
    breed.BreedId
FROM (VALUES
    ('Buddy', 3,
     'Friendly dog who enjoys walks and fetch.',
     'Happy Paws Shelter', 'Labrador Retriever'),
    ('Luna', 2,
     'Gentle cat who enjoys quiet afternoons.',
     'Happy Paws Shelter', 'Domestic Shorthair'),
    ('Max', 4,
     'Playful beagle who loves exploring outdoors.',
     'Second Chance Rescue', 'Beagle'),
    ('Bella', 1,
     'Energetic dog who enjoys playing with toys.',
     'Second Chance Rescue', 'Mixed Breed'),
    ('Oliver', 5,
     'Calm cat who enjoys sitting by the window.',
     'Forever Friends Shelter', 'Siamese'),
    ('Daisy', 2,
     'Affectionate dog who loves attention.',
     'Forever Friends Shelter', 'Labrador Retriever'),
    ('Charlie', 6,
     'Relaxed beagle who enjoys short walks.',
     'Happy Paws Shelter', 'Beagle'),
    ('Milo', 1,
     'Curious cat who loves climbing and toys.',
     'Second Chance Rescue', 'Domestic Shorthair'),
    ('Lucy', 3,
     'Social cat who enjoys spending time with people.',
     'Forever Friends Shelter', 'Siamese'),
    ('Rocky', 4,
     'Active dog who enjoys hiking and running.',
     'Happy Paws Shelter', 'Mixed Breed'),
    ('Cleo', 7,
     'Quiet cat looking for a peaceful home.',
     'Second Chance Rescue', 'Domestic Shorthair'),
    ('Teddy', 2,
     'Friendly dog who enjoys learning new tricks.',
     'Forever Friends Shelter', 'Labrador Retriever')
) AS sample (Name, Age, Description, ShelterName, BreedName)
INNER JOIN dbo.Shelters AS shelter
    ON shelter.Name = sample.ShelterName
INNER JOIN dbo.Breeds AS breed
    ON breed.Name = sample.BreedName;

-- 8 adopters
INSERT INTO dbo.Adopters (Name, Email, Phone)
VALUES
('Emma Davis', 'emma@example.com', '803-555-0201'),
('Noah Wilson', 'noah@example.com', '803-555-0202'),
('Olivia Brown', 'olivia@example.com', '803-555-0203'),
('Liam Taylor', 'liam@example.com', '803-555-0204'),
('Ava Johnson', 'ava@example.com', '803-555-0205'),
('Ethan Miller', 'ethan@example.com', '803-555-0206'),
('Sophia Clark', 'sophia@example.com', '803-555-0207'),
('James Lewis', 'james@example.com', '803-555-0208');

-- 6 adoption applications
INSERT INTO dbo.AdoptionApplications
    (PetId, AdopterId, ApplicationDate, Status)
SELECT
    pet.PetId,
    adopter.AdopterId,
    CONVERT(date, sample.ApplicationDate, 112),
    'Pending'
FROM (VALUES
    ('Buddy', 'emma@example.com', '20261110'),
    ('Luna', 'noah@example.com', '20261111'),
    ('Max', 'olivia@example.com', '20261112'),
    ('Bella', 'liam@example.com', '20261113'),
    ('Oliver', 'ava@example.com', '20261114'),
    ('Daisy', 'ethan@example.com', '20261115')
) AS sample (PetName, AdopterEmail, ApplicationDate)
INNER JOIN dbo.Pets AS pet
    ON pet.Name = sample.PetName
INNER JOIN dbo.Adopters AS adopter
    ON adopter.Email = sample.AdopterEmail;

COMMIT TRANSACTION;

-- Verify the expected counts: 3, 5, 12, 8, and 6.
SELECT 'Shelters' AS TableName, COUNT(*) AS RecordCount
FROM dbo.Shelters
UNION ALL
SELECT 'Breeds', COUNT(*) FROM dbo.Breeds
UNION ALL
SELECT 'Pets', COUNT(*) FROM dbo.Pets
UNION ALL
SELECT 'Adopters', COUNT(*) FROM dbo.Adopters
UNION ALL
SELECT 'AdoptionApplications', COUNT(*)
FROM dbo.AdoptionApplications;