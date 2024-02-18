# Travel and Accommodation Booking Platform API
This API provides a range of endpoints designed for the management of various hotel-related tasks, including booking handling, hotel and city information management, and offering guest services.

## Key Features

### User Authentication

- **User Registration**: Allows new users to create accounts by providing necessary information.
- **User Login**: Enables registered users to log in securely to access booking features.

### Global Hotel Search

- **Search by Various Criteria**: Users can search for hotels using criteria such as hotel name, room type, room capacities, price range, and other properties through text fields.
- **Comprehensive Search Results**: Provides users with detailed information about hotels matching their search criteria.

### Image Management

- **Management of Images and Thumbnails**: Allows for the addition, deletion, and updating of images associated with cities and hotels.

### Popular Cities Display

- **Display Most Visited Cities**: Showcases popular cities based on user traffic, allowing users to explore trending destinations easily.

### Email Notifications

- **Invoice Requests via Email**: Users can effortlessly request detailed invoices anytime. the email service promptly delivers comprehensive invoices, including total price, hotel location, and relevant details, ensuring ease and transparency for the guests.
- **Enhanced User Communication**: Facilitates effective communication with users, keeping them informed about their bookings.

### Admin Interface

- **Search, Add, Update, and Delete Entities**: Provides administrators with full control over system entities, enabling efficient management of cities, hotels, rooms, and other components.
- **Streamlined Administrative Tasks**: Simplifies administrative tasks through a user-friendly interface, enhancing system maintenance.


## Endpoints
### RoomAmenities

| HTTP Method | Endpoint                      | Description                                    |
|--------|-------------------------------|---------------------------------------------|
| GET    | /api/room-amenities               | Retrieve a page of room amenities       
| POST   | /api/room-amenities               | Create a new room amenity              
| GET    | /api/room-amenities/{id}          | Get a room amenity specified by ID 
| DELETE | /api/room-amenities/{id}          | Delete room amenity specified by ID 
| PUT    | /api/room-amenities/{id}           | Update an existing room amenity
| PATCH  | /ap/room-amenities/{id}           | Partially Update an existing room amenity


### Authentication

| HTTP Method | Endpoint                           | Description                                                               |
|-------------|------------------------------------|---------------------------------------------------------------------------|
| POST        | /api/authentication/sign-in       | Processes a login request
| POST        | /api/authentication/register      | Processes registering a guest request                          |

### Home

| HTTP Method | Endpoint                                 | Description                                           |
|-------------|------------------------------------------|-------------------------------------------------------|
| GET         | /api/home/destinations/trending         | Retrieves the top 5 trending cities                |
| GET         | /api/home/search                        | Searches for hotels based on the search query|
| GET         | /api/home/featured-deals                | Retrieves the top 5 featured hotel deals            |

### Cities

| HTTP Method | Endpoint                                 | Description                                                               |
|-------------|------------------------------------------|---------------------------------------------------------------------------|
| GET         | /api/cities                             | Retrieve a page of cities           |
| POST        | /api/cities                             | Creates a new city                                                      |
| GET         | /api/cities/{cityId}                    | Retrieves details for a specific city       |
| DELETE      | /api/cities/{cityId}                    | Deletes a city by ID                                                   |
| PUT         | /api/cities/{cityId}                    | Updates an existing city                                                 |
| PATCH       | /api/cities/{cityId}                    | Partially updates a city                  |
| GET         | /api/cities/{cityId}/photos             | Retrieves all photos for a city                                          |
| POST        | /api/cities/{cityId}/gallery            | Uploads an image for a city                                              |
| PUT         | /api/cities/{cityId}/thumbnail          | Uploads a thumbnail for a city                                           |
| DELETE      | /api/cities/{cityId}/photos/{photoId}  | Deletes an image associated with a city                                  |


### RoomTypes

