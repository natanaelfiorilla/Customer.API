variable "function_name" {
  type = string
}

variable "environment" {
  type    = string
  default = "dev"
}

variable "stage_auto_deploy" {
  type    = bool
  default = true
}

variable "stage_name" {
  description = "The stage name."
  type        = string
  default     = "$default"
}

variable "apigw_log_retention" {
  default = 1
}