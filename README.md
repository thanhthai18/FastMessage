# FastMessage

<p align="center">
  <img src="https://img.shields.io/badge/Unity-2020.3+-000000.svg?style=flat-square&logo=unity" alt="Unity" />
  <img src="https://img.shields.io/badge/Language-C%23_8.0-239120.svg?style=flat-square&logo=c-sharp" alt="C#" />
  <img src="https://img.shields.io/badge/License-MIT-blue.svg?style=flat-square" alt="License" />
</p>

<p align="center">
  <strong>An ultra-fast, zero-allocation Pub/Sub messaging system for Unity & C#.</strong>
</p>

---

## 📖 Introduction

**FastMessage** is a lightweight, high-performance Event Bus designed specifically for game development where performance is critical.

Unlike standard C# Events or naive `List<T>` based implementations, FastMessage uses a **Cached Array Snapshot** technique. This ensures:
1.  **Zero Allocation** during message publishing (No GC pressure).
2.  **Thread-Safety** during iteration.
3.  **Safe Modification**: You can safely Subscribe/Unsubscribe *while* a message is being published without causing `InvalidOperationException`.
4.  **Fast**: Achieves raw throughput slightly faster than MessagePipe 5%-10% by focusing on a minimal yet sufficient feature set.

> "FastMessage: Hệ thống Pub/Sub C# hiệu năng cao, tối ưu Zero-Allocation và Thread-Safe bằng cơ chế Snapshot — phát triển dựa trên nền móng đầu tiên từ một người anh đồng nghiệp cũ #TweakKit."

---

## 🚀 Key Features

* **⚡ Blazing Fast:** Optimized for hot-paths (Update loops). Iterates over raw Arrays, which is faster than List iteration in C#.
* **🗑️ Zero Allocation:** Using `struct` based messages prevents boxing. The cached array strategy prevents allocation during the `Publish` call.
* **🛡️ Iteration Safety:** Uses "Copy-on-Write" logic. Subscribers can remove themselves during a callback without breaking the loop.
* **🧘 Simple API:** Minimal boilerplate. Just `Subscribe`, `Publish`, and `Dispose`.

---

## 💾 Installation

Simply copy the `Runtime/Message` folder into your Unity project.

Folder structure:
```text
Runtime/
└── Message/
    ├── IMessage.cs
    ├── Messenger.cs
    ├── MessageHandler.cs
    ├── MessageHandlersHolder.cs
    └── MessageRegistry.cs
