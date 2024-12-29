resource "aws_s3_object" "lambda_artifacts" {
  bucket = var.artifacts_bucket_id
  key    = var.artifacts_bucket_key
  source = var.artifacts_location
  etag   = filemd5(var.artifacts_location)
}

resource "aws_lambda_function" "function" {
  function_name                  = var.function_name
  s3_bucket                      = var.artifacts_bucket_id
  s3_key                         = aws_s3_object.lambda_artifacts.key
  handler                        = var.handler
  role                           = aws_iam_role.lambda_function_role.arn
  runtime                        = "dotnet8"
  timeout                        = var.timeout
  memory_size                    = var.mem_size
  reserved_concurrent_executions = "-1"
  description                    = "Lambda function ${var.function_name}-lambda, running on ${var.environment}."
  source_code_hash               = filebase64sha256(var.artifacts_location)
  dynamic "vpc_config" {
    for_each = var.vpc_config != null ? [1] : []
    content {
      security_group_ids = var.vpc_config.security_group_ids
      subnet_ids         = var.vpc_config.subnet_ids
    }
  }

  environment {
    variables = var.environment_variables
  }
}

resource "aws_iam_role" "lambda_function_role" {
  name = "${var.function_name}-role"
  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [{
      Action = "sts:AssumeRole"
      Effect = "Allow"
      Sid    = ""
      Principal = {
        Service = "lambda.amazonaws.com"
      }
      }
    ]
  })
}

resource "aws_iam_role_policy_attachment" "lambda_policy_attach" {
  role       = aws_iam_role.lambda_function_role.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AWSLambdaBasicExecutionRole"
}

resource "aws_iam_role_policy_attachment" "iam_role_policy_attachment_lambda_vpc_access_execution" {
  role       = aws_iam_role.lambda_function_role.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AWSLambdaVPCAccessExecutionRole"
}

resource "aws_cloudwatch_log_group" "lambda_log" {
  name              = "/aws/lambda/${aws_lambda_function.function.function_name}"
  retention_in_days = var.log_retention_days
}

resource "aws_iam_role_policy" "cloudwatch" {
  name   = "${var.function_name}-iamRole-cloudWatch"
  role   = aws_iam_role.lambda_function_role.id
  policy = <<EOF
{
    "Version": "2012-10-17",
    "Statement": [
        {
            "Effect": "Allow",
            "Action": [
                "logs:CreateLogGroup",
                "logs:CreateLogStream",
                "logs:DescribeLogGroups",
                "logs:DescribeLogStreams",
                "logs:PutLogEvents",
                "logs:GetLogEvents",
                "logs:FilterLogEvents"
            ],
            "Resource": "*"
        }
    ]
}
  EOF
}

resource "aws_iam_role_policy_attachment" "policy_attachments" {
  for_each   = var.policies_to_attach
  role       = aws_iam_role.lambda_function_role.name
  policy_arn = each.value
}