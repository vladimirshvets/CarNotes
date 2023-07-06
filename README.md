# CarNotes

### Implemented
- User login/registration
- User profile Page, ability to edit profile info
- Garage Page, notes counter
- Create/update/delete car
- Car details Page
- Stats:
    - Mileage (total, per month)
    - Average fuel consumption
    - Spendings (total, per km, per month, per note type)
    - Note history (sorted by mileages with related notes)
- Mileage chart
- CRUD actions for Notes: Refuelings, Washings, Spare Parts, Services, Legal Procedures, Text Notes

### To Do
- Remove car avatar and carousel photos from storage on car removal
- New/edit car Page design
- Add content to Homepage
- Upload user photo, car photo
- Filter spare parts by category/group
- Social auth
- Allow to add spare parts with no installation date/mileage
- Calculate USD prices based on exchange rate on a specific date
- Use another mileage chart
- Datepickers on forms
- Notifications

### Stack | Features | Libraries

#### Backend
- C# 11, .NET 7
- ASP.NET Core WebAPI
- Neo4j data storage
- JWT Authentication
- AutoMapper

#### Frontend
- Vue.js client app
