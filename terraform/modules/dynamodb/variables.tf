variable "table_name" {
  type        = string
  description = "Name of the table to be created"
}

variable "partition_key" {
  type        = string
  description = "Name of the table's hash key (partition key)"
}

variable "sorting_key" {
  type        = string
  description = "Name of the table's range key (sorting key)"
  default     = ""
}

variable "ttl" {
  type        = string
  description = "Name of the table attribute to store the TTL timestamp in"
  default     = ""
}

variable "attributes" {
  type = list(object({
    name = string
    type = string
  }))
  description = "List of predefined attributes of the table and its types"
}

variable "global_secondary_indexes" {
  type = list(object({
    name               = string
    hash_key           = string
    range_key          = string
    projection_type    = string
    non_key_attributes = set(string)
  }))
  description = "List of global secondary indexes to add to this table with the parameters to setup them"
  default     = []
}

variable "enable_point_in_time_recovery" {
  type        = bool
  description = "Enables DynamoDB Point-in-time recovery feature"
  default     = false
}