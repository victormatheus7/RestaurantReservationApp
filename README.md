# Resestaurant Reservation App

This .NET 6.0/C# application is an example of the adoption of several concepts and technologies, including:  
- Application of Clean Architecture;  
- Configuration and creation of tests using the NUnit library; 
- Generation of encrypted records using HMAC/SHA512;  
- Authorization to access resources using JSON Web Token (JWT); 
- Access to the database using the micro ORM Dapper; 
- Configuration of orchestrated containers to run the application alongside its own database;
- API exposure using OpenAPI (Swagger).


## Business Rule

To illustrate these points, the following generic use case was created:
>**Title:**  Restaurant Reservation Workflow
**As**  the manager of a renowned restaurant
**I want** a solution that allows me and my clients to create, access, update, and delete table reservations efficiently
**So that**  we can provide an exceptional experience to our customers and optimize restaurant operations

The system then has two users: the **client** and the **administrator**.  The client is authorized to view and manage only their own reservations, while the administrator is authorized to view and manage all reservations created.

## Database

![Database diagram](https://raw.githubusercontent.com/victormatheus7/RestaurantReservationApp/master/misc/db-diagram.png)
The structure above presents the adopted database modeling.  
- The **role** table stores the role options (0 -> Client, 1 -> Admin).  
- The **location_preference** table stores the seating options for the reservation (0 -> Indoor, 1 -> Outdoor).  
- The **user** table stores user data (with password fields encrypted).  
- The **reservation** table stores table reservation data.

## How to run?

Running the following command at the project root: *docker compose up*.

>You can also run it using the *dotnet* direct command and setting up the PostgreSQL container manually.

Immediately after initializing the containers (one for the application and the other for the PostgreSQL database) you will be able to access the [Swagger interface](https://localhost:5000/swagger/index.html).

## Methods & Endpoints

**User methods:**

>**Create a user.**
**POST** [api/v1.0/users/create](https://localhost:5000/api/v1.0/users/create)  
Expected response: HTTP Status Code 200.
>>email -> The user's email (**string**).
password -> An alpha-numeric password with 6 to 12 characters (**string**).
role -> The user's role. Could be 0 (Client) or 1 (Admin) (**int**).

<br>

>**Login a user.**
**GET** [api/v1.0/users/login?email=**{email}**&password=**{password}**](https://localhost:5000/api/v1.0/users/login?email={email}&password={password})
Expected response: HTTP Status Code 200 and a  **JWT**.

<br>

**Reservation methods:**

>**Create a reservation for the logged in user.**
**POST** [api/v1.0/reservations](https://localhost:5000/api/v1.0/reservations)
Expected response: HTTP Status Code 200.
>>HEADER: Authorization Bearer **{JWT}**
date -> The reservation date. It must be greater than now (**string** in date time format: YYYY-MM-DDTHH:mm:ss.sssZ).
numberSeats -> The desired number of seats. Should be a number between 1 and 100 (**int**).
locationPreference -> The location preference. Could be 0 (Indoor) or 1 (Outdoor) (**int**).
observation -> A text up to 1000 characters (**string**).

<br>

>**List all user's reservations. If the user is an admin, it will list all reservations of all users.**
**GET** [api/v1.0/reservations](https://localhost:5000/api/v1.0/reservations)
Expected response: HTTP Status Code 200 and a list of reservations.
>>HEADER: Authorization Bearer **{JWT}**

<br>

>**Get a specific reservation.**
**GET** [api/v1.0/reservations/**{reservationGuid}**](https://localhost:5000/api/v1.0/reservations/{reservationGuid})
Expected response: HTTP Status Code 200 and a reservation.
>>HEADER: Authorization Bearer **{JWT}**

<br>

>**Update a reservation.**
**PUT** [api/v1.0/reservations/**{reservationGuid}**](https://localhost:5000/api/v1.0/reservations/{reservationGuid})
Expected response: HTTP Status Code 200.
>>HEADER: Authorization Bearer **{JWT}**
date -> The reservation date. It must be greater than now (**string** in date time format: YYYY-MM-DDTHH:mm:ss.sssZ).
numberSeats -> The desired number of seats. Should be a number between 1 and 100 (**int**).
locationPreference -> The location preference. Could be 0 (Indoor) or 1 (Outdoor) (**int**).
observation -> A text up to 1000 characters (**string**).

<br>

>**Delete a reservation.**
**DELETE** [api/v1.0/reservations/**{reservationGuid}**](https://localhost:5000/api/v1.0/reservations/{reservationGuid})
Expected response: HTTP Status Code 200.
>>HEADER: Authorization Bearer **{JWT}**

<br>

>**Count all reservations for a specific day.**
**GET**  [api/v1.0/api/v1.0/reservations/count/**{date}**](https://localhost:5000/api/v1.0/reservations/count/{date})
Expected response: HTTP Status Code 200 and a number.

## Contact
[Victor Castro | LinkedIn](https://www.linkedin.com/in/victorcastro7/)