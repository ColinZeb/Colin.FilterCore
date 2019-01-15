# Dynamic Global Filters for Entity Framework Core

Create global and scoped filters for Entity Framework queries.  The filters are automatically applied to every query and can be used to support use cases such as Multi-Tenancy, Soft Deletes, Active/Inactive, etc.

Filters can be created using boolean linq expressions .

Access to DynamicFilters is done via extension methods in the Colin.FilterCore namespace on the DbContext and DbModelBuilder classes.


## Installation
The package is also available on NuGet: [Colin.FilterCore](https://www.nuget.org/packages/Colin.FilterCore).