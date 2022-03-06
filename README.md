# My movies
### Full stack project containing a list of movies with CRUD operations.

The project uses:
- .NET Core 3.1
- Entity Framework Core 3.1.22
- SQL Server Express LocalDB
- Vite
- Vue.js 2.6.14
- Vuelidate 0.7.7
- Axios
- Bootstrap

## Setting up the development environment
1.  Clone the repository or download it in ZIP (and unpack it)
2.  Go to `my-movies-master` directory and open `my-movies.sln` using Visual Studio
3.  Select `my_movies_backend` debug profile from the drop-down list

![Launch profiles](docs/readme/readme_launch_profiles.png?raw=true "Launch profiles")

4.  Click on the button with the profile name - the project should build and start in debug mode

![Launch button](docs/readme/readme_launch_button.png?raw=true "Launch button")

5.  Go to the `my-movies-frontend` directory and open the command-line interface
6.  Run the `npm install` command to install all necessary packages
7.  Run the `npm run dev` command to run the frontend development server

The backend runs on `http://localhost:5000`

The frontend runs on `http://localhost:3000`

Use `.env.production` file for storing configuration data in production

The database connection string is stored in the `appsettings.json`
