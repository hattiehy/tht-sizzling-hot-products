<div id="top"></div>

<h1 align="center">
  <br> 
  <img src="readme/logo.png" width="800" alt="Bunnings Take Home Assignment" />
  <br>  <br>
  Sizzling Hot Products
  <br>
</h1>

<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#brief">Brief</a>
    </li>
    <li><a href="#objective">Objective</a></li>
    <li><a href="#business-rules">Business Rules</a></li>
    <li><a href="#technical-requirements">Technical Requirements</a></li>
    <li><a href="#getting-started">Getting Started</a></li>
    <li><a href="#assumptions">Assumptions</a></li>
    <li><a href="#expected-outcomes">Expected Outcomes</a></li>
     <li><a href="#extra-points">Extra points</a></li>
  </ol>
</details>

## 📖 Brief

Bunnings is launching a brand-new page on their website to allow customers to view

- A history of the top sizzling hot product for each day
- The top sizzling hot product over the past 3 days

<p align="right">(<a href="#top">back to top</a>)</p>

## 🎯 Objective

Your assignment is to implement a solution to calculate the sizzling hot products using the business rules defined below.

<p align="right">(<a href="#top">back to top</a>)</p>

## 🚦 Business Rules

1. A product sale should only be counted once per order.
   <br>
   > For example if an order contains a purchase of five hammers it should be counted as a one sale
   > towards the product sales total, not five.
   ```json
   [
     {
       "orderId": "O10",
       "customerId": "C1",
       "entries": [{ "id": "P1", "quantity": 2 }],
       "date": "19/07/2021",
       "status": "completed"
     }
   ]
   ```
2. Multiple orders of the same product by the same customer on the same day should be excluded from the product sales total.
   <br>

   > For example if a customer purchased the same product twice in one day in two separate orders, it should be counted as a one sale towards the product total, not two.

   ```json
   [
     {
       "orderId": "O10",
       "customerId": "C1",
       "entries": [{ "id": "P1", "quantity": 2 }],
       "date": "19/07/2021",
       "status": "completed"
     },
     {
       "orderId": "O11",
       "customerId": "C1",
       "entries": [{ "id": "P1", "quantity": 3 }],
       "date": "19/07/2021",
       "status": "completed"
     }
   ]
   ```

3. Cancelled orders should be credited against the product total.
   <br>

   > For example if a customer cancels an order today that was placed yesterday, that product sale should be removed from the product sales total for that day or period.

   ```json
   [
     {
       "orderId": "O10",
       "customerId": "C1",
       "entries": [...],
       "date": "19/07/2021",
       "status": "completed"
     }, {
       "orderId": "O11",
       "date": "20/07/2021",
       "status": "cancelled"
     }
   ]
   ```

4. In the case of product sales being equal for two or more products, sort the products alphabetically and select the first one in the list.
   <br>
   > For example If a "Hammer" and "BBQ" had similar sales you select "BBQ"

<p align="right">(<a href="#top">back to top</a>)</p>

## 🔧 Technical Requirements

1. Use any runtime/language of your choosing
2. Solution should be able to accept data as json files from input folder.

<p align="right">(<a href="#top">back to top</a>)</p>

## ⚙️ Getting Started

1. Fork the repository.
2. Read the brief and make sure you understand the business rules.
3. Use the sample files in the input folder for your calculations.
4. Commit your solution back to any publicly accessible repo in Github and share the URL back for review.
5. Document instructions on how to install and run your solution in the README.

<p align="right">(<a href="#top">back to top</a>)</p>

## 🔨 Assumptions

1. For calculations assume today's date is 21/07/2021.
2. Document any additional assumptions your have made in your README.

<p align="right">(<a href="#top">back to top</a>)</p>

## 🎓 Expected Outcomes

Based on the inputs in this repo and using the business rules in the README the outcomes should be as follows:

<table>
  <tr>
    <td nowrap><strong>Date or Period</strong></td>
    <td nowrap><strong>Top Sizzling Hot Product</strong></td>
  </tr>
  <tr>
    <td nowrap>19/07/2021</td>
    <td>Ezy Storage 37L Flexi Laundry Basket - White</td>
  </tr>
 <tr>
    <td nowrap>20/07/2021</td>
    <td>Ezy Storage 37L Flexi Laundry Basket - White</td>
  </tr>
   <tr>
    <td nowrap>21/07/2021</td>
    <td>Arlec 160W Crystalline Solar Foldable Charging Kit</td>
  </tr>
    <tr>
    <td nowrap>19/07/2021 - 21/07/2021 </td>
    <td>Ezy Storage 37L Flexi Laundry Basket - White</td>
  </tr>
</table>

<p align="right">(<a href="#top">back to top</a>)</p>

## ✨ Extra points

1. Providing well unit-tested code.
2. Considering design patterns & S.O.L.I.D principles
3. Considering other inputs and edge cases outside the supplied ones.

<p align="right">(<a href="#top">back to top</a>)</p>
