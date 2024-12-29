PROJECT="Taxdown.API"
ARTIFACT_FOLDER="terraform/artifacts"
ENVIRONMENT="dev"

dotnet lambda package -c Release -f "net8.0" --project-location "../$PROJECT" --output-package "../$ARTIFACT_FOLDER/$PROJECT.zip"

cd ../terraform/

terraform init
terraform apply -var-file="./environments/$ENVIRONMENT.tfvars"