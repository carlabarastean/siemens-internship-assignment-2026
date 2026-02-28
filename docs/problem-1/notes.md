# Problem 1 – Design Notes

## Domain Modeling

The system separates:
- Core entities: `Customer`, `Order`, `OrderItem`, `Barista`, `CoffeeShop`
- Configuration entities: `Menu`, `MenuItem`, `Extra`
- Enums for controlled values: `BeverageType`, `Size`, `MembershipType`, `OrderStatus`, `ExtraType`

This keeps business logic structured and maintainable.

---

## Pricing & Order Snapshot

`MenuItem` represents the current menu configuration.

`OrderItem` stores:
- `line_total_cents`
- referenced `menu_item_id`

`Order` stores:
- `total_price_cents`

Prices are persisted at order time to preserve historical accuracy even if menu prices change.

Money is stored in the database as integer cents to avoid floating-point precision issues.

---

## Extras Modeling

Extras are modeled as a separate entity with a many-to-many relationship to `OrderItem`
via `order_item_extra`.

This allows:
- Multiple extras per item
- Quantity tracking
- Storing the extra price at purchase time

---

## Loyalty Program

Customers have:
- `membership_type` (REGULAR = 1 point/€; GOLD = 2 points/€)
- `loyalty_points`

Orders store:
- `loyalty_points_earned`
- `points_redeemed`

This ensures auditability and consistent historical tracking.

---

## Relationships

- A `CoffeeShop` has multiple `Barista`.
- An `Order` belongs to one `CoffeeShop`.
- An `Order` is prepared by one `Barista`.
- A `Customer` can place multiple `Order`.

---

## Assumptions

- Each order belongs to exactly one coffee shop.
- Prices are immutable at order time.
- Points required for free drinks are defined at menu level.
