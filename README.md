# Geico Challenge - Task Api

## Project Details

- Project was created using Visual Studio 2022 Community Edition
- The project uses the .Net Web Api Core template
- The project targets .Net 6.0 Framework
- All unit tests are done using MSTest framework
- The Api is using the IN MEMORY database and Entity Framework, so data is not persisted over restarts
- The swagger inteface on the API is active to simplify reveiew 

## Project Structure 

Solution Folder
	geico_challenge.sln
		TaskApi
			TaskApi.csproj
		TaskApi.Tests
			TaskApi.Tests.csproj

## Building and Running

1. Extract the contents of geico_challenge.zip
2. Open the geico_challenge.sln file in Visual Studio 2022
3. Build the solution 
4. Run the solution. TaskApi will launch in your default browser with the swagger ui page visible 

## Unit Testing 

1. Extract the contents of geico_challenge.zip
2. Open the geico_challenge.sln file in Visual Studio 2022
3. Build the solution 
4. Open the Test Explorer
5. Execute the unit tests 

## Api Interfaces

GET api/taskitems 
GET api/taskitems/{id}
POST api/taskitems 
PUT api/taskitems/{id}
DELETE api/taskitems/{id}

## Azure Deployment

Azure deployment is available at https://taskapi20220716.azurewebsites.net

