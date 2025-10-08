# ---------------------------------------
# AgriculturalLandManagement Project
# Instructions to run the project and tests
# ---------------------------------------

# 1. Clone the repository
git clone https://github.com/abolfazlshahsavaryyy/KandaIdeaTestProject.git
cd KandaIdeaTestProject

# 2. Navigate to the main project folder
cd AgriculturalLandManagement

# 3. Restore NuGet packages
dotnet restore

# 4. Build the project
dotnet build

# 5. Apply EF Core migrations
# Install EF CLI if not installed
dotnet tool install --global dotnet-ef || echo "dotnet-ef already installed"
# Add migration (only if migrations folder is empty)
dotnet ef migrations add InitialCreate || echo "Migration already exists"
# Update database
dotnet ef database update

# 6. Run the web application
dotnet run

# 7. Open a new terminal or tab and navigate to the test project folder
cd ../AgriculturalLandManagement.Tests

# 8. Restore test dependencies
dotnet restore

# 9. Add reference to main project (if not already added)
dotnet add reference ../AgriculturalLandManagement/AgriculturalLandManagement.csproj || echo "Reference already exists"

# 10. Run the unit tests
dotnet test
