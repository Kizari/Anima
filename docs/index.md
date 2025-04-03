---
_layout: landing
---

# Anima

General purpose .NET libraries for everyday use.

> [!WARNING]  
> Anima is currently in alpha, so releases are liable to regularly include breaking changes.

## Packages

| Name                                                | Description                                                                     |
|-----------------------------------------------------|---------------------------------------------------------------------------------|
| `Visium.Anima`                                      | Helper classes, types, methods, and extensions for common .NET operations.      |
| `Visium.Anima.Utilities.SourceGeneration`           | Utility classes and helper methods to aid in the creation of source generators. |
| `Visium.Anima.EntityFrameworkCore.SourceGeneration` | Source generators to reduce boilerplate when working with EntityFrameworkCore.  |

## Installation

To install a package, simply search for the package name from the table above in the NuGet package manager of your
favourite IDE.

Alternatively, they can be installed from the commandline if you have the .NET SDK installed by using the following
command:
```
dotnet add package <PackageName>
```

For example, to install `Anima.EntityFrameworkCore.SourceGeneration`, run the following command:

```
dotnet add package Visium.Anima.EntityFrameworkCore.SourceGeneration
```

## Usage

Refer to the API documentation by clicking the `API` tab in the header bar of this site.