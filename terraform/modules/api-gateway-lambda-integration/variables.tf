variable "api_id" {
  type = string
}

variable "api_arn" {
  description = "The ARN of the HTTP API to attach to."
  type        = string
}

variable "function_arn" {
  description = "The ARN of the Lambda function."
  type        = string
}

variable "function_name" {
  description = "The name of the Lambda function."
  type        = string
}

variable "http_method" {
  description = "The HTTP method to use (GET, PUT, POST, DELETE)."
  type        = string
  default     = "ANY"
}

variable "route" {
  description = "The API route."
  type        = string
  default     = "$default"
}

variable "integration_timeout_ms" {
  description = "Integration timeout in ms."
  type        = number
  default     = 50
}