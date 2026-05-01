# Bsc-Szakdolgozat

A 7 Csoda Párbaj társasjátékot fogom csináltam meg mobil és asztali számítógép/laptop applikáció formájában. Mesterséges intelligenciával és online játékmóddal.

---

## Solution-ök

A repository két solution fájlt tartalmaz:

| Solution | Leírás |
|----------|--------|
| `Seven_Wonders_Duel.sln` | Fő solution – tartalmazza az összes projektet (UI, szerver, AI, stb.) |
| `SevenWonders.UnitTests.sln` | Unit tesztek solution-je (Game.Engine és Game.Logic tesztek) |

### Projektek a fő solution-ben

| Projekt | Típus | Leírás |
|---------|-------|--------|
| **SevenWonders.UI** | .NET MAUI alkalmazás | Kliens alkalmazás (Android + Windows) |
| **SevenWonders.Web.Server** | ASP.NET Core Web API | Backend szerver (SignalR, Identity, EF Core) |
| **SevenWonders.Web.Server.Model** | Osztálykönyvtár | Szerver oldali adatmodell (EF Core, Identity) |
| **SevenWonders.Web.Server.Contract** | Osztálykönyvtár | Szerver-kliens közös szerződések |
| **SevenWonders.Web.Client.Model** | Osztálykönyvtár | Kliens oldali SignalR kommunikáció |
| **SevenWonders.Game.Logic** | Osztálykönyvtár | Játékszabályok, adatok (XML, CSV) |
| **SevenWonders.Game.Engine** | Osztálykönyvtár | Játékmotor (SkiaSharp renderelés) |
| **SevenWonders.Game.Presenter** | Osztálykönyvtár | Presenter réteg (MVVM) |
| **SevenWonders.AI.Model** | Osztálykönyvtár | AI modellek (ONNX inferencia) |
| **SevenWonders.AI.Trainer.Server** | Konzol alkalmazás | AI tréning szerver (.NET) |
| **SevenWonders.AI.Trainer** | Python projekt | AI tréning (ML-Agents, PPO) |
| **SevenWonders.Game.Scene.Editor** | .NET MAUI alkalmazás | Pálya/jelenet szerkesztő eszköz (Windows) |
| **SevenWonders.Common** | Osztálykönyvtár | Közös segédeszközök, naplózás (Serilog) |

---

## Felhasznált technológiák és keretrendszerek

- **.NET 9** – minden C# projekt target framework-je
- **.NET MAUI** – keresztplatformos UI (Android + Windows)
- **ASP.NET Core Web API** – backend szerver
- **SignalR** – valós idejű kommunikáció kliens és szerver között
- **Entity Framework Core 9 + SQL Server** – adatbázis kezelés
- **ASP.NET Core Identity + JWT** – hitelesítés és jogosultságkezelés
- **SkiaSharp** – 2D grafikus renderelés (játéktábla, kártyák)
- **CommunityToolkit.Maui** – MAUI kiegészítő komponensek
- **Serilog** – strukturált naplózás (konzol + fájl)
- **ONNX Runtime** – AI modell inferencia
- **Python (ML-Agents / Maskable PPO)** – AI ágensek tanítása
- **Swashbuckle (Swagger)** – API dokumentáció

---

## Telepítési útmutató fejlesztőknek

### Előfeltételek

1. **Visual Studio 2022 (17.13+)** a következő workload-okkal:
   - .NET Multi-platform App UI development (.NET MAUI)
   - ASP.NET and web development
2. **.NET 9 SDK**
3. **SQL Server** (LocalDB vagy teljes verzió) az EF Core migrációkhoz
4. **Python 3.x** (opcionális, csak AI tréninghez)

### Első futtatás előtti lépések

1. **Klónozd a repository-t:**
   ```bash
   git clone <repo-url>
   ```

2. **Futtasd a `CreateScene.ps1` szkriptet** (PowerShell):
   ```powershell
   .\CreateScene.ps1
   ```
   > Ez a szkript a `scene/` mappa tartalmát becsomagolja `scene.zip` fájlba és bemásolja a MAUI alkalmazás `Resources\Raw\` mappájába, illetve a Scene Editor build könyvtárába. **Ezt minden alkalommal futtasd, ha a scene mappa tartalma változik!**

3. **Nyisd meg a `Seven_Wonders_Duel.sln` solution-t** Visual Studio-ban.

4. **Állítsd be a Startup projektet** a kívánt futtatási profilnak megfelelően (lásd lent).

5. **Build + futtatás** (F5).

---

## Futtatási profilok

### Teljes alkalmazás (kliens + szerver együtt)

A `Seven_Wonders_Duel.slnLaunch.user` fájl egy előre konfigurált multi-startup profilt tartalmaz **„New Profile"** néven, amely egyszerre indítja:
- `SevenWonders.UI` (MAUI kliens)
- `SevenWonders.Web.Server` (backend)

> Visual Studio-ban: jobb klikk a Solution-re → *Configure Startup Projects...* → *Multiple startup projects* – vagy használd az `.slnLaunch.user` profilt.

### Web Server profilok

| Profil | URL | Leírás |
|--------|-----|--------|
| `http` | `http://localhost:5011` | Fejlesztői HTTP profil |
| `https` | `https://localhost:7206` (+ `http://localhost:5011`) | Fejlesztői HTTPS profil |

### MAUI UI profil

| Profil | Leírás |
|--------|--------|
| `Windows Machine` | Windows asztali futtatás (MSIX csomag) |

> Android futtatáshoz válaszd ki az Android emulátort vagy csatlakoztatott eszközt a Visual Studio toolbar-ján.

### Scene Editor profil

| Profil | Leírás |
|--------|--------|
| `Windows Machine` | Windows asztali futtatás |

### AI Trainer Server

A `SevenWonders.AI.Trainer.Server` konzol alkalmazásként indul, nincs külön launch profil – egyszerűen állítsd be Startup projektnek és futtasd.

---

## Unit tesztek futtatása

Nyisd meg a `SevenWonders.UnitTests.sln` solution-t, vagy a fő solution-ben a Test Explorer segítségével futtasd a teszteket:
- `SevenWonders.Game.Engine_UnitTests`
- `SevenWonders.Game.Logic_UnitTests`
