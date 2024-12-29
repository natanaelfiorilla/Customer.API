# Taxdown Customer API

This project provides a simple .NET 8.0 Web API with DynamoDB integration. It showcases a layered architecture (domain, repository, service, controllers) and uses Terraform for infrastructure deployment.

## Deployed Environment

- **Base URL**: [https://7tzavkdxw1.execute-api.us-east-2.amazonaws.com/](https://7tzavkdxw1.execute-api.us-east-2.amazonaws.com/)
- **Swagger URL**: [https://7tzavkdxw1.execute-api.us-east-2.amazonaws.com/swagger/index.html](https://7tzavkdxw1.execute-api.us-east-2.amazonaws.com/swagger/index.html)

You can access the OpenAPI documentation and test endpoints directly from the Swagger UI link above.

## Requirements

- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- An **AWS Account**, with permissions to manage DynamoDB tables and deploy Lambda/API Gateway resources.
- **Terraform** (v1.x+)

## Local Development

If you wish to run the application locally (e.g., for development or debugging), you can:

1. Ensure you have the .NET 8.0 SDK installed.
2. Clone the repository.
3. Navigate to the project folder and run:
   ```bash
   dotnet build
   dotnet run
   ```
   This will start the Web API on your localhost port (by default, `http://localhost:5000` or `https://localhost:5001`).

## Deploying to AWS with Terraform

1. **Edit dev.tfvars**  
   Open the file located at:
   ```
   /terraform/environments/dev.tfvars
   ```
   Add/update your **AWS Account ID** (and any other required variables). For example:
   ```hcl
   aws_account_id = "123456789012"
   region         = "us-east-2"
   ...
   ```
   
2. **Run the Deployment Script**  
   From the root of the repository (or wherever the `scripts` folder is located), run:
   ```bash
   sh ./scripts/lambda_deploy_from_local.sh
   ```
   This script will:
   - Build the .NET 8 project.
   - Package and upload the Lambda.
   - Apply the Terraform configuration (which sets up or updates the Lambda, API Gateway, DynamoDB, etc.).

3. **Verify the Deployment**  
   Once finished, Terraform should output relevant URLs or confirmation messages.  
   You can test the API at the above **Base URL** and check **Swagger** at:
   ```
   https://<api-id>.execute-api.<region>.amazonaws.com/swagger/index.html
   ```

## Contact / Contributing

For questions or contributions, please create an issue or open a pull request. We welcome feedback and suggestions!