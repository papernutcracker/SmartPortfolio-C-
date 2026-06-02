#System Extension for "Smart Dividend Portfolio Tracker"

## 🎯 Objective

To gain hands-on experience with Object-Oriented Programming (OOP) principles in C#, utilizing collections, writing declarative queries using LINQ, separating business logic into distinct layers (Models, Services, Managers), and refactoring an interactive command-line interface.

---

## 💻 Technical Specification & Implementation Steps

### Phase 1: Universal English Localization & Cleanup (`Program.cs`)

The current codebase features a mix of Ukrainian and English UI components alongside manual console encoding configurations, which often causes character rendering bugs.

1. **Encoding Cleanup:** Remove any manual UTF-8 console encoding statements (`Console.OutputEncoding` / `Console.InputEncoding`) from the codebase.


2. **English-Only Interface:** Translate all console outputs, variables, sectors, menu options, and error messages strictly to **English**. Completely remove the `bool isUa` variable and all language-conditional `switch` or `if-else` blocks.



### Phase 2: Core Business Logic & Robust CRUD Operations

Ensure reliable asset handling inside the `PortfolioManager` class:

* **Smart Stock Addition (`AddStock`):** Implement duplicate tracking logic. If a stock with the input ticker symbol already exists in the portfolio, do not append a new object. Instead, update the existing stock's shares count (`Shares`) and dynamically recalculate its new weighted average price (`AveragePrice`).
* **Stock Removal (`RemoveStock`):** Implement a safe element removal routine from the data collection using its unique uppercase ticker symbol.

### Phase 3: Analytical Reports Integration using LINQ

Incorporate structured data queries inside the "View Assets" layout or a dedicated analytics dashboard using **LINQ**:

1. **Sector Diversification Breakdown:** Build a report that groups assets by their `Sector` and computes the exact percentage share each sector holds relative to the total portfolio value.
2. **P/E Ratio Evaluation:** Introduce a query method to screen for potentially undervalued companies (e.g., filtering out stocks where `PERatio < 15` and `PERatio > 0`).

### Phase 4: Mocking the "AI Advisor" Service

Create a standalone `AIAdvisorService` that analyzes the user's active profile (`UserProfile`):

* For `Beginner` users, the service should display essential introductory tips focusing on asset diversification and foundational safety metrics.
* For `Experienced` users, the service should run the P/E screening routine built in Phase 3 to highlight market opportunities and offer portfolio rebalancing suggestions.

---

## 📝 Assessment Criteria (Checklist)

* [ ] The project compiles seamlessly under the target framework `.NET 10.0`.


* [ ] All interactive terminal outputs, menu headers, stock sectors, and validation warnings are presented exclusively in English.
* [ ] Language-toggling infrastructure (`isUa`) and manual console encoding overrides have been entirely removed.
* [ ] Stock aggregation logic successfully prevents duplicate ticker records by combining shares and average prices.
* [ ] Analytical computations (sector allocations, average yields) are processed using declarative LINQ operators (`GroupBy`, `Sum`, `Select`).
* [ ] Terminal inputs feature strong defense clauses against non-numeric entries (robust parsing for double/int values).

---

## 📂 Expected Target Architecture

The completed project must retain a structured, decoupled layout:

* **`Models/`** — `UserProfile`, `DividendStock`, along with `InvestmentGoal` and `ExperienceLevel` enums.
* **`Services/`** — `OnboardingService`, `TutorialService`, `CompoundCalculatorService`, and the newly added `AIAdvisorService`.
* **`Managers/`** — `PortfolioManager` (housing the updated data structures and LINQ queries).
* **`Program.cs`** — The streamlined, localized main application lifecycle entry point.
