# Downotifier
The project is called  Downotifier(Down+Notifier)  
!!Docker-compose is not ready yet.

To schedule calling APIs, I used Hangfire.  
I created 4 microservices:   
<p align="center"><img src=https://github.com/moslem-hadi/Downotifier/assets/9815699/f4fa3f42-1ae7-4045-b0c8-04eb5d4ff7ed /></p>

1- IdentityProvider: Uses Duende Software (Identity Server 6) to authenticate users. It is meant to be an SSO for users to log in, log out, and register. It uses SQLite file database.  

2- Jobs Application: It is the API to do Crud operations on ApiCallJob (entity). It uses clean architecture, CQRS, MetiatR, EF Core, JWT Authentication, MediatR Pipelines, AutoMapper, and Fluent Validation.  

3- Scheduler: This microservice, uses Hangfire to schedule recurring jobs. it uses an SQL server as the database. it is responsible for creating, updating, and deleting jobs. and running jobs in their scheduled time.  

4- Notifier: Its responsibility is to send notifications. it has an "INotifyService" interface and any notification provider should implement this interface. then, by using the Strategy design pattern, the context class calls the correct notifier.
  


0- Shared Project: It consists of some services and classes that are shared between microservices. eg: message publisher and subscriber, helpers, email service,...  
This project should be a Nuget package that other projects add as a package.  

<p align="center"><img src=https://github.com/moslem-hadi/Downotifier/assets/9815699/693f5085-c7aa-4793-834c-c98933122686 /></p>
  
There is also a frontend app using ReactJs:  

<p align="center"><img src=https://github.com/moslem-hadi/Downotifier/assets/9815699/d38e7b05-ff71-4ead-8ed9-d453f919052f /></p>

The frontend app uses Identity provider API to get an API access token. Then it uses the Rest Api provided by "Jobs Application" microservice to do crud operations.
  
<p align="center"><img src=https://github.com/moslem-hadi/Downotifier/assets/9815699/8457bf42-fcbd-4300-bae8-1fa40e2acdeb /></p>
  
And u can create an entry and add details like type of the HTTP call, headers, body, notifications,...  
  
<p align="center"><img src=https://github.com/moslem-hadi/Downotifier/assets/9815699/d02aae1d-5d85-4a5a-975f-d46413ba16a0 /></p>
  
This is how the app works:  
1- After login, you create a job using react application.  
2- Job API will create the job in the DB and publishes a message to RabbitMQ, stating that a job has been created.  
3- The Scheduler microservice is subscribed to that message. When Scheduler receives a message from the "jobCreate" queue, it adds it to the Hangfire RecurringJobs.  
4- Hangfire runs recurring jobs based on their interval. when the job runs, the API URL will be called. If the API return anything other than a success status code, it sends a message to the "notify" queue.  
5- Notify microservice is subscribed to the "notify" queue. and when it receives a message, based on the type(email, SMS,...) it does some actions.  

for Updating and Deleting ApiCallJobs, there are 2 queues as well. and Scheduler microservice is subscribed to them.  

There are a few things that I wanted to do, but no time!  
- Logging the events or API calls and the result.  
- Create a docker-compose file to run every microservices and its dependencies together.  
- Error handling and validation in the frontend and some cases backend.  
- Authentication for Hangfire dashboard (Although we don't need to expose the dashboard to users)  
- Using a retry policy with Polly, plus preventing continuous notifications if an API fails.  
- API versioning.  


Please keep in mind that there are a few over-engineering in this project, just to demonstrate some skills. So KISS and YANGNI principals are not met!!  
