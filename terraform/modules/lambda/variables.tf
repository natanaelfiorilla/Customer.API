variable "function_name" {
  type = string
}

variable "artifacts_location" {
  type        = string
  description = "Lambda artifact local path"
}

variable "artifacts_bucket_id" {
  type        = string
  description = "Lambda artifact bucket id"
}

variable "artifacts_bucket_key" {
  type        = string
  description = "Lambda artifact bucket key"
}

variable "handler" {
  type        = string
  description = "Function entrypoint in the code"
}

variable "timeout" {
  type    = number
  default = 30
}

variable "mem_size" {
  type    = number
  default = 128
}

variable "environment" {
  type    = string
  default = "dev"
}

variable "vpc_config" {
  type = object({
    vpc_id             = string
    security_group_ids = list(string)
    subnet_ids         = list(string)
  })
  default = null
}

variable "environment_variables" {
  type        = map(any)
  description = "List of environment variables to set in the lambda"
}

variable "log_retention_days" {
  type        = number
  description = "Retention of the lambda logs (in days)"
  default     = 30
}

variable "policies_to_attach" {
  type        = map(any)
  description = "Attaches a Managed IAM Policy to an IAM role"
  default     = {}
}