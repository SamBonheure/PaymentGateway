# Payment Gateway API
Payment gateway API that allows a merchant to offer a way for their shoppers to pay for their product.

![api workflow](https://github.com/SamBonheure/PaymentGateway/actions/workflows/netcore.yml/badge.svg)

# Deliverables
+ Build an API that allows a merchant to:
  + `Process a payment through your payment gateway.`
  + `Retrieve details of a previously made payment.`
+ Build a simulator to mock the responses from the bank to test the API from your first deliverable. 

# Assumptions
1. Banks could be slow in processing requests or fail
2. The MerchantId is determined based on the API-Key passed into the request
3. CVV codes could be 3 or 4 digits (Amex)
5. PaymentId is generated by merchant not gateway

# High level flow

 ![FlowChart](https://github.com/SamBonheure/PaymentGateway/blob/master/images/Gateway_Flow.png)

# Architecture

## Command Query Responsibility Segregation Pattern (CQRS)

Based on the requirements and above flowchart, it makes sense to split the responsiblity of:
- Making a payment (Write)
- Retrieving details of previously made payment (Read)

### Implementation

To achieve the above there are 2 elements:
- `PaymentWriteController`: Responsible for sending commands and creating payments
- `PaymentReadController`: Reponsible for queying payments
- `MediatR`: Reponsible for handling the requests and sending them to the correct handler

In a production environment these would be hosted on seperate resources so they can be scaled independently and access use seperate data storage. But for simplicity these are kept in the same API and communicate using the same repository.

# Installation

## Pre-requisites

- .NET Core 3.1 SDK
- If you wish to use Docker, you'll need to have Docker installed

## Running the application

There are 2 ways to run the application:

1. Run the app through visual studio. Set `PaymentGateway.Api` as the default startup project. It will launch on port 44395. But this is configurable in the `launchsettings.json`.
2. Run `docker-compose build` followed by `docker-compose up` and browse to `http:localhost:7000`

Swagger documentation is available on `/docs`

I have also included a Postman file in the solution called `Gateway API.postman_collection.json`

# Project Structure

The outlined project structure below is inspired by https://github.com/jasontaylordev/CleanArchitecture

![Architecture](https://github.com/SamBonheure/PaymentGateway/blob/master/images/CleanArchitecture.png)

## PaymentGateway.Api 

This layer contains all the application logic. It is dependent on the domain layer, but has no dependencies on any other layer or project.

As per the CQRS pattern defined above, it has a read and write controller with the following endpoints:

### POST /Payments 
Endpoint to make a payment request

**Sample Body**:

```json
{
  "id": "1f325b1f-b57c-4b8b-82c0-003dd8107dda",
  "amount": 50.26,
  "currency": "USD",
  "description": "Payment to amazon",
  "card":{
       "number": "1424 4587 9898 2230",
       "cvv": "665",
       "expiryMonth": 5,
       "expiryYear": 22,
       "ownerName": "Tom Cruise"
  }
}
```

**Sample Response**:

**Success (201)**:

```json
{
  "paymentId": "1f325b1f-b57c-4b8b-82c0-003dd8107dda",
  "status": "processing"
}
```

**Bad Request (400)**:

- Duplicate payment
- Invalid Cardnumber
- Invalid CVV
- Invalid Expiry Date
- Invalid Currency
- Invalid Amount

**Too many requests (429)**:

Returns when you are being rate limited based on your client

### GET /Payment/{id}
Endpoint to request details of a previous transaction

**Sample Response**:

**Success (200)**:

```json
{
  "paymentId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "status": "Processing",
  "isApproved": false,
  "card": {
    "cardNumber": "**** **** **** 2230",
    "expiryMonth": 6,
    "expiryYear": 22
  },
  "amount": {
    "currency": "USD",
    "amount": 50.00
  },
  "description": "Payment to amazon"
}
```

```json
{
  "paymentId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "status": "Approved",
  "isApproved": true,
  "card": {
    "cardNumber": "**** **** **** 2230",
    "expiryMonth": 6,
    "expiryYear": 22
  },
  "amount": {
    "currency": "USD",
    "amount": 50.00
  },
  "description": "Payment to amazon"
}
```

```json
{
  "paymentId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "status": "Declined - Insufficient Funds",
  "isApproved": false,
  "card": {
    "cardNumber": "**** **** **** 2230",
    "expiryMonth": 6,
    "expiryYear": 22
  },
  "amount": {
    "currency": "USD",
    "amount": 50.00
  },
  "description": "Payment to amazon"
}
```

**Not Found (404)**:
Payment with id could not be found

**Too many requests (429)**:

Returns when you are being rate limited based on your client

## PaymentGateway.Infrastructure

This layer contains classes for accessing external resources such as databases, web services, event busses, and so on. 
These classes are be based on interfaces defined within the domain layer.

## PaymentGateway.Domain
This is the domain layer of the application and the center of the architecture. All other project dependencies will point towards it.

The Domain project includes things such as:
- Entities
- Interfaces
- Enums
- Exceptions
- Events
- Domain Services

## MockBank

This project is responsible for simulating the acquiring bank's behaviour. 

I have included the following mock behavior:
- Randomized status codes returned
- Randomized declined reason codes returned
- Automatically decline payments above 1000

In order to switch this out for a real bank the specific Adaptar should implement the IBankAdapter interface.

## Tests

I have created the following test projects using xunit.

### PaymentGateway.Domain.UnitTests

This project covers the unit tests for the domain entities of our gateway.

It will test the following behaviours:
- CardNumber validity
- CVV validity
- Expiry date validity
- Card validity
- Payment validity
- Card masking

### PaymentGateway.Api.UnitTests

This project covers the unit tests for the handlers of our API gateway.

It will test the following behaviours:
- EventDispatcher processing

### PaymentGateway.Api.IntegrationTests

This project covers the integration tests for the API.

It will test the following behaviours:
+ Created: POST /Payments
+ BadRequest: Invalid CardNumber
+ BadRequest: Invalid CVV
+ BadRequest: Invalid Expiry date
+ BadRequest: Expired Card
+ BadRequest: Duplicate payments
+ Ok: GET /Payments/{id}
+ NotFound: GET /Payments/{id}

### PaymentGateway.Api.Sdk

This is the SDK project that can be used to easily implement the gateway API by a Merchant. It is built using `Refit`

### PaymentGateway.Api.Sdk.Sample

This is a sample project that shows how to use the SDK using `Refit`

# Bonus points
## Authentication
I've added `API-Key based authentication` to the API and injected it into the Swagger UI.
This will also inject the matching MerchantId so that it does not have to be passed into the payment requests.

You can find the keys in the Appsettings.Development.Json or as follows:
+ Merchant A: `123`
+ Merchant B: `456`

To use this in Swagger, simply click the "Authorize" button and insert your key of choice.

In practice these keys will be returned by a authentication server and not stored in a config file!

## API Client Rate Limiting
I have implemented client based rate limiting into the app using the `AspNetCoreRateLimiting` package and used the Api Keys as clients.

+ ApiKey `123`: Set to max 2 requests in 10s window
+ ApiKey `456`: Whitelisted and not throttled

These settings can be tweaked in the `appsettings.json` file

## API Client SDK
I Created a API Client using `Refit`. A sample project can be found under `PaymentGateway.Api.Sdk.Sample`

If you wish to test this, simply run the console app and API together.

## Logging
I have added `SeriLog` for structured file logging.
The settings can be changed through the appsettings file.

By default the log files can be found in the /Logs folder

## GitHub Actions (CI)

I have set up `GitHub actions` to perform continious integration on our code base and run tests.

The workflow file can be found in the `.github` folder

## Containerization (Docker)

The project contains containerization support using Docker. 

Run `docker-compose build` and `docker-compose up` to start the gateway.

This is currently very basic, but will become more usefull once a datastore has been added to the mix.

# Improvements
- Connect to a datastore instead of using a in memory store (NoSQL seems approriate)
- Add better resilience through retry/circuit break functionality to external bank (Eg: Poly)
- We should have store pre-defined list of merchants and its details
- More elegant tests, add Performance/Acceptance tests
- Add API metrics with something like prometheus
- Connect Serilog to cloud for centralized logging, or implement something like appinsights/sentry.io
