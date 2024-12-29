variable "region" {
  type = string
}

variable "environment" {
  type = string
}

variable "function_name" {
  type = string
}

variable "dotnet_project_name" {
  type = string
}

variable "lambda_timeout" {
  type    = number
  default = 120
}

variable "api_gtw_timeout" {
  type    = number
  default = 29000
}

variable "lambda_mem_size" {
  type    = number
  default = 1024
}

variable "lambda_log_retention_days" {
  type        = number
  description = "Retention of the lambda logs (in days)"
  default     = 14
}

variable "artifacts_bucket_name" {
  type        = string
  description = "Lambda artifact bucket name"
}

variable "account_id" {
  description = "Account id where the resources will be deployed"
  type        = string
}

variable "table_name" {
  type    = string
  default = "Customers"
}