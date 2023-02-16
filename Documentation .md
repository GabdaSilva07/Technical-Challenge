# Documentation

## Steps to Refactor
To start the task, I made sure to thoroughly understand the requirements before proceeding. Next, I confirmed that the application was properly set up and that the database was functional. To verify the functionality of the controller endpoints, I utilized Postman to test them.

During the refactoring phase, I chose to follow a Test-Driven Development (TDD) approach to ensure that the application remained fully functional throughout the process. Unfortunately, I encountered some issues due to a lack of experience with mappers. While I would typically work through these problems, I recognized that doing so would require a significant amount of time. As a result, I made the decision to skip this step for the time being.


During the refactoring phase, I opted to employ the CQRS pattern due to its ability to enhance scalability and maintainability, as well as improve performance by allowing for the breakdown of queries with excessive joins. This approach aligns well with the principles of domain-driven design (DDD) by emphasizing the importance of separating workload.

In addition to the CQRS pattern, I also implemented the factory pattern in order to enable the separation of commands based on their functionalities. This helped to loosen coupling between the classes and facilitated easier unit testing of object creation. Finally, I integrated the factories by injecting them into the controller and calling them based on the object type, resulting in a more efficient process.

In the OrderController file, I chose to incorporate both orders and order details, as this mirrored the approach used in the previous controller. However, I later realized that it would be beneficial to extract order details into its own controller, which would result in a more efficient and organized implementation.

During the refactoring phase, I modified the parameters of each endpoint to receive objects from the body of the request and filters to be passed through the route. Queries were specifically designed to return a specified amount of data set at the endpoint, which was initially set to 10. However, this value can easily be adjusted, and by performing pagination logic on the server, we can improve performance of the client.

As I progressed with the design, I utilized Postman to test and verify the functionality of the application. Additionally, I created unit tests to ensure that the code is robust and that any future refactoring efforts can be undertaken with confidence. This proactive approach to testing should reduce the risk of bugs and errors that could arise during any future changes to the code.

To enhance the portability and scalability of the application, I created a Dockerfile that successfully ran locally. Additionally, I created a Docker Compose file that could be used to quickly spin up the containers for both the API and the database. However, as I am still teaching myself these technologies, I encountered some file permission issues that, due to a lack of time, I was unable to resolve.

While exploring the use of GitHub Actions to build images on a pipeline, I encountered some challenges as it was my first time using the tool. Despite my best efforts, I was unable to complete the task due to lack of time as I was trying to finish this project by Thurday night.

However, I was able to accomplish this task in a personal project of mine using Azure Pipelines. Using variable libraries and secrets from Azure Key Vault, I was able to build images and push them to GitLab.