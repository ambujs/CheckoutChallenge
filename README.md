# Checkout .NET Challenge
This repository has got the .NET code to complete the Checkout.com .NET challenge.

## Components
- Payment Gateway: Checkout.PaymentGateway
- Acquiring bank mock service: Checkout.AcquiringBank.Mock
- MongoDb: for data storage to be used by Acquiring bank mock service

## Requirements
- A merchant can post a message to Checkout.PaymentGateway's `/api/Payment` endpoint for processing a card payment. If the payment is successful, then this endpoint will return an unique payment reference back.
- The unique payment reference can then be used to send a message to Checkout.PaymentGateway's `/api/Payment/{paymentId}` endpoint to retrieve the details of the payment. Only the last 4 digits of the card number are returned.

## Possible improvements
- A CD pipeline can be implemented.
- The application is currently using basic auth but a more secure authentication method can be implemented using JWT tokens.
- Add more unit, integration and performance tests. I have just laid out the basic framework for the tests but these can definitely be extended provided if we have more time.
- When running from the Docker container, the two APIs use http protocol. This was done just for simplicity of this demo but can be improved to use https with certficates
- I have left TODOs and NOTEs in the code which should give an idea of areas that can be extended & expanded.

## How to run the code
There are two possible ways of running the code in this repository:
- If you have Docker installed then the best and easiest way will be to run the following two commands from the solution folder:
  -   `docker-compose build`
  -   `docker-compose up`
  This should build & start a Docker container with these three services:
![image](https://user-images.githubusercontent.com/1502181/119105789-99a8fb00-ba15-11eb-96b0-207a1221c442.png)

- Then you should have these two Swagger URLs available for testing the application:
  - http://localhost:9088/api/index.html (Payment Gateway service)
  - http://localhost:8088/api/index.html (Acquiring bank mock service)
- You'll need to authenticate before sending a request from Swagger. Use the following details:
  - Username: checkout
  - Password: checkout123

- If you don't have Docker installed or run into any issues trying to do the above (and that's possible for a multitude of reasons!) then you can open & run the solution from Visual Studio 2019 too. Just remember to:
  - ensure that both `Checkout.AcquiringBank.Mock` and `Checkout.PaymentGateway` are setup as startup projects
  - ensure that you have an instance of MongoDb running and accessible and the update the connection string in appsettings.json of Checkout.PaymentGateway


## Extra mile!
- Application logging: Logging has been implemented using Serilog.
- Application metrics: Metrics are being collected using AppMetrics (data available on /metrics endpoint) which can integrated with Prometheus & Grafana.
- Containerization: The entire application, including two services & mongodb, have been containerized.
- Authentication: Authentication has been implemented using basic auth.
- API client: Swagger has been enabled on both the payment gateway and acquiring bank mock services.
- Build script / CI: CI has been enabled on the repository. A CI pipeline is triggered every time there's a commit on the repository (https://github.com/ambujs/CheckoutChallenge/actions)
- Performance testing: A performance test project has been added to the solution which utilises Benchmark (https://github.com/dotnet/BenchmarkDotNet) to do some basic tests.
- Encryption: I'm encrypting the card numbers using `Aes` before storing them into mongo
- Data storage: The acquiring bank mock service needs to store the data somewhere so that it can return the payments when requested. MongoDb has been used for this.


## Notes
- Some of the models are duplicated between Payment Gateway & Acquiring bank mock service. This is intentional and in keeping with a micro-service architecture of keeping the services loosely coupled.
