# CLO Skalbara Uppgift 02 - API

## Översikt
.NET 9 serverless API som körs som AWS Lambda-funktion bakom API Gateway. Hanterar CRUD-operationer för items i DynamoDB.

## Arkitektur
```
API Gateway → Lambda (.NET 9) → DynamoDB
```

## Endpoints
- `GET /api/items` - Hämta alla items
- `POST /api/items` - Skapa nytt item
- `PUT /api/items/{id}` - Uppdatera item
- `DELETE /api/items/{id}` - Ta bort item

## Teknologi
- **.NET 9**: Lambda runtime med AOT compilation
- **DynamoDB**: NoSQL databas för skalbarhet
- **API Gateway**: REST API routing
- **ECR**: Container registry för Lambda

## CI/CD
GitHub Actions bygger och deployer automatiskt:
1. Bygg .NET Docker image
2. Push till Amazon ECR
3. Uppdatera Lambda-funktion
4. Deploy API Gateway

## Säkerhet
- IAM roller med minimal behörighet
- API Gateway med CORS-konfiguration
- Miljövariabler för konfiguration

## Build Commands
```bash
# Restore dependencies
dotnet restore

# Build for release with AOT
dotnet publish -c Release -r linux-x64 --self-contained

# Build Docker image
docker build -t api-lambda .
```

## Setup
1. Deploy infrastructure: `terraform apply` i `/IaC/Terraform`
2. Konfigurera GitHub secrets med AWS credentials
3. Push kod-ändringar för att trigga automatisk deployment

### GitHub Actions Workflow
- Triggers on pushes to `main` branch affecting `API/**` files
- Builds .NET 9 application with AOT compilation
- Creates and pushes Docker container to AWS ECR
- Updates Lambda function with new container image
- Waits for deployment completion

See `/CI-CD-SETUP.md` for detailed setup instructions.