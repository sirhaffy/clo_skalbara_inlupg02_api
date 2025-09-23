# JSON API Lambda

Minimal .NET 9 Lambda function with REST API for DynamoDB operations.

## Features
- **AOT Compilation** for faster cold starts
- **Container-based deployment** on AWS Lambda
- **REST API** with full CRUD operations
- **DynamoDB integration** with strongly typed models
- **Minimal API** design pattern

## API Endpoints
- `GET /items` - Get all items
- `GET /items/{id}` - Get specific item
- `POST /items` - Create new item
- `PUT /items/{id}` - Update existing item
- `DELETE /items/{id}` - Delete item

## Build Commands
```bash
# Restore dependencies
dotnet restore

# Build for release with AOT
dotnet publish -c Release -r linux-x64 --self-contained

# Build Docker image
docker build -t api-lambda .

# Tag for ECR (replace with your ECR URI)
docker tag api-lambda:latest <account-id>.dkr.ecr.<region>.amazonaws.com/api-lambda:latest

# Push to ECR
docker push <account-id>.dkr.ecr.<region>.amazonaws.com/api-lambda:latest
```

## Infrastructure
IAM roles, DynamoDB table, and Lambda deployment handled by Terraform in `/IaC`.

## CI/CD Pipeline
Automated deployment pipeline:
```
Code Push → GitHub Actions → Build Container → Push to ECR → Update Lambda
```

### Setup
1. Deploy infrastructure: `terraform apply` in `/IaC/Terraform`
2. Configure GitHub secrets with AWS credentials from Terraform output
3. Push code changes to trigger automated deployment

### GitHub Actions Workflow
- Triggers on pushes to `main` branch affecting `API/**` files
- Builds .NET 9 application with AOT compilation
- Creates and pushes Docker container to AWS ECR
- Updates Lambda function with new container image
- Waits for deployment completion

See `/CI-CD-SETUP.md` for detailed setup instructions.