| HTTP Method | Endpoint                                      | Description                                               |
|-------------|-----------------------------------------------|-----------------------------------------------------------|
| GET         | /api/room-types/{roomTypeId}/discounts       | Retrieves discounts for a room type                      |
| GET         | /api/room-types/discounts/{discountId}       | Retrieves a discount by its ID                           |
| DELETE      | /api/room-types/discounts/{discountId}       | Deletes a discount by its ID                             |
| POST        | /api/room-types/discounts                    | Creates a new discount                                   |


### Guests

| HTTP Method | Endpoint                                 | Description                                           |
|-------------|------------------------------------------|-------------------------------------------------------|
| GET         | /api/guests/{guestId}/recently-visited-hotels | Retrieves recent 5 distinct hotels for a guest  |
| GET         | /api/guests/recently-visited-hotels    | Fetches 5 recent unique hotels for the authenticated guest |
| GET         | /api/guests/bookings                    | Retrieves bookings for the authenticated guest      |
| POST        | /api/guests/bookings                    | Reserve a room                                       |
| DELETE      | /api/guests/bookings/{bookingId}       | Cancels a booking for the guest                     |
| GET         | /api/guests/bookings/{bookingId}/invoice | Retrieves invoice for a booking                     |



### Hotels

| HTTP Method | Endpoint                                  | Description                                                     |
|-------------|-------------------------------------------|-----------------------------------------------------------------|
| GET         | /api/hotels                               | Retrieves all hotels                                           |
| POST        | /api/hotels                               | Creates a new hotel                                            |
| GET         | /api/hotels/{hotelId}                     | Retrieves information about a hotel                            |
| DELETE      | /api/hotels/{hotelId}                     | Deletes a hotel                                                |
| PUT         | /api/hotels/{hotelId}                     | Updates information about a hotel                              |
| GET         | /api/hotels/{hotelId}/available-rooms     | Retrieves available rooms for a hotel                          |
| GET         | /api/hotels/{hotelId}/photos              | Retrieves all photos associated with a hotel                   |
| POST        | /api/hotels/{hotelId}/gallery             | Uploads an image for a hotel                                   |
| PUT         | /api/hotels/{hotelId}/thumbnail           | Uploads a thumbnail for a hotel                                |
| DELETE      | /api/hotels/{hotelId}/photos/{photoId}    | Deletes an image associated with a hotel                       |
| GET         | /api/hotels/{hotelId}/rooms               | Retrieves all rooms available for a hotel                     |
| POST        | /api/hotels/{hotelId}/rooms               | Creates a new room for a hotel                                 |
| GET         | /api/hotels/{hotelId}/rooms/{roomId}      | Retrieves a room from a hotel                                  |
| GET         | /api/hotels/{hotelId}/room-types          | Retrieves all room categories available for a hotel           |
| GET         | /api/hotels/{hotelId}/bookings            | Retrieves all bookings associated with a hotel                |


## Reviews

| HTTP Method | Endpoint                              | Description                                          |
|-------------|---------------------------------------|------------------------------------------------------|
| GET         | /api/reviews/hotels/{hotelId}        | Retrieves reviews for a specific hotel              |
| POST        | /api/reviews                          | Creates a new review                                |




## Technology Stack Overview

### Technologies Used

- **C#**: Main programming language.
- **ASP.NET Core**: Framework for building high-performance, cross-platform web APIs.

### Database

- **Entity Framework Core**: Employing Entity Framework Core for streamlined object-relational mapping (ORM) within the .NET ecosystem, simplifying database interactions.
- **SQL Server**: Utilizing SQL Server for reliable and scalable backend database management, ensuring efficient storage and retrieval of application data.

### Image Storage

- **Firebase Storage**: Part of Google's Firebase platform, offers cloud-based storage for images in this hotel booking system. It provides scalability, reliability, and seamless API integration.

### API Documentation and Design

- **Swagger/OpenAPI**: For API specification and documentation.
- **Swagger UI**: Provides a user-friendly interface for API interaction.

