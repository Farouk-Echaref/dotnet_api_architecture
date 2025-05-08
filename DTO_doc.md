# DTO -Data Transfer Object-

A **DTO (Data Transfer Object)** is an object that carries data between processes, layers, or services. It’s used to **encapsulate request or response data** without exposing internal domain or database models, ensuring **data validation, consistency, and security**.

---

### 🔹 Purpose of DTOs

* **Decouple** layers (e.g., controller from domain/service logic).
* **Validate** and **transform** incoming/outgoing data.
* Avoid over-posting or leaking sensitive fields.
* Simplify API contracts.

---

## ✅ Example in **NestJS (TypeScript)**

Let’s say you’re building a user registration API.

### `create-user.dto.ts`

```ts
import { IsEmail, IsNotEmpty, Length } from 'class-validator';

export class CreateUserDto {
  @IsNotEmpty()
  @Length(2, 30)
  username: string;

  @IsEmail()
  email: string;

  @IsNotEmpty()
  @Length(8, 20)
  password: string;
}
```

### `users.controller.ts`

```ts
import { Body, Controller, Post } from '@nestjs/common';
import { CreateUserDto } from './dto/create-user.dto';
import { UsersService } from './users.service';

@Controller('users')
export class UsersController {
  constructor(private readonly usersService: UsersService) {}

  @Post()
  async create(@Body() createUserDto: CreateUserDto) {
    return this.usersService.create(createUserDto);
  }
}
```

---

## ✅ Example in **.NET (C#)**

Assume you're building a similar user registration endpoint in ASP.NET Core.

### `CreateUserDto.cs`

```csharp
using System.ComponentModel.DataAnnotations;

public class CreateUserDto
{
    [Required]
    [StringLength(30, MinimumLength = 2)]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(20, MinimumLength = 8)]
    public string Password { get; set; }
}
```

### `UsersController.cs`

```csharp
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    [HttpPost]
    public IActionResult Create([FromBody] CreateUserDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Call service to create user
        return Ok(new { message = "User created" });
    }
}
```

---

### 🔄 Summary Table

| Feature    | NestJS                       | .NET                           |
| ---------- | ---------------------------- | ------------------------------ |
| Validation | `class-validator` decorators | Data Annotations               |
| Usage      | `@Body() dto: CreateUserDto` | `[FromBody] CreateUserDto dto` |
| Structure  | Class with typed fields      | Class with typed properties    |

---

## 💡 What *Exactly* is a DTO?

A **DTO (Data Transfer Object)** is a *simple object used to transfer data* between parts of a system. It's:

* Not tied to your database schema.
* Not containing any business logic.
* Primarily used for API requests/responses or inter-service communication.

> Think of a DTO as a **"data envelope"** — it carries only what you need, nothing more, and ensures it's in the correct format.

---

## 🔄 How Does a DTO Relate to a Database?

**DTOs are separate from database models (entities).**

* You **don’t expose** raw database models to the client.
* Instead, you **map** from a DTO → Entity (for create/update) and from Entity → DTO (for responses).
* This keeps your internal structure **safe and flexible**.

### Example Flow:

```
Client sends JSON → Controller uses DTO → Service maps DTO → Entity → Save to DB
```

---

## ✅ NestJS Example (TypeScript)

Let’s build a `User` feature.

### 1. DTO: `create-user.dto.ts`

```ts
import { IsEmail, IsNotEmpty } from 'class-validator';

export class CreateUserDto {
  @IsNotEmpty()
  username: string;

  @IsEmail()
  email: string;

  @IsNotEmpty()
  password: string;
}
```

### 2. Entity (Database Model): `user.entity.ts`

```ts
import { Entity, PrimaryGeneratedColumn, Column } from 'typeorm';

@Entity()
export class User {
  @PrimaryGeneratedColumn()
  id: number;

  @Column()
  username: string;

  @Column()
  email: string;

  @Column()
  password: string;
}
```

### 3. Mapping and Usage

```ts
// users.service.ts
import { Injectable } from '@nestjs/common';
import { InjectRepository } from '@nestjs/typeorm';
import { Repository } from 'typeorm';
import { User } from './user.entity';
import { CreateUserDto } from './dto/create-user.dto';

@Injectable()
export class UsersService {
  constructor(@InjectRepository(User) private repo: Repository<User>) {}

  async create(dto: CreateUserDto) {
    const user = this.repo.create(dto);  // Automatically maps fields
    return this.repo.save(user);
  }
}
```

---

## ✅ .NET Example (C#)

### 1. DTO: `CreateUserDto.cs`

```csharp
using System.ComponentModel.DataAnnotations;

public class CreateUserDto
{
    [Required]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}
```

### 2. Entity (EF Core): `User.cs`

```csharp
public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
```

### 3. Mapping and Usage

```csharp
// UsersController.cs
[HttpPost]
public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
{
    if (!ModelState.IsValid)
        return BadRequest(ModelState);

    var user = new User
    {
        Username = dto.Username,
        Email = dto.Email,
        Password = dto.Password // Normally hash this!
    };

    _context.Users.Add(user);
    await _context.SaveChangesAsync();

    return Ok(new { user.Id });
}
```

---

## 🛡️ Why Not Just Use the Entity?

Using DTOs instead of your DB model prevents:

* Accidental over-posting (e.g., user sets `isAdmin: true`).
* Breaking changes when DB model evolves.
* Data leaks (e.g., sending password hashes in responses).

