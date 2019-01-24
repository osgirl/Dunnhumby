# Dunnhumby
Dunnhumby home task, for description please look at the pdf file

## Note
* To keep the work simple some aspect omited such as: Clean up in test files
* However suggested to used GUID as entity Id, I used integers for this purpose. It is due to the fact that integer takes less data in database. It is really easy to change the datatype to GUID by just changing EntityBase and some points in stores.

## Requirments to run the project
* .NET Core 2.2
* Visual Studio 2017
* SQL Server File DB
* Database ConnectionStrings can be change in appsettings.json

## Points covered in this project
* Table display of products/campaigns
* Filter active/inactive
* Pagination setup
* Some basic validation setup
* DB schema
* RESTful endpoints
* Read/Write for both campaign and product
* Pagination support through query params
* Some basic validation and data sanitization
* Migration/seeding setup
* Base unit tests setup
* Sever Side Rendering (SSR) setup
* Basic caching setup