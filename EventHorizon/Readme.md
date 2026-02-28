# 🌌 EventHorizon

> *"Captain's Log, Stardate 2026.02.28 – a near-Earth object has been detected..."*

**EventHorizon** ist ein C#/.NET Lernprojekt das **Event Sourcing mit Marten** anhand echter NASA-Weltraumdaten demonstriert.
Die NASA NeoWs API liefert täglich neue Asteroiden-Daten – jede Information wird als unveränderlicher Event in PostgreSQL gespeichert.
Was zunächst nach einem langweiligen Log-System aussieht, überwacht in Wirklichkeit den Weltraum. 🪨

---

## 🎯 Lernziele

Dieses Projekt zeigt in der Praxis:

- **Event Sourcing** – Zustand wird nicht überschrieben, sondern als Abfolge von Events gespeichert
- **Marten** als Event Store auf Basis von PostgreSQL
- **Aggregates** – wie Domänenobjekte aus Events rekonstruiert werden
- **Background Services** in ASP.NET Core
- **Minimal API** mit Scalar als modernes Swagger UI
- **Externe API-Integration** mit dem NASA NeoWs Endpoint

---

## 🗺️ Architektur

```mermaid
graph TB
    NASA[🛸 NASA NeoWs API] -->|HTTP Poll beim Start| PS[NeoWsPollingService\nBackgroundService]
    PS -->|NearEarthObjectDetected\nApproachDataUpdated| MS[(Marten\nEvent Store)]
    MS -->|PostgreSQL JSONB| DB[(🐘 PostgreSQL\nin Docker)]
    API[Minimal API\nASP.NET Core] -->|IQuerySession| MS
    UI[🔭 Scalar UI\nlocalhost:PORT/scalar] -->|HTTP GET| API

    subgraph Domain
        AGG[NearEarthObject\nAggregate]
        E1[NearEarthObjectDetected]
        E2[ApproachDataUpdated]
    end

    MS --> AGG
    E1 --> AGG
    E2 --> AGG
```

---

## 🚀 Voraussetzungen

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- Visual Studio 2022 oder Rider

---

## ▶️ Projekt starten

### 1. Repository klonen

```bash
git clone https://github.com/lady-logic/EventHorizon.git
cd EventHorizon
```

### 2. Docker Desktop starten

Stelle sicher dass Docker Desktop läuft (grünes Symbol in der Taskleiste).

### 3. PostgreSQL starten

```bash
docker compose up -d
```

Prüfen ob der Container läuft:

```bash
docker ps
```

Du solltest `eventhorizon-db` mit Status `Up` sehen.

### 4. App starten

In Visual Studio: **Strg+F5**

Oder per CLI:
```bash
cd EventHorizon.Api
dotnet run
```

### 5. Scalar UI öffnen

Die App gibt beim Start die URL aus, z.B.:
```
Now listening on: http://localhost:5152
```

Öffne dann: **http://localhost:5152/scalar**

Beim Start pollt die App automatisch die NASA NeoWs API und speichert alle Asteroiden der nächsten 7 Tage als Events.

---

## 🔭 API Endpoints

| Method | Endpoint | Beschreibung |
|--------|----------|--------------|
| GET | `/` | Health Check |
| GET | `/asteroids` | Alle gespeicherten Events |

---

## 📁 Projektstruktur

```
EventHorizon/
├── src/
│   └── EventHorizon.Api/
│       ├── Domain/
│       │   ├── Events/
│       │   │   └── NeoEvents.cs          ← NearEarthObjectDetected, ApproachDataUpdated
│       │   └── Aggregates/
│       │       └── NearEarthObject.cs    ← Aggregate, rekonstruiert aus Events
│       ├── Infrastructure/
│       │   ├── NasaApiClient.cs          ← HTTP Client für NASA NeoWs API
│       │   └── NasaApiModels.cs          ← Deserialisierungsmodelle
│       ├── BackgroundServices/
│       │   └── NeoWsPollingService.cs    ← Pollt NASA API beim Start
│       └── Program.cs
├── docker-compose.yml
└── README.md
```

---

## 💡 Das Event Sourcing Prinzip

In einer klassischen Datenbank wird der Zustand **überschrieben**. In EventHorizon wird jede Veränderung als **unveränderlicher Event** gespeichert:

```
Stream "2003 DZ15" (NASA ID: 3150194):
  [0] NearEarthObjectDetected  → Asteroid erstmals erkannt
  [1] ApproachDataUpdated      → Annäherungsdaten: 64.166.386 km, 8,9 km/s
  [2] ApproachDataUpdated      → Neue NASA-Berechnung: Bahn aktualisiert
  ...
```

Der aktuelle Zustand ist **immer aus der Geschichte berechenbar** – man kann zu jedem Zeitpunkt zurückspulen und fragen: *"Was wussten wir an Tag X über diesen Asteroiden?"*

---

## 🛣️ Roadmap

- [x] **Stufe 1** – Marten Event Store mit NASA NeoWs API
- [ ] **Stufe 2** – Clean Architecture + CQRS mit MediatR + Read Model Projektionen
- [ ] **Stufe 3** – Observability mit Grafana + Loki + Prometheus

---

## 🔑 NASA API Key (optional)

Die App verwendet standardmäßig den kostenlosen `DEMO_KEY` (30 Requests/Stunde).

Für mehr Requests: Kostenloser API Key unter [api.nasa.gov](https://api.nasa.gov/)

In `appsettings.Development.json` eintragen:
```json
{
  "Nasa": {
    "ApiKey": "DEIN_KEY_HIER"
  }
}
```

---

## 👩‍💻 Autorin

**Katharina** – Senior Software Developer  
Lernprojekt zur Vertiefung moderner Architekturmuster: Event Sourcing, CQRS, Clean Architecture  
GitHub: [@lady-logic](https://github.com/lady-logic)
