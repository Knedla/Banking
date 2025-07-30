# Banking

probably not all files are in place

not much attention was paid to the model regarding referencing properties, navigations, lists
if a real persistence layer was put in place, it would define the rules of writing entities, as EF does for example

entities are not fully completed
a subset of the data is defined in them
prone to change


normalize code for asycn awate task			- poorly implemented
normalize exceptions						- poorly implemented
add logic for CancellationToken				- not implemented
add cache layer								- not implemented
logging										- not implemented

normalize data for requests and response	- poorly implemented
add valid DTO layer							- poorly implemented
add object mappers							- not implemented

all strings move to enums or consts			- poorly implemented
add resources								- not implemented


-- Add Entities --
ReversalRequest
HoldPeriod
AccountPolicy
AccountMandate
MandateRule
AuditLog
KycVerification
