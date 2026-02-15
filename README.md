# ThtSizzlingHotProduct

## 📋 Business Rules Implementation

The application strictly adheres to the following business requirements:

1. **Rule 1: Net Sales Calculation** - Cancelled orders are deducted from the product sales total.
2. **Rule 2: Daily Customer Limit** - Multiple purchases of the same product by the same customer on the same day are counted as a single sale towards the product total.
3. **Rule 3: Date Range Support** - Capability to calculate the top product for a specific day and over a rolling 3-day period.
4. **Rule 4: Tie-breaking** - If multiple products have the same top score, the product with the name that appears first alphabetically is selected.

## 💡 Technical Assumptions

To ensure data consistency and system reliability, the following technical assumptions were made:

- **Date Normalization**: All timestamps are normalized to `.Date` (00:00:00) when calculating Rule 2. This prevents different purchase times on the same day from being counted as multiple sales.
- **Order Status and Entry Consistency**:
  - **Completed Orders**: It is assumed that every order with a `Completed` status contains a non-null and non-empty `Entries` list. These entries represent the actual items sold.
  - **Cancelled Orders**: It is assumed that `Cancelled` orders share the same `OrderId` as the original `Completed` order but contain an empty `Entries` list.
  - **Cancellation Level**: Cancellation is assumed to occur strictly at the **Order level**. The business logic does not support partial cancellations of individual products within an order; an order is either fully valid or fully voided.
- **Retroactive Balancing**: The system assumes that a `Cancelled` status on any date should invalidate the corresponding `Completed` order, regardless of whether they occurred on the same day.
- **Data Integrity**: If a sale is associated with a Product ID that is missing from the products repository, the system defaults to "No Product Found" to ensure graceful degradation.

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
