# Banking

where there is a 'mock' comment, the implementation is basic, just enough to cover the basic scenario

probably not all files are in place

not much attention was paid to the model regarding referencing properties, navigations, lists
if a real persistence layer was put in place, it would define the rules of writing entities, as EF does for example

entities are not fully completed
a subset of the data is defined in them
prone to change


normalize code for asycn awate task						- poorly implemented
normalize exceptions logic, when to stop pipeline		- poorly implemented
add logic for CancellationToken							- not implemented
add cache layer											- not implemented
logging													- not implemented
implement IdGenereator									- not implemented
currency code comparison solution						- not implemented

normalize data for requests and response				- poorly implemented
add valid DTO layer										- poorly implemented
add object mappers										- not implemented

all strings move to enums or consts						- poorly implemented
add resources											- not implemented


notifications are not tested, but refactoring is needed	- poorly implemented


-- Add Entities --
ReversalRequest
HoldPeriod
AccountPolicy
AccountMandate
MandateRule
AuditLog
KycVerification


-- missing CashPayment transaction --
when the client comes to the branch, fills out the payment slip and gives cash
flow:
- entering payment slip into the system
- trigger fee calculation
- taking money from the client
- create CashPayment transaction
	step 1: creates a deposit transaction to the bank account with that amount of money
	step 2: creates a transfer transaction from the bank account to the account from the payment slip