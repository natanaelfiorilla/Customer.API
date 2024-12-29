resource "aws_apigatewayv2_api" "lambda" {
  name          = var.function_name
  protocol_type = "HTTP"
  description   = "Serverlessland API Gwy HTTP API and AWS Lambda function"
}

resource "aws_apigatewayv2_stage" "lambda" {
  api_id      = aws_apigatewayv2_api.lambda.id
  name        = var.stage_name
  auto_deploy = var.stage_auto_deploy

  access_log_settings {
    destination_arn = aws_cloudwatch_log_group.apigateway_log.arn

    format = jsonencode({
      requestTime               = "$context.requestTime"
      requestId                 = "$context.requestId"
      integrationRequestId      = "$context.integration.requestId"
      sourceIp                  = "$context.identity.sourceIp"
      userAgent                 = "$context.identity.userAgent"
      protocol                  = "$context.protocol"
      httpMethod                = "$context.httpMethod"
      path                      = "$context.path"
      resourcePath              = "$context.resourcePath"
      routeKey                  = "$context.routeKey"
      status                    = "$context.status"
      responseLength            = "$context.responseLength"
      responseLatency           = "$context.responseLatency"
      integrationLatency        = "$context.integration.latency"
      integrationResponseStatus = "$context.integration.status"
      integrationErrorMessage   = "$context.integrationErrorMessage"
      }
    )
  }
  depends_on = [
    aws_cloudwatch_log_group.apigateway_log
  ]

}

resource "aws_cloudwatch_log_group" "apigateway_log" {
  name              = "/aws/api_gw/${aws_apigatewayv2_api.lambda.name}"
  retention_in_days = var.apigw_log_retention
}
