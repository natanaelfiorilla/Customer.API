output "api_gw" {
  value = aws_apigatewayv2_api.lambda
}

output "api_gw_stage" {
  value = aws_apigatewayv2_stage.lambda
}