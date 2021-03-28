# .NET Challenge - Building a Payment Gateway

## What has been achieved
* Built Payment Gateway API with 2 endpoints:
  * Endpoint to make a payment
    * Conducts server-side validations on client bank details
    * Processes payment with (fake) bank - fake bank validations, fake client bank check, fake merchant bank check
    * Stores payment information in MongoDb collection
    * Returns success/failure response (with payment Id returned for reference on success)
  * Endpoint to fetch a payment by ID
    * Takes input of an ID to request a payment from MongoDb
    * Returns success/failure response (with payment returned for reference on success)
* Built a fake bank (can be switched out by configuration)
* Built Nunit test solution with unit tests (not full coverage as time-permitting)
* Running on HTTPS with .NET dev certificate (adds encryption)
* Dockerized the application (running on HTTPS and exposed on localhost:5001)

## Areas of Improvement
* Requires a CHA/CHP certificate as opposed to a dev certificate
* Should add API Authentication with granular permissions (IdentityService or OpenIdConnect)
* Store card details in a separate encrypted DB
* Caching for quicker data access
