# Lottery Project 🎰

Full-stack application with Angular & .NET Web API.

## Project Structure
- **Client**: Angular application.
- **Server**: .NET Web API.

## Setup & Running

### Database Setup 🗄️
The project uses Entity Framework Core. To initialize the database:
1. Ensure your connection string in `appsettings.json` is correct.
2. Open terminal in `server/LotteryApi`.
3. Run the following command:
   ```bash
   dotnet ef database update