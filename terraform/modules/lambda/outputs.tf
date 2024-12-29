output "function" {
  value = aws_lambda_function.function
}

output "role" {
  value = aws_iam_role.lambda_function_role
}