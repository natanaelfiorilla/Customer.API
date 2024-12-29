resource "aws_apigatewayv2_integration" "lambda_integration" {
  api_id = var.api_id

  integration_uri      = var.function_arn
  integration_type     = "AWS_PROXY"
  integration_method   = "POST"
  timeout_milliseconds = var.integration_timeout_ms
  request_parameters = {
    "overwrite:path" = "$request.path"
  }
}

resource "aws_apigatewayv2_route" "any" {
  api_id    = var.api_id
  route_key = var.route
  target    = "integrations/${aws_apigatewayv2_integration.lambda_integration.id}"
}

resource "aws_lambda_permission" "api_gw_permission" {
  statement_id  = "AllowLambdaExecutionFromAPIGateway_${var.function_name}"
  action        = "lambda:InvokeFunction"
  function_name = var.function_name
  principal     = "apigateway.amazonaws.com"

  source_arn = "${var.api_arn}/*/*"
}
