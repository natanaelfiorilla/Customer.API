locals {
  entrypoint_api = "${var.dotnet_project_name}::${var.dotnet_project_name}.LambdaEntryPoint::FunctionHandlerAsync"

  artifacts_location    = "./artifacts/${var.dotnet_project_name}.zip"
  artifacts_bucket_name = "${var.environment}-${var.artifacts_bucket_name}"
  artifacts_bucket_key  = "${var.dotnet_project_name}.zip"
  function_name         = "${var.environment}-${var.function_name}"

  lambda_env_vars = { # List of environment variables to set in the lambdas    
    DOTNET_ENVIRONMENT = var.environment
  }

  default_tags = {
    Environment  = var.environment
    FunctionName = var.function_name
  }

  table_partition_key = "Id"
  table_attributes = [
    { name = "Id", type = "S" }
  ]
}