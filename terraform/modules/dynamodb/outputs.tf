output "policy_basic_access" {
  value = aws_iam_policy.policy_basic_access
}
output "table" {
  value = aws_dynamodb_table.main
}