# CurrencyRates.GatewayService

Единая точка входа. Проксирует запросы на User и Finance через **YARP**. JWT проверяется на gateway; `Authorization` пробрасывается downstream.

## Маршруты

| Путь | Auth | Upstream |
|------|------|----------|
| `POST /api/users/register` | anonymous | UserService :5001 |
| `POST /api/users/login` | anonymous | UserService :5001 |
| `/api/users/**` | JWT | UserService :5001 |
| `/api/finance/dev/**` | anonymous | FinanceService :5002 |
| `/api/finance/**` | JWT | FinanceService :5002 |
| `GET /health` | — | сам Gateway |

Пока **UserService** не запущен, `/api/users/**` вернёт **502** — это ожидаемо.

## Проект

```
CurrencyRates.GatewayService.Api/   — ASP.NET Core + YARP (один проект)
```

## NuGet

- `Yarp.ReverseProxy`
- `Microsoft.AspNetCore.Authentication.JwtBearer`

## Локальный запуск

1. Поднять Finance (или весь стек через Orchestrator).
2. Gateway:

```powershell
dotnet run --project CurrencyRates.GatewayService.Api
```

Порт: **http://localhost:5000**

### Проверка через gateway

```http
GET  http://localhost:5000/health
POST http://localhost:5000/api/finance/dev/token
GET  http://localhost:5000/api/finance/rates
Authorization: Bearer <token>
```

Или файл `CurrencyRates.GatewayService.Api.http`.

Без токена `GET /api/finance/rates` → **401**.

## Docker / Orchestrator

В контейнере destinations: `http://finance:8080`, `http://user:8080` (через env).  
Сборка: context = корень репозитория Gateway, Dockerfile в `CurrencyRates.GatewayService.Api/Dockerfile`.

## .NET

`net8.0`
