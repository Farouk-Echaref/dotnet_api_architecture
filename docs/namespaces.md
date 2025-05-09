# 📦 What is a `namespace` in C#/.NET?

A **namespace** is a way to **organize your code** into logical groups — like folders for your classes, DTOs, controllers, etc.

It helps prevent **naming conflicts** and makes your codebase **easier to manage and read**.

---

## ✅ Example: `namespace dotnet_api.Dtos;`

This is a C# **file-scoped namespace** introduced in **C# 10** (with .NET 6+).

It means:

* Everything in that file belongs to the `dotnet_api.Dtos` namespace.
* You don’t need curly braces or extra indentation.

### 📁 Folder Structure:

```
/dotnet_api
  └── /Dtos
      └── CreateUserDto.cs
```

### 📄 `CreateUserDto.cs`:

```csharp
namespace dotnet_api.Dtos;

public record CreateUserDto(string Username, string Email, string Password);
```

✅ That file now belongs to `dotnet_api.Dtos`.

---

## 🔁 Using It Elsewhere

Anywhere else in your code, you can use that DTO like this:

```csharp
using dotnet_api.Dtos;

var dto = new CreateUserDto("fechcha", "f@ex.com", "123");
```

---

## 🧠 Summary

| Concept            | Description                                     |
| ------------------ | ----------------------------------------------- |
| `namespace`        | Organizes code and avoids name clashes          |
| `namespace xyz;`   | File-scoped (C# 10+), cleaner and more modern   |
| Folder ≠ Namespace | But good practice to keep them aligned          |
| Usage              | Use `using xyz;` to import and use code from it |

---
