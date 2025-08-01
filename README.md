# Xperience Community: Exports

## Description

This package provides Xperience by Kentico administrators with an option to export data across various listing interfaces. The implementation also allows for developers to easily extend the implementation to allow for exporting across other listing pages within the administration.

![Xperience by Kentico Exports](https://raw.githubusercontent.com/benquinlan-07/xperience-community-exports/refs/heads/main/images/export-users.png)

## Library Version Matrix

| Xperience Version | Library Version |
| ----------------- | --------------- |
| >= 30.8.0         | 1.0.0           |


### Dependencies

- [ASP.NET Core 8.0](https://dotnet.microsoft.com/en-us/download)
- [Xperience by Kentico](https://docs.kentico.com)

## Package Installation

Add the package to your application using the .NET CLI

```
dotnet add package XperienceCommunity.Exports
```

or via the Package Manager

```
Install-Package XperienceCommunity.Exports
```

## Quick Start

1. Install the NuGet package.

1. Login to the admin and look for the export button on any of the following applications:

    - Users
    - Members
    - Orders
    - Customers
    - Forms
    - Form Submissions

## Adding Export to Listing Pages

As per the implementations in this repository, simply create a class that extends from the `XperienceCommunity.Exports.PageExtenders.Base.ExportPageExtender<>` class and implement any required methods.

Don't forget to register the page extender with an assembly attribute
````csharp
[assembly: PageExtender(typeof(UserListPageExtender))]
````