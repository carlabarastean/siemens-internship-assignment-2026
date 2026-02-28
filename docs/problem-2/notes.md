# SieMarket Project Notes

## Project Overview
Online electronics store system that processes orders from customers across Europe with automatic discount application.

## Key Features
- Order management with multiple items per order
- Automatic 10% discount for orders exceeding €500
- Customer spending tracking across multiple orders
- Top spending customer identification

## Domain Classes
- **OrderItem**: Product name, quantity, unit price
- **Order**: Contains items, calculates subtotal and final price with discount
- **Customer**: Tracks customer information and order history
- **OrderService**: Business logic for customer queries

## Business Rules
- Discount threshold: €500 (exclusive)
- Discount rate: 10% on total order value
- Customers from: France, Spain, Italy, Germany

## Test Coverage
- 10 comprehensive unit tests
- All business logic validated
- 100% pass rate

## Current Status
- Code complete and tested



