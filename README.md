# .NET Core IoC Auto Register

This simple extension provides a way to scan and auto-register components in assemblies according to user-specified rules in .NET Core default dependency injection container.

## Available packages

Package name               | Current version (master branch)
-----------------------|-------------------------------------------
`NetCoreIoCAutoRegister`             | [![NuGet](https://img.shields.io/nuget/v/NetCoreIoCAutoRegister.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/NetCoreIoCAutoRegister/)

## How to get started

### Scanning for Types

```csharp
services.RegisterTypes(Assembly.GetExecutingAssembly())
		.Where(x => x.Name.EndsWith("Repository"))
		.AsScoped();
```

Remember: Each `RegisterTypes()` call will apply one set of rules only - you can't do multiple invocations in one approach.

To submit given rules call `AsSingleton()`, `AsTransient()`, `AsScoped()` according which service lifetime do you want to choose.

### Filtering Types

`RegisterTypes()` accepts as a parameter assembly. You can provide more assemblies by calling `OfAssemblies()` where the parameter is list of assemblies.

```csharp
services.RegisterTypes()
	.OfAssemblies(new List<Assembly> { Assembly.GetExecutingAssembly() } )
	.AsScoped();
```

If you only want your public classes registered, use `PublicOnly()`:

```csharp
services.RegisterTypes(Assembly.GetExecutingAssembly())
		.PublicOnly()
		.AsScoped();
```

To apply custom filtering use `Where()` method:

```csharp
services.RegisterTypes(Assembly.GetExecutingAssembly())
		.Where(x => x.Name.EndsWith("Service"))
		.AsScoped();
```

To exclude types from scanning use `Except<>()` method:

```csharp
services.RegisterTypes(Assembly.GetExecutingAssembly())
		.Where(x => x.Name.EndsWith("Repository"))
		.Except<IFirstRepository>()
		.AsScoped();
```

