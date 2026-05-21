# EasyShop — Trip Management UI

## Goal
Build a functional trip management UI with global current-travel state, dashboard, entity CRUD pages, and sidebar navigation.

## Constraints & Preferences
- UI stack: .NET MAUI Blazor Hybrid + Radzen.Blazor v10.4.1 (material-light theme)
- All entity amounts (products, expenses, incomes) are stored/entered in `MoneyToTravel.Currency` (locked, no dropdown)
- `ChangeValue.Currency` is the price currency — used for display when it differs from `MoneyToTravel.Currency`
- `PriceCurrencyCode` removed from `Travel` — `ChangeValue` now defines the price currency relationship
- Spanish UI labels throughout
- Build must produce zero errors across all target frameworks (android, ios, maccatalyst, windows)

## Progress
### Done
- Created `Components/Pages/Travel/Create.razor` with form for: StartDate, EndDate, MoneyToTravel (amount + currency as CostCurrencyCode), Multiplicator, ChangeValue (always visible)
- Wired "Nuevo viaje" button in `Travel/Index.razor` to navigate to `/travels/create`
- Implemented `Travel/Index.razor` with card‑based list (`EasyTravelCard`), loading state (`Loader`), and empty state (`EmptyState`)
- Created `Services/TravelStateService.cs` — Scoped service holding `CurrentTravel`, with `LoadLastTravelAsync()`, `SetCurrentTravelAsync()`, `OnTravelChanged` event
- Registered `TravelStateService` in `DependencyContainer.cs`
- Updated `MainLayout.razor` — calls `LoadLastTravelAsync()` on init, shows badge in header (`"15/05 → 30/05"`)
- Added `IsCurrent` parameter + `OnSelected` callback to `EasyTravelCard.razor`; icon turns blue when active
- Updated `Travel/Index.razor` to pass `IsCurrent` and handle card selection (calls `TravelState.SetCurrentTravelAsync`)
- Removed `CostCurrencyCode` from `Travel.cs`, `ApplicationDbContext.cs`, `CreateTravel.cs`, `UpdateTravel.cs`, `TravelMapping.cs`, `Create.razor` request, and `Home/Index.razor` (now uses `MoneyToTravel.Currency`)
- Removed `PriceCurrencyCode` from `Travel` entity, requests, mappings, DB config, `Create.razor`, `EasyTravelCard.razor`, `Home/Index.razor` — replaced by `ChangeValue.Currency`
- Changed `Create.razor` and `EasyTravelCard.razor` so `ChangeValue` is always displayed (no `@if` conditional)
- Added `GetProductsProfit()` and `GetProductsProfitInTravelCurrency()` methods to `Travel.cs`
- Updated `Home/Index.razor` dashboard — balance card now includes "Ganancias" row (always in `MoneyToTravel.Currency` + second row in `ChangeValue.Currency` when currencies differ); removed standalone profit card
- Made dashboard summary cards (Productos, Gastos, Entradas) clickable — navigate to `/travels/products`, `/travels/expenses`, `/travels/incomes`
- Created entity list pages: `Products.razor` (`/travels/products`), `Expenses.razor` (`/travels/expenses`), `Incomes.razor` (`/travels/incomes`)
- Created feature handlers: `CreateProductHandler`, `CreateExpenseHandler`, `CreateIncomeHandler`
- Created create pages: `ProductCreate.razor`, `ExpenseCreate.razor`, `IncomeCreate.razor` with currency locked to `MoneyToTravel.Currency` (no dropdown, shown as label text)
- Registered all 3 handlers in `DependencyContainer.cs`
- Updated sidebar in `MainLayout.razor` — conditional menu items (Productos, Gastos, Entradas) appear when `TravelState.CurrentTravel != null`

### In Progress
- (none)

### Blocked
- (none)

