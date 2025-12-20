* Technical Specifications

** System Wide Specifications
- the system shall use flatten namespaces
- the Backend shall have 3 projects
    - {system}.Core
    - {system}.Infrastructure
    - {system}.Api
- the system shall NOT use AutoMapper. 
- the system shall create and use extensions methods of Core models shall be created in the Api layer with a ToDto method that returns the Mapped Dto
- the system shall not use IRepositories, instead use the I{system}Context interface
- the shalled include the name of the entity in for Ids.

	Do
	{entity}Id

	Don't 
	Id
- the system shall have exactly one (class, enum, record, etc..) per file.
- the system shall NOT have mutiple object defined in a file.
- the system shall use SQLExpress by default

* Core Project Specifications

- the core project shall be named {system}.Core
- aggregates shall go in the {system}.Core\Model folder
- each aggregate shall have a folder in the {system}.Core\Model folder called {system}.Core\Model\{aggregate}Aggregate
- the core project shall contain an interface called I{system}Context with DbSet properties for each entity in system. The interface represents the persistence surface. The implemtation of the interface is in the Infrastructure project
- the core project shall contain a folder called Services which contains services (interface and classes) with core behaviour logic to the system. Authentication, Emailing, Azure AI Integration etc,,
** Aggregate Folder
- aggregate folder shall be named {system}.Core\Model\{aggregate}Aggregate
- inside the {system}.Core\Model\{aggregate}Aggregate contains all the Entities, Enums, Events and AggregateRoot, etc... 
- each of the types inside of {system}.Core\Model\{aggregate}Aggregate has their own folder. (Events folder, Enums folder, etc....)

** {system}.Infrastructure
- shall contain the I{system}Context implementation. The implementation class is called {system}Context
- shall contain EF Miigrations
- shall contain Entity Configurations
- shall contain Seeding services

** The Api Project
- the api project shall be named {system}.Api
- the api project shall have a folder called Features containing all Commands, Queries (using MediatR) grouped in folders by BoundedContext
- the subfolders within the Features folder shall contain the Dtos
- the api project shall have Api Controllers in a Controllers folder
- the shall optionally have MediatR behaviours in a folder called Behaviours

* Frontend Speficiations
- components files in the frontend shall be seperate.
    - a file for html
    - a file for scss
    - a file for typescript
- the e2e folder shall be located in the src folder within the project folder
    - src/{system}.WebApp/projects/{system}/src/e2e    
- the frontend shall be named {system}.WebApp (src\{system}.WebApp)
- the frontend shall be a workspace with projects
- the frontend project shall be called {system} if not an admin frontend
- the frontend project shall be called {system}-admin if the it's an admin frontend
- the system shall use the latest version of Angular
- the system shall use the latest version of Angular Material for all UI elements
- the frontned shall NOT use ngrx
- the frontned shall NOT add a "Component" prefix to Angular component class names
    Do
    Header

    Do not:
    HeaderComponent
- the frontned shall NOT add a "component" prefix to Angular component file names
    Do
    header.html

    Do not:
    header.component.html
- the frontned shall use rxjs and signals for state management
- the frontend shall be responsive and mobile first
- the frontend shall use BEM html class naming strategy
- the frontend shall use design tokens for consistent spacing    
- the frontend shall use jest for unit tests and playwright for e2e tests. Jest tests are configured to ignore the e2e folder
- frontend shall be configured with a baseUrl which is the url of the backend in the launchSettings
- frontend shall contain a folder called "pages" for components that can appear in the <router-outlet> and a folder called components for re-usable components. Child components of a page is contained within the components folder
- the system shall create barrels for every folder and export typescript code except test code.
- the system shall strictly adhere to Material 3 guideline and not use any colors that are not defined in the angular material theme