---

## 🧠 Summary

| Concept           | NestJS                         | .NET                            |
| ----------------- | ------------------------------ | ------------------------------- |
| DTO Definition    | Class with decorators          | Class with annotations          |
| Entity Definition | TypeORM `@Entity()` class      | EF Core `DbSet`-backed class    |
| Validation        | `class-validator`              | Data Annotations (`[Required]`) |
| Usage             | Controller → DTO → Entity → DB | Controller → DTO → Entity → DB  |
| Mapping           | Manual or `repo.create(dto)`   | Manual object construction      |

---

How **DTOs relate to JSON**, **serialization/deserialization**, and how to create **response DTOs**, with complete examples in **NestJS** and **.NET**.

---

## 🔁 1. DTOs & JSON: The Relationship

DTOs are tightly related to **JSON**, because:

* In REST APIs, clients typically **send** JSON → which becomes a **DTO (deserialization)**.
* The server processes the request, and returns **JSON → often generated from a DTO (serialization)**.

In short:

```txt
Client JSON  →  Deserialized →  Request DTO  →  Entity (DB)  →  Response DTO  →  Serialized →  JSON
```

---

## 📦 2. Serialization / Deserialization

| Concept             | Meaning                                                  |
| ------------------- | -------------------------------------------------------- |
| **Serialization**   | Convert object (DTO) → JSON (to send back to the client) |
| **Deserialization** | Convert incoming JSON → DTO (to handle inside the app)   |

---

## ✅ NestJS Full Example

### 💬 Incoming JSON (Client sends this):

```json
{
  "username": "fechcha",
  "email": "fechcha@example.com",
  "password": "securepass123"
}
```

---

### 1. **Request DTO** - `create-user.dto.ts`

```ts
import { IsEmail, IsNotEmpty } from 'class-validator';

export class CreateUserDto {
  @IsNotEmpty()
  username: string;

  @IsEmail()
  email: string;

  @IsNotEmpty()
  password: string;
}
```

---

### 2. **Entity** - `user.entity.ts`

```ts
import { Entity, PrimaryGeneratedColumn, Column } from 'typeorm';

@Entity()
export class User {
  @PrimaryGeneratedColumn()
  id: number;

  @Column()
  username: string;

  @Column()
  email: string;

  @Column()
  password: string;
}
```

---

### 3. **Response DTO** - `user-response.dto.ts`

```ts
export class UserResponseDto {
  id: number;
  username: string;
  email: string;

  constructor(partial: Partial<UserResponseDto>) {
    Object.assign(this, partial);
  }
}
```

---

### 4. **Service Layer (Mapping)** - `users.service.ts`

```ts
import { CreateUserDto } from './dto/create-user.dto';
import { UserResponseDto } from './dto/user-response.dto';

async create(dto: CreateUserDto): Promise<UserResponseDto> {
  const user = this.repo.create(dto);
  const saved = await this.repo.save(user);
  return new UserResponseDto({
    id: saved.id,
    username: saved.username,
    email: saved.email,
  });
}
```

---

### 🔁 Result JSON (Response):

```json
{
  "id": 1,
  "username": "fechcha",
  "email": "fechcha@example.com"
}
```

Note: Password is not exposed — that's the job of the **response DTO**.

---

## ✅ .NET Full Example

### 💬 Incoming JSON:

```json
{
  "username": "fechcha",
  "email": "fechcha@example.com",
  "password": "securepass123"
}
```

---

### 1. **Request DTO** - `CreateUserDto.cs`

```csharp
using System.ComponentModel.DataAnnotations;

public class CreateUserDto
{
    [Required]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}
```

---

### 2. **Entity** - `User.cs`

```csharp
public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
```

---

### 3. **Response DTO** - `UserResponseDto.cs`

```csharp
public class UserResponseDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
}
```

---

### 4. **Controller Mapping Example**

```csharp
[HttpPost]
public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
{
    if (!ModelState.IsValid)
        return BadRequest(ModelState);

    var user = new User
    {
        Username = dto.Username,
        Email = dto.Email,
        Password = dto.Password
    };

    _context.Users.Add(user);
    await _context.SaveChangesAsync();

    var response = new UserResponseDto
    {
        Id = user.Id,
        Username = user.Username,
        Email = user.Email
    };

    return Ok(response);
}
```

---

## 🚀 Bonus: Automating Mapping

### 🧰 In NestJS:

* You can use libraries like [`class-transformer`](https://github.com/typestack/class-transformer) to help map between entities and DTOs.

### 🧰 In .NET:

* Use `AutoMapper` to map DTOs ↔ Entities easily:

```csharp
var response = _mapper.Map<UserResponseDto>(user);
```

---

## 🧠 Summary

| Topic               | NestJS                                    | .NET                                      |
| ------------------- | ----------------------------------------- | ----------------------------------------- |
| **JSON to DTO**     | `@Body() dto: CreateUserDto`              | `[FromBody] CreateUserDto dto`            |
| **DTO to Entity**   | `repo.create(dto)`                        | Manual or AutoMapper                      |
| **Entity to DTO**   | `new UserResponseDto({ ... })`            | Manual or AutoMapper                      |
| **Serialization**   | Automatic by NestJS                       | Automatic by ASP.NET Core                 |
| **Deserialization** | Automatic + Validated (`class-validator`) | Automatic + Validated (`DataAnnotations`) |

---

