resource "aws_s3_bucket" "lambda_artifacts_bucket" {
  bucket        = local.artifacts_bucket_name
  force_destroy = true
}

resource "aws_s3_bucket_ownership_controls" "private_bucket" {
  bucket = aws_s3_bucket.lambda_artifacts_bucket.id
  rule {
    object_ownership = "BucketOwnerPreferred"
  }
}

resource "aws_s3_bucket_acl" "private_bucket" {
  depends_on = [aws_s3_bucket_ownership_controls.private_bucket]
  bucket     = aws_s3_bucket.lambda_artifacts_bucket.id
  acl        = "private"
}


module "dynamodb" {
  source        = "./modules/dynamodb"
  table_name    = var.table_name
  partition_key = local.table_partition_key
  attributes    = local.table_attributes
}

module "lambda" {
  source                = "./modules/lambda"
  function_name         = local.function_name
  artifacts_location    = local.artifacts_location
  artifacts_bucket_id   = aws_s3_bucket.lambda_artifacts_bucket.id
  artifacts_bucket_key  = local.artifacts_bucket_key
  handler               = local.entrypoint_api
  timeout               = var.lambda_timeout
  mem_size              = var.lambda_mem_size
  log_retention_days    = var.lambda_log_retention_days
  environment_variables = local.lambda_env_vars
  policies_to_attach = {
    one = module.dynamodb.policy_basic_access.arn
  }
}

module "api_gateway" {
  source        = "./modules/api-gateway"
  function_name = local.function_name
  environment   = var.environment
}

module "lambda_api_gateway_integration" {
  source                 = "./modules/api-gateway-lambda-integration"
  api_id                 = module.api_gateway.api_gw.id
  api_arn                = module.api_gateway.api_gw.execution_arn
  function_arn           = module.lambda.function.invoke_arn
  function_name          = module.lambda.function.function_name
  integration_timeout_ms = var.api_gtw_timeout
}