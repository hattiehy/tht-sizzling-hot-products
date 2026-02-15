# ThtSizzlingHotProduct

## 💡 Technical Assumptions

To ensure data consistency and system reliability, the following technical assumptions were made:

- **Date Normalization**: All timestamps are normalized to `.Date` (00:00:00) when calculating Rule 2. This prevents different purchase times on the same day from being counted as multiple sales.
- **Order Status and Entry Consistency**:
  - **Completed Orders**: It is assumed that every order with a `Completed` status contains a non-null and non-empty `Entries` list. These entries represent the actual items sold.
  - **Cancelled Orders**: It is assumed that `Cancelled` orders share the same `OrderId` as the original `Completed` order but contain an empty `Entries` list.
  - **Cancellation Level**: Cancellation is assumed to occur strictly at the **Order level**. The business logic does not support partial cancellations of individual products within an order; an order is either fully valid or fully voided.
- **Retroactive Balancing**: The system assumes that a `Cancelled` status on any date should invalidate the corresponding `Completed` order, regardless of whether they occurred on the same day.
- **Data Integrity**: If a sale is associated with a Product ID that is missing from the products repository, the system defaults to "No Product Found" to ensure graceful degradation.

## 🛠 Future Improvements & Roadmap

While the current solution focuses on the core business logic and order processing, the following enhancements are planned for a production-ready version:

- **Input Data Validation**:
  - Due to the project's current scope and time constraints, explicit input validation for the JSON data source was not implemented.
  - **Proposed Solution**: Integrate **FluentValidation** to enforce strict schemas for the `Order` and `Product` models (e.g., `Entries` are not null for completed orders).
- **Logging and Monitoring**:
  - Implement structured logging to track data processing errors or missing product references during runtime.

## 🚀 Installation & Instructions

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or higher.
- Visual Studio Code or Visual Studio 2022.

### Setup and Execution

1. **Clone the Repository**

   ```bash
   git clone <your-repository-url>
   cd ThtSizzlingHotProduct
   ```

2. **Restore Dependencies**

   ```bash
   dotnet restore
   dotnet build
   ```

3. **Run the Application**

   ```bash
   dotnet run
   ```

4. **Run Unit Tests**
   ```bash
   dotnet test
   ```
