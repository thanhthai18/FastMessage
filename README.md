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
```

## 📝 Usage

### 1. Define a Message
Always use `struct` (or `readonly struct`) to ensure **zero allocation**.
*Tip: Keep messages pure data (no logic).*

```csharp
public readonly struct PlayerLevelUpMessage : IMessage
{
    public readonly int NewLevel;
    public readonly int GoldReward;

    public PlayerLevelUpMessage(int newLevel, int goldReward)
    {
        NewLevel = newLevel;
        GoldReward = goldReward;
    }
}
```

### 2. Publish (The Sender)
Publishing is extremely fast and generates 0 garbage. You can call this from anywhere (Managers, Controllers, Triggers).

```csharp
// Example: Called inside a Game Manager or Logic Class
Messenger.Publish(new PlayerLevelUpMessage(10, 500));
```

### 3. Subscribe (The Receiver)
Returns an IDisposable. You must dispose of it when the object is destroyed to prevent memory leaks.

```csharp
using Runtime.Message;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private IDisposable _subscription;

    void Start()
    {
        // Subscribe and store the token
        _subscription = Messenger.Subscribe<PlayerLevelUpMessage>(OnLevelUp);
    }

    private void OnLevelUp(PlayerLevelUpMessage msg)
    {
        // Handle the event
        Debug.Log($"Level Up! Level: {msg.NewLevel}, Gold: {msg.GoldReward}");
    }

    void OnDestroy()
    {
        // IMPORTANT: Always dispose to prevent memory leaks!
        _subscription?.Dispose();
    }
}
```

## 💡 Best Practice: Handling Multiple Subscriptions
If a class subscribes to multiple messages, use a List to manage them easily.

```csharp
private List<IDisposable> _disposables = new List<IDisposable>();

void Start() 
{
    _disposables.Add(Messenger.Subscribe<PlayerLevelUpMessage>(OnLevelUp));
    _disposables.Add(Messenger.Subscribe<GamePauseMessage>(OnPause));
}

void OnDestroy() 
{
    foreach (var d in _disposables) d.Dispose();
    _disposables.Clear();
}
```

## ⚡ Performance: List vs. FastMessage
Why is FastMessage better than a standard List<Action>?

| Feature | Standard `List<T>` | FastMessage (Cached Array) |
| :--- | :--- | :--- |
| **Iteration Speed** | Good | **Best** (Array is faster than List) |
| **Memory Alloc (Publish)** | Zero | **Zero** |
| **Modification Safety** | ❌ **Crash** (InvalidOperationException) if modified during loop | ✅ **Safe** (Snapshot isolation) |
| **Thread Safety** | ❌ No | ✅ **Yes** (Snapshot isolation) |

The "Snapshot" Logic
When you call Publish, FastMessage iterates over a cached array. If a listener unsubscribes during the loop, it modifies the underlying list but not the array currently being iterated. The cache is only rebuilt (lazily) when the next modification happens.

## ❤️ Acknowledgement
Thank for #TweakKit
Dedicated to my former colleague who laid the foundation for this system.
Tri ân người anh đồng nghiệp cũ đã tạo ra phiên bản đầu tiên, đặt nền móng cho sự phát triển của thư viện này.

## 📄 License
This project is free to use. No license is required. Author: thanhthai18
