# Naming Scheme for Scripting :

- **PascalCase** untuk nama script  `PlayerController.cs`
- **Satu script = satu class** dengan nama sama persis.
- **Prefiks kategori** sangat penting untuk mempermudah pencarian di Unity Project.
- Hindari nama generik seperti `Script1.cs` atau `GameScript.cs`.
- Gunakan **singkatan konsisten** jika perlu (`UI`, `FX`, `Net`, `Core`, `Util`).
- Jika script **terhubung langsung dengan prefab**, tambahkan nama prefab di belakangnya.
    
    Contoh: `UIQuestionPanelController` (prefab `UIQuestionPanel`).
    

## 📜 Naming Rules

| **Kategori**                      | **Format**            | **Contoh**                                | **Keterangan**                                        |
| **Core System**                   | `Core[Function]`      | `CoreGameManager`, `CoreDataHandler`      | Semua sistem inti game. Game Manager, Data, Save/Load |
| **Gameplay Mechanic**             | `[Mechanic][Type]`    | `MathQuizController`, `ShapeSpawner`      | Mengatur logika mekanik utama.                        |
| **UI / UX**                       | `UI[Function]`        | `UIQuestionPanel`, `UIResultPopup`        | Semua script UI diawali `UI`.                         |
| **Managers**                      | `[Feature]Manager`    | `AudioManager`, `LevelManager`            | Mengatur satu fitur tertentu. non-core                |
| **Data Model**                    | `[Name]Data`          | `QuestionData`, `PlayerProgressData`      | Script untuk menyimpan data.                          |    
| **Utility / Helper**              | `Util[Function]`      | `UtilMath`, `UtilTimer`                   | Kumpulan fungsi bantu.                                |
| **Networking / Multiplayer**      | `Net[Function]`       | `NetRoomHandler`, `NetSyncPlayer`         | Untuk sinkronisasi online.                            |
| **Animation / VFX Controller**    | `FX[Function]`        | `FXCoinSparkle`, `FXExplosion`            | Mengontrol efek visual & animasi.                     |
| **Testing / Debug**               | `Debug[Function]`     | `DebugSpawner`, `DebugMathTools`          | Script sementara untuk testing.                       |