## Key Decisions
- Remove `CostCurrencyCode` from `Travel` entirely — `MoneyToTravel.Currency` is the single source of truth for cost currency
- Remove `PriceCurrencyCode` from `Travel` — `ChangeValue.Currency` defines the price currency; when `ChangeValue.Amount == 1` and same currency, prices are in travel currency
- Entity create forms (product, expense, income) use locked currency = `MoneyToTravel.Currency` — no dropdown, shown as label text like "Valor (USD)"
- Profit displayed inline in balance card (not as separate card) — always shows in `MoneyToTravel.Currency`, with optional second line in `ChangeValue.Currency`
- Keep `TravelStateService` as Scoped (not Singleton) because MAUI has a single circuit and DbContext/Handlers are Scoped
- List pages follow `Travel/Index.razor` pattern: title + "Nuevo" button (no "Volver" button)
- Sidebar items for entities are conditional on active travel

## Critical Context
- `TravelStateService` requires `GetTravelHandler` (loads with `Products`, `Expenses`, `Incomes` via `.Include()`) — dashboard sums and entity list pages rely on these navigation properties being loaded
- `GetTravelsHandler` returns travels ordered by `StartDate` descending (most recent first)
- After creating any entity, `TravelState.SetCurrentTravelAsync(_travel.Id)` is called to refresh the in-memory state
- Entity list pages redirect to EmptyState when no active travel exists
- The `UpdateTravel` record and mapping still exist but have not been used by any UI yet

## Relevant Files
- `Services/TravelStateService.cs` — global travel state with `CurrentTravel`, load/select/clear methods, change event
- `Components/Pages/Home/Index.razor` — dashboard with balance card (includes Ganancias rows) and clickable summary cards
- `Components/Pages/Travel/Index.razor` — travel list with card selection
- `Components/Pages/Travel/Create.razor` — create travel form (no `CostCurrencyCode`, no `PriceCurrencyCode`, ChangeValue always visible)
- `Components/Pages/Travel/Products.razor` — product list page (`/travels/products`)
- `Components/Pages/Travel/Expenses.razor` — expense list page (`/travels/expenses`)
- `Components/Pages/Travel/Incomes.razor` — income list page (`/travels/incomes`)
- `Components/Pages/Travel/ProductCreate.razor` — create product form (locked currency)
- `Components/Pages/Travel/ExpenseCreate.razor` — create expense form (locked currency)
- `Components/Pages/Travel/IncomeCreate.razor` — create income form (locked currency)
- `Components/Shared/EasyTravelCard.razor` — reusable travel card with `IsCurrent` and `OnSelected`
- `Components/Shared/EmptyState.razor` — empty state component used across all pages
- `Components/Shared/Loader.razor` — loading spinner component
- `Components/Shared/PageHeader.razor` — breadcrumb header with icon
- `Components/Layout/MainLayout.razor` — app shell, initializes `TravelStateService`, conditional sidebar menu
- `DependencyContainer.cs` — DI registration for all handlers (Travel + Product + Expense + Income)
- `Domain/Models/Travel.cs` — entity without `CostCurrencyCode` or `PriceCurrencyCode`, with `GetProductsProfit()`/`GetProductsProfitInTravelCurrency()`
- `Features/Products/CreateProduct.cs` — handler + request record
- `Features/Expenses/CreateExpense.cs` — handler + request record
- `Features/Incomes/CreateIncome.cs` — handler + request record
- `Features/Travels/CreateTravel.cs` — handler + request record (no `CostCurrencyCode`, no `PriceCurrencyCode`)
- `Features/Travels/UpdateTravel.cs` — handler + request record (no `CostCurrencyCode`, no `PriceCurrencyCode`)
- `Features/Travels/Commons/TravelMapping.cs` — mappers for create/update (no `CostCurrencyCode`, no `PriceCurrencyCode`)
- `Features/Travels/GetTravels.cs` — list handler (returns ordered by StartDate desc)
- `Features/Travels/GetTravel.cs` — detail handler (includes Products, Expenses, Incomes)
