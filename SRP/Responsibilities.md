## WardBoard

Responsibilities:
- Manage beds and patients.
- Calculate the acuity score.
- Manage pager alerts.
- Build the handoff note.
- Export ward data to CSV.

Why this is a problem:
- The class has more than one reason to change.
- A change in scoring, alerts, formatting, or exporting would require changing the same class.s



## CheckoutBasket

Responsibilities:
- Manage basket items.
- Handle coupon discount rules.
- Calculate basket totals.
- Handle gift-wrap fees.
- Build gift message cards.
- Create payment authorization codes.

Why this is a problem:
- The class has more than one reason to change.
- Coupon rules, pricing rules, gift messages, gift-wrap fees, and payment logic can change separately.


## SupportTicket

Responsibilities:
- Manage support ticket data and customer messages.
- Calculate ticket priority from its text.
- Calculate the SLA deadline and breach status.
- Build the public customer reply.
- Build the internal escalation message.

Why this is a problem:
- The class has more than one reason to change.
- Priority rules, SLA rules, customer replies, and internal messages can change separately.


## LoanDesk

Responsibilities:
- Store loan application data.
- Calculate the risk score.
- Check loan eligibility.
- Build the required documents list.
- Build the decision letter.
- Export underwriter data to CSV.

Why this is a problem:
- The class has more than one reason to change.
- Risk rules, eligibility rules, document requirements, letter wording, and CSV format can change separately.



## CourseEnrollmentDesk

Responsibilities:
- Manage course enrollment and the waitlist.
- Build welcome packet content.
- Create tuition invoice lines.

Why this is a problem:
- The original class had more than one reason to change.
- Enrollment rules, welcome content, and finance formatting can change independently.



## KitchenTicket

Responsibilities:
- Manage kitchen items.
- Detect allergens.
- Calculate the estimated ready time.
- Render the thermal ticket.
- Select the expo lane.

Why this is a problem:
- The class has more than one reason to change.
- Allergen rules, timing rules, printing format, and lane rules can change separately.




## SubscriptionBilling

Responsibilities:
- Store subscription data and failed payment state.
- Calculate prorated subscription amounts.
- Generate invoice numbers.
- Build dunning emails.
- Export ledger journal lines.

Why this is a problem:
- The original class had more than one reason to change.
- Pricing rules, invoice numbering, email content, and accounting export can change independently.



## GradeBook

Responsibilities:
- Store and record student scores.
- Calculate student averages.
- Calculate letter grades.
- Check honor roll eligibility.
- Build student transcripts.
- Export grade data to CSV.

Why this is a problem:
- The original class had more than one reason to change.
- Average rules, grade policies, honor rules, transcript formatting, and CSV formatting can change independently.



## WarehousePickList

Responsibilities:
- Store warehouse pick-list data.
- Allocate available stock for requested items.
- Calculate the walking order through the warehouse.
- Build picker instructions.
- Export allocation data as WMS XML.

Why this is a problem:
- The original class had more than one reason to change.
- Allocation rules, warehouse path rules, picker instructions, and WMS integration can change independently.





## AppointmentDesk

Responsibilities:
- Manage appointment scheduling, business hours, and booked slots.
- Generate ICS calendar content.
- Build SMS reminder messages.

Why this is a problem:
- The original class had more than one reason to change.
- Scheduling rules, calendar formatting, and SMS content can change independently.