### Authentication and Authorization

- **JWT (JSON Web Tokens)**: For secure transmission of information between parties.

### Monitoring and Logging

- **Serilog**: Logging library for .NET applications.

### Design Patterns

- **RESTful Principles**:  Adhering to RESTful design principles to ensure that APIs are designed for simplicity, scalability, and ease of use.
- **CQRS (Command Query Responsibility Segregation) Pattern**:  CQRS separates the responsibility for handling read and write operations into separate components. It can improve scalability and performance by allowing optimized data access paths for read and write operations.

### Architecture
- **Clean Architecture**
  - **External Layers**: 
    - Web: Controllers for handling requests and managing client-server communication.
    - Infrastructure: Manages external resources such as databases, email services, PDF generation, and identity management.
  - **Core Layers**:
     - Application Layer: Implements business logic and orchestrates interactions between components.
    - Domain Layer: Contains fundamental business rules and entities, independent of external concerns like databases or user interfaces.

### Security

- **HTTPS**: Ensures secure communication over the network.
- **Data Encryption**: Password hashing using `Isopoh.Cryptography.Argon2`.

## API Versioning
This API leverages the Asp.Versioning.Mvc library to implement a streamlined header-based versioning mechanism. This approach facilitates seamless client access to different API versions without requiring adjustments to the base URL or path. By utilizing headers, the versioning process is standardized and offers improved clarity and maintainability. This method also enhances flexibility and ease of integration for users while ensuring optimal compatibility with various client applications.
curl?
This API uses Asp.Versioning.Mvc for header-based versioning, allowing seamless access to different versions without changing the URL. It standardizes versioning via headers, improving clarity and flexibility for users and compatibility with client apps.

Users can effortlessly specify their preferred API version by including the x-api-version header in their requests. In instances where no version is explicitly specified, the API seamlessly defaults to the latest available version. For instance:

To explicitly request version 1.0:
```bash
curl -X GET "localhost:5231/api/cities" -H "x-api-version: 1.0"
```


To utilize the latest version by default:
```bash
curl -X GET "localhost:5231/api/cities"
```

## Setup Guide

This guide provides instructions on setting up an existing ASP.NET API project. Follow these steps to clone the repository, configure the `appsettings.json` file, and run the API locally.

### Prerequisites

- [.NET 7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0) installed on your system.
- Running SQL Server instance with a database.

### Step-by-Step Guide

#### 1. Clone the Repository

Clone the repository of your existing ASP.NET API project to your local machine:

```bash
git clone https://github.com/obada-yahya/Travel-And-Accommodation-Booking-Platform.git
```

#### 2. Navigate to the Project Directory

Change your current directory to the root directory of your ASP.NET API project:

```bash
cd TAABP\TAABP.API
```

#### 3. Configure `appsettings.json`

Open the `appsettings.json` file located in your project directory and configure the connection string for SQL Server. Replace the `<connection_string>` placeholder with your SQL Server connection string:

```json
{
  "ConnectionStrings": {
    "TAABPCoreDb: "<connection_string>"
  }
}
```

#### 4. Run the API Locally

Start the ASP.NET API project using the following command:

```bash
dotnet run
```

The API will be accessable using https://localhost:7022.

The swagger UI will open automatically where you can try and explore the endpoints or you can open it using https://localhost:7022/swagger.

## Get Involved
Your Feedback and Contributions are Welcome!

### Ways to Contribute:
- **Feedback**: Share your thoughts and ideas.
- **Issue Reporting**: Help us by reporting any bugs or issues on GitHub.
- **Code Contributions**: Contribute to the codebase.

### Contact and Support:
Email: [obadayahya.an@gmail.com](mailto:obadayahya.an@gmail.com).

GitHub: [Obada Yahya](https://github.com/obada-yahya).

Thank you for your interest. I look forward to hearing from you!
