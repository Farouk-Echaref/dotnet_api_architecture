# 📘 What is a `record` in C#?

A **record** is a special kind of C# type designed for **immutable data models** and **value-based equality**.

Think of a `record` as a **lightweight data container**.

---

## 🆚 `record` vs `class` — Key Differences

| Feature          | `class`            | `record`                                   |
| ---------------- | ------------------ | ------------------------------------------ |
| **Equality**     | Reference-based    | Value-based (compares property values)     |
| **Immutability** | Mutable by default | Immutable by default (can be made mutable) |
| **ToString()**   | Not very helpful   | Auto-generated summary of properties       |
| **Use case**     | Behavior and logic | Data modeling and transfers                |

---

## ✅ Example

### 1. Class Example:

```csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}
```

```csharp
var p1 = new Person { Name = "Alice", Age = 25 };
var p2 = new Person { Name = "Alice", Age = 25 };
Console.WriteLine(p1 == p2); // False (different references)
```

---

### 2. Record Example:

```csharp
public record Person(string Name, int Age);
```

```csharp
var p1 = new Person("Alice", 25);
var p2 = new Person("Alice", 25);
Console.WriteLine(p1 == p2); // True (same values)
```

---

## 🧠 When to Use `record`?

* Use **records** for:

  * DTOs
  * API models
  * Data transfer or query result objects
* Use **classes** when:

  * You need **behavior**, **state changes**, or **inheritance**